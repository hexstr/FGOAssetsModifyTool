using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
namespace FGOAssetsModifyTool
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1: Encrypt\t2:Decrypt\t3: Display_name\n4: Repack\t5: Decrypt AssetStorage.txt\t6: Encrpyt AssetStorage.txt\t7: Export AssetStorage to json\t8: Calculate files crc");
                int arg = Convert.ToInt32(Console.ReadLine());
                string path = System.IO.Directory.GetCurrentDirectory();
                DirectoryInfo folder = new DirectoryInfo(path + @"\com.aniplex.fategrandorder\files\data\d713\");
                DirectoryInfo needEncrypt = new DirectoryInfo(path + @"\Android\");
                byte[] raw;
                byte[] output;
                switch(arg)
                {
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
                                    stream.Seek(0x50, SeekOrigin.Begin);
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
                            byte[] data = new byte[64];
                            string filename = "";
                            folder = new DirectoryInfo(path + @"\Android\");
                            foreach (FileInfo file in folder.GetFiles("*.bin"))
                            {
                                Console.WriteLine("Reading file from: " + file.FullName);
                                using (var stream = File.OpenRead(file.FullName))
                                {
                                    stream.Seek(0x4B, SeekOrigin.Begin);
                                    stream.Read(data, 0, data.Length);
                                    for (int i = 0; i < 64; ++i)
                                    {
                                        if (data[i] == 0x00)
                                        {
                                            filename = Encoding.UTF8.GetString(data, 0, i);
                                            break;
                                        }
                                    }
                                    stream.Seek(0, SeekOrigin.Begin);
                                    filename = filename.Replace("CAB-", "");
                                    Console.WriteLine("Encrypt: " + filename);
                                    raw = new byte[stream.Length];
                                    stream.Read(raw, 0, (int)stream.Length);
                                    output = CatAndMouseGame.CatGame4(raw);
                                    filename = getShaName(filename + ".unity3d");
                                    if (!Directory.Exists(path + @"\Modify\"))
                                        Directory.CreateDirectory(path + @"\Modify\");
                                    Console.WriteLine("Writing file to: " + path + @"\Modify\" + filename);
                                    File.WriteAllBytes(path + @"\Modify\" + filename, output);
                                }
                            }
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
                            Console.WriteLine(@"Read from \Android\AssetStorage_mod.txt");
                            string loadData = File.ReadAllText(path + @"\Android\AssetStorage_mod.txt");
                            string tmp;
                            int num2 = loadData.IndexOfAny(new char[] { '\r','\n' });
                            if (num2 > 1)
                            {
                                string text = loadData.Substring(0, num2);
                                text = text.Substring(1);
                                Console.WriteLine("old Crc: " + text);
                                tmp = loadData.Substring(num2 + 1);
                                byte[] bytes = Encoding.UTF8.GetBytes(tmp);
                                uint num3 = Crc32.Compute(bytes);
                                Console.WriteLine("new Crc: " + num3);
                                loadData = loadData.Replace(text, num3.ToString());
                            }
                            loadData = CatAndMouseGame.CatGame8(loadData);
                            if (!Directory.Exists(path + @"\Encrypt\"))
                                Directory.CreateDirectory(path + @"\Encrypt\");
                            File.WriteAllText(path + @"\Encrypt\AssetStorage.txt", loadData);
                            Console.WriteLine("Writing file to: " + path + @"\Encrypt\AssetStorage.txt");
                            break;
                        }
                    case 7:
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
                        case 8:
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
                    default:
                        {
                            Console.WriteLine("3: Export asset bundle name to json\n4: Encrypt and rename to 'ShaName.bin'\n6: Parse item from AssetStorage file");
                            break;
                        }
                }
                Console.WriteLine("pause...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey(true);
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
    }
}