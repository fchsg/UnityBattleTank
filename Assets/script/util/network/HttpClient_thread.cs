using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Text;

public class HttpClientThreadMode {

	public delegate void DelegateRequestCallback(bool success, string data);

	public class Request
	{
		public string url;
		public string data;
		public DelegateRequestCallback callback;

		public int retryTimesLeft = MAX_RETRY;

		public Request(string url, string stream, DelegateRequestCallback callback)
		{
			this.url = url;
			this.data = stream;
			this.callback = callback;
		}
	}


	private bool _canBeIgnore = false;

	private List<Request> _pendingRequests = new List<Request> ();
	private Request _sendingRequest = null;
	private object _requestsQueueLocker = new object ();

	private Thread _thread;
	private bool _finished = false;

	public static int MAX_RETRY = 999999;

	public HttpClientThreadMode(bool canBeIgnore)
	{
		this._canBeIgnore = canBeIgnore;

		_thread = new Thread (Loop);
		_thread.Start ();

	}

	private void Loop()
	{
		while (!_finished) {
			TrySendNextRequest ();

			Thread.Sleep(3);
		}
	}

	public void Send(string url, string stream, DelegateRequestCallback callback)
	{
//		Request request = new Request (url, stream, callback);
//		Post (request);

		lock (_requestsQueueLocker) {
			Request request = new Request (url, stream, callback);
			_pendingRequests.Add (request);
		}
	}

	private void TrySendNextRequest()
	{
		if (_sendingRequest == null) {
			Request nextRequest = null;

			lock (_requestsQueueLocker) {
				if (_pendingRequests.Count > 0) {
					nextRequest = _pendingRequests[0];
					_pendingRequests.RemoveAt(0);
				}
			}

			if(nextRequest != null)
			{
				Post(nextRequest);
			}

		}
	}


	public void Post(Request request)
	{
		_sendingRequest = request;

		HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(request.url);
		webRequest.Method = "POST";
		webRequest.ContentType = "application/octet-stream";
		webRequest.Timeout = 30 * 1000;
		webRequest.ReadWriteTimeout = 30 * 1000;
		webRequest.KeepAlive = false;
		webRequest.ContentLength = request.data.Length;
		
		try
		{
			byte[] bytes = Encoding.ASCII.GetBytes(request.data);
			
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(bytes, 0, request.data.Length);
			requestStream.Close();
		}
		catch (System.Net.WebException ex)
		{
			if(_canBeIgnore)
			{
				OnFail(request, ex);
			}
			else
			{
				if(request.retryTimesLeft-- <= 0)
				{
					OnFail(request, ex);
				}
				else
				{
					//retry
					lock (_requestsQueueLocker) {
						_pendingRequests.Insert(0, request);
					}
				}
			}

			webRequest.Abort();
			_sendingRequest = null;
			return;
		}
		catch (System.Exception ex)
		{
			OnFail(request, ex);
			
			webRequest.Abort();
			_sendingRequest = null;
			return;
		}
		/*
		catch (System.Net.ProtocolViolationException ex)
		{
			OnFail(_request, ex);

			webRequest.Abort();
			return;
		}
		catch (System.ObjectDisposedException ex)
		{
			OnFail(_request, ex);
			
			webRequest.Abort();
			return;
		}
		catch (System.InvalidOperationException ex)
		{
			OnFail(_request, ex);
			
			webRequest.Abort();
			return;
		}
		catch (System.NotSupportedException ex)
		{
			OnFail(_request, ex);
			
			webRequest.Abort();
			return;
		}
		*/

		OnSuccess (request, webRequest);

		webRequest.Abort();
		_sendingRequest = null;
	}

	private void OnFail(Request request, System.Exception e)
	{
		Trace.trace("http error = " + e.Message, Trace.CHANNEL.HTTP);
		request.callback (false, null);
	}

	private void OnSuccess(Request request, HttpWebRequest webRequest)
	{
		HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
		Stream webResponseStream = webResponse.GetResponseStream();
		
		StreamReader streamReader = new StreamReader (webResponseStream);
		string responseData = streamReader.ReadToEnd ();
		
		webResponse.Close();

		string msg = "http success, data = " + responseData + "\n";
		msg += "binary = \n";
		for (int i = 0; i < responseData.Length; ++i) {
			int c = (int)responseData[i];
			msg += c.ToString("x16").Replace("000000", "") + "\t";
			if((i & 0x0f) == 0x0f)
			{
				msg += "\n";
			}
		}
		Trace.trace(msg, Trace.CHANNEL.HTTP);
		
		request.callback (true, responseData);
	}

}

