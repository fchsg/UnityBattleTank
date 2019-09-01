using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HttpClient : MonoBehaviour {

	public delegate void DelegateRequestCallback(bool success, byte[] data);
	
	public class RequestData
	{
		public string url;
		public byte[] data;
		public DelegateRequestCallback callback;
		
		public int retryTimesLeft = MAX_RETRY;
		
		public RequestData(string url, byte[] data, DelegateRequestCallback callback)
		{
			this.url = url;
			this.data = data;
			this.callback = callback;
		}
	}


	public bool canBeIgnore = false;
	
	private List<RequestData> _pendingRequests = new List<RequestData> ();
	private RequestData _sendingRequest = null;


	private WWW _www;
	public WWW www
	{
		get { return _www; }
	}

	public static int MAX_RETRY = int.MaxValue;


	public void Send(string url, byte[] data, DelegateRequestCallback callback)
	{
		RequestData request = new RequestData (url, data, callback);
		_pendingRequests.Add (request);

		TrySendNextRequest ();
	}
	
	private void TrySendNextRequest()
	{
		if (_sendingRequest == null) {
			if (_pendingRequests.Count > 0) {
				RequestData nextRequest = _pendingRequests[0];
				_pendingRequests.RemoveAt(0);

				StartCoroutine(Post(nextRequest));
			}
		}
	}

	IEnumerator Post(RequestData request)
	{
		_sendingRequest = request;
		
//		WWWForm wwwForm = new WWWForm();
//		wwwForm.AddField ("", request.data);

//		byte[] bytes = request.data;
//		wwwForm.AddBinaryData("", bytes);

		DumpStream ("http post", request.data);

		Dictionary<string, string> header = new Dictionary<string, string> ();
		header.Add ("Content-Type", "text/xml");

//		WWW www = new WWW (request.url, wwwForm);
		_www = new WWW (request.url, request.data, header);
		yield return _www;

		if (_www.error != null)
		{
			Trace.trace("http error = " + _www.error, Trace.CHANNEL.HTTP);
			_www.Dispose();
			_www = null;

			if(canBeIgnore)
			{
				if(request.callback != null)
				{
					request.callback(false, null);
				}
			}
			else
			{
				if(request.retryTimesLeft-- <= 0)
				{
					if(request.callback != null)
					{
						request.callback(false, null);
					}
				}
				else
				{
					Trace.trace("retry url = " + request.url, Trace.CHANNEL.HTTP);
					_pendingRequests.Insert(0, request);
				}
			}
		}
		else
		{
			byte[] responseData = _www.bytes;
			_www.Dispose();
			_www = null;

			DumpStream("http response success", responseData);

			if(request.callback != null)
			{
				request.callback (true, responseData);
			}
		}
		
		_sendingRequest = null;
		TrySendNextRequest ();
	}

	// ====================================================================
	// helper

	public static void DumpStream(string title, byte[] bytes)
	{
		string msg = title + ",\tdata = " + bytes + "\n";
		msg += "length = " + bytes.Length + ",\tbinary = \n";
		for (int i = 0; i < bytes.Length; ++i) {
			int c = (int)bytes[i];
			msg += c.ToString("x16").Replace("000000", "") + "\t";
			if(i % 10 == 9)
			{
				msg += "\n";
			}
		}
		Trace.trace(msg, Trace.CHANNEL.HTTP);

	}

}
