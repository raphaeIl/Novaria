using Google.Protobuf;
using Google.Protobuf.Collections;
using Ionic.Zip;
using Nova.Client;
using Novaria.Common.Util;
using Serilog;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace Novaria.GameServer.Services
{
    // note that all operations in this class MUST be thread-safe, idk if it is currently but if it's not, there will be race condition problems
    public class TableService(ILogger<TableService> _logger)
    {
        private readonly ILogger<TableService> logger = _logger;

        // example: maps Achievement (type) -> table_Achievement (instance), table_Achievement has a list that stores table
        private readonly Dictionary<Type, IMessage> caches = [];
        public static string ResourceDir = Path.Join(Path.GetDirectoryName(AppContext.BaseDirectory), "Resources");
        
        private Type currentTableTypeCache; // this is the type that we're currently loading, ex. Achievement (type), reason for caching it in a field member? Nova uses hardcoded params Action<int, byte[]> so im too lazy to change

        /// <summary>
        /// Please <b>only</b> use this to get table that <b>have a respective file</b> (i.e. <c>CharacterExcelTable</c> have <c>characterexceltable.bytes</c>)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public IMessage GetTable<T>(bool bypassCache = false) where T : IMessage
        {
            var type = typeof(T);
            currentTableTypeCache = type;

            if (!bypassCache && caches.TryGetValue(type, out var cache))
                return (T)cache;

            var tableDir = Path.Combine(ResourceDir, "Tables");
            var bytesFilePath = Path.Combine(tableDir, $"{typeof(T).Name}.bytes");

            if (!File.Exists(bytesFilePath))
            {
                throw new FileNotFoundException($"bytes files for {type.Name} not found");
            }

            //TableEncryptionService.XOR(type.Name, bytes);
             
            this.LoadCommonBin<T>(bytesFilePath); // after this, loaded table will be in the cache

            Log.Information("{Excel} Table loaded and cached", type.Name);

            return caches[currentTableTypeCache];
        }

        private void LoadCommonBin<T>(string bytesFilePath)
        {
            currentTableTypeCache = typeof(T); // too lazy to change actions params, so do this

            // get the table_XXX type
            Type table_Type = Assembly.GetAssembly(typeof(table_Achievement)).GetTypes().Where(t => t.Name == $"table_{typeof(T).Name}").FirstOrDefault();

            if (table_Type == null)
            {
                Log.Error($"table_{typeof(T).Name} type was not found.");
                return;
            }

            var inst = (IMessage)Activator.CreateInstance(table_Type);
            caches[currentTableTypeCache] = inst;

            GameDataController.Instance.LoadCommonBinData(bytesFilePath, new Action<int, byte[]>(this.AddCommonBin),
                                                                         new Action<string, byte[]>(this.AddCommonBin),
                                                                         new Action<long, byte[]>(this.AddCommonBin),
                                                                         new Action<byte[]>(this.AddCommonBin));
        }

        private void AddCommonBin(int _, byte[] data) { AddCommonBin_internal(_, data); }
        private void AddCommonBin(string _, byte[] data) { AddCommonBin_internal(_, data); }
        private void AddCommonBin(long _, byte[] data) { AddCommonBin_internal(_, data); }
        private void AddCommonBin(byte[] data) { AddCommonBin_internal(null, data); }

        private void AddCommonBin_internal(object _, byte[] data)
        {
            var parserProperty = currentTableTypeCache.GetProperties().Where(p => p.Name == "Parser").SingleOrDefault();
            var parserInstance = parserProperty.GetValue(null);

            var parsedData = typeof(MessageParser).GetMethods().Where(m => m.Name == "ParseFrom").FirstOrDefault().Invoke(parserInstance, new object[] { data }); ;
            // add to target table, very inefficient rn
            IMessage targetTable = caches[currentTableTypeCache];
            var __ = currentTableTypeCache.GetProperties();


            var listField = targetTable.GetType().GetProperties().Where(p => p.Name == "List").SingleOrDefault();

            object repeatedFieldInstance = listField.GetValue(targetTable);

            var addMethod = repeatedFieldInstance.GetType().GetMethods().Where(m => m.Name == "Add").FirstOrDefault();

            addMethod.Invoke(repeatedFieldInstance, new object[] { parsedData });

            //Log.Information($"Added {_}");
        }
    }

    internal static class ExcelTableServiceExtensions
    {
        public static void AddExcelTableService(this IServiceCollection services)
        {
            services.AddSingleton<TableService>();
        }
    }
}
