using UnityEngine;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;

public class GZipHelper {


	public static byte[] Compress(byte[] binary)
	{
		MemoryStream stream = new MemoryStream();
		GZipOutputStream zipStream = new GZipOutputStream(stream);
		zipStream.Write(binary, 0, binary.Length);
		zipStream.Close();
		byte[] compress = stream.ToArray();
		return compress;
	}


	public static byte[] Decompress(byte[] binary)
	{
		GZipInputStream zipStream = new GZipInputStream(new MemoryStream(binary));
		
		MemoryStream stream = new MemoryStream();
		byte[] buffer = new byte[4096];
		while (true) {
			int count = zipStream.Read(buffer, 0, buffer.Length);
			if(count != 0)
			{
				stream.Write(buffer, 0, count);
			}
			else
			{
				break;
			}
		}
		byte[] decompress = stream.ToArray();
		return decompress;
	}
	
}
