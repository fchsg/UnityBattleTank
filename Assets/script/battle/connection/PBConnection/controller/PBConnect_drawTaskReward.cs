using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_drawTaskReward : PBConnect<DrawTaskRewardRequest, DrawTaskRewardResponse> {


	private DrawTaskRewardRequest _request;

	public static List<SlgPB.PrizeItem> dropPrizeItems = new List<PrizeItem> ();

	public PBConnect_drawTaskReward() :
	base(false)
	{
	}

	override public void Send(DrawTaskRewardRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;
	}

	override protected string GetUrl()
	{
		return URL_BASE + "drawTaskReward.php";
	}

	override protected void DisposeContent(DrawTaskRewardResponse content)
	{
		dropPrizeItems = content.prizeItems;

		InstancePlayer.instance.model_User.model_task.Remove (_request.taskId);
	}

	override protected int ValidateContent(DrawTaskRewardResponse content)
	{
		return ValidateApiResponse(content.response);
	}







	// ======================================
	// helper

	public enum USE_ITEM_RESULT
	{
		OK,
		NOT_COMPLETE,

	}

	public static USE_ITEM_RESULT CheckUseItem(int taskId)
	{
		if (!InstancePlayer.instance.model_User.model_task.IsTaskComplete (taskId)) {
			return USE_ITEM_RESULT.NOT_COMPLETE;
		}
		
		return USE_ITEM_RESULT.OK;
	}

	private static PBConnect_drawTaskReward.DelegateConnectCallback _callback = null;

	public static USE_ITEM_RESULT DrawTask(PBConnect_drawTaskReward.DelegateConnectCallback callback, int taskId)
	{

		USE_ITEM_RESULT r = CheckUseItem (taskId);
		if (r != USE_ITEM_RESULT.OK) {
			return r;
		}

		Assert.assert (_callback == null);
		_callback = callback;

		DrawTaskRewardRequest request = new DrawTaskRewardRequest ();
		request.api = new Model_ApiRequest ().api;

		request.taskId = taskId;

		(new PBConnect_drawTaskReward ()).Send (request, OnDrawTask);
		return r;
	}

	private static void OnDrawTask(bool success, System.Object content)
	{
		if (success) {
			SyncRequest request = new SyncRequest ();
			request.api = new Model_ApiRequest ().api;

			PBConnect_sync.FillSyncRequest (dropPrizeItems, request);

			(new PBConnect_sync ()).Send (request, OnSyncUser);

		} else {
			PBConnect_drawTaskReward.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

	private static void OnSyncUser(bool success, System.Object content)
	{
		if (success) {
			PBConnect_drawTaskReward.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		} else {
			PBConnect_drawTaskReward.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

}
