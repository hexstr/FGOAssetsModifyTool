using System;
using System.Text;

namespace FGOAssetsModifyTool
{
	internal class UniversalUnpacker
	{
		public static string MasterKey = "pX6q6xK2UymhFKcaGHHUlfXqfTsWF0uH";
		public static string AssetBundleKey = "W0Juh4cFJSYPkebJB9WpswNF51oa6Gm7";
		protected static byte[] InfoTop = new byte[32];
		protected static byte[] InfoData = new byte[32];

		public static object Unpack(byte[] data, string key)
		{
			var array = new byte[data.Length - 32];
			InfoData = Encoding.UTF8.GetBytes(key);
			Array.Copy(data, 0, InfoTop, 0, 32);
			Array.Copy(data, 32, array, 0, data.Length - 32);

			var buf = CatAndMouseGame.MouseHomeMain(array, InfoData, InfoTop, true);
			return new MiniMessagePacker().Unpack(buf);
		}
	}
}
