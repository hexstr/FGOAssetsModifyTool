using System;
using System.Text;
namespace FGOAssetsModifyTool
{
    public static class FNV1a
    {
	    public static uint Hash32(byte[] bytes, int offset, int len, uint hash = 2166136261u)
	    {
		    for (int i = offset; i < len; i++)
		    {
			    hash = (hash ^ (uint)bytes[i]) * 16777619u;
		    }
		    return hash;
	    }
	    public static ulong Hash64(byte[] bytes, int offset, int len, ulong hash = 14695981039346656037UL)
	    {
		    for (int i = offset; i < len; i++)
		    {
			    hash = (hash ^ (ulong)bytes[i]) * 1099511628211UL;
		    }
		    return hash;
	    }
	    public static uint Hash32(string str)
	    {
		    byte[] bytes = Encoding.UTF8.GetBytes(str);
		    return FNV1a.Hash32(bytes, 0, bytes.Length, 2166136261u);
	    }
	    public const uint FnvOffsetBasis32 = 2166136261u;
	    public const ulong FnvOffsetBasis64 = 14695981039346656037UL;
	    private const uint FnvPrime32 = 16777619u;
	    private const ulong FnvPrime64 = 1099511628211UL;
    }
}