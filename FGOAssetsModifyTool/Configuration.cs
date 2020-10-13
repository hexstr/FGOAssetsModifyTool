using System.IO;

namespace FGOAssetsModifyTool
{
	public static class Configuration
	{
		public static string NowPath = Directory.GetCurrentDirectory();
		public static DirectoryInfo AssetsFolder = new DirectoryInfo(NowPath + @"\Android\");
		public static DirectoryInfo ScriptsFolder = new DirectoryInfo(NowPath + @"\Android\Scripts");
		public static string GameDataFolder = new DirectoryInfo(NowPath + @"\Android\gamedata\").FullName;
		public static string GameDataUnpackFolder = new DirectoryInfo(NowPath + @"\Android\gamedata\unpack_master\").FullName;
		public static DirectoryInfo DecryptedFolder = new DirectoryInfo(NowPath + @"\Decrypted\");
		public static string EncryptedFolder = new DirectoryInfo(NowPath + @"\Encrypted\").FullName;
		public static DirectoryInfo DecryptedScriptsFolder = new DirectoryInfo(NowPath + @"\DecryptedScripts\");
		public static string EncryptedScriptsFolder = new DirectoryInfo(NowPath + @"\EncryptedScripts\").FullName;

		static Configuration()
		{
			if (!Directory.Exists(AssetsFolder.FullName))
				Directory.CreateDirectory(AssetsFolder.FullName);

			if (!Directory.Exists(ScriptsFolder.FullName))
				Directory.CreateDirectory(ScriptsFolder.FullName);

			if (!Directory.Exists(GameDataFolder))
				Directory.CreateDirectory(GameDataFolder);

			if (!Directory.Exists(GameDataUnpackFolder))
				Directory.CreateDirectory(GameDataUnpackFolder);

			if (!Directory.Exists(DecryptedFolder.FullName))
				Directory.CreateDirectory(DecryptedFolder.FullName);

			if (!Directory.Exists(EncryptedFolder))
				Directory.CreateDirectory(EncryptedFolder);

			if (!Directory.Exists(DecryptedScriptsFolder.FullName))
				Directory.CreateDirectory(DecryptedScriptsFolder.FullName);

			if (!Directory.Exists(EncryptedScriptsFolder))
				Directory.CreateDirectory(EncryptedScriptsFolder);
		}
	}
}
