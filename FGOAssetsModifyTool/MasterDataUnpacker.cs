using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.GZip;
namespace FGOAssetsModifyTool
{
    public class MasterDataUnpacker
    {
        public object Unpack(byte[] buf, int offset, int size)
        {
            object result;
            using (MemoryStream memoryStream = new MemoryStream(buf, offset, size))
            {
                result = this.Unpack(memoryStream);
            }
            return result;
        }
        public object Unpack(byte[] buf)
        {
            return this.Unpack(buf, 0, buf.Length);
        }
        public void UnpackByte(Stream s, Stream ws)
        {
            int num = s.ReadByte();
            ws.WriteByte((byte)num);
            if (num < 0)
            {
                throw new FormatException();
            }
            if (num > 127)
            {
                if (num <= 143)
                {
                    this.UnpackMapByte(num, s, ws);
                }
                else if (num <= 159)
                {
                    this.UnpackArrayByte(num, s, ws);
                }
                else if (num <= 191)
                {
                    this.UnpackBinary(num, s, ws);
                }
                else if (num >= 224)
                {
                }
            }
            switch (num)
            {
                case 196:
                case 197:
                case 198:
                case 217:
                case 218:
                case 219:
                    this.UnpackBinary(num, s, ws);
                    break;
                case 202:
                    s.Read(this.tmp0, 0, 4);
                    ws.Write(this.tmp0, 0, 4);
                    break;
                case 203:
                    s.Read(this.tmp0, 0, 8);
                    ws.Write(this.tmp0, 0, 8);
                    break;
                case 204:
                    ws.WriteByte((byte)s.ReadByte());
                    break;
                case 205:
                    s.Read(this.tmp0, 0, 2);
                    ws.Write(this.tmp0, 0, 2);
                    break;
                case 206:
                    s.Read(this.tmp0, 0, 4);
                    ws.Write(this.tmp0, 0, 4);
                    break;
                case 207:
                    if (s.Read(this.tmp0, 0, 8) != 8)
                    {
                        throw new FormatException();
                    }
                    ws.Write(this.tmp0, 0, 8);
                    break;
                case 208:
                    ws.WriteByte((byte)s.ReadByte());
                    break;
                case 209:
                    if (s.Read(this.tmp0, 0, 2) != 2)
                    {
                        throw new FormatException();
                    }
                    ws.Write(this.tmp0, 0, 2);
                    break;
                case 210:
                    if (s.Read(this.tmp0, 0, 4) != 4)
                    {
                        throw new FormatException();
                    }
                    ws.Write(this.tmp0, 0, 4);
                    break;
                case 211:
                    if (s.Read(this.tmp0, 0, 8) != 8)
                    {
                        throw new FormatException();
                    }
                    ws.Write(this.tmp0, 0, 8);
                    break;
                case 220:
                case 221:
                    this.UnpackArrayByte(num, s, ws);
                    break;
                case 222:
                case 223:
                    this.UnpackMapByte(num, s, ws);
                    break;
            }
        }
        public void UnpackBinary(int b, Stream s, Stream ws)
        {
            if (b <= 191)
            {
                int num = b & 31;
                if (num != 0)
                {
                    byte[] array = new byte[num];
                    s.Read(array, 0, num);
                    ws.Write(array, 0, num);
                }
            }
            else
            {
                switch (b)
                {
                    case 196:
                        break;
                    case 197:
                        goto IL_9D;
                    case 198:
                        goto IL_EE;
                    default:
                        switch (b)
                        {
                            case 217:
                                break;
                            case 218:
                                goto IL_9D;
                            case 219:
                                goto IL_EE;
                            default:
                                return;
                        }
                        break;
                }
                int num2 = s.ReadByte();
                byte[] array2 = new byte[num2];
                s.Read(array2, 0, num2);
                ws.WriteByte((byte)num2);
                ws.Write(array2, 0, num2);
                return;
            IL_9D:
                byte[] array3 = new byte[2];
                s.Read(array3, 0, 2);
                byte[] array4 = new byte[(long)((int)array3[0] << 8 | (int)array3[1])];
                s.Read(array4, 0, array4.Length);
                ws.Write(array3, 0, 2);
                ws.Write(array4, 0, array4.Length);
                return;
            IL_EE:
                byte[] array5 = new byte[4];
                s.Read(array5, 0, 4);
                byte[] array6 = new byte[(long)array5[0] << 24 | (long)array5[1] << 16 | (long)array5[2] << 8 | (long)array5[3]];
                s.Read(array6, 0, array6.Length);
                ws.Write(array5, 0, 4);
                ws.Write(array6, 0, array6.Length);
            }
        }
        public void UnpackArrayByte(int b, Stream s, Stream ws)
        {
            long num = 0L;
            if (b <= 159)
            {
                num = (long)(b & 15);
            }
            else if (b != 220)
            {
                if (b == 221)
                {
                    s.Read(this.tmp0, 0, 4);
                    num = ((long)this.tmp0[0] << 24 | (long)this.tmp0[1] << 16 | (long)this.tmp0[2] << 8 | (long)this.tmp0[3]);
                    ws.Write(this.tmp0, 0, 4);
                }
            }
            else
            {
                s.Read(this.tmp0, 0, 2);
                num = (long)((int)this.tmp0[0] << 8 | (int)this.tmp0[1]);
                ws.Write(this.tmp0, 0, 2);
            }
            int num2 = 0;
            while ((long)num2 < num)
            {
                this.UnpackByte(s, ws);
                num2++;
            }
        }
        public void UnpackMapByte(int b, Stream s, Stream ws)
        {
            long num = 0L;
            if (b <= 143)
            {
                num = (long)(b & 15);
            }
            else if (b != 222)
            {
                if (b == 223)
                {
                    s.Read(this.tmp0, 0, 4);
                    num = ((long)this.tmp0[0] << 24 | (long)this.tmp0[1] << 16 | (long)this.tmp0[2] << 8 | (long)this.tmp0[3]);
                    ws.Write(this.tmp0, 0, 4);
                }
            }
            else
            {
                s.Read(this.tmp0, 0, 2);
                num = (long)((int)this.tmp0[0] << 8 | (int)this.tmp0[1]);
                ws.Write(this.tmp0, 0, 2);
            }
            int num2 = 0;
            while ((long)num2 < num)
            {
                this.UnpackByte(s, ws);
                this.UnpackByte(s, ws);
                num2++;
            }
        }
        public object Unpack(Stream s)
        {
            int num = s.ReadByte();
            if (num < 0)
            {
                throw new FormatException();
            }
            if (num <= 127)
            {
                return (long)num;
            }
            if (num <= 143)
            {
                return this.UnpackMap(s, (long)(num & 15));
            }
            if (num <= 159)
            {
                return this.UnpackArray(s, (long)(num & 15));
            }
            if (num <= 191)
            {
                return this.UnpackString(s, (long)(num & 31));
            }
            if (num >= 224)
            {
                return (long)((sbyte)num);
            }
            switch (num)
            {
                case 192:
                    return null;
                case 194:
                    return false;
                case 195:
                    return true;
                case 196:
                    return this.UnpackBinary(s, (long)s.ReadByte());
                case 197:
                    return this.UnpackBinary(s, this.UnpackUint16(s));
                case 198:
                    return this.UnpackBinary(s, this.UnpackUint32(s));
                case 202:
                    s.Read(this.tmp0, 0, 4);
                    if (BitConverter.IsLittleEndian)
                    {
                        this.tmp1[0] = this.tmp0[3];
                        this.tmp1[1] = this.tmp0[2];
                        this.tmp1[2] = this.tmp0[1];
                        this.tmp1[3] = this.tmp0[0];
                        return (double)BitConverter.ToSingle(this.tmp1, 0);
                    }
                    return (double)BitConverter.ToSingle(this.tmp0, 0);
                case 203:
                    s.Read(this.tmp0, 0, 8);
                    if (BitConverter.IsLittleEndian)
                    {
                        this.tmp1[0] = this.tmp0[7];
                        this.tmp1[1] = this.tmp0[6];
                        this.tmp1[2] = this.tmp0[5];
                        this.tmp1[3] = this.tmp0[4];
                        this.tmp1[4] = this.tmp0[3];
                        this.tmp1[5] = this.tmp0[2];
                        this.tmp1[6] = this.tmp0[1];
                        this.tmp1[7] = this.tmp0[0];
                        return BitConverter.ToDouble(this.tmp1, 0);
                    }
                    return BitConverter.ToDouble(this.tmp0, 0);
                case 204:
                    return (long)s.ReadByte();
                case 205:
                    return this.UnpackUint16(s);
                case 206:
                    return this.UnpackUint32(s);
                case 207:
                    if (s.Read(this.tmp0, 0, 8) != 8)
                    {
                        throw new FormatException();
                    }
                    return (long)this.tmp0[0] << 56 | (long)this.tmp0[1] << 48 | (long)this.tmp0[2] << 40 | ((long)this.tmp0[3] << 32) + ((long)this.tmp0[4] << 24) | (long)this.tmp0[5] << 16 | (long)this.tmp0[6] << 8 | (long)this.tmp0[7];
                case 208:
                    return (long)((sbyte)s.ReadByte());
                case 209:
                    if (s.Read(this.tmp0, 0, 2) != 2)
                    {
                        throw new FormatException();
                    }
                    return (long)((sbyte)this.tmp0[0]) << 8 | (long)this.tmp0[1];
                case 210:
                    if (s.Read(this.tmp0, 0, 4) != 4)
                    {
                        throw new FormatException();
                    }
                    return (long)((sbyte)this.tmp0[0]) << 24 | (long)this.tmp0[1] << 16 | (long)this.tmp0[2] << 8 | (long)this.tmp0[3];
                case 211:
                    if (s.Read(this.tmp0, 0, 8) != 8)
                    {
                        throw new FormatException();
                    }
                    return (long)((sbyte)this.tmp0[0]) << 56 | (long)this.tmp0[1] << 48 | (long)this.tmp0[2] << 40 | ((long)this.tmp0[3] << 32) + ((long)this.tmp0[4] << 24) | (long)this.tmp0[5] << 16 | (long)this.tmp0[6] << 8 | (long)this.tmp0[7];
                case 217:
                    return this.UnpackString(s, (long)s.ReadByte());
                case 218:
                    return this.UnpackString(s, this.UnpackUint16(s));
                case 219:
                    return this.UnpackString(s, this.UnpackUint32(s));
                case 220:
                    return this.UnpackArray(s, this.UnpackUint16(s));
                case 221:
                    return this.UnpackArray(s, this.UnpackUint32(s));
                case 222:
                    return this.UnpackMap(s, this.UnpackUint16(s));
                case 223:
                    return this.UnpackMap(s, this.UnpackUint32(s));
            }
            return null;
        }
        private long UnpackUint16(Stream s)
        {
            if (s.Read(this.tmp0, 0, 2) != 2)
            {
                throw new FormatException();
            }
            return (long)((int)this.tmp0[0] << 8 | (int)this.tmp0[1]);
        }
        private long UnpackUint32(Stream s)
        {
            if (s.Read(this.tmp0, 0, 4) != 4)
            {
                throw new FormatException();
            }
            return (long)this.tmp0[0] << 24 | (long)this.tmp0[1] << 16 | (long)this.tmp0[2] << 8 | (long)this.tmp0[3];
        }
        private string UnpackString(Stream s, long len)
        {
            if (MasterDataUnpacker.sb == null)
            {
                MasterDataUnpacker.sb = new StringBuilder((int)len);
            }
            else
            {
                MasterDataUnpacker.sb.Length = 0;
                MasterDataUnpacker.sb.EnsureCapacity((int)len);
            }
            uint num = 0u;
            uint num2 = 0u;
            uint num3 = 0u;
            int num4 = 0;
            while ((long)num4 < len)
            {
                uint num5 = (uint)s.ReadByte();
                if (num2 == 0u)
                {
                    if (num5 < 128u)
                    {
                        MasterDataUnpacker.sb.Append((char)num5);
                    }
                    else if ((num5 & 224u) == 192u)
                    {
                        num = (num5 & 31u);
                        num3 = 1u;
                        num2 = 2u;
                    }
                    else if ((num5 & 240u) == 224u)
                    {
                        num = (num5 & 15u);
                        num3 = 1u;
                        num2 = 3u;
                    }
                    else if ((num5 & 248u) == 240u)
                    {
                        num = (num5 & 7u);
                        num3 = 1u;
                        num2 = 4u;
                    }
                    else if ((num5 & 252u) == 248u)
                    {
                        num = (num5 & 3u);
                        num3 = 1u;
                        num2 = 5u;
                    }
                    else if ((num5 & 254u) == 252u)
                    {
                        num = (num5 & 3u);
                        num3 = 1u;
                        num2 = 6u;
                    }
                }
                else if ((num5 & 192u) == 128u)
                {
                    num = (num << 6 | (num5 & 63u));
                    if ((num3 += 1u) >= num2)
                    {
                        if (num < 65536u)
                        {
                            MasterDataUnpacker.sb.Append((char)num);
                        }
                        else if (num < 1114112u)
                        {
                            num -= 65536u;
                            MasterDataUnpacker.sb.Append((char)((num >> 10) + 55296u));
                            MasterDataUnpacker.sb.Append((char)((num & 1023u) + 56320u));
                        }
                        num2 = 0u;
                    }
                }
                num4++;
            }
            return MasterDataUnpacker.sb.ToString();
        }
        private byte[] UnpackBinary(Stream s, long len)
        {
            byte[] array = new byte[len];
            s.Read(array, 0, (int)len);
            return array;
        }
        private List<object> UnpackArray(Stream s, long len)
        {
            List<object> list = new List<object>((int)len);
            for (long num = 0L; num < len; num += 1L)
            {
                list.Add(this.Unpack(s));
            }
            return list;
        }
        private Dictionary<string, byte[]> UnpackMap(Stream s, long len)
        {
            Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>((int)len);
            for (long num = 0L; num < len; num += 1L)
            {
                string text = this.Unpack(s) as string;
                this.writeStream.Position = 0L;
                this.writeStream.SetLength(0L);
                this.UnpackByte(s, this.writeStream);
                if (text != null)
                {
                    dictionary.Add(text, this.writeStream.ToArray());
                }
            }
            return dictionary;
        }
        private static StringBuilder sb;
        private byte[] tmp0 = new byte[8];
        private byte[] tmp1 = new byte[8];
        private readonly MemoryStream writeStream = new MemoryStream(2000000);

        protected static byte[] ownerTop = new byte[32];
        protected static byte[] ownerData = new byte[32];
        protected static byte[] InfoTop = new byte[32];
        protected static byte[] infoData = new byte[32];
        public static byte[] MouseHomeSub(byte[] data, byte[] home, byte[] info, bool isCompress = false)
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
                }.CreateDecryptor(home, info);
                byte[] array = new byte[data.Length];
                memoryStream = new MemoryStream(data);
                cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
                cryptoStream.Read(array, 0, array.Length);
                if (isCompress)
                {
                    MemoryStream memoryStream2 = new MemoryStream();
                    MemoryStream memoryStream3 = new MemoryStream(array);
                    GZipInputStream gzipInputStream = new GZipInputStream(memoryStream3);
                    byte[] array2 = new byte[16384];
                    int num;
                    while ((num = gzipInputStream.Read(array2, 0, array2.Length)) > 0)
                    {
                        memoryStream2.Write(array2, 0, num);
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
        public static object MouseHomeMaster(byte[] data, byte[] home, byte[] info, bool isCompress = false)
        {
            byte[] buf = MouseHomeSub(data, home, info, isCompress);
            MasterDataUnpacker MasterDataUnpacker = new MasterDataUnpacker();
            return MasterDataUnpacker.Unpack(buf);
        }
        public static object MouseGame2Unpacker(byte[] data, bool isCompress = false)
        {
            Array.Copy(data, 0, ownerTop, 0, 32);
            byte[] array = new byte[data.Length - 32];
            Array.Copy(data, 32, array, 0, data.Length - 32);
            ownerData = Encoding.UTF8.GetBytes("pX6q6xK2UymhFKcaGHHUlfXqfTsWF0uH");
            return MouseHomeMaster(array, ownerData, ownerTop, true);
        }
        public static object MouseHomeMsgPack(byte[] data, byte[] home, byte[] info, bool isCompress = false)
        {
            MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
            byte[] buf = MouseHomeSub(data, home, info, isCompress);
            return miniMessagePacker.Unpack(buf);
        }
        public static object MouseInfoMsgPack(byte[] data)
        {
            byte[] array = new byte[data.Length - 32];
            infoData = Encoding.UTF8.GetBytes("W0Juh4cFJSYPkebJB9WpswNF51oa6Gm7");
            Array.Copy(data, 0, InfoTop, 0, 32);
            Array.Copy(data, 32, array, 0, data.Length - 32);
            return MouseHomeMsgPack(array, infoData, InfoTop, true);
        }
        public static object MouseGame2MsgPack(byte[] data, bool isCompress = false)
        {
            Array.Copy(data, 0, ownerTop, 0, 32);
            byte[] array = new byte[data.Length - 32];
            Array.Copy(data, 32, array, 0, data.Length - 32);
            ownerData = Encoding.UTF8.GetBytes("pX6q6xK2UymhFKcaGHHUlfXqfTsWF0uH");
            return MouseHomeMsgPack(array, ownerData, ownerTop, true);
        }
    }
}