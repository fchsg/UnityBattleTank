using UnityEngine;
using System.Collections;
using System.IO;

public class StringHelper {

	public static string ReadFromBytes(byte[] bytes)
	{
		MemoryStream stream = new MemoryStream (bytes);
		StreamReader reader = new StreamReader (stream);
		string content = reader.ReadToEnd ();
		reader.Close ();
		stream.Close ();
		return content;
	}


	public static int[] ReadIntArrayFromString(string str, char split = '/')
	{
		if (str != null) {
			string[] temp = str.Split (split);
			
			int[] array = new int[temp.Length];
			for (int i = 0; i < array.Length; i++) {
				array[i] = int.Parse(temp[i]);
			}
			
			return array;
		}

		return new int[0];
	}

	public static float[] ReadFloatArrayFromString(string str, char split = '/')
	{
		if (str != null) {
			string[] temp = str.Split (split);
			
			float[] array = new float[temp.Length];
			for (int i = 0; i < array.Length; i++) {
				array[i] = float.Parse(temp[i]);
			}
			
			return array;
		}

		return new float[0];
	}

	public static string GenerateCompleteDescription(string str, params System.Object[] parameter)
	{
		if (str != null)
		{
			int n = parameter.Length;
			for (int i = 0; i < n; ++i) 
			{
				string param = parameter [i] + "";
				if (param != "0") 
				{
					string token = (i +1) + "%";
					str = str.Replace (token, param);
				}
			}
		}

		return str;
	}
	
}
