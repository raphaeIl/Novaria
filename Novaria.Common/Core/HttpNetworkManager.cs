using Google.Protobuf;
using Novaria.Common.Crypto;
using Proto;
using System.Reflection;
using Serilog;
using Novaria.Common.Util;

namespace Novaria.Common.Core
{
    public class HttpNetworkManager : Singleton<HttpNetworkManager>
    {
        public byte[] BuildIkeResponse(Packet packet)
        {
            BinaryWriter packetWriter = new BinaryWriter(new MemoryStream());

            byte[] msgIdBytes = BitConverter.GetBytes(packet.msgId);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse<byte>(msgIdBytes);
            }

            packetWriter.Write(msgIdBytes.AsSpan<byte>());
            packetWriter.Write(packet.msgBody.AsSpan<byte>()); 


            byte[] packetData = ((MemoryStream)packetWriter.BaseStream).ToArray();
            Span<byte> encryptedPacketData = (new byte[packetData.Length + 16]).AsSpan();

            AeadTool.Encrypt(encryptedPacketData, AeadTool.IKE_KEY, AeadTool.PS_REQUEST_NONCE, packetData, false);

            BinaryWriter rawResponseWriter = new BinaryWriter(new MemoryStream());
            rawResponseWriter.Write(AeadTool.PS_REQUEST_NONCE);
            rawResponseWriter.Write(encryptedPacketData);

            return ((MemoryStream)rawResponseWriter.BaseStream).ToArray();
        }

        public T DecodePacket<T>(Packet packet) where T : IMessage
        {
            Assembly assembly = Assembly.GetAssembly(typeof(LoginReq));
            Type targetType = typeof(T);

            PropertyInfo parserProperty = targetType.GetProperty("Parser", BindingFlags.Static | BindingFlags.Public);
            object parserInstance = parserProperty.GetValue(null);
            MethodInfo parseFromMethod = parserInstance.GetType().GetMethod("ParseFrom", new[] { typeof(byte[]) });

            IMessage parsedMessage = (IMessage)parseFromMethod.Invoke(parserInstance, new object[] { packet.msgBody });

            if (parsedMessage == null)
            {
                throw new InvalidOperationException("Failed to parse message.");
            }

            return (T)parsedMessage;
        }

        public IMessage DecodePacket(Type targetType, Packet packet)
        {
            PropertyInfo parserProperty = targetType.GetProperty("Parser", BindingFlags.Static | BindingFlags.Public);
            object parserInstance = parserProperty.GetValue(null);
            MethodInfo parseFromMethod = parserInstance.GetType().GetMethod("ParseFrom", new[] { typeof(byte[]) });

            IMessage parsedMessage = null;
            try 
            { 
                parsedMessage = (IMessage)parseFromMethod.Invoke(parserInstance, new object[] { packet.msgBody });
            } catch (Exception ex)
            {
                Log.Error($"Failed to parse message of type {targetType.Name}: {ex.Message}");
            }


            //if (parsedMessage == null)
            //{
            //    throw new InvalidOperationException("Failed to parse message.");
            //}

            return parsedMessage;
        }

        public byte[] BuildResponse(Packet packet)
        {
            BinaryWriter packetWriter = new BinaryWriter(new MemoryStream());

            byte[] msgIdBytes = BitConverter.GetBytes(packet.msgId);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse<byte>(msgIdBytes);
            }

            packetWriter.Write(msgIdBytes.AsSpan<byte>());
            packetWriter.Write(packet.msgBody.AsSpan<byte>());

            byte[] packetData = ((MemoryStream)packetWriter.BaseStream).ToArray();
            Span<byte> encryptedPacketData = (new byte[packetData.Length + 16]).AsSpan();

            AeadTool.Encrypt(encryptedPacketData, AeadTool.key3, AeadTool.PS_REQUEST_NONCE, packetData, false);

            BinaryWriter rawResponseWriter = new BinaryWriter(new MemoryStream());
            rawResponseWriter.Write(AeadTool.PS_REQUEST_NONCE);
            rawResponseWriter.Write(encryptedPacketData);

            return ((MemoryStream)rawResponseWriter.BaseStream).ToArray();
        }

        public Packet ParseRequest(byte[] encryptedRawPayload)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(encryptedRawPayload));

            byte[] nonceBytes = new byte[12]; // nonce 12 bytes length
            reader.Read(nonceBytes);

            int packetSize = encryptedRawPayload.Length - nonceBytes.Length; // skip nonce length (12)

            byte[] packetBytes = new byte[packetSize];
            reader.Read(packetBytes);

            if (reader.BaseStream.Position != encryptedRawPayload.Length)
            {
                Log.Error("something went wrong, not all the bytes were read");
                Log.Error("reader pos: " + reader.BaseStream.Position);
                Log.Error("original len:" + encryptedRawPayload.Length);
            }

            Span<byte> decrypt_result = new Span<byte>(new byte[packetSize - 16]); // for chacha20 decrypt, the result is 16 bytes less than the input data

            Span<byte> nonce = nonceBytes.AsSpan();
            Span<byte> packet_data = packetBytes.AsSpan();

            bool success = AeadTool.Decrypt(decrypt_result, AeadTool.key3, nonce, packet_data, null);

            if (!success)
            {
                Log.Error("something went wrong when chacha20 decrypting the data");
                throw new InvalidDataException("something went wrong when chacha20 decrypting the data");
            }

            byte[] decrypted_bytes = decrypt_result.ToArray();

            // might wanna use reader here
            byte[] msgid_bytes = decrypted_bytes[10..12]; // two bytes before msg data is msgid
            Array.Reverse<byte>(msgid_bytes); // should check BitConverter.IsLittleEndian (if true -> reverse, was true on my pc)

            short msgId = BitConverter.ToInt16(msgid_bytes);

            Packet packet = new Packet()
            {
                msgId = msgId,
                msgBody = decrypted_bytes[12..], // 2 + 2 (seqid) + 8 (timestamp)
            };

            return packet;
        }

        public Packet ParseResponse(byte[] encryptedRawPayload)
        {
            bool flag = false; 

            BinaryReader reader = new BinaryReader(new MemoryStream(encryptedRawPayload));

            byte[] nonceBytes = new byte[12]; // nonce 12 bytes length
            reader.Read(nonceBytes);

            byte[] aeadBytes = new byte[1]; // aead byte is 1 byte, original field in AeadTool
            int packetSize = encryptedRawPayload.Length - nonceBytes.Length; // skip nonce length (12)

            if (flag)
            {
                reader.Read(aeadBytes); // read to skip in memory, idk whats the use for this
                packetSize--; // skip in payload
            }

            byte[] packetBytes = new byte[packetSize];
            reader.Read(packetBytes);

            if (reader.BaseStream.Position != encryptedRawPayload.Length)
            {
                Log.Error("something went wrong, not all the bytes were read");
                Log.Error("reader pos: " + reader.BaseStream.Position);
                Log.Error("original len:" + encryptedRawPayload.Length);
            }

            Span<byte> decrypt_result = new Span<byte>(new byte[packetSize - 16]); // for chacha20, the result is 16 bytes less than the input data

            Span<byte> nonce = nonceBytes.AsSpan();
            Span<byte> packet_data = packetBytes.AsSpan();

            bool success = AeadTool.Decrypt(decrypt_result, AeadTool.key3, nonce, packet_data, null); // associateData NULL FOR THIS response pcap 

            if (!success)
            {
                Console.WriteLine("something went wrong when chacha20 decrypting the data");
            }

            byte[] decrypted_bytes = decrypt_result.ToArray();

            //Utils.PrintByteArray(decrypted_bytes);

            byte[] msgid_bytes = decrypted_bytes[..2]; // first two bytes is msgid
            Array.Reverse<byte>(msgid_bytes); // should check BitConverter.IsLittleEndian (if true -> reverse, was true on my pc)

            short msgId = BitConverter.ToInt16(msgid_bytes);

            Packet packet = new Packet()
            {
                msgId = msgId,
                msgBody = decrypted_bytes[2..],
            };

            return packet;
        }

        // used for parsing ike requests in ps (or any request ig) client -> server
        public IKEReq ParseIkeRequest(byte[] encryptedRawPayload)
        {
            bool flag = true; // original this flag is: flag = msg.msgId == 1, so true

            BinaryReader reader = new BinaryReader(new MemoryStream(encryptedRawPayload));

            byte[] nonceBytes = new byte[12]; // nonce 12 bytes length
            reader.Read(nonceBytes);

            byte[] aeadBytes = new byte[1]; // aead byte is 1 byte, original field in AeadTool
            int packetSize = encryptedRawPayload.Length - nonceBytes.Length; // skip nonce length (12)

            if (flag)
            {
                reader.Read(aeadBytes); // read to skip in memory, idk whats the use for this
                packetSize--; // skip in payload
            }

            Console.WriteLine("aeadbyte: " + aeadBytes[0]);

            byte[] packetBytes = new byte[packetSize];
            reader.Read(packetBytes);

            if (reader.BaseStream.Position != encryptedRawPayload.Length)
            {
                Log.Error("something went wrong, not all the bytes were read");
                Log.Error("reader pos: " + reader.BaseStream.Position);
                Log.Error("original len:" + encryptedRawPayload.Length);
            }

            Span<byte> decrypt_result = new Span<byte>(new byte[packetSize - 16]); // for chacha20 decrypt, the result is 16 bytes less than the input data

            Span<byte> nonce = nonceBytes.AsSpan();
            Span<byte> packet_data = packetBytes.AsSpan();

            byte[] associateData = new byte[13]; // this is needed for req decrypt (size: nonce(12) + 1)

            nonceBytes.CopyTo(associateData, 0); // associateData: [nonce, 1], 1 means AesGcm not supported

            if (AeadTool.clientType == ClientType.PC)
            {
                associateData[associateData.Length - 1] = 0;
            }

            else
            {
                associateData[associateData.Length - 1] = 1;
            }

            bool success = AeadTool.Decrypt(decrypt_result, AeadTool.IKE_KEY, nonce, packet_data, associateData);

            if (!success)
            {
                Log.Error("something went wrong when chacha20 decrypting the data");
            }

            byte[] decrypted_bytes = decrypt_result.ToArray();
            byte[] msgid_bytes = decrypted_bytes[..2]; // first two bytes is msgid
            Array.Reverse<byte>(msgid_bytes); // should check BitConverter.IsLittleEndian (if true -> reverse, was true on my pc)

            short msgId = BitConverter.ToInt16(msgid_bytes);

            Packet packet = new Packet()
            {
                msgId = msgId,
                msgBody = decrypted_bytes[2..],
            };

            Utils.PrintByteArray(decrypted_bytes);

            IKEReq ike_request = IKEReq.Parser.ParseFrom(packet.msgBody);

            return ike_request;
        }

        // used to parse offical pcaps, server -> client
        public IKEResp ParseIkeResponse(byte[] encryptedRawPayload)
        {
            bool flag = false; // false for this pcap ike resp

            BinaryReader reader = new BinaryReader(new MemoryStream(encryptedRawPayload));

            byte[] nonceBytes = new byte[12]; // nonce 12 bytes length
            reader.Read(nonceBytes);

            byte[] aeadBytes = new byte[1]; // aead byte is 1 byte, original field in AeadTool
            int packetSize = encryptedRawPayload.Length - nonceBytes.Length; // skip nonce length (12)

            if (flag)
            {
                reader.Read(aeadBytes); // read to skip in memory, idk whats the use for this
                packetSize--; // skip in payload
            }

            byte[] packetBytes = new byte[packetSize];
            reader.Read(packetBytes);

            if (reader.BaseStream.Position != encryptedRawPayload.Length)
            {
                Log.Error("something went wrong, not all the bytes were read");
                Log.Error("reader pos: " + reader.BaseStream.Position);
                Log.Error("original len:" + encryptedRawPayload.Length);
            }

            Span<byte> decrypt_result = new Span<byte>(new byte[packetSize - 16]); // for chacha20, the result is 16 bytes less than the input data

            Span<byte> nonce = nonceBytes.AsSpan();
            Span<byte> packet_data = packetBytes.AsSpan();

            bool success = AeadTool.Decrypt(decrypt_result, AeadTool.IKE_KEY, nonce, packet_data, null); // associateData NULL FOR THIS response pcap 

            if (!success)
            {
                Log.Error("something went wrong when chacha20 decrypting the data");
            }


            byte[] decrypted_bytes = decrypt_result.ToArray();
            byte[] msgid_bytes = decrypted_bytes[..2]; // first two bytes is msgid
            Array.Reverse<byte>(msgid_bytes); // should check BitConverter.IsLittleEndian (if true -> reverse, was true on my pc)

            short msgId = BitConverter.ToInt16(msgid_bytes);

            Packet packet = new Packet()
            {
                msgId = msgId,
                msgBody = decrypted_bytes[2..],
            };

            IKEResp ike_response = IKEResp.Parser.ParseFrom(packet.msgBody);

            return ike_response;
        }
    }
}
