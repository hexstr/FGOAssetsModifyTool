using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net.Http;

// https://line1-s1-bili-fate.bilibiligame.net/rongame_beta/rgfate/60_member/network/AssetStorage.txt	国服-Android
// https://line1.s1.ios.fate.biligame.net/rongame_beta/rgfate/60_member/network/AssetStorage.txt		国服-iOS

namespace FGOAssetsModifyTool
{
	class Program
	{
		static async void DisplayMenuAsync()
		{
			Console.Clear();
			try
			{
				Console.WriteLine(
					"1: 加密\t" +
					"2: 解密\n" +
					"3: 解密AssetStorage.txt\t" +
					"4: 导出资源名 - 实际文件名\n" +
					"5: 加密剧情文本(scripts)\n" +
					"6: 解密剧情文本(scripts)\n" +
					"7: 汉化UI\n" +
					"8: 从服务器下载游戏数据\n" +
					"9: 解密游戏数据\n" +
					"67: 切换为国服密钥");
				int arg = Convert.ToInt32(Console.ReadLine());
				
				switch (arg)
				{
					case 67:
						{
							CatAndMouseGame.CN();
							break;
						}
					case 1:
						{
							foreach (FileInfo file in Configuration.DecryptedFolder.GetFiles("*.bin"))
							{
								Console.WriteLine("Encrypt: " + file.FullName);
								byte[] raw = File.ReadAllBytes(file.FullName);
								byte[] output = CatAndMouseGame.CatGame4(raw);
								File.WriteAllBytes(Configuration.EncryptedFolder + file.Name, output);
							}
							break;
						}
					case 2:
						{
							foreach (FileInfo file in Configuration.AssetsFolder.GetFiles("*.bin"))
							{
								Console.WriteLine("Decrypt: " + file.FullName);
								byte[] raw = File.ReadAllBytes(file.FullName);
								byte[] output = CatAndMouseGame.MouseGame4(raw);
								File.WriteAllBytes(Configuration.DecryptedFolder.FullName + file.Name, output);
							}
							break;
						}
					case 3:
						{
							string data = File.ReadAllText(Configuration.AssetsFolder.FullName + "AssetStorage.txt");
							string loadData = CatAndMouseGame.MouseGame8(data);
							File.WriteAllText(Configuration.AssetsFolder.FullName + "AssetStorage_dec.txt", loadData);
							Console.WriteLine("Writing file to: " + Configuration.AssetsFolder.FullName + "AssetStorage_dec.txt");
							break;
						}
					case 4:
						{
							string[] assetStore = File.ReadAllLines(Configuration.AssetsFolder.FullName + "AssetStorage_dec.txt");
							Console.WriteLine("Parsing json...");
							JArray AudioArray = new JArray();
							JArray AssetArray = new JArray();
							for (int i = 2; i < assetStore.Length; ++i)
							{
								string[] tmp = assetStore[i].Split(',');
								string assetName;
								string fileName;

								if (tmp.Length == 5)
								{
									if (tmp[4].Contains("Audio"))
									{
										assetName = tmp[4].Replace('/', '@');
										fileName = CatAndMouseGame.GetMD5String(assetName);
										AudioArray.Add(new JObject(new JProperty("audioName", assetName), new JProperty("fileName", fileName)));
									}
									else if (!tmp[4].Contains("Movie"))
									{
										assetName = tmp[4].Replace('/', '@') + ".unity3d";
										fileName = CatAndMouseGame.getShaName(assetName);
										AssetArray.Add(new JObject(new JProperty("assetName", assetName), new JProperty("fileName", fileName)));
									}
								}
								else if (tmp.Length == 7)
								{
									// 国服安装包中提取的奇葩格式
									if (tmp[4].Contains("Audio"))
									{
										assetName = tmp[6].Replace('/', '@');
										fileName = CatAndMouseGame.GetMD5String(assetName);
										AudioArray.Add(new JObject(new JProperty("audioName", assetName), new JProperty("fileName", fileName)));
									}
									else if (!tmp[4].Contains("Movie"))
									{
										assetName = tmp[4] + ".unity3d";
										fileName = tmp[0] + ".bin";
										AssetArray.Add(new JObject(new JProperty("assetName", assetName), new JProperty("fileName", fileName)));
									}
								}
								else
								{
									throw new Exception("Not supported format.");
								}
							}
							Console.WriteLine("Writing file to: AudioName.json");
							File.WriteAllText(Configuration.AssetsFolder.FullName + "AudioName.json", AudioArray.ToString());
							Console.WriteLine("Writing file to: AssetName.json");
							File.WriteAllText(Configuration.AssetsFolder.FullName + "AssetName.json", AssetArray.ToString());
							break;
						}
					case 5:
						{
							foreach (FileInfo file in Configuration.DecryptedScriptsFolder.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								Console.WriteLine("Encrypting: " + file.FullName);
								string ScriptsFolderName = Path.GetFileNameWithoutExtension(file.Directory.Name);
								string outputTxt = CatAndMouseGame.CatGame3(File.ReadAllText(file.FullName));
								if (!Directory.Exists(Configuration.EncryptedScriptsFolder + ScriptsFolderName))
									Directory.CreateDirectory(Configuration.EncryptedScriptsFolder + ScriptsFolderName);
								File.WriteAllText(Configuration.EncryptedScriptsFolder + ScriptsFolderName + "\\" + file.Name, outputTxt);
							}
							break;
						}
					case 6:
						{
							foreach (FileInfo file in Configuration.ScriptsFolder.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								Console.WriteLine("Decrypting: " + file.FullName);
								string ScriptsFolderName = Path.GetFileNameWithoutExtension(file.Directory.Name);
								string outputTxt = CatAndMouseGame.MouseGame3(File.ReadAllText(file.FullName));
								if (!Directory.Exists(Configuration.DecryptedScriptsFolder.FullName + ScriptsFolderName))
									Directory.CreateDirectory(Configuration.DecryptedScriptsFolder.FullName + ScriptsFolderName);
								File.WriteAllText(Configuration.DecryptedScriptsFolder.FullName + ScriptsFolderName + "\\" + file.Name, outputTxt);
							}
							break;
						}
					case 7:
						{
							string jptext = File.ReadAllText(Configuration.DecryptedFolder.FullName + "JP.txt");
							jptext = Regex.Replace(jptext, @".*//.*\n", "", RegexOptions.Multiline);
							jptext = Regex.Replace(jptext, "\"$", "\",", RegexOptions.Multiline);
							JObject jp = JObject.Parse(jptext);
							JObject cn = JObject.Parse(File.ReadAllText(Configuration.DecryptedFolder.FullName + "CN.txt"));
							JObject no = new JObject();
							foreach (JProperty jProperty in jp.Properties())
							{
								if (cn[jProperty.Name] != null)
								{
									jp[jProperty.Name] = cn[jProperty.Name];
								}
								else
								{
									no.Add(jProperty.Name, jProperty.Value);
								}
							}
							File.WriteAllText(Configuration.DecryptedFolder.FullName + "LocalizationJpn.txt", jp.ToString());
							File.WriteAllText(Configuration.DecryptedFolder.FullName + "Non-Translation.txt", no.ToString());
							break;
						}
					case 8:
						{
							HttpClient Client = new HttpClient();
							var Response = Client.GetAsync("https://game.fate-go.jp/gamedata/top?appVer=2.20.1");
							string Result = await Response.Result.Content.ReadAsStringAsync();
							JObject res = JObject.Parse(Result);
							if (res["response"][0]["fail"]["action"] != null)
							{
								if(res["response"][0]["fail"]["action"].ToString() == "app_version_up")
								{
									string NewVersion = res["response"][0]["fail"]["detail"].ToString();
									NewVersion = Regex.Replace(NewVersion, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
									Console.WriteLine("new version: " + NewVersion.ToString());
									Response = Client.GetAsync("https://game.fate-go.jp/gamedata/top?appVer=" + NewVersion.ToString());
									Result = await Response.Result.Content.ReadAsStringAsync();
									res = JObject.Parse(Result);
								}
								else
								{
									throw new Exception(res["response"][0]["fail"]["detail"].ToString());
								}
							}
							File.WriteAllText(Configuration.GameDataFolder + "raw", Result);
							File.WriteAllText(Configuration.GameDataFolder + "master", res["response"][0]["success"]["master"].ToString());
							Console.WriteLine("Writing file to: " + Configuration.GameDataFolder + "master");
							break;
						}
					case 9:
						{
							string data = File.ReadAllText(Configuration.GameDataFolder + "master");
							Dictionary<string, byte[]> masterData = (Dictionary<string, byte[]>)MasterDataUnpacker.MouseGame2Unpacker(Convert.FromBase64String(data));
							JObject job = new JObject();
							MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
							foreach (KeyValuePair<string, byte[]> item in masterData)
							{
								List<object> unpackeditem = (List<object>)miniMessagePacker.Unpack(item.Value);
								string json = JsonConvert.SerializeObject(unpackeditem, Formatting.Indented);
								File.WriteAllText(Configuration.GameDataUnpackFolder + item.Key, json);
								Console.WriteLine("Writing file to: " + Configuration.GameDataUnpackFolder + item.Key);
							}
							break;
						}
					default:
						{
							Console.WriteLine("请输入一个可接受的选项");
							break;
						}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				Console.ReadKey(true);
			}
		}
		static void Main(string[] args)
		{
			while (true)
			{
				DisplayMenuAsync();
				Console.WriteLine("pause...");
				Console.ReadKey(true);
			}
		}
	}
}