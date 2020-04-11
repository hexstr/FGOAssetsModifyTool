using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;

public class CatAndMouseGame
{
    static string salt = "pN6ds2Bg";
    public static string GetMD5String(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input + salt);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

    private static byte[] BattleIV = new byte[32];
    private static byte[] BattleKey = new byte[32];
    public static void CN()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("d3b13d9093cc6b457fd89766bafa1626ee2ef76626d49ce0d424f4156231ce56");
        byte[] bytes2 = Encoding.UTF8.GetBytes("5ec7ce0fddc50bca9f82b8338b9135c69e0e9e169648df69054dcb96553598e6");
        for (int i = 0; i < bytes2.Length; i++)
        {
            if (i % 2 == 0)
            {
                CatAndMouseGame.baseData[i / 2] = bytes2[i];
            }
            else
            {
                CatAndMouseGame.baseTop[i / 2] = bytes2[i];
            }
        }
        for (int j = 0; j < bytes.Length / 4; j++)
        {
            if (j % 2 == 0)
            {
                CatAndMouseGame.stageData[j / 2 * 4] = bytes[j * 4];
                CatAndMouseGame.stageData[j / 2 * 4 + 1] = bytes[j * 4 + 1];
                CatAndMouseGame.stageData[j / 2 * 4 + 2] = bytes[j * 4 + 2];
                CatAndMouseGame.stageData[j / 2 * 4 + 3] = bytes[j * 4 + 3];
            }
            else
            {
                CatAndMouseGame.stageTop[j / 2 * 4] = bytes[j * 4];
                CatAndMouseGame.stageTop[j / 2 * 4 + 1] = bytes[j * 4 + 1];
                CatAndMouseGame.stageTop[j / 2 * 4 + 2] = bytes[j * 4 + 2];
                CatAndMouseGame.stageTop[j / 2 * 4 + 3] = bytes[j * 4 + 3];
            }
        }
    }
    public static void EN()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("xaVPXPtrkXlUZsJRa3Eu1o1kSDYtjlwhoRQI2MHq2Q4szmpVvDcbmpi7UIZF9Rle");
        byte[] bytes2 = Encoding.UTF8.GetBytes("FEq45VzsnHv8ynuLIGGF9qRA2tJ6vJ61FkG6KliUnD77cN7pvveVAH5gcPeLEzOR");
        for (int i = 0; i < bytes2.Length / 4; i++)
        {
            if (i % 2 == 0)
            {
                CatAndMouseGame.baseData[i / 2 * 4] = bytes2[i * 4];
                CatAndMouseGame.baseData[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
                CatAndMouseGame.baseData[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
                CatAndMouseGame.baseData[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
            }
            else
            {
                CatAndMouseGame.baseTop[i / 2 * 4] = bytes2[i * 4];
                CatAndMouseGame.baseTop[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
                CatAndMouseGame.baseTop[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
                CatAndMouseGame.baseTop[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
            }
        }
        for (int i = 0; i < bytes.Length; i++)
        {
            if (i % 2 == 0)
            {
                CatAndMouseGame.stageData[i / 2] = bytes[i];
            }
            else
            {
                CatAndMouseGame.stageTop[i / 2] = bytes[i];
            }
        }
    }
    public static string getShaName(string name)
    {
        SHA1 sha = new SHA1CryptoServiceProvider();
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        byte[] bytes = utf8Encoding.GetBytes(name);
        byte[] array = sha.ComputeHash(bytes);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in array)
        {
            stringBuilder.AppendFormat("{0,0:x2}", (int)(b ^ 170));
        }
        stringBuilder.Append(".bin");
        return stringBuilder.ToString();
    }
    static CatAndMouseGame()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("kzdMtpmzqCHAfx00saU1gIhTjYCuOD1JstqtisXsGYqRVcqrHRydj3k6vJCySu3g");
        byte[] bytes2 = Encoding.UTF8.GetBytes("PFBs0eIuunoxKkCcLbqDVerU1rShhS276SAL3A8tFLUfGvtz3F3FFeKELIk3Nvi4");
        for (int i = 0; i < bytes2.Length / 4; i++)
        {
            if (i % 2 == 0)
            {
                CatAndMouseGame.baseData[i / 2 * 4] = bytes2[i * 4];
                CatAndMouseGame.baseData[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
                CatAndMouseGame.baseData[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
                CatAndMouseGame.baseData[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
            }
            else
            {
                CatAndMouseGame.baseTop[i / 2 * 4] = bytes2[i * 4];
                CatAndMouseGame.baseTop[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
                CatAndMouseGame.baseTop[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
                CatAndMouseGame.baseTop[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
            }
        }
        for (int i = 0; i < bytes.Length; i++)
        {
            if (i % 2 == 0)
            {
                CatAndMouseGame.stageData[i / 2] = bytes[i];
            }
            else
            {
                CatAndMouseGame.stageTop[i / 2] = bytes[i];
            }
        }
    }
    public static string CatGame3(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)~bytes[i];
        }
        return CatAndMouseGame.CatHome(bytes, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
    }
    public static string MouseGame3(string str)
    {
        byte[] data = Convert.FromBase64String(str);
        byte[] array = CatAndMouseGame.MouseHomeMain(data, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
        if (array == null)
        {
            return null;
        }
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)~array[i];
        }
        return Encoding.UTF8.GetString(array).TrimEnd(new char[1]);
    }
    public static byte[] CatGame4(byte[] data)
    {
        for (int i = 0; i < data.Length; i += 2)
        {
            if (i + 1 >= data.Length)
            {
                break;
            }
            byte b = data[i];
            byte b2 = data[i + 1];
            data[i] = (byte)(b2 ^ 206);
            data[i + 1] = (byte)(b ^ 210);
        }
        return CatAndMouseGame.CatHomeMain(data, CatAndMouseGame.baseData, CatAndMouseGame.baseTop, false);
    }
    public static byte[] MouseGame4(byte[] data)
    {
        byte[] array = CatAndMouseGame.MouseHomeMain(data, CatAndMouseGame.baseData, CatAndMouseGame.baseTop, false);
        if (array == null)
        {
            Console.WriteLine("MouseHomeMain failed");
            return null;
        }

        for (int i = 0; i < array.Length; i += 2)
        {
            if (i + 1 >= array.Length)
            {
                break;
            }
            byte b = array[i];
            byte b2 = array[i + 1];
            array[i] = (byte)(b2 ^ 210);
            array[i + 1] = (byte)(b ^ 206);
        }
        return array;
    }
    public static string CatGame8(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)~bytes[i];
        }
        return CatAndMouseGame.CatHomeZ2(bytes, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
    }
    public static string MouseGame8(string str)
    {
        byte[] data = Convert.FromBase64String(str);
        byte[] array = CatAndMouseGame.MouseHomeMainZ2(data, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
        if (array == null)
        {
            array = CatAndMouseGame.MouseHomeMain(data, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
            if (array == null)
            {
                return null;
            }
        }
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)~array[i];
        }
        return Encoding.UTF8.GetString(array).TrimEnd(new char[1]);
    }
    public static byte[] CatHomeMain(byte[] data, byte[] home, byte[] info, bool isCompress = false)
    {
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;
        byte[] result;
        try
        {
            ICryptoTransform transform = new RijndaelManaged
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256
            }.CreateEncryptor(home, info);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            if (isCompress)
            {
                BZip2OutputStream bzip2OutputStream = new BZip2OutputStream(cryptoStream, 1);
                bzip2OutputStream.Write(data, 0, data.Length);
                bzip2OutputStream.Close();
            }
            else
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
            }
            result = memoryStream.ToArray();
        }
        catch (Exception)
        {
            result = null;
        }
        finally
        {
            if (memoryStream != null)
            {
                memoryStream.Close();
            }
            if (cryptoStream != null)
            {
                cryptoStream.Close();
            }
        }
        return result;
    }
    public static byte[] CatHomeMainZ2(byte[] data, byte[] home, byte[] info, bool isCompress = false)
    {
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;
        byte[] result;
        try
        {
            ICryptoTransform cryptoTransform = new RijndaelManaged
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256
            }.CreateEncryptor(home, info);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
            if (isCompress)
            {
                GZipOutputStream gzipOutputStream = new GZipOutputStream(cryptoStream);
                gzipOutputStream.Write(data, 0, data.Length);
                gzipOutputStream.Close();
            }
            else
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
            }
            result = memoryStream.ToArray();
        }
        catch (Exception)
        {
            result = null;
        }
        finally
        {
            if (memoryStream != null)
            {
                memoryStream.Close();
            }
            if (cryptoStream != null)
            {
                cryptoStream.Close();
            }
        }
        return result;
    }
    public static byte[] MouseHomeMainZ2(byte[] data, byte[] home, byte[] info, bool isCompress = false)
    {
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;
        byte[] result;
        try
        {
            ICryptoTransform transform = new RijndaelManaged
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256
            }.CreateDecryptor(home, info);
            byte[] array = new byte[data.Length];
            memoryStream = new MemoryStream(data);
            cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            cryptoStream.Read(array, 0, array.Length);
            if (isCompress)
            {
                if (array[0] == 66 && array[1] == 90)
                {
                    return null;
                }
                MemoryStream memoryStream2 = new MemoryStream();
                MemoryStream memoryStream3 = new MemoryStream(array);
                GZipInputStream gzipInputStream = new GZipInputStream(memoryStream3);
                byte[] array2 = new byte[16384];
                int count;
                while ((count = gzipInputStream.Read(array2, 0, array2.Length)) > 0)
                {
                    memoryStream2.Write(array2, 0, count);
                }
                gzipInputStream.Close();
                array = memoryStream2.ToArray();
                memoryStream3.Close();
                memoryStream2.Close();
            }
            result = array;
        }
        catch (Exception)
        {
            result = null;
        }
        finally
        {
            if (memoryStream != null)
            {
                memoryStream.Close();
            }
            if (cryptoStream != null)
            {
                cryptoStream.Close();
            }
        }
        return result;
    }
    public static byte[] MouseHomeMain(byte[] data, byte[] home, byte[] info, bool isCompress = false)
    {
        byte[] result;
        try
        {
            using (ICryptoTransform cryptoTransform = new RijndaelManaged
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256
            }.CreateDecryptor(home, info))
            {
                using (CatAndMouseGame.DataDecryptor dataDecryptor = new CatAndMouseGame.DataDecryptor(cryptoTransform, data, isCompress))
                {
                    dataDecryptor.ApplyWrite();
                    result = dataDecryptor.ToByteArray();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            result = null;
        }
        return result;
    }
    public static string CatHome(byte[] data, byte[] home, byte[] info, bool isCompress = false)
    {
        byte[] array = CatAndMouseGame.CatHomeMain(data, home, info, isCompress);
        if (array != null)
        {
            return Convert.ToBase64String(array);
        }
        return null;
    }
    public static string CatHomeZ2(byte[] data, byte[] home, byte[] info, bool isCompress = false)
    {
        byte[] array = CatAndMouseGame.CatHomeMainZ2(data, home, info, isCompress);
        if (array != null)
        {
            return Convert.ToBase64String(array);
        }
        return null;
    }
    protected static byte[] stageTop = new byte[32];

    protected static byte[] stageData = new byte[32];

    protected static byte[] baseTop = new byte[32];

    protected static byte[] baseData = new byte[32];

    private class DataDecryptor : IDisposable
    {
        public DataDecryptor(ICryptoTransform decryptor, byte[] data, bool isCompress)
        {
            this.data = data;
            this.isCompress = isCompress;
            this.memoryStream = new MemoryStream(data.Length);
            this.cryptoStream = new CryptoStream(this.memoryStream, decryptor, CryptoStreamMode.Write);
        }

        public void ApplyWrite()
        {
            this.cryptoStream.Write(this.data, 0, this.data.Length);
            this.cryptoStream.FlushFinalBlock();
            if (this.isCompress)
            {
                this.memoryStream.Seek(0L, SeekOrigin.Begin);
                this.memoryStreamBZip = new MemoryStream();
                this.bzipStream = new BZip2InputStream(this.memoryStream);
                byte[] array = new byte[16384];
                int count;
                while ((count = this.bzipStream.Read(array, 0, array.Length)) > 0)
                {
                    this.memoryStreamBZip.Write(array, 0, count);
                }
            }
        }

        public byte[] ToByteArray()
        {
            if (!this.isCompress && this.memoryStream != null)
            {
                return ((long)this.memoryStream.Capacity != this.memoryStream.Length) ? this.memoryStream.ToArray() : this.memoryStream.GetBuffer();
            }
            if (this.memoryStreamBZip != null)
            {
                return ((long)this.memoryStreamBZip.Capacity != this.memoryStreamBZip.Length) ? this.memoryStreamBZip.ToArray() : this.memoryStreamBZip.GetBuffer();
            }
            Console.Write("memoryStream is null !");
            return (byte[])Enumerable.Empty<byte>();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (this.isDisposed)
            {
                return;
            }
            if (isDisposing)
            {
                if (this.memoryStream != null)
                {
                    this.memoryStream.Dispose();
                    this.memoryStream = null;
                }
                if (this.cryptoStream != null)
                {
                    this.cryptoStream.Dispose();
                    this.cryptoStream = null;
                }
                if (this.memoryStreamBZip != null)
                {
                    this.memoryStreamBZip.Dispose();
                    this.memoryStreamBZip = null;
                }
                if (this.bzipStream != null)
                {
                    this.bzipStream.Dispose();
                    this.bzipStream = null;
                }
            }
            this.isDisposed = true;
        }

        private readonly byte[] data;

        private readonly bool isCompress;

        private MemoryStream memoryStream;

        private CryptoStream cryptoStream;

        private MemoryStream memoryStreamBZip;

        private BZip2InputStream bzipStream;

        private bool isDisposed;
    }
}