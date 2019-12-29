using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using MiniMessagePack;
using UnityEngine;

// Token: 0x0200068A RID: 1674
public class CatAndMouseGame
{
	// Token: 0x06003B8F RID: 15247 RVA: 0x00138280 File Offset: 0x00136480
	static CatAndMouseGame()
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

	// Token: 0x06003B90 RID: 15248 RVA: 0x00138434 File Offset: 0x00136634
	public static string CatGame1(string str, bool isCompress = false)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		byte[] bytes2 = Encoding.UTF8.GetBytes("b************c**********");
		byte[] bytes3 = Encoding.UTF8.GetBytes("a****a**");
		TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
		byte[] inArray;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateEncryptor(bytes2, bytes3), CryptoStreamMode.Write))
			{
				if (isCompress)
				{
					using (BZip2OutputStream bzip2OutputStream = new BZip2OutputStream(cryptoStream))
					{
						bzip2OutputStream.Write(bytes, 0, bytes.Length);
						bzip2OutputStream.Close();
					}
				}
				else
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
				}
				cryptoStream.Close();
			}
			inArray = memoryStream.ToArray();
			memoryStream.Close();
		}
		return Convert.ToBase64String(inArray);
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x00138568 File Offset: 0x00136768
	public static string MouseGame1(string str, bool isCompress = false)
	{
		byte[] array = Convert.FromBase64String(str);
		byte[] bytes = Encoding.UTF8.GetBytes("b************c**********");
		byte[] bytes2 = Encoding.UTF8.GetBytes("a****a**");
		TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
		byte[] array2;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Write))
			{
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.Close();
			}
			array2 = memoryStream.ToArray();
			memoryStream.Close();
		}
		if (isCompress)
		{
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				using (MemoryStream memoryStream3 = new MemoryStream(array2))
				{
					using (BZip2InputStream bzip2InputStream = new BZip2InputStream(memoryStream3))
					{
						byte[] array3 = new byte[16384];
						int count;
						while ((count = bzip2InputStream.Read(array3, 0, array3.Length)) > 0)
						{
							memoryStream2.Write(array3, 0, count);
						}
						bzip2InputStream.Close();
					}
					memoryStream3.Close();
				}
				array2 = memoryStream2.ToArray();
				memoryStream2.Close();
			}
		}
		return Encoding.UTF8.GetString(array2);
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0013873C File Offset: 0x0013693C
	public static string CatGame2(string str, bool isCompress = false)
	{
		if (!Application.isPlaying)
		{
		}
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		return CatAndMouseGame.CatHome(bytes, CatAndMouseGame.ownerData, CatAndMouseGame.ownerTop, true);
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x00138778 File Offset: 0x00136978
	public static string MouseGame2(byte[] data, bool isCompress = false)
	{
		Array.Copy(data, 0, CatAndMouseGame.ownerTop, 0, 32);
		byte[] array = new byte[data.Length - 32];
		Array.Copy(data, 32, array, 0, data.Length - 32);
		CatAndMouseGame.ownerData = Encoding.UTF8.GetBytes("a*******************************");
		return CatAndMouseGame.MouseHome(array, CatAndMouseGame.ownerData, CatAndMouseGame.ownerTop, true);
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x001387D8 File Offset: 0x001369D8
	public static object MouseGame2MsgPack(byte[] data, bool isCompress = false)
	{
		Array.Copy(data, 0, CatAndMouseGame.ownerTop, 0, 32);
		byte[] array = new byte[data.Length - 32];
		Array.Copy(data, 32, array, 0, data.Length - 32);
		CatAndMouseGame.ownerData = Encoding.UTF8.GetBytes("a*******************************");
		return CatAndMouseGame.MouseHomeMsgPack(array, CatAndMouseGame.ownerData, CatAndMouseGame.ownerTop, true);
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x00138838 File Offset: 0x00136A38
	public static object MouseGame2Unpacker(byte[] data, bool isCompress = false)
	{
		Array.Copy(data, 0, CatAndMouseGame.ownerTop, 0, 32);
		byte[] array = new byte[data.Length - 32];
		Array.Copy(data, 32, array, 0, data.Length - 32);
		CatAndMouseGame.ownerData = Encoding.UTF8.GetBytes("a*******************************");
		return CatAndMouseGame.MouseHomeMaster(array, CatAndMouseGame.ownerData, CatAndMouseGame.ownerTop, true);
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x00138898 File Offset: 0x00136A98
	public static string CatGame3(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] = ~bytes[i];
		}
		return CatAndMouseGame.CatHome(bytes, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x001388E0 File Offset: 0x00136AE0
	public static Stream CatGameZ(Stream stream)
	{
		return CatAndMouseGame.CatHomeMainZ(stream, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x001388F4 File Offset: 0x00136AF4
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
			array[i] = ~array[i];
		}
		return Encoding.UTF8.GetString(array).TrimEnd(new char[1]);
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x00138954 File Offset: 0x00136B54
	public static Stream MouseGameZ(Stream stream)
	{
		return CatAndMouseGame.MouseHomeMainZ(stream, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x00138968 File Offset: 0x00136B68
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
			data[i] = (b2 ^ 206);
			data[i + 1] = (b ^ 210);
		}
		return CatAndMouseGame.CatHomeMain(data, CatAndMouseGame.baseData, CatAndMouseGame.baseTop, false);
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x001389CC File Offset: 0x00136BCC
	public static byte[] MouseGame4(byte[] data)
	{
		byte[] array = CatAndMouseGame.MouseHomeMain(data, CatAndMouseGame.baseData, CatAndMouseGame.baseTop, false);
		if (array == null)
		{
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
			array[i] = (b2 ^ 210);
			array[i + 1] = (b ^ 206);
		}
		return array;
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x00138A3C File Offset: 0x00136C3C
	public static string CatGame5(string str)
	{
		byte[] array = new byte[CatAndMouseGame.BattleKey.Length];
		byte[] array2 = new byte[CatAndMouseGame.BattleIV.Length];
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		for (int i = 0; i < CatAndMouseGame.BattleKey.Length; i++)
		{
			array[i] = (CatAndMouseGame.BattleKey[i] ^ 4);
		}
		for (int j = 0; j < CatAndMouseGame.BattleIV.Length; j++)
		{
			array2[j] = (CatAndMouseGame.BattleIV[j] ^ 8);
		}
		return CatAndMouseGame.CatHome(bytes, array, array2, false);
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x00138AC8 File Offset: 0x00136CC8
	public static string MouseGame5(string str)
	{
		byte[] array = new byte[CatAndMouseGame.BattleKey.Length];
		byte[] array2 = new byte[CatAndMouseGame.BattleIV.Length];
		byte[] data = Convert.FromBase64String(str);
		for (int i = 0; i < CatAndMouseGame.BattleKey.Length; i++)
		{
			array[i] = (CatAndMouseGame.BattleKey[i] ^ 4);
		}
		for (int j = 0; j < CatAndMouseGame.BattleIV.Length; j++)
		{
			array2[j] = (CatAndMouseGame.BattleIV[j] ^ 8);
		}
		return CatAndMouseGame.MouseHome(data, array, array2, false);
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x00138B50 File Offset: 0x00136D50
	public static byte[] CatGame7(byte[] data)
	{
		byte[] array = new byte[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			array[i] = ~data[i];
		}
		return CatAndMouseGame.CatHomeMain(array, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x00138B94 File Offset: 0x00136D94
	public static byte[] MouseGame7(byte[] data)
	{
		byte[] array = CatAndMouseGame.MouseHomeMain(data, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ~array[i];
		}
		return array;
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x00138BD0 File Offset: 0x00136DD0
	public static string CatGame8(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] = ~bytes[i];
		}
		return CatAndMouseGame.CatHomeZ2(bytes, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x00138C18 File Offset: 0x00136E18
	public static string CatGame8(byte[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = ~data[i];
		}
		return CatAndMouseGame.CatHomeZ2(data, CatAndMouseGame.stageData, CatAndMouseGame.stageTop, true);
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x00138C54 File Offset: 0x00136E54
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
			array[i] = ~array[i];
		}
		return Encoding.UTF8.GetString(array).TrimEnd(new char[1]);
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x00138CCC File Offset: 0x00136ECC
	public static string MouseInfo(byte[] data)
	{
		byte[] array = new byte[data.Length - 32];
		CatAndMouseGame.infoData = Encoding.UTF8.GetBytes("*******************************b");
		Array.Copy(data, 0, CatAndMouseGame.InfoTop, 0, 32);
		Array.Copy(data, 32, array, 0, data.Length - 32);
		return CatAndMouseGame.MouseHome(array, CatAndMouseGame.infoData, CatAndMouseGame.InfoTop, true);
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x00138D2C File Offset: 0x00136F2C
	public static object MouseInfoMsgPack(byte[] data)
	{
		byte[] array = new byte[data.Length - 32];
		CatAndMouseGame.infoData = Encoding.UTF8.GetBytes("*******************************b");
		Array.Copy(data, 0, CatAndMouseGame.InfoTop, 0, 32);
		Array.Copy(data, 32, array, 0, data.Length - 32);
		return CatAndMouseGame.MouseHomeMsgPack(array, CatAndMouseGame.infoData, CatAndMouseGame.InfoTop, true);
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x00138D8C File Offset: 0x00136F8C
	public static void ThirdHomeBuilding(string data)
	{
		byte[] array = (!Application.isPlaying) ? Encoding.UTF8.GetBytes("d3b13d9093cc6b457fd89766bafa1626ee2ef76626d49ce0d424f4156231ce56") : Encoding.UTF8.GetBytes(data);
		Array.Copy(array, 0, CatAndMouseGame.BattleKey, 0, 32);
		Array.Copy(array, 32, CatAndMouseGame.BattleIV, 0, array.Length - 32);
		for (int i = 0; i < array.Length; i++)
		{
			if (i % 2 == 0)
			{
				CatAndMouseGame.stageData[i / 2] = array[i];
			}
			else
			{
				CatAndMouseGame.stageTop[i / 2] = array[i];
			}
		}
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x00138E20 File Offset: 0x00137020
	public static void ForthHomeBuilding(string data)
	{
		byte[] array = (!Application.isPlaying) ? Encoding.UTF8.GetBytes("5ec7ce0fddc50bca9f82b8338b9135c69e0e9e169648df69054dcb96553598e6") : Encoding.UTF8.GetBytes(data);
		for (int i = 0; i < array.Length / 4; i++)
		{
			if (i % 2 == 0)
			{
				CatAndMouseGame.baseData[i / 2 * 4] = array[i * 4];
				CatAndMouseGame.baseData[i / 2 * 4 + 1] = array[i * 4 + 1];
				CatAndMouseGame.baseData[i / 2 * 4 + 2] = array[i * 4 + 2];
				CatAndMouseGame.baseData[i / 2 * 4 + 3] = array[i * 4 + 3];
			}
			else
			{
				CatAndMouseGame.baseTop[i / 2 * 4] = array[i * 4];
				CatAndMouseGame.baseTop[i / 2 * 4 + 1] = array[i * 4 + 1];
				CatAndMouseGame.baseTop[i / 2 * 4 + 2] = array[i * 4 + 2];
				CatAndMouseGame.baseTop[i / 2 * 4 + 3] = array[i * 4 + 3];
			}
		}
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x00138F14 File Offset: 0x00137114
	public static string CatGame6(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		return CatAndMouseGame.CatHome(bytes, CatAndMouseGame.sideData, CatAndMouseGame.sideTop, false);
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x00138F40 File Offset: 0x00137140
	public static string CatGame6_UWA(object obj)
	{
		return CatAndMouseGame.CatHome_UWA(obj, CatAndMouseGame.sideData, CatAndMouseGame.sideTop, false);
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x00138F54 File Offset: 0x00137154
	public static string MouseGame6(string str)
	{
		byte[] data = Convert.FromBase64String(str);
		return CatAndMouseGame.MouseHome(data, CatAndMouseGame.sideData, CatAndMouseGame.sideTop, false);
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x00138F7C File Offset: 0x0013717C
	public static byte[] MouseGame6_UWA(string str)
	{
		byte[] data = Convert.FromBase64String(str);
		return CatAndMouseGame.MouseHome_UWA(data, CatAndMouseGame.sideData, CatAndMouseGame.sideTop, false);
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x00138FA4 File Offset: 0x001371A4
	public static void SixHomeBuilding(string data)
	{
		CatAndMouseGame.sideData = Encoding.UTF8.GetBytes(data.Substring(32));
		CatAndMouseGame.sideTop = Encoding.UTF8.GetBytes(data.Substring(0, 32));
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x00138FE0 File Offset: 0x001371E0
	public static string CatHome(byte[] data, byte[] home, byte[] info, bool isCompress = false)
	{
		byte[] array = CatAndMouseGame.CatHomeMain(data, home, info, isCompress);
		if (array != null)
		{
			return Convert.ToBase64String(array);
		}
		return null;
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x00139008 File Offset: 0x00137208
	public static string CatHome_UWA(object data, byte[] home, byte[] info, bool isCompress = false)
	{
		byte[] array = CatAndMouseGame.CatHomeMain_UWA(data, home, info, isCompress);
		if (array != null)
		{
			return Convert.ToBase64String(array);
		}
		return null;
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x00139030 File Offset: 0x00137230
	public static string CatHomeZ2(byte[] data, byte[] home, byte[] info, bool isCompress = false)
	{
		byte[] array = CatAndMouseGame.CatHomeMainZ2(data, home, info, isCompress);
		if (array != null)
		{
			return Convert.ToBase64String(array);
		}
		return null;
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x00139058 File Offset: 0x00137258
	public static Stream CatHomeMainZ(Stream stream, byte[] home, byte[] info, bool isCompress = false)
	{
		ICryptoTransform transform = new RijndaelManaged
		{
			Padding = PaddingMode.PKCS7,
			Mode = CipherMode.CBC,
			KeySize = 256,
			BlockSize = 256
		}.CreateEncryptor(home, info);
		CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
		return (!isCompress) ? cryptoStream : new GZipOutputStream(cryptoStream);
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x001390B4 File Offset: 0x001372B4
	public static byte[] CatHomeMainZ2(byte[] data, byte[] home, byte[] info, bool isCompress = false)
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
		catch (Exception ex)
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

	// Token: 0x06003BB1 RID: 15281 RVA: 0x001391B0 File Offset: 0x001373B0
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
		catch (Exception ex)
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

	// Token: 0x06003BB2 RID: 15282 RVA: 0x001392AC File Offset: 0x001374AC
	public static byte[] CatHomeMain_UWA(object data, byte[] home, byte[] info, bool isCompress = false)
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
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(cryptoStream, data);
			cryptoStream.FlushFinalBlock();
			result = memoryStream.ToArray();
		}
		catch (Exception ex)
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

	// Token: 0x06003BB3 RID: 15283 RVA: 0x00139388 File Offset: 0x00137588
	public static string MouseHome(byte[] data, byte[] home, byte[] info, bool isCompress = false)
	{
		byte[] bytes = CatAndMouseGame.MouseHomeMain(data, home, info, isCompress);
		string @string = Encoding.UTF8.GetString(bytes);
		return @string.TrimEnd(new char[1]);
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x001393B8 File Offset: 0x001375B8
	public static byte[] MouseHome_UWA(byte[] data, byte[] home, byte[] info, bool isCompress = false)
	{
		return CatAndMouseGame.MouseHomeMain(data, home, info, isCompress);
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x001393C4 File Offset: 0x001375C4
	public static object MouseHomeMsgPack(byte[] data, byte[] home, byte[] info, bool isCompress = false)
	{
		MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
		byte[] buf = CatAndMouseGame.MouseHomeSub(data, home, info, isCompress);
		return miniMessagePacker.Unpack(buf);
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x001393EC File Offset: 0x001375EC
	public static object MouseHomeMaster(byte[] data, byte[] home, byte[] info, bool isCompress = false)
	{
		byte[] buf = CatAndMouseGame.MouseHomeSub(data, home, info, isCompress);
		MasterDataUnpakcer masterDataUnpakcer = new MasterDataUnpakcer();
		return masterDataUnpakcer.Unpack(buf);
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x00139410 File Offset: 0x00137610
	public static Stream MouseHomeMainZ(Stream stream, byte[] home, byte[] info, bool isCompress = false)
	{
		ICryptoTransform transform = new RijndaelManaged
		{
			Padding = PaddingMode.PKCS7,
			Mode = CipherMode.CBC,
			KeySize = 256,
			BlockSize = 256
		}.CreateDecryptor(home, info);
		CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		return (!isCompress) ? cryptoStream : new GZipInputStream(cryptoStream);
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x0013946C File Offset: 0x0013766C
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
		catch (Exception ex)
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

	// Token: 0x06003BB9 RID: 15289 RVA: 0x001395D4 File Offset: 0x001377D4
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
			result = null;
		}
		return result;
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x001396B8 File Offset: 0x001378B8
	public static byte[] MouseHomeSub(byte[] data, byte[] home, byte[] info, bool isCompress = false)
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
		catch (Exception ex)
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

	// Token: 0x04002D25 RID: 11557
	protected const int WRITE_BUFFER_SIZE = 16384;

	// Token: 0x04002D26 RID: 11558
	protected const int KEY_LENGTH = 32;

	// Token: 0x04002D27 RID: 11559
	private const string AKEY = "b************c**********";

	// Token: 0x04002D28 RID: 11560
	private const string AVEC = "a****a**";

	// Token: 0x04002D29 RID: 11561
	private const string MKEY = "a*******************************";

	// Token: 0x04002D2A RID: 11562
	private const string IKEY = "*******************************b";

	// Token: 0x04002D2B RID: 11563
	private const string SKEY = "ee2ef76626d49ce0d424f4156231ce56";

	// Token: 0x04002D2C RID: 11564
	private const string SVEC = "d3b13d9093cc6b457fd89766bafa1626";

	// Token: 0x04002D2D RID: 11565
	private const string BKEY = "9e0e9e169648df69054dcb96553598e6";

	// Token: 0x04002D2E RID: 11566
	private const string BVEC = "5ec7ce0fddc50bca9f82b8338b9135c6";

	// Token: 0x04002D2F RID: 11567
	private const string BSKEY = "****************************************************************";

	// Token: 0x04002D30 RID: 11568
	private const int BlockSize = 256;

	// Token: 0x04002D31 RID: 11569
	private const int KeySize = 256;

	// Token: 0x04002D32 RID: 11570
	protected static byte[] ownerTop = new byte[32];

	// Token: 0x04002D33 RID: 11571
	protected static byte[] ownerData = new byte[32];

	// Token: 0x04002D34 RID: 11572
	protected static byte[] InfoTop = new byte[32];

	// Token: 0x04002D35 RID: 11573
	protected static byte[] infoData = new byte[32];

	// Token: 0x04002D36 RID: 11574
	protected static byte[] stageTop = new byte[32];

	// Token: 0x04002D37 RID: 11575
	protected static byte[] stageData = new byte[32];

	// Token: 0x04002D38 RID: 11576
	protected static byte[] baseTop = new byte[32];

	// Token: 0x04002D39 RID: 11577
	protected static byte[] baseData = new byte[32];

	// Token: 0x04002D3A RID: 11578
	private static byte[] BattleKey = new byte[32];

	// Token: 0x04002D3B RID: 11579
	private static byte[] BattleIV = new byte[32];

	// Token: 0x04002D3C RID: 11580
	protected static byte[] sideTop = new byte[32];

	// Token: 0x04002D3D RID: 11581
	protected static byte[] sideData = new byte[32];

	// Token: 0x0200068B RID: 1675
	private class DataDecryptor : IDisposable
	{
		// Token: 0x06003BBB RID: 15291 RVA: 0x00139804 File Offset: 0x00137A04
		public DataDecryptor(ICryptoTransform decryptor, byte[] data, bool isCompress)
		{
			this.data = data;
			this.isCompress = isCompress;
			this.memoryStream = new MemoryStream(data.Length);
			this.cryptoStream = new CryptoStream(this.memoryStream, decryptor, CryptoStreamMode.Write);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0013983C File Offset: 0x00137A3C
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

		// Token: 0x06003BBD RID: 15293 RVA: 0x001398DC File Offset: 0x00137ADC
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
			return (byte[])Enumerable.Empty<byte>();
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x00139984 File Offset: 0x00137B84
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x00139994 File Offset: 0x00137B94
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

		// Token: 0x04002D3E RID: 11582
		private readonly byte[] data;

		// Token: 0x04002D3F RID: 11583
		private readonly bool isCompress;

		// Token: 0x04002D40 RID: 11584
		private MemoryStream memoryStream;

		// Token: 0x04002D41 RID: 11585
		private CryptoStream cryptoStream;

		// Token: 0x04002D42 RID: 11586
		private MemoryStream memoryStreamBZip;

		// Token: 0x04002D43 RID: 11587
		private BZip2InputStream bzipStream;

		// Token: 0x04002D44 RID: 11588
		private bool isDisposed;
	}
}