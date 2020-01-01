using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace FGOAssetsModifyTool
{
	public class MiniMessagePacker
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
			if (MiniMessagePacker.sb == null)
			{
				MiniMessagePacker.sb = new StringBuilder((int)len);
			}
			else
			{
				MiniMessagePacker.sb.Length = 0;
				MiniMessagePacker.sb.EnsureCapacity((int)len);
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
						MiniMessagePacker.sb.Append((char)num5);
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
							MiniMessagePacker.sb.Append((char)num);
						}
						else if (num < 1114112u)
						{
							num -= 65536u;
							MiniMessagePacker.sb.Append((char)((num >> 10) + 55296u));
							MiniMessagePacker.sb.Append((char)((num & 1023u) + 56320u));
						}
						num2 = 0u;
					}
				}
				num4++;
			}
			return MiniMessagePacker.sb.ToString();
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
		private Dictionary<string, object> UnpackMap(Stream s, long len)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>((int)len);
			for (long num = 0L; num < len; num += 1L)
			{
				string text = this.Unpack(s) as string;
				object obj = this.Unpack(s);
				if (text != null)
				{
					dictionary.Add(text, obj);
				}
			}
			return dictionary;
		}
		public void LL_Skip(Stream s)
		{
			int num = s.ReadByte();
			if (num < 0)
			{
				throw new FormatException();
			}
			if (num <= 127)
			{
				return;
			}
			if (num <= 143)
			{
				this.LL_SkipMap(s, num & 15);
				return;
			}
			if (num <= 159)
			{
				this.LL_SkipArray(s, num & 15);
				return;
			}
			if (num <= 191)
			{
				this.LL_Seek(s, (long)(num & 31));
				return;
			}
			if (num >= 224)
			{
				return;
			}
			switch (num)
			{
			case 192:
				return;
			case 194:
				return;
			case 195:
				return;
			case 196:
				this.LL_Seek(s, (long)s.ReadByte());
				return;
			case 197:
				this.LL_Seek(s, this.UnpackUint16(s));
				return;
			case 198:
				this.LL_Seek(s, this.UnpackUint32(s));
				return;
			case 202:
				this.LL_Seek(s, 4L);
				return;
			case 203:
				this.LL_Seek(s, 8L);
				return;
			case 204:
				this.LL_Seek(s, 1L);
				return;
			case 205:
				this.LL_Seek(s, 2L);
				return;
			case 206:
				this.LL_Seek(s, 4L);
				return;
			case 207:
				this.LL_Seek(s, 8L);
				return;
			case 208:
				this.LL_Seek(s, 1L);
				return;
			case 209:
				this.LL_Seek(s, 2L);
				return;
			case 210:
				this.LL_Seek(s, 4L);
				return;
			case 211:
				this.LL_Seek(s, 8L);
				return;
			case 217:
				this.LL_Seek(s, (long)s.ReadByte());
				return;
			case 218:
				this.LL_Seek(s, this.UnpackUint16(s));
				return;
			case 219:
				this.LL_Seek(s, this.UnpackUint32(s));
				return;
			case 220:
				this.LL_SkipArray(s, (int)this.UnpackUint16(s));
				return;
			case 221:
				this.LL_SkipArray(s, (int)this.UnpackUint32(s));
				return;
			case 222:
				this.LL_SkipMap(s, (int)this.UnpackUint16(s));
				return;
			case 223:
				this.LL_SkipMap(s, (int)this.UnpackUint32(s));
				return;
			}
			throw new FormatException();
		}
		private void LL_SkipArray(Stream s, int len)
		{
			for (int i = 0; i < len; i++)
			{
				this.LL_Skip(s);
			}
		}
		private void LL_SkipMap(Stream s, int len)
		{
			for (int i = 0; i < len; i++)
			{
				this.LL_Skip(s);
				this.LL_Skip(s);
			}
		}
		private void LL_Seek(Stream s, long offset)
		{
			if (s.Length < s.Seek(offset, SeekOrigin.Current))
			{
				throw new FormatException();
			}
		}
		private const int TmpStringHashCapacity = 2000;
		private const int InternPoolCapacity = 40000;
		private static StringBuilder sb;
		private byte[] tmp0 = new byte[8];
		private byte[] tmp1 = new byte[8];
		private Encoding encoder = Encoding.UTF8;
		private static byte[] tmpStringHash = new byte[2000];
	}
}