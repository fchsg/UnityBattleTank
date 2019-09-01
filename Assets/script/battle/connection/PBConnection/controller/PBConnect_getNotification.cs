using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_getNotification : PBConnect<GetNotifyRequest, GetNotifyResponse>  {

//	private GetNotifyRequest _request;

	public PBConnect_getNotification() :
	base(false)
	{
	}

//	public override void Send (GetNotifyRequest content, DelegateConnectCallback callback)
//	{
//		base.Send (content, callback);
//
//		_request = content;
//	}

	override protected string GetUrl()
	{
		return URL_BASE + "GetNotify.php"; // 升级CD结束时调用
	}

	override protected void DisposeContent(GetNotifyResponse content)
	{
		Trace.trace ("download notifications, count = " + content.notifies.Count, Trace.CHANNEL.IO);
		InstancePlayer.instance.model_User.model_notificationGroup.Push (content);

//		if (content.notifies.Count > 0) {
//			SlgPB.Notify lastNotify = content.notifies [content.notifies.Count - 1];
//			DataPersistent.gotNotificationId = lastNotify.notifyId;
//		}

	}

	override protected int ValidateContent(GetNotifyResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	public static void GetNotification(PBConnect_getNotification.DelegateConnectCallback callback)
	{

		GetNotifyRequest request = new GetNotifyRequest ();
		request.api = (new Model_ApiRequest ()).api;

		request.beginNotifyId = DataNotificationStorage.gotNotificationId;

		(new PBConnect_getNotification ()).Send (request, callback);

	}

}

