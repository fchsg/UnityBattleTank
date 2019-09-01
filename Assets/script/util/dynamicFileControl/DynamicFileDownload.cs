using UnityEngine;
using System.Collections;
using System.IO;

public class DynamicFileDownload {

	public enum STATE
	{
		IDLE,
		DOWNLOAD,
		COMPLETE,
		ERROR,
	}

	private STATE _state = STATE.IDLE;
	public STATE state
	{
		get { return _state; }
	}

	private string _name;
	private string _url;
	private SlgPB.InitResponse _response;

	public DynamicFileDownload(string url, SlgPB.InitResponse response)
	{
		_url = url;
		_response = response;

		int index = url.LastIndexOf ("/");
		_name = _url.Substring (index + 1);
	}

	private string GetLocalDynamicUrl(string name)
	{
		string localUrl = DynamicFileControl.GetDynamicDataFolder() + AppConfig.FOLDER_DATACONFIG + name;
		return localUrl;
	}

	public void Download()
	{
		Assert.assert (_state == STATE.IDLE);

		if (AppConfig.USE_DYNAMIC_CONFIG) {
			string localUrl = GetLocalDynamicUrl (_name);
			if (IsFileExisting (localUrl)) {
				Trace.trace ("=====================", Trace.CHANNEL.DYNAMIC_DATACONFIG);
				Trace.trace ("use local data config", Trace.CHANNEL.DYNAMIC_DATACONFIG);
				Trace.trace ("=====================", Trace.CHANNEL.DYNAMIC_DATACONFIG);
				_state = STATE.COMPLETE;
			} else {
				StartDownload ();
			}
		} else {
			Trace.trace ("==============================", Trace.CHANNEL.DYNAMIC_DATACONFIG);
			Trace.trace ("ignore dynamic config checking", Trace.CHANNEL.DYNAMIC_DATACONFIG);
			Trace.trace ("==============================", Trace.CHANNEL.DYNAMIC_DATACONFIG);
			_state = STATE.COMPLETE;
		}
	}

	private void StartDownload()
	{
		_state = STATE.DOWNLOAD;

		HttpClient client = HttpClientHelper.GetHttpComponent (false);
		client.Send (_url, new byte[]{0}, OnFinishDownload);

	}

	private void OnFinishDownload(bool success, byte[] data)
	{
		if (!success) {
			_state = STATE.ERROR;
			return;
		}

		if(!Decompress (data))
		{
			_state = STATE.ERROR;
			return;
		}
		
		if(!SaveFile(data, _name))
		{
			_state = STATE.ERROR;
			return;
		}

		_state = STATE.COMPLETE;
		Trace.trace ("=============================", Trace.CHANNEL.DYNAMIC_DATACONFIG);
		Trace.trace ("Success download data configs", Trace.CHANNEL.DYNAMIC_DATACONFIG);
		Trace.trace ("=============================", Trace.CHANNEL.DYNAMIC_DATACONFIG);
	}

	private bool Decompress(byte[] data)
	{
		for(int i = 0; i < _response.fileDesc.Count; ++i)
		{
			SlgPB.FileDesc desc = _response.fileDesc[i];

			MemoryStream stream = new MemoryStream();
			stream.Write(data, desc.start, desc.length);

			try
			{
				byte[] buffer = stream.ToArray();
				buffer = GZipHelper.Decompress(buffer);
				if(buffer != null)
				{
					if(!SaveFile(buffer, desc.name))
					{
						return false;
					}
				}
			}
			catch(System.Exception e)
			{
				return false;
			}
		}

		return true;
	}

	private bool SaveFile(byte[] data, string name)
	{
		try
		{
			string localUrl = GetLocalDynamicUrl (name);
			FileStream stream = new FileStream(localUrl, FileMode.OpenOrCreate);
			stream.Write(data, 0, data.Length);
			stream.Close();
			return true;
		}
		catch(System.Exception e)
		{
			Trace.trace(e.ToString(), Trace.CHANNEL.IO);
			return false;
		}
	}

	public static bool IsFileExisting(string fileUrl)
	{
		try
		{
			FileStream stream = new FileStream(fileUrl, FileMode.Open);
			stream.Close();
			return true;
		}
		catch(System.Exception e)
		{
			return false;
		}
	}
	


}
