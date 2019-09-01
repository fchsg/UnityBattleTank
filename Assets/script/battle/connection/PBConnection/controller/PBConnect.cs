using UnityEngine;
using System.Collections;
using System.IO;

using SlgPB;

public class PBConnect<Tin, Tout> {

	public delegate void DelegateConnectCallback(bool success, System.Object content);


	protected bool _canBeIgnore;
	protected string _url;
	protected byte[] _data;
	protected DelegateConnectCallback _callback;


//	protected const string URL_BASE = "http://alpha.tank.api.zjfygames.com/test.php";
	protected const string URL_BASE = "http://alpha.tank.api.zjfygames.com/";


	private const int FETAL_ERROR = 100;


	public PBConnect(bool canBeIgnore)
	{
		this._canBeIgnore = canBeIgnore;
	}

	virtual public void Send(Tin content, DelegateConnectCallback callback)
	{
		this._url = GetUrl ();
		this._data = GetData (content);
		this._callback = callback;

		HttpClient.DumpStream ("PB class = " + this, _data);
		HttpClientHelper.Send (_url, _data, OnComplete, _canBeIgnore);
	}

	virtual protected string GetUrl()
	{
		return URL_BASE;
	}

	private void OnComplete (bool success, byte[] data)
	{
		int errorCode = 0;
		string errorMsg = "unknown";

		Tout content = default(Tout);

		if (success) {

			if (errorCode == 0) {
				if (AppConfig.DEBUGGING) {
					content = GetContent (data);
				} else {
					try {
						content = GetContent (data);
					} catch (System.Exception e) {
						errorCode = FETAL_ERROR;
						errorMsg = "PB data error";
					}
				}
			}

			if (errorCode == 0) {
				errorCode = ValidateContent (content);
				if (errorCode != 0) {
					errorMsg = "PB content error";
				}
			}

			if (errorCode == 0) {
				if (AppConfig.DEBUGGING) {
					DisposeContent (content);
				} else {
					try {
						DisposeContent (content);
					} catch (System.Exception e) {
						errorCode = FETAL_ERROR;
						errorMsg = "PB dispose error";
					}
				}
			}


			/*
			try {
				content = GetContent (data);

				bool correct = ValidateContent (content);
				if (correct) {
					DisposeContent (content);
				} else {
					success = false;
				}
			} catch (System.Exception e) {
				success = false;

				Trace.trace ("PB error " + this, Trace.CHANNEL.INTEGRATION);
				UIProcessErrorCode (FETAL_ERROR);
			}
			*/
		} else {
			errorCode = FETAL_ERROR;
			errorMsg = "RPC error";
		}

		if (errorCode != 0) {
			success = false;

			Trace.trace (errorMsg + ", code: " + errorCode, Trace.CHANNEL.INTEGRATION);
			UIProcessErrorCode (errorCode);
		}

		if (_callback != null) {
			_callback(success, content);
		}
	}
	
	private byte[] GetData(Tin content)
	{
		MemoryStream stream = new MemoryStream ();
		ProtoBuf.Serializer.Serialize<Tin> (stream, content);
		
		stream.Position = 0;
		byte[] data = new byte[stream.Length];
		stream.Read (data, 0, (int)stream.Length);
		
		return data;
	}

	private Tout GetContent(byte[] data)
	{
		MemoryStream stream = new MemoryStream (data);
		stream.Position = 0;
		Tout content = ProtoBuf.Serializer.Deserialize<Tout> (stream);
		return content;
	}

	virtual protected int ValidateContent(Tout content)
	{
		Assert.assert (false);
		return 0;
	}


	protected int ValidateApiResponse(ApiResponse response)
	{
		/*
		 * if(content.apiResponse.error != null && content.apiResponse.error != "")
		 * {
		 * TRACE(PB error, msg = " + content.apiResponse.error);
		 * return false;
		 * }
		 */

		if (response.error != 0) 
		{
			DataErrorCode error = DataManager.instance.dataErrorCodeGroup.GetErrorCode (response.error);
			if (error.operation == 2) {
				return 0;
			} else {
				return response.error;
			}

//			Trace.trace("PB error " + response.error, Trace.CHANNEL.INTEGRATION);
//			UIProcessErrorCode(response.error);
//
//			return false;
		}

		DisposeApiResponse (response);
		return 0;
	}

	protected void DisposeApiResponse(ApiResponse response)
	{
		InstancePlayer.instance.model_User.model_task.ResetTasks (response.tasks);
//		if (response.tasks.Count > 0) {
//			Trace.trace ("===");
//		}

	}

	virtual protected void DisposeContent(Tout content)
	{
	}


	private void UIProcessErrorCode(int errorCodeId)
	{
		string curSceneName = Application.loadedLevelName;
		if (curSceneName.Equals (AppConfig.SCENE_NAME_UI) ||
		    curSceneName.Equals (AppConfig.SCENE_NAME_BATTLE))
		{
			UIController.instance.CreatePanel (UICommon.UI_PANEL_MESSAGEERROR, errorCodeId);
		}
	}

}
