using System;
using System.Security.Cryptography;
using System.Text;
using Mono.Math;
using Mono.Security.Cryptography;
using NSec.Cryptography;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Novaria.Common.Crypto
{
    public static class AeadTool
    {
        public static ClientType clientType = ClientType.PC; // PC uses AESGCM while Android uses chacha20 (Aes256Gcm not supported on android ig)
                                                             // for now set this field when switching, TODO: implement auto client detection from sdk auth request

        public static AeadTool.AeadEncryptHandler Encrypt;
        public static AeadTool.AeadDecryptHandler Decrypt;

        private static readonly byte[] associatedData = new byte[13];
        public static readonly int NonceSize = 12;
        public static readonly int MacSize = 128;
        public static readonly int KeySize = 32;
        public static readonly int IVSize = 16;

        public static byte[] PS_REQUEST_NONCE = new byte[12]; // hardcoded nonce, probably makes it easier for server idk? could also just randomly generate but me lazy
        //public static byte[] PS_PUBLIC_KEY { get => DiffieHellman.Instance.ServerPublicKey.GetBytes(); }
        public static string TOKEN = "seggs"; // hardcoded for now

        //public static readonly byte[] DEFAULT_SERVERLIST_KEY = new byte[] { 74, 72, 42, 67, 80, 51, 50, 57, 89, 120, 111, 103, 81, 74, 69, 120 };
        public static readonly byte[] DEFAULT_SERVERLIST_IV = new byte[] { 225, 92, 61, 72, 193, 89, 3, 64, 50, 61, 50, 145, 59, 128, 99, 72 };
        public static readonly byte[] DEFAULT_AND_IV = new byte[] { 105, 7, 110, 72, 167, 117, 102, 212, 150, 44, 52, 229, 65, 61, 204, 205 };
        public static readonly byte[] DEFAULT_WIN_IV = new byte[] { 144, 129, 81, 233, 8, 4, 33, 39, 106, 181, 229, 64, 68, 134, 31, 107 };

        public static readonly byte[] DEFAULT_SERVERLIST_KEY = Encoding.ASCII.GetBytes("qPmz8@Pd&F!N38Xm");

        public static readonly byte[] IKE_KEY = Encoding.ASCII.GetBytes("D&4r2fnroZgBaRmueWyW%A4KL!!@Arvs");

        private static DiffieHellmanManaged SendKey;
        private static BigInteger p = BigInteger.Parse("1552518092300708935130918131258481755631334049434514313202351194902966239949102107258669453876591642442910007680288864229150803718918046342632727613031282983744380820890196288509170691316593175367469551763119843371637221007210577919");
        private static GcmBlockCipher cipher;
        private static AesEngine engine;

        public delegate void AeadEncryptHandler(Span<byte> result, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> data, bool needAssociatedData);
        public delegate bool AeadDecryptHandler(Span<byte> result, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> data, Span<byte> associated);

        public static ReadOnlySpan<byte> AEADMARK
        {
            get
            {
                return AeadTool.associatedData.AsSpan(12, 1);
            }
        }

        public static byte[] key3 { get; set; }

        static AeadTool()
        {
            PS_REQUEST_NONCE[0] = 42;
            PS_REQUEST_NONCE[11] = 42;

            InitAeadTool();
            InitDH();
        }

        private static int InitDH()
        {
            int nRandom = 1;
            byte[] bytes = BitConverter.GetBytes(2);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse<byte>(bytes);
            }
            byte[] bytes2 = BitConverter.GetBytes(nRandom);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse<byte>(bytes2);
            }

            SendKey = new DiffieHellmanManaged(p.GetBytes(), bytes, bytes2);
            return nRandom;
        }

        public static void InitAeadTool()
        {
            if (clientType == ClientType.PC)
            {
                AeadTool.InitBouncyCastle();
                AeadTool.Encrypt = new AeadTool.AeadEncryptHandler(AeadTool.Encrypt_AESGCM_BouncyCastle);
                AeadTool.Decrypt = new AeadTool.AeadDecryptHandler(AeadTool.Dencrypt_AESGCM_BouncyCastle);

                AeadTool.associatedData[AeadTool.associatedData.Length - 1] = 2;
                Console.WriteLine("Select PC client, now using AESGCM");

                return;
            }
            AeadTool.Encrypt = new AeadTool.AeadEncryptHandler(AeadTool.Encrypt_ChaCha20);
            AeadTool.Decrypt = new AeadTool.AeadDecryptHandler(AeadTool.Dencrypt_ChaCha20);
            AeadTool.associatedData[AeadTool.associatedData.Length - 1] = 1;
            Console.WriteLine("Select Mobile client, now using ChaCha20");
        }

        public static void InitBouncyCastle()
        {
            AeadTool.engine = new AesEngine();
            AeadTool.cipher = new GcmBlockCipher(AeadTool.engine);
        }

        public static void SetAeadKey(byte[] pubKey)
        {
            byte[] array = SendKey.DecryptKeyExchange(pubKey);

            int num = array.Length;
            if (num > 32)
            {
                num = 32;
            }
            key3 = new byte[32];
            Buffer.BlockCopy(array, 0, key3, 0, num);
        }

        public static byte[] GetPubKey()
        {
            return new BigInteger(SendKey.CreateKeyExchange()).GetBytes();
        }

        private static void Encrypt_AESGCM_BouncyCastle(Span<byte> result, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> data, bool needAssociatedData)
        {
            if (!needAssociatedData)
            {
                byte[] array = AeadTool.Encrypt_BouncyCastle(key, nonce, data, null);
                result.Clear();
                MemoryExtensions.CopyTo<byte>(array, result);
                return;
            }
            nonce.CopyTo(MemoryExtensions.AsSpan<byte>(AeadTool.associatedData, 0, 12));
            Span<byte> associated = MemoryExtensions.AsSpan<byte>(AeadTool.associatedData);
            byte[] array2 = AeadTool.Encrypt_BouncyCastle(key, nonce, data, associated);
            result.Clear();
            MemoryExtensions.CopyTo<byte>(array2, result);
        }

        private static bool Dencrypt_AESGCM_BouncyCastle(Span<byte> result, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> data, Span<byte> associated)
        {
            bool result2;
            try
            {
                byte[] array = AeadTool.Decrypt_BouncyCastle(key.ToArray(), nonce, data.ToArray(), associated);
                result.Clear();
                MemoryExtensions.CopyTo<byte>(array, result);
                result2 = true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result2 = false;
            }
            return result2;
        }

        private static void Encrypt_ChaCha20(Span<byte> result, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> data, bool needAssociatedData)
        {
            Algorithm chaCha20Poly = AeadAlgorithm.ChaCha20Poly1305;
            KeyBlobFormat keyBlobFormat = KeyBlobFormat.RawSymmetricKey;
            KeyCreationParameters keyCreationParameters = default(KeyCreationParameters);
            using (Key key2 = Key.Import(chaCha20Poly, key, keyBlobFormat, ref keyCreationParameters))
            {
                if (!needAssociatedData)
                {
                    AeadAlgorithm.ChaCha20Poly1305.Encrypt(key2, nonce, null, data, result);
                } else
                {
                    nonce.CopyTo(AeadTool.associatedData.AsSpan(0, 12));
                    AeadAlgorithm.ChaCha20Poly1305.Encrypt(key2, nonce, AeadTool.associatedData.AsSpan<byte>(), data, result);
                }
            }
        }

        private static bool Dencrypt_ChaCha20(Span<byte> result, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> data, Span<byte> associatedData)
        {
            Algorithm chaCha20Poly = AeadAlgorithm.ChaCha20Poly1305;
            KeyBlobFormat keyBlobFormat = KeyBlobFormat.RawSymmetricKey;
            KeyCreationParameters keyCreationParameters = default(KeyCreationParameters);
            bool result2;

            using (Key key2 = Key.Import(chaCha20Poly, key, keyBlobFormat, ref keyCreationParameters))
            {
                result2 = AeadAlgorithm.ChaCha20Poly1305.Decrypt(key2, nonce, associatedData, data, result);
            }

            return result2;
        }

        // probably wrong
        public static byte[] DecryptAesCBCInfo(byte[] key, byte[] IV, byte[] cipherBytes)
        {
            PaddedBufferedBlockCipher paddedBufferedBlockCipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            ParametersWithIV parametersWithIV = new ParametersWithIV(new KeyParameter(key), IV);
            paddedBufferedBlockCipher.Init(false, parametersWithIV);
            byte[] array = new byte[paddedBufferedBlockCipher.GetOutputSize(cipherBytes.Length)];
            int num = paddedBufferedBlockCipher.ProcessBytes(cipherBytes, array, 0);
            paddedBufferedBlockCipher.DoFinal(array, num);
            return array;
        }

        public static byte[] EncryptAesCBCInfo(byte[] key, byte[] IV, byte[] plainBytes)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }
        }

        private static byte[] Encrypt_BouncyCastle(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> secretMessage, Span<byte> associated)
        {
            if (key == null || key.Length != AeadTool.KeySize)
            {
                throw new ArgumentException(string.Format("Key needs to be {0} bit!", AeadTool.KeySize), "key");
            }
            if (secretMessage == null || secretMessage.Length == 0)
            {
                throw new ArgumentException("Secret Message Required!", "secretMessage");
            }
            if (AeadTool.cipher == null)
            {
                Console.WriteLine("Must Init:BouncyCastleAesGcm");
                return null;
            }
            AeadParameters aeadParameters = new AeadParameters(new KeyParameter(key.ToArray()), AeadTool.MacSize, nonce.ToArray(), associated.ToArray());
            AeadTool.cipher.Init(true, aeadParameters);
            byte[] array = new byte[AeadTool.cipher.GetOutputSize(secretMessage.Length)];
            int num = AeadTool.cipher.ProcessBytes(secretMessage.ToArray(), 0, secretMessage.Length, array, 0);
            AeadTool.cipher.DoFinal(array, num);
            return array;
        }

        private static byte[] Decrypt_BouncyCastle(byte[] key, ReadOnlySpan<byte> nonce, byte[] cipherText, Span<byte> associated)
        {
            if (cipherText == null || cipherText.Length == 0)
            {
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
            }
            if (AeadTool.cipher == null)
            {
                throw new ArgumentException("BouncyCastleAesGcm must be initialized first");
            }
            AeadParameters aeadParameters = new AeadParameters(new KeyParameter(key), AeadTool.MacSize, nonce.ToArray(), associated.ToArray());
            AeadTool.cipher.Init(false, aeadParameters);
            byte[] array = new byte[AeadTool.cipher.GetOutputSize(cipherText.Length)];
            int num = AeadTool.cipher.ProcessBytes(cipherText, 0, cipherText.Length, array, 0);
            AeadTool.cipher.DoFinal(array, num);
            return array;
        }
    }

    public enum ClientType
    {
        PC,
        Mobile,
    }

}