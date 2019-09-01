using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_readNotify : PBConnect<ReadNotifyRequest, ReadNotifyResponse> {

	private ReadNotifyRequest _request;


	public static List<SlgPB.MultiPrizeItem> dropPrizeItems = new List<MultiPrizeItem> ();

	public PBConnect_readNotify() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "readNotify.php"; // 升级CD结束时调用
	}

	public override void Send (ReadNotifyRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;

		dropPrizeItems.Clear ();
	}

	override protected void DisposeContent(ReadNotifyResponse content)
	{
		InstancePlayer.instance.model_User.model_notificationGroup.MarkDisposed (_request.notifyIds.ToArray ());

		dropPrizeItems = content.prizeItems;
	}

	override protected int ValidateContent(ReadNotifyResponse content)
	{
		return ValidateApiResponse(content.response);
	}



	public enum RESULT
	{
		OK,
		CANT_PICK,
	}

	public static RESULT Check(int[] ids)
	{
		foreach (int id in ids) {
			if (!InstancePlayer.instance.model_User.model_notificationGroup.HasBonus (id)) {
				return RESULT.CANT_PICK;
			}
		}

		return RESULT.OK;
	}


	private static PBConnect_readNotify.DelegateConnectCallback _callback = null;

	public static RESULT ReadNotification(int[] ids, PBConnect_readNotify.DelegateConnectCallback callback)
	{
		RESULT r = Check (ids);
		if (r != RESULT.OK) {
			return r;
		}

		Assert.assert (_callback == null);
		_callback = callback;

		ReadNotifyRequest request = new ReadNotifyRequest ();
		request.api = (new Model_ApiRequest ()).api;

		ListHelper.Push<int> (request.notifyIds, ids);

		(new PBConnect_readNotify ()).Send (request, OnReadNotification);

		return r;
	}



	private static void OnReadNotification(bool success, System.Object content)
	{
		if (success) {
			SyncRequest request = new SyncRequest ();
			request.api = new Model_ApiRequest ().api;

			PBConnect_sync.FillSyncRequest (dropPrizeItems, request);

			(new PBConnect_sync ()).Send (request, OnSyncUser);

		} else {
			PBConnect_readNotify.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

	private static void OnSyncUser(bool success, System.Object content)
	{
		if (success) {
			PBConnect_readNotify.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		} else {
			PBConnect_readNotify.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}


}
