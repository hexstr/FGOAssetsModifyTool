using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace FGOAssetsModifyTool
{
	class Program
	{
		static CatAndMouseGame decryptor = new(CatAndMouseGame.FileType.JP);
		static Dictionary<string, string> AssetBundleKeyList = new();
		static Dictionary<string, string> AssetBundleWithExtraKey = new();
		static async void DisplayMenuAsync()
		{
			Console.Clear();
			try
			{
				Console.WriteLine(
					"初始化顺序：3->7->4->6->0\n" +
					"之后直接选择：0\n" +
					"注意：日服的AssetStorage.txt必须选择4下载，从游戏中提取的格式不同\n" +
					"0: 载入assetbundleinfo\n" +
					"1: 加密\t" +
					"2: 解密\n" +
					"3: 从服务器下载游戏数据\n" +
					"4: 下载并解密AssetStorage.txt\t" +
					"5: 解密AssetStorage.txt\t" +
					"6: 解析AssetStorage.txt\n" +
					"7: 解析AssetBundle&Key\t" +
					"8: 解析Master\n" +
					"9: 加密剧情文本(scripts)\n" +
					"10: 解密剧情文本(scripts)\n" +
					"11: 汉化UI\n" +
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
					case 0:
						{
							if (File.Exists($"{Configuration.GameDataUnpackAssetBundleFolder}assetbundleKey.json"))
							{
								string assetbundlekey_str = File.ReadAllText($"{Configuration.GameDataUnpackAssetBundleFolder}assetbundleKey.json");
								JsonArray assetbundlekey = JsonNode.Parse(assetbundlekey_str).AsArray();

								foreach (var item in assetbundlekey)
								{
									AssetBundleKeyList.Add(item["id"].ToString(), item["decryptKey"].ToString());
								}
							}
							else
							{
								Console.WriteLine("先解析AssetBundle&Key");
							}

							if (File.Exists($"{Configuration.AssetsFolder.FullName}AssetListWithExtraKeyType.json"))
							{
								string assetbundleextrakeytype_str = File.ReadAllText($"{Configuration.AssetsFolder.FullName}AssetListWithExtraKeyType.json");
								JsonArray assetbundleextrakeytype = JsonNode.Parse(assetbundleextrakeytype_str).AsArray();
								foreach (var item in assetbundleextrakeytype)
								{
									AssetBundleWithExtraKey.Add(item["FileName"].ToString(), item["AssetName"].ToString());
								}
							}
							else
							{
								Console.WriteLine("先解析AssetStorage.txt");
							}
							break;
						}
					case 1:
						{
							if (decryptor.fileType == CatAndMouseGame.FileType.JP)
							{
								if (AssetBundleKeyList.Count == 0 || AssetBundleWithExtraKey.Count == 0)
								{
									Console.WriteLine("先载入assetbundleinfo");
									break;
								}
							}

							foreach (FileInfo file in Configuration.DecryptedFolder.GetFiles("*.bin"))
							{
								Console.WriteLine("Encrypt: " + file.FullName);
								byte[] raw = File.ReadAllBytes(file.FullName);
								byte[] output = null;
								string keyType;
								string key;
								if (AssetBundleWithExtraKey.TryGetValue(file.Name, out keyType))
								{
									if (AssetBundleKeyList.TryGetValue(keyType, out key))
									{
										output = decryptor.CatGame4(raw, key);
									}
									else
									{
										Console.WriteLine($"No such value for this key type: {keyType}");
									}
								}
								else
								{
									output = decryptor.CatGame4(raw);
								}
								File.WriteAllBytes($"{Configuration.EncryptedFolder}{file.Name}", output);
							}
							break;
						}
					case 2:
						{
							if (decryptor.fileType == CatAndMouseGame.FileType.JP)
							{
								if (AssetBundleKeyList.Count == 0 || AssetBundleWithExtraKey.Count == 0)
								{
									Console.WriteLine("先载入assetbundleinfo");
									break;
								}
							}

							foreach (FileInfo file in Configuration.AssetsFolder.GetFiles("*.bin"))
							{
								Console.WriteLine("Decrypt: " + file.FullName);
								byte[] raw = File.ReadAllBytes(file.FullName);
								byte[] output = null;
								string keyType;
								string key;
								if (AssetBundleWithExtraKey.TryGetValue(file.Name, out keyType))
								{
									if (AssetBundleKeyList.TryGetValue(keyType, out key))
									{
										output = decryptor.MouseGame4(raw, key);
									}
									else
									{
										Console.WriteLine($"No such value for this key type: {keyType}");
									}
								}
								else
								{
									output = decryptor.MouseGame4(raw);
								}
								File.WriteAllBytes($"{Configuration.DecryptedFolder.FullName}{file.Name}", output);
							}
							break;
						}
					case 3:
						{
							HttpClient Client = new();
							var Response = Client.GetAsync("https://game.fate-go.jp/gamedata/top?appVer=0.0");
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
								}
								else
								{
									throw new Exception(res["response"][0]["fail"]["detail"].ToString());
								}
							}
							File.WriteAllText(Configuration.GameDataRawPath, Result);
							Console.WriteLine($"Writing file to: {Configuration.GameDataRawPath}");
							break;
						}
					case 4:
						{
							if (File.Exists($"{Configuration.GameDataUnpackAssetBundleFolder}assetbundle.json"))
							{
								JsonObject assetbundle = JsonNode.Parse(File.ReadAllText($"{Configuration.GameDataUnpackAssetBundleFolder}assetbundle.json")).AsObject();

								string folderName = assetbundle["folderName"].ToString();
								string DownloadURL = $"https://cdn.data.fate-go.jp/AssetStorages/{folderName}Android/AssetStorage.txt";

								HttpClient Client = new();
								Console.WriteLine("Downloading AssetStorage...");
								var Response = Client.GetAsync(DownloadURL);
								string Result = await Response.Result.Content.ReadAsStringAsync();

								string DecryptedData = decryptor.MouseGame8(Result);
								File.WriteAllText($"{Configuration.AssetsFolder.FullName}AssetStorage_dec.txt", DecryptedData);
								Console.WriteLine($"Writing file to: {Configuration.AssetsFolder.FullName}AssetStorage_dec.txt");
							}
							else
							{
								Console.WriteLine("先解析AssetBundle&Key");
							}
							break;
						}
					case 5:
						{
							if (File.Exists($"{Configuration.AssetsFolder.FullName}AssetStorage.txt"))
							{
								string data = File.ReadAllText($"{Configuration.AssetsFolder.FullName}AssetStorage.txt");
								string DecryptedData = decryptor.MouseGame8(data);
								File.WriteAllText($"{Configuration.AssetsFolder.FullName}AssetStorage_dec.txt", DecryptedData);
								Console.WriteLine($"Writing file to: {Configuration.AssetsFolder.FullName}AssetStorage_dec.txt");
							}
							else
							{
								Console.WriteLine("先下载或放置AssetStorage.txt");
							}
							break;
						}
					case 6:
						{
							Console.WriteLine("Parsing AssetStorage...");
							List<Asset> AudioList = new();
							List<Asset> AssetList = new();
							List<Asset> MovieList = new();
							List<Asset> AssetListWithExtraKeyType = new();
							string[] AssetStore = File.ReadAllLines($"{Configuration.AssetsFolder.FullName}AssetStorage_dec.txt");

							for (int i = 2; i < AssetStore.Length; ++i)
							{
								string[] tmp = AssetStore[i].Split(',');
								string assetName;
								string fileName;
								string keyType = "";

								if (tmp[0] == "1")
								{
									// FGO download from server.
									if (tmp[4].Contains("Audio"))
									{
										assetName = tmp[4].Replace('/', '@');
										fileName = CatAndMouseGame.GetMD5String(assetName);
										AudioList.Add(new Asset
										{
											AssetName = assetName,
											FileName = fileName,
										});
									}
									else if (tmp[4].Contains("Movie"))
									{
										assetName = tmp[4].Replace('/', '@');
										fileName = CatAndMouseGame.GetMD5String(assetName);
										MovieList.Add(new Asset
										{
											AssetName = assetName,
											FileName = fileName,
										});
									}
									else
									{
										assetName = tmp[4].Replace('/', '@') + ".unity3d";
										fileName = CatAndMouseGame.GetShaName(assetName);

										if (tmp.Length == 6)
										{
											keyType = tmp[5];
											AssetListWithExtraKeyType.Add(new Asset
											{
												AssetName = keyType,
												FileName = fileName,
											});
										}

										AssetList.Add(new Asset
										{
											AssetName = assetName,
											FileName = fileName,
										});
									}
								}
								else if (tmp.Length == 5)
								{
									// FGO cfb1d36393fd67385e046b084b7cf7ed
									assetName = assetName = tmp[4].Replace('/', '@');
									fileName = assetName;
									Asset asset = new() { AssetName = assetName, FileName = fileName };

									if (tmp[4].Contains("Audio"))
									{
										asset.FileName = CatAndMouseGame.GetMD5String(asset.FileName);
										AudioList.Add(asset);
									}
									else if (tmp[4].Contains("Movie"))
									{
										asset.FileName = CatAndMouseGame.GetMD5String(asset.FileName);
										MovieList.Add(asset);
									}
									else
									{
										asset.FileName = CatAndMouseGame.GetShaName(asset.FileName + ".unity3d");
										AssetList.Add(asset);
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
									else if (tmp[4].Contains("Movie"))
									{
										MovieList.Add(asset);
									}
									else
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
							File.WriteAllText($"{Configuration.AssetsFolder.FullName}AudioName.json", result);

							result = JsonSerializer.Serialize(AssetList, options);
							Console.WriteLine("Writing file to: AssetName.json");
							File.WriteAllText($"{Configuration.AssetsFolder.FullName}AssetName.json", result);

							result = JsonSerializer.Serialize(MovieList, options);
							Console.WriteLine("Writing file to: MovieName.json");
							File.WriteAllText($"{Configuration.AssetsFolder.FullName}MovieName.json", result);

							result = JsonSerializer.Serialize(AssetListWithExtraKeyType, options);
							Console.WriteLine("Writing file to: AssetListWithExtraKeyType.json");
							File.WriteAllText($"{Configuration.AssetsFolder.FullName}AssetListWithExtraKeyType.json", result);
							break;
						}
					case 7:
						{
							if (File.Exists($"{Configuration.GameDataRawPath}"))
							{
								string raw_str = File.ReadAllText($"{Configuration.GameDataRawPath}");
								JsonObject res = JsonNode.Parse(raw_str).AsObject();

								ValueTuple<string, string, string, string>[] RawData = new ValueTuple<string, string, string, string>[]
								{
									new ("assetbundle", "", Configuration.GameDataAssetBundlePath, UniversalUnpacker.AssetBundleKey),
									new ("assetbundleKey", "", Configuration.GameDataAssetBundleKeyPath, UniversalUnpacker.AssetBundleKey)
								};

								for (int i = 0; i < RawData.Length; i++)
								{
									if (File.Exists($"{RawData[i].Item3}"))
									{
										RawData[i].Item2 = File.ReadAllText(RawData[i].Item3);
									}
									else
									{
										RawData[i].Item2 = res["response"][0]["success"][RawData[i].Item1].ToString();
										File.WriteAllText(RawData[i].Item3, RawData[i].Item2);
									}
								}

								{
									Dictionary<string, object> BundleData = (Dictionary<string, object>)UniversalUnpacker.Unpack(Convert.FromBase64String(RawData[0].Item2), RawData[0].Item4);
									var options = new JsonSerializerOptions
									{
										WriteIndented = true,
										Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
									};
									string result = JsonSerializer.Serialize(BundleData, options);
									File.WriteAllText($"{Configuration.GameDataUnpackAssetBundleFolder}assetbundle.json", result);
									Console.WriteLine($"Writing file to: {Configuration.GameDataUnpackAssetBundleFolder}assetbundle.json");
								}

								{
									List<object> KeyData = (List<object>)UniversalUnpacker.Unpack(Convert.FromBase64String(RawData[1].Item2), RawData[1].Item4);
									var options = new JsonSerializerOptions { WriteIndented = true };
									string result = JsonSerializer.Serialize(KeyData, options);
									File.WriteAllText($"{Configuration.GameDataUnpackAssetBundleFolder}assetbundleKey.json", result);
									Console.WriteLine($"Writing file to: {Configuration.GameDataUnpackAssetBundleFolder}assetbundleKey.json");
								}
							}
							else
							{
								Console.WriteLine("先从服务器下载游戏数据");
							}
							break;
						}
					case 8:
						{
							if (File.Exists($"{Configuration.GameDataRawPath}"))
							{
								string raw_str = File.ReadAllText($"{Configuration.GameDataRawPath}");
								JsonObject res = JsonNode.Parse(raw_str).AsObject();

								ValueTuple<string, string, string, string>[] RawData = new ValueTuple<string, string, string, string>[]
								{
									new ("master", "", Configuration.GameDataMasterPath, UniversalUnpacker.MasterKey)
								};

								for (int i = 0; i < RawData.Length; i++)
								{
									if (File.Exists($"{RawData[i].Item3}"))
									{
										RawData[i].Item2 = File.ReadAllText(RawData[i].Item3);
									}
									else
									{
										RawData[i].Item2 = res["response"][0]["success"][RawData[i].Item1].ToString();
										File.WriteAllText(RawData[i].Item3, RawData[i].Item2);
									}
								}

								{
									Dictionary<string, object> MasterData = (Dictionary<string, object>)UniversalUnpacker.Unpack(Convert.FromBase64String(RawData[0].Item2), RawData[0].Item4);
									foreach (var item in MasterData)
									{
										var options = new JsonSerializerOptions
										{
											WriteIndented = true,
											Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
										};
										string result = JsonSerializer.Serialize(item.Value, options);
										File.WriteAllText($"{Configuration.GameDataUnpackFolder}{item.Key}", result);
										Console.WriteLine($"Writing file to: {Configuration.GameDataUnpackFolder}{item.Key}");
									}
								}
							}
							else
							{
								Console.WriteLine("没有游戏数据，请先下载");
							}
							break;
						}
					case 9:
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
					case 10:
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
					case 11:
						{
							string JPText = File.ReadAllText(Configuration.DecryptedFolder.FullName + "JP.txt");
							string CNText = File.ReadAllText(Configuration.DecryptedFolder.FullName + "CN.txt");

							JPText = Regex.Replace(JPText, "\"\n", "\",\n");
							JPText = Regex.Replace(JPText, "\",,\n", "\",\n");
							JPText = JPText.Replace("\u00A0", "");
							JPText = Regex.Replace(JPText, "\",\n\n}", "\"\n}");

							var options = new JsonSerializerOptions
							{
								WriteIndented = true,
								Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
								ReadCommentHandling = JsonCommentHandling.Skip,
								AllowTrailingCommas = true // Not working
							};
							var JP = JsonSerializer.Deserialize<Dictionary<string, string>>(JPText, options);
							JsonObject CN = JsonNode.Parse(CNText).AsObject();
							JsonObject NoTranslation = new();
							foreach (var node in JP)
							{
								if (CN[node.Key] != null)
								{
									JP[node.Key] = CN[node.Key].ToString();
								}
								else
								{
									NoTranslation.Add(node.Key, node.Value);
								}
							}
							string LocalizationJpn = JsonSerializer.Serialize(JP, options);
							string Non_Translation = JsonSerializer.Serialize(NoTranslation, options);
							File.WriteAllText(Configuration.DecryptedFolder.FullName + "LocalizationJpn.txt", LocalizationJpn);
							File.WriteAllText(Configuration.DecryptedFolder.FullName + "Non-Translation.txt", Non_Translation);
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