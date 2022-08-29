using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace FGOAssetsModifyTool
{
	public class CatAndMouseGame
	{
		public enum FileType
		{
			JP,
			CN,
			EN
		}

		static readonly string salt = "pN6ds2Bg";
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
		public static string GetShaName(string name)
		{
			SHA1 sha = SHA1.Create();
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			byte[] bytes = utf8Encoding.GetBytes(name);
			byte[] array = sha.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.AppendFormat("{0,0:x2}", b ^ 170);
			}
			stringBuilder.Append(".bin");
			return stringBuilder.ToString();
		}
		public FileType fileType;
		public CatAndMouseGame(FileType _)
		{
			fileType = _;
			switch (fileType)
			{
				case FileType.JP:
					JP();
					break;
				case FileType.CN:
					CN();
					break;
				case FileType.EN:
					EN();
					break;
				default:
					throw new ArgumentException("Unknown server type.");
			}
		}

		public void CN()
		{
			byte[] bytes = Encoding.UTF8.GetBytes("d3b13d9093cc6b457fd89766bafa1626ee2ef76626d49ce0d424f4156231ce56");
			byte[] bytes2 = Encoding.UTF8.GetBytes("5ec7ce0fddc50bca9f82b8338b9135c69e0e9e169648df69054dcb96553598e6");
			for (int i = 0; i < bytes2.Length; i++)
			{
				if (i % 2 == 0)
				{
					baseData[i / 2] = bytes2[i];
				}
				else
				{
					baseTop[i / 2] = bytes2[i];
				}
			}
			for (int j = 0; j < bytes.Length / 4; j++)
			{
				if (j % 2 == 0)
				{
					stageData[j / 2 * 4] = bytes[j * 4];
					stageData[j / 2 * 4 + 1] = bytes[j * 4 + 1];
					stageData[j / 2 * 4 + 2] = bytes[j * 4 + 2];
					stageData[j / 2 * 4 + 3] = bytes[j * 4 + 3];
				}
				else
				{
					stageTop[j / 2 * 4] = bytes[j * 4];
					stageTop[j / 2 * 4 + 1] = bytes[j * 4 + 1];
					stageTop[j / 2 * 4 + 2] = bytes[j * 4 + 2];
					stageTop[j / 2 * 4 + 3] = bytes[j * 4 + 3];
				}
			}
		}
		public void JP()
		{
			byte[] bytes = Encoding.UTF8.GetBytes("kzdMtpmzqCHAfx00saU1gIhTjYCuOD1JstqtisXsGYqRVcqrHRydj3k6vJCySu3g");
			byte[] bytes2 = Encoding.UTF8.GetBytes("PFBs0eIuunoxKkCcLbqDVerU1rShhS276SAL3A8tFLUfGvtz3F3FFeKELIk3Nvi4");
			for (int i = 0; i < bytes2.Length / 4; i++)
			{
				if (i % 2 == 0)
				{
					baseData[i / 2 * 4] = bytes2[i * 4];
					baseData[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
					baseData[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
					baseData[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
				}
				else
				{
					baseTop[i / 2 * 4] = bytes2[i * 4];
					baseTop[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
					baseTop[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
					baseTop[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
				}
			}
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i % 2 == 0)
				{
					stageData[i / 2] = bytes[i];
				}
				else
				{
					stageTop[i / 2] = bytes[i];
				}
			}
		}
		public void EN()
		{
			byte[] bytes = Encoding.UTF8.GetBytes("xaVPXPtrkXlUZsJRa3Eu1o1kSDYtjlwhoRQI2MHq2Q4szmpVvDcbmpi7UIZF9Rle");
			byte[] bytes2 = Encoding.UTF8.GetBytes("FEq45VzsnHv8ynuLIGGF9qRA2tJ6vJ61FkG6KliUnD77cN7pvveVAH5gcPeLEzOR");
			for (int i = 0; i < bytes2.Length / 4; i++)
			{
				if (i % 2 == 0)
				{
					baseData[i / 2 * 4] = bytes2[i * 4];
					baseData[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
					baseData[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
					baseData[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
				}
				else
				{
					baseTop[i / 2 * 4] = bytes2[i * 4];
					baseTop[i / 2 * 4 + 1] = bytes2[i * 4 + 1];
					baseTop[i / 2 * 4 + 2] = bytes2[i * 4 + 2];
					baseTop[i / 2 * 4 + 3] = bytes2[i * 4 + 3];
				}
			}
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i % 2 == 0)
				{
					stageData[i / 2] = bytes[i];
				}
				else
				{
					stageTop[i / 2] = bytes[i];
				}
			}
		}
		public string CatGame3(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = (byte)~bytes[i];
			}
			return CatHome(bytes, stageData, stageTop, true);
		}
		public string MouseGame3(string str)
		{
			byte[] data = Convert.FromBase64String(str);
			byte[] array = MouseHomeMain(data, stageData, stageTop, true);
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
		public byte[] CatGame4(byte[] data)
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
			return CatHomeMain(data, baseData, baseTop, false);
		}
		public byte[] MouseGame4(byte[] data)
		{
			byte[] array = MouseHomeMain(data, baseData, baseTop, false);
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
		public string MouseGame8(string str)
		{
			byte[] data = Convert.FromBase64String(str);
			byte[] array = MouseHomeMain(data, stageData, stageTop, true);
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


		public static void OtherHomeBuilding(string data, out byte[] home, out byte[] info)
		{
			var bytes = Encoding.UTF8.GetBytes(data);
			home = new byte[32];
			info = new byte[32];
			for (var i = 0; i < bytes.Length; i++)
				if (i == 0)
					home[i] = bytes[i];
				else
					info[i] = bytes[i];
		}
		public byte[] CatGame4(byte[] data, string key)
		{
			byte[] info;
			byte[] home;
			OtherHomeBuilding(key, out home, out info);

			byte[] array = CatHomeMain(data, home, info, false);
			if (array == null)
			{
				Console.WriteLine("CatHomeMain failed");
				return null;
			}
			return array;
		}
		public byte[] MouseGame4(byte[] data, string key)
		{
			byte[] info;
			byte[] home;
			OtherHomeBuilding(key, out home, out info);

			byte[] array = MouseHomeMain(data, home, info, false);
			if (array == null)
			{
				Console.WriteLine("MouseHomeMain failed");
				return null;
			}
			return array;
		}

		public byte[] CatHomeMain(byte[] data, byte[] home, byte[] info, bool isCompress = false)
		{
			byte[] result = null;
			try
			{
				if (isCompress)
				{
					using (MemoryStream inStream = new MemoryStream(data))
					{
						using (MemoryStream outStream = new MemoryStream())
						{
							if (fileType == FileType.CN)
							{
								BZip2.Compress(inStream, outStream, true, -9);
							}
							else
							{
								GZip.Compress(inStream, outStream, true);
							}
							result = outStream.ToArray();
						}
					}
				}
				var blockCipher = new CbcBlockCipher(new RijndaelEngine(256));
				var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
				var keyParam = new KeyParameter(home);
				var keyParamWithIV = new ParametersWithIV(keyParam, info, 0, 32);
				cipher.Init(true, keyParamWithIV);
				var buffer = new byte[cipher.GetOutputSize(result.Length)];
				var length = cipher.ProcessBytes(result, buffer, 0);
				cipher.DoFinal(buffer, length);
				result = buffer;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				result = null;
			}
			return result;
		}
		public static byte[] MouseHomeMain(byte[] data, byte[] home, byte[] info, bool isCompress = false)
		{
			byte[] result = null;
			try
			{
				var blockCipher = new CbcBlockCipher(new RijndaelEngine(256));
				var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
				var keyParam = new KeyParameter(home);
				var keyParamWithIV = new ParametersWithIV(keyParam, info, 0, 32);
				cipher.Init(false, keyParamWithIV);
				var buffer = new byte[cipher.GetOutputSize(data.Length)];
				var length = cipher.ProcessBytes(data, buffer, 0);
				cipher.DoFinal(buffer, length);

				if (isCompress)
				{
					using (MemoryStream inStream = new(buffer))
					{
						using (MemoryStream outStream = new())
						{
							if (buffer[0] == 0x42 &&
								buffer[1] == 0x5A &&
								buffer[2] == 0x68)
							{
								BZip2.Decompress(inStream, outStream, true);
							}
							else if (buffer[0] == 0x1F &&
								buffer[1] == 0x8B &&
								buffer[2] == 0x08)
							{
								GZip.Decompress(inStream, outStream, true);
							}
							result = outStream.ToArray();
						}
					}
				}
				else
				{
					return buffer;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				result = null;
			}
			return result;
		}
		public string CatHome(byte[] data, byte[] home, byte[] info, bool isCompress = false)
		{
			byte[] array = CatHomeMain(data, home, info, isCompress);
			if (array != null)
			{
				return Convert.ToBase64String(array);
			}
			return null;
		}

		protected byte[] stageTop = new byte[32];

		protected byte[] stageData = new byte[32];

		protected byte[] baseTop = new byte[32];

		protected byte[] baseData = new byte[32];
	}
}