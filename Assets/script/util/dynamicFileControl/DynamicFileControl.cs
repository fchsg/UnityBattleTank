using UnityEngine;
using System.Collections;
using System.IO;


public class DynamicFileControl {

	private static string DATA_FOLDER = "/DynamicData/";

	public static byte[] QueryFileContent(string fileName)
	{
		byte[] bin = null;
		if (AppConfig.USE_DYNAMIC_CONFIG) {
			bin = TryLoadFileContent (GetDynamicDataFolder () + fileName);
		}
		if(bin == null)
		{
			int n = fileName.IndexOf(".");
			string name = fileName.Substring(0, n);

			System.Object obj = Resources.Load(name);
			if(obj is TextAsset)
			{
				bin = ((TextAsset)obj).bytes;
			}

			if(bin == null)
			{
				Trace.trace("unsupport file type, name = " + fileName, Trace.CHANNEL.IO);
			}
		}

		return bin;
	}

	public static string GetDynamicDataFolder()
	{
		string directory;
		
#if UNITY_EDITOR
		directory = Application.dataPath + DATA_FOLDER;
#elif UNITY_STANDALONE
		directory = Application.dataPath + DATA_FOLDER;
#else
		directory = Application.persistentDataPath + DATA_FOLDER;	
#endif

		return directory;
	}

	public static byte[] TryLoadFileContent(string fileUrl)
	{
		string[] dirs = fileUrl.Split('/');
		int count = dirs.Length - 1;
		
		string tempDir = "";
		for(int i = 0; i < count; ++i)
		{
			tempDir += "/" + dirs[i];

			try
			{
				if (!Directory.Exists(tempDir))
					Directory.CreateDirectory(tempDir);
			}
			catch(System.Exception e)
			{
			}
		}

		byte[] bin = null;
		
		try
		{
			FileStream stream = new FileStream (fileUrl, FileMode.Open);
			stream.Position = 0;
			bin = new byte[stream.Length];
			stream.Read(bin, 0, bin.Length);
			stream.Close();
		}
		catch(System.Exception e)
		{
		}

		return bin;
	}


}

