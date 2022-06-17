using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace FGOAssetsModifyTool
{
	class Program
	{
		static CatAndMouseGame decryptor = new(CatAndMouseGame.FileType.JP);
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
					"67: 切换为国服密钥\n" +
					"69: 切换为美服密钥");
				int arg = Convert.ToInt32(Console.ReadLine());

				switch (arg)
				{
					case 67:
						{
							decryptor = new(CatAndMouseGame.FileType.CN);
							break;
						}
					case 69:
						{
							decryptor = new(CatAndMouseGame.FileType.EN);
							break;
						}
					case 1:
						{
							foreach (FileInfo file in Configuration.DecryptedFolder.GetFiles("*.bin"))
							{
								Console.WriteLine("Encrypt: " + file.FullName);
								byte[] raw = File.ReadAllBytes(file.FullName);
								byte[] output = decryptor.CatGame4(raw);
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
								byte[] output = decryptor.MouseGame4(raw);
								File.WriteAllBytes(Configuration.DecryptedFolder.FullName + file.Name, output);
							}
							break;
						}
					case 3:
						{
							string data = File.ReadAllText(Configuration.AssetsFolder.FullName + "AssetStorage.txt");
							string loadData = decryptor.MouseGame8(data);
							File.WriteAllText(Configuration.AssetsFolder.FullName + "AssetStorage_dec.txt", loadData);
							Console.WriteLine("Writing file to: " + Configuration.AssetsFolder.FullName + "AssetStorage_dec.txt");
							break;
						}
					case 4:
						{
							string[] assetStore = File.ReadAllLines(Configuration.AssetsFolder.FullName + "AssetStorage_dec.txt");
							Console.WriteLine("Parsing json...");

							List<Asset> AudioList = new();
							List<Asset> AssetList = new();

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
										AudioList.Add(new Asset
										{
											AssetName = assetName,
											FileName = fileName
										});
									}
									else if (!tmp[4].Contains("Movie"))
									{
										assetName = tmp[4].Replace('/', '@') + ".unity3d";
										fileName = decryptor.getShaName(assetName);
										AssetList.Add(new Asset
										{
											AssetName = assetName,
											FileName = fileName
										});
									}
								}
								else if (tmp.Length == 7)
								{
									// BGO format.
									assetName = tmp[4] + ".unity3d";
									fileName = tmp[0] + ".bin";
									Asset asset = new() { AssetName = assetName, FileName = fileName };

									if (tmp[4].Contains("Audio"))
									{
										AudioList.Add(asset);
									}
									else if (!tmp[4].Contains("Movie"))
									{
										AssetList.Add(asset);
									}
								}
								else
								{
									throw new Exception("Not supported format.");
								}
							}
							var options = new JsonSerializerOptions { WriteIndented = true };
							string result = JsonSerializer.Serialize(AudioList, options);

							Console.WriteLine("Writing file to: AudioName.json");
							File.WriteAllText(Configuration.AssetsFolder.FullName + "AudioName.json", result);

							result = JsonSerializer.Serialize(AssetList, options);
							Console.WriteLine("Writing file to: AssetName.json");
							File.WriteAllText(Configuration.AssetsFolder.FullName + "AssetName.json", result);
							break;
						}
					case 5:
						{
							foreach (FileInfo file in Configuration.DecryptedScriptsFolder.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								Console.WriteLine("Encrypting: " + file.FullName);
								string ScriptsFolderName = Path.GetFileNameWithoutExtension(file.Directory.Name);
								string outputTxt = decryptor.CatGame3(File.ReadAllText(file.FullName));
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
								string OutputTxt = decryptor.MouseGame3(File.ReadAllText(file.FullName));
								if (!Directory.Exists(Configuration.DecryptedScriptsFolder.FullName + ScriptsFolderName))
									Directory.CreateDirectory(Configuration.DecryptedScriptsFolder.FullName + ScriptsFolderName);
								File.WriteAllText(Configuration.DecryptedScriptsFolder.FullName + ScriptsFolderName + "\\" + file.Name, OutputTxt);
							}
							break;
						}
					case 7:
						{
							string JPText = File.ReadAllText(Configuration.DecryptedFolder.FullName + "JP.txt");
							string CNText = File.ReadAllText(Configuration.DecryptedFolder.FullName + "CN.txt");
							JPText = Regex.Replace(JPText, @".*//.*\n", "", RegexOptions.Multiline);
							JPText = Regex.Replace(JPText, "\"$", "\",", RegexOptions.Multiline);

							JsonObject JP = JsonNode.Parse(JPText).AsObject();
							JsonObject CN = JsonNode.Parse(CNText).AsObject();
							JsonObject NoTranslation = new();
							foreach (var node in JP)
							{
								if (CN[node.Key] != null)
								{
									JP[node.Key] = CN[node.Key];
								}
								else
								{
									NoTranslation.Add(node.Key, node.Value);
								}
							}
							File.WriteAllText(Configuration.DecryptedFolder.FullName + "LocalizationJpn.txt", JP.ToString());
							File.WriteAllText(Configuration.DecryptedFolder.FullName + "Non-Translation.txt", NoTranslation.ToString());
							break;
						}
					case 8:
						{
							HttpClient Client = new();
							var Response = Client.GetAsync("https://game.fate-go.jp/gamedata/top?appVer=2.20.1");
							string Result = await Response.Result.Content.ReadAsStringAsync();
							JsonObject res = JsonNode.Parse(Result).AsObject();
							if (res["response"][0]["fail"]["action"] != null)
							{
								if (res["response"][0]["fail"]["action"].ToString() == "app_version_up")
								{
									string NewVersion = res["response"][0]["fail"]["detail"].ToString();
									NewVersion = Regex.Replace(NewVersion, @".*新ver.：(.*)、現.*", "$1", RegexOptions.Singleline);
									Console.WriteLine("new version: " + NewVersion.ToString());
									Response = Client.GetAsync("https://game.fate-go.jp/gamedata/top?appVer=" + NewVersion.ToString());
									Result = await Response.Result.Content.ReadAsStringAsync();
									res = JsonNode.Parse(Result).AsObject();
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
							MiniMessagePacker miniMessagePacker = new MiniMessagePacker();
							foreach (KeyValuePair<string, byte[]> item in masterData)
							{
								List<object> unpackeditem = (List<object>)miniMessagePacker.Unpack(item.Value);
								var options = new JsonSerializerOptions { WriteIndented = true };
								string result = JsonSerializer.Serialize(unpackeditem, options);
								File.WriteAllText(Configuration.GameDataUnpackFolder + item.Key, result);
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