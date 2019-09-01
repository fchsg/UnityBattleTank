using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using SlgPB;

public class PBConnect_delNotify : PBConnect<DelNotifyRequest, CommonResponse> {

	private DelNotifyRequest _request;

	public PBConnect_delNotify() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "delNotify.php"; // 升级CD结束时调用
	}

	public override void Send (DelNotifyRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;
	}

	override protected void DisposeContent(CommonResponse content)
	{
		InstancePlayer.instance.model_User.model_notificationGroup.Delete (_request.notifyIds.ToArray ());
	}

	override protected int ValidateContent(CommonResponse content)
	{
		return ValidateApiResponse(content.response);
	}



	public enum RESULT
	{
		OK,
		CANT_DEL,
	}

	public static RESULT Check(int[] ids)
	{
		foreach (int id in ids) {
			if (!InstancePlayer.instance.model_User.model_notificationGroup.CanBeDel (id)) {
				return RESULT.CANT_DEL;
			}
		}

		return RESULT.OK;
	}

	public static RESULT DelNotification(int[] ids, PBConnect_delNotify.DelegateConnectCallback callback)
	{
		RESULT r = Check (ids);
		if (r != RESULT.OK) {
			return r;
		}

		DelNotifyRequest request = new DelNotifyRequest ();
		request.api = (new Model_ApiRequest ()).api;

		ListHelper.Push<int> (request.notifyIds, ids);

		(new PBConnect_delNotify ()).Send (request, callback);

		return r;
	}

}
