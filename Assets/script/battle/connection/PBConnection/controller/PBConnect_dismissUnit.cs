using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_dismissUnit : PBConnect<ProcessUnitRequest, ProcessUnitResponse> {
	
	public PBConnect_dismissUnit() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "dismissUnit.php"; // 解散Unit
	}
	
	override protected void DisposeContent(ProcessUnitResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);
		InstancePlayer.instance.model_User.UpdateUnit (content.unit);
	}
	
	override protected int ValidateContent(ProcessUnitResponse content)
	{
		return ValidateApiResponse(content.response);
	}

	public enum RESULT
	{
		OK,
		LACK_UNIT,
	}

	public static RESULT Check(int id, int count)
	{
		int unitCount = InstancePlayer.instance.model_User.GetUnitCount (id);
		if (count > unitCount) {
			return RESULT.LACK_UNIT;
		}


		return RESULT.OK;
	}


	public static RESULT Dismiss(int id, int count, PBConnect_dismissUnit.DelegateConnectCallback callback)
	{
		RESULT r = Check (id, count);
		if (r != RESULT.OK) {
			return r;
		}

		ProcessUnitRequest request = new ProcessUnitRequest ();
		request.api = (new Model_ApiRequest ()).api;

		request.unitId = id;
		request.num = count;

		(new PBConnect_dismissUnit ()).Send (request, callback);

		return r;

	}

	
}