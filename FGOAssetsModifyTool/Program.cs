using System;
using System.Text;
using System.IO;
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
                    "1: Encrypt\t" +
                    "2: Decrypt\t" +
                    "3: Display_name\n" +
                    "4: Convert CN text to JP\t" +
                    "5: Decrypt AssetStorage.txt\t" +
                    "6: Export AssetStorage to json\n" +
                    "7: Calculate files crc\t" +
                    "8: Decrypt scripts\n" +
                    "0: Change to CN mode");
                int arg = Convert.ToInt32(Console.ReadLine());
                string path = System.IO.Directory.GetCurrentDirectory();
                //DirectoryInfo folder = new DirectoryInfo(path + @"\com.aniplex.fategrandorder\files\data\d713\");
                DirectoryInfo folder = new DirectoryInfo(path + @"\Android\");
                DirectoryInfo needEncrypt = new DirectoryInfo(path + @"\Decrypt\");
                byte[] raw;
                byte[] output;
                switch (arg)
                {
                    case 0:
                        {
                            CatAndMouseGame.isCN = true;
                            displayMenu();
                            break;
                        }
                    case 1:
                        {
                            foreach (FileInfo file in needEncrypt.GetFiles("*.bin"))
                            {
                                Console.WriteLine("Encrypt: " + file.FullName);
                                raw = File.ReadAllBytes(file.FullName);
                                output = CatAndMouseGame.CatGame4(raw);
                                if (!Directory.Exists(path + @"\Encrypt\"))
                                    Directory.CreateDirectory(path + @"\Encrypt\");
                                File.WriteAllBytes(path + @"\Encrypt\" + file.Name, output);
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
                                if (!Directory.Exists(path + @"\Decrypt\"))
                                    Directory.CreateDirectory(path + @"\Decrypt\");
                                File.WriteAllBytes(path + @"\Decrypt\" + file.Name, output);
                            }
                            break;
                        }
                    case 3:
                        {
                            byte[] data = new byte[64];
                            folder = new DirectoryInfo(path + @"\Decrypt\");
                            StringBuilder stringBuilder = new StringBuilder("{");
                            foreach (FileInfo file in folder.GetFiles("*.bin"))
                            {
                                Console.WriteLine("Reading file from 'Decrypt': " + file.FullName);
                                using (var stream = File.OpenRead(file.FullName))
                                {
                                    stream.Seek(0x4B, SeekOrigin.Begin);
                                    stream.Read(data, 0, data.Length);
                                    for (int i = 0; i < 64; ++i)
                                    {
                                        if (data[i] == 0x00)
                                        {
                                            stringBuilder.Append("{\"hash\":\"" + file.Name + "\",");
                                            stringBuilder.Append("\"name\":\"" + Encoding.UTF8.GetString(data, 0, i) + "\"},");
                                            break;
                                        }
                                    }
                                }
                            }
                            string result = stringBuilder.ToString().TrimEnd(',');
                            result += "}";
                            File.WriteAllText(path + @"\Android\Name.json", result);
                            break;
                        }
                    case 4:
                        {
                            JObject jp = JObject.Parse(File.ReadAllText(needEncrypt + "JP.txt"));
                            JObject cn = JObject.Parse(File.ReadAllText(needEncrypt + "CN.txt"));
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
                            File.WriteAllText(needEncrypt + "LocalizationJpn.txt", jp.ToString());
                            File.WriteAllText(needEncrypt + "none.txt", no.ToString());
                            break;
                        }
                    case 5:
                        {
                            string data = File.ReadAllText(path + @"\Android\AssetStorage.txt");
                            string loadData = CatAndMouseGame.MouseGame8(data);
                            File.WriteAllText(path + @"\Android\AssetStorage_dec.txt", loadData);
                            Console.WriteLine("Writing file to: " + path + @"\Android\AssetStorage_dec.txt");
                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("Reading file from: " + path + @"\Android\AssetStorage_dec.txt");
                            string loadData = File.ReadAllText(path + @"\Android\AssetStorage_dec.txt");
                            string[] listData = null;
                            loadData = loadData.Trim();
                            int num2 = loadData.IndexOfAny(new char[] { '\r', '\n' });
                            loadData = loadData.Substring(num2 + 1);
                            byte[] bytes = Encoding.UTF8.GetBytes(loadData);
                            listData = loadData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] array = listData[0].Split(',');
                            Console.WriteLine("Parsing  json...");
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
                            string json = stringBuilder.ToString().TrimEnd(',');
                            json += "}";
                            File.WriteAllText(path + @"\Android\AssetStorage.json", json);
                            Console.WriteLine("Writing file to: " + path + @"\Android\AssetStorage.json");
                            break;
                        }
                    case 7:
                        {
                            try
                            {
                                folder = new DirectoryInfo(path + @"\Encrypt\");
                                foreach (FileInfo file in folder.GetFiles("*.bin"))
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
                    case 8:
                        {
                            folder = new DirectoryInfo(path + @"\EncryptScript\");
                            System.Collections.Generic.List<DirectoryInfo> paths = new System.Collections.Generic.List<DirectoryInfo>();
                            foreach (DirectoryInfo NextFolder in folder.GetDirectories())
                            {
                                paths.Add(NextFolder);
                            }
                            foreach (DirectoryInfo NextFile in paths)
                            {
                                foreach (FileInfo file in NextFile.GetFiles("*.txt"))
                                {
                                    Console.WriteLine("Decrypt: " + file.FullName);
                                    string txt = File.ReadAllText(file.FullName);
                                    string outputTxt = CatAndMouseGame.MouseGame3(txt);
                                    if (!Directory.Exists(path + @"\DecryptScript\" + NextFile.Name))
                                        Directory.CreateDirectory(path + @"\DecryptScript\" + NextFile.Name);
                                    File.WriteAllText(path + @"\DecryptScript\" + NextFile.Name + '\\' + file.Name, outputTxt);
                                }
                            }
                            foreach (FileInfo file in folder.GetFiles("*.txt"))
                            {
                                Console.WriteLine("Decrypt: " + file.FullName);
                                string txt = File.ReadAllText(file.FullName);
                                string outputTxt = CatAndMouseGame.MouseGame3(txt);
                                if (!Directory.Exists(path + @"\DecryptScript\"))
                                    Directory.CreateDirectory(path + @"\DecryptScript\");
                                File.WriteAllText(path + @"\DecryptScript\" + file.Name, outputTxt);
                            }
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Select a item pls");
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