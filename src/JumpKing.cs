using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Linq;
using JumpKing.SaveThread;

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace JumpKing
{
    sealed class TypeConverter : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName;
            typeName = serializedType.FullName.Replace(
                        "mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                        "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
        }
    }

    public class JumpKing
    {
        const string ENCRYPTION_KEY = "5pH3MA7gB9drJ8VJ";
        static byte[] ivKey = new byte[16];
        static CombinedSaveFile save;

        static Tuple<MemoryStream, CryptoStream> Encrypt(bool randomIV = false)
        {
            var rmCrypto = new RijndaelManaged();
            rmCrypto.Padding = PaddingMode.PKCS7;

            if(randomIV)
            {
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetNonZeroBytes(ivKey);
                }
            }

            var stream = new MemoryStream();
            stream.Write(ivKey, 0, ivKey.Length);
            CryptoStream cryptoStream = new CryptoStream(
                stream,
                rmCrypto.CreateEncryptor(Convert.FromBase64String(ENCRYPTION_KEY), ivKey),
                CryptoStreamMode.Write);

            return new Tuple<MemoryStream, CryptoStream>(stream, cryptoStream);
        }

        static CryptoStream Decrypt(MemoryStream stream)
        {
            var rmCrypto = new RijndaelManaged();
            rmCrypto.Padding = PaddingMode.PKCS7;

            if (stream.Read(ivKey, 0, ivKey.Length) != ivKey.Length)
            {
                throw new ApplicationException("Failed to read IV from stream");
            }

            CryptoStream cryptoStream = new CryptoStream(stream,
                rmCrypto.CreateDecryptor(Convert.FromBase64String(ENCRYPTION_KEY), ivKey),
                CryptoStreamMode.Read);

            return cryptoStream;
        }

        static WebAssembly.Core.Uint8Array MemoryStreamToUint8Array(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = stream.ToArray();
            WebAssembly.Core.Array array = new WebAssembly.Core.Array();
            for(int i = 0; i < buffer.Length; i++)
            {
                array.Push(buffer[i]);
            }
            stream.Close();

            WebAssembly.Core.Uint8Array uint8Array = new WebAssembly.Core.Uint8Array(array.Length);
            uint8Array.Set(array);

            return uint8Array;
        }

        static WebAssembly.Core.Uint8Array Save<T>(T data)
        {
            var streams = Encrypt();

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Binder = new TypeConverter();
            binaryFormatter.Serialize(streams.Item2, data);
            streams.Item2.FlushFinalBlock();
            
            return MemoryStreamToUint8Array(streams.Item1);
        }

        static T Load<T>(byte[] data)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                using(var cryptoStream = Decrypt(stream))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    T result = (T)(binaryFormatter.Deserialize(cryptoStream));

                    return result;
                }
            }
        }

        static WebAssembly.Core.Uint8Array SaveCombined()
        {
            return Save<CombinedSaveFile>(save);
        }

        static void LoadCombined(WebAssembly.Core.Uint8Array data)
        {
            save = Load<CombinedSaveFile>(data.ToArray());
        }

        static float GetX()
        {
            return save.player_position.position.X;
        }

        static float GetY()
        {
            return save.player_position.position.Y;
        }

        static float GetVelX()
        {
            return save.player_position.velocity.X;
        }

        static float GetVelY()
        {
            return save.player_position.velocity.Y;
        }

        static void SetPosition(float x, float y)
        {
            save.player_position.position = new SaveState.Vector2(x, y);
        }

        static void SetVelocity(float x, float y)
        {
            save.player_position.velocity = new SaveState.Vector2(x, y);
        }
    }
}
