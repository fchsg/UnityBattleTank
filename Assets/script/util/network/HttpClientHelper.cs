using UnityEngine;
using System.Collections;

public class HttpClientHelper {

	private const string HTTP_OBJECT_PATH_NAME = "profab/system/PersistHTTPClient";

	public static GameObject GetHttpObject()
	{
		GameObject http = GameObject.FindGameObjectWithTag (AppConfig.TAB_HTTP);
		if (http == null) {
			Object profab = Resources.Load(HTTP_OBJECT_PATH_NAME);
			http = GameObject.Instantiate(profab) as GameObject;
			Assert.assert(http != null);

			MonoBehaviour.DontDestroyOnLoad(http);
		}
		return http;
	}

	public static HttpClient GetHttpComponent(bool canBeIgnore)
	{
		GameObject o = GetHttpObject ();
		HttpClient[] clients = o.GetComponents<HttpClient> ();
		foreach (HttpClient c in clients) {
			if(c.canBeIgnore == canBeIgnore)
			{
				return c;
			}
		}

		return null;
	}

	public static void Send(string url, byte[] data, HttpClient.DelegateRequestCallback callback, bool canBeIgnore = false)
	{
		HttpClient c = GetHttpComponent (canBeIgnore);
		if (c != null) {
			c.Send(url, data, callback);
		}
	}

}
