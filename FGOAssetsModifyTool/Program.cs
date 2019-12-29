using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
namespace FGOAssetsModifyTool
{
    class Program
    {
        static void displayMenu()
        {
            try
            {
                Console.WriteLine(
                    "1: 加密\t" +
                    "2: 解密\n" +
                    "3: 加密AssetStorage.txt\t" +
                    "4: 解密AssetStorage.txt\t" +
                    "5: 把AssetStorage转换为Json格式\n" +
                    "6: 加密剧情文本(scripts)\n" +
                    "7: 解密剧情文本(scripts)\n" +
                    "8: 把国服文本转换为日服适用\n" +
                    "9: 计算CRC值\n" +
                    "0: 导出资源名 - 实际文件名\n" +
                    "11: [gamedata/top]解密master(空的)\n" +
                    "12: [gamedata/top]解密assetbundle(文件夹名)\n" +
                    "13: [gamedata/top]解密webview(url)\n" +
                    "69: 切换为美服密钥\n" +
                    "67: 切换为国服密钥");
                int arg = Convert.ToInt32(Console.ReadLine());
                string path = System.IO.Directory.GetCurrentDirectory();
                DirectoryInfo folder = new DirectoryInfo(path + @"\Android\");
                DirectoryInfo decrypt = new DirectoryInfo(path + @"\Decrypt\");
                DirectoryInfo encrypt = new DirectoryInfo(path + @"\Encrypt\");
                DirectoryInfo decryptScripts = new DirectoryInfo(path + @"\DecryptScripts\");
                DirectoryInfo encryptScripts = new DirectoryInfo(path + @"\EncryptScripts\");
                byte[] raw;
                byte[] output;
                switch (arg)
                {
                    case 69:
                        {
                            CatAndMouseGame.EN();
                            displayMenu();
                            break;
                        }
                    case 67:
                        {
                            CatAndMouseGame.CN();
                            displayMenu();
                            break;
                        }
                    case 1:
                        {
                            foreach (FileInfo file in decrypt.GetFiles("*.bin"))
                            {
                                Console.WriteLine("Encrypt: " + file.FullName);
                                raw = File.ReadAllBytes(file.FullName);
                                output = CatAndMouseGame.CatGame4(raw);
                                if (!Directory.Exists(encrypt.FullName))
                                    Directory.CreateDirectory(encrypt.FullName);
                                File.WriteAllBytes(encrypt.FullName + file.Name, output);
                            }
                            break;
                        }
                    case 2:
                        {
                            foreach (FileInfo file in folder.GetFiles("*.bin"))
                            {
                                Console.WriteLine("Decrypt: " + file.FullName);
                                raw = File.ReadAllBytes(file.FullName);
                                output = CatAndMouseGame.MouseGame4(raw);
                                if (!Directory.Exists(decrypt.FullName))
                                    Directory.CreateDirectory(decrypt.FullName);
                                File.WriteAllBytes(decrypt.FullName + file.Name, output);
                            }
                            break;
                        }
                    case 3:
                        {
                            string data = File.ReadAllText(folder.FullName + "AssetStorage_dec.txt");
                            //string tmp = data;
                            //tmp = tmp.Trim(new char[]
                            //{
                            //    '﻿'
                            //});
                            //int ri = data.IndexOfAny(new char[]
                            //{
                            //    '\r',
                            //    '\n'
                            //});
                            //if (ri > 1)
                            //{
                            //    string crcString = tmp.Substring(0, ri);
                            //    if (crcString.StartsWith("~"))
                            //    {
                            //        crcString = crcString.Substring(1);
                            //        Console.WriteLine("OldAssetStorageCrc: " + crcString);
                            //        tmp = tmp.Substring(ri + 1);
                            //        byte[] readData = Encoding.UTF8.GetBytes(tmp);
                            //        uint crc = Crc32.Compute(readData);
                            //        Console.WriteLine("AssetStorageCrc: " + crc);
                            //        data = data.Replace(crcString.ToString(), crc.ToString());
                            //    }
                            //}
                            string loadData = CatAndMouseGame.CatGame8(data);
                            File.WriteAllText(folder.FullName + "AssetStorage_enc.txt", loadData);
                            Console.WriteLine("Writing file to: " + folder.FullName + "AssetStorage_enc.txt");
                            break;
                        }
                    case 4:
                        {
                            string data = File.ReadAllText(folder.FullName + "AssetStorage.txt");
                            string loadData = CatAndMouseGame.MouseGame8(data);
                            File.WriteAllText(folder.FullName + "AssetStorage_dec.txt", loadData);
                            Console.WriteLine("Writing file to: " + folder.FullName + "AssetStorage_dec.txt");
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Reading file from: " + folder.FullName + "AssetStorage_dec.txt");
                            string loadData = File.ReadAllText(folder.FullName + "AssetStorage_dec.txt");
                            string[] listData = null;
                            loadData = loadData.Trim();
                            int num2 = loadData.IndexOfAny(new char[] { '\r', '\n' });
                            loadData = loadData.Substring(num2 + 1);
                            byte[] bytes = Encoding.UTF8.GetBytes(loadData);
                            listData = loadData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] array = listData[0].Split(',');
                            Console.WriteLine("Parsing json...");
                            StringBuilder stringBuilder = new StringBuilder("{");
                            int num4;
                            string attrib;
                            int size;
                            uint crc;
                            string name;
                            for (int i = 1; i < listData.Length; i++)
                            {
                                array = listData[i].Split(',');
                                if (array.Length != 5)
                                {
                                    break;
                                }
                                num4 = int.Parse(array[0].Trim());
                                stringBuilder.Append("{\"num4\":\"" + num4.ToString() + "\",");
                                attrib = array[1];
                                stringBuilder.Append("\"attrib\":\"" + attrib.ToString() + "\",");
                                size = int.Parse(array[2].Trim());
                                stringBuilder.Append("\"size\":\"" + size.ToString() + "\",");
                                crc = uint.Parse(array[3].Trim());
                                stringBuilder.Append("\"crc\":\"" + crc.ToString() + "\",");
                                name = array[4];
                                stringBuilder.Append("\"name\":\"" + name + "\"},");
                            }
                            stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            stringBuilder.Append("}");
                            File.WriteAllText(folder.FullName + "AssetStorage.json", stringBuilder.ToString());
                            Console.WriteLine("Writing file to: " + folder.FullName + "AssetStorage.json");
                            break;
                        }
                    case 6:
                        {
                            foreach (FileInfo file in decryptScripts.GetFiles("*.txt", SearchOption.AllDirectories))
                            {

                                Console.WriteLine("Encrypting: " + file.FullName);
                                string dePath = Path.GetFileNameWithoutExtension(file.Directory.Name);
                                string txt = File.ReadAllText(file.FullName);
                                string outputTxt = CatAndMouseGame.CatGame3(txt);
                                if (!Directory.Exists(encryptScripts.FullName + dePath))
                                    Directory.CreateDirectory(encryptScripts.FullName + dePath);
                                File.WriteAllText(encryptScripts.FullName + dePath + "\\" + file.Name, outputTxt);
                            }
                            break;
                        }
                    case 7:
                        {
                            foreach (FileInfo file in encryptScripts.GetFiles("*.txt", SearchOption.AllDirectories))
                            {

                                Console.WriteLine("Decrypting: " + file.FullName);
                                string dePath = Path.GetFileNameWithoutExtension(file.Directory.Name);
                                string txt = File.ReadAllText(file.FullName);
                                string outputTxt = CatAndMouseGame.MouseGame3(txt);
                                if (!Directory.Exists(encryptScripts.FullName + dePath))
                                    Directory.CreateDirectory(encryptScripts.FullName + dePath);
                                File.WriteAllText(encryptScripts.FullName + dePath + "\\" + file.Name, outputTxt);
                            }
                            break;
                        }
                    case 8:
                        {
                            JObject jp = JObject.Parse(File.ReadAllText(decrypt.FullName + "JP.txt"));
                            JObject cn = JObject.Parse(File.ReadAllText(decrypt.FullName + "CN.txt"));
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
                            File.WriteAllText(decrypt.FullName + "LocalizationJpn.txt", jp.ToString());
                            File.WriteAllText(decrypt.FullName + "noTranslation.txt", no.ToString());
                            break;
                        }
                    case 9:
                        {
                            try
                            {
                                foreach (FileInfo file in encrypt.GetFiles("*.bin"))
                                {
                                    Console.WriteLine(file.Name + "\r\nsize: " + file.Length + "\r\ncrc: " + Crc32.Compute(File.ReadAllBytes(file.FullName)));
                                    Console.WriteLine("======================================");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                            }
                            break;
                        }
                    case 0:
                        {
                            string[] assetStore = File.ReadAllLines(folder.FullName + "AssetStorage_dec.txt");
                            Console.WriteLine("Parsing json...");
                            StringBuilder stringBuilder = new StringBuilder("{");
                            for (int i = 2; i < assetStore.Length; ++i)
                            {
                                string[] tmp = assetStore[i].Split(',');
                                string assetName = tmp[tmp.Length - 1].Replace('/', '@') + ".unity3d";
                                string name = CatAndMouseGame.getShaName(assetName);
                                stringBuilder.Append("{\"assetName\":\"" + assetName + "\",");
                                stringBuilder.Append("\"fileName\":\"" + name + "\"},");
                            }
                            stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            stringBuilder.Append("}");
                            File.WriteAllText(folder.FullName + "AssetStorageName.json", stringBuilder.ToString());
                            break;
                        }
                    case 11:
                        {
                            string data = File.ReadAllText(folder.FullName + "master");
                            Dictionary<string, byte[]> dictionary = (Dictionary<string, byte[]>)MasterDataUnpakcer.MouseGame2Unpacker(Convert.FromBase64String(data));
                            break;
                        }
                    case 12:
                        {
                            string data = File.ReadAllText(folder.FullName + "assetbundle");
                            Dictionary<string, object> dictionary = (Dictionary<string, object>)MasterDataUnpakcer.MouseInfoMsgPack(Convert.FromBase64String(data));
                            Console.WriteLine("folder name: " + dictionary["folderName"].ToString());
                            break;
                        }
                    case 13:
                        {
                            string data = File.ReadAllText(folder.FullName + "webview");
                            Dictionary<string, object> dictionary = (Dictionary<string, object>)MasterDataUnpakcer.MouseGame2MsgPack(Convert.FromBase64String(data));
                            string str = "baseURL: " + dictionary["baseURL"].ToString() + "\r\ncontactURL: " + dictionary["contactURL"].ToString() + "\r\n";
                            Console.WriteLine(str);
                            Dictionary<string, object> filePassInfo = (Dictionary<string, object>)dictionary["filePass"];
                            foreach (var a in filePassInfo)
                            {
                                str += a.Key + ": " + a.Value.ToString() + "\r\n";
                            }
                            File.WriteAllText(folder.FullName + "filePassInfo.txt", str);
                            Console.WriteLine("Writing file to: " + folder.FullName + "filePassInfo.txt");
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
            displayMenu();
            Console.WriteLine("pause...");
            Console.ReadKey(true);
        }
    }
}