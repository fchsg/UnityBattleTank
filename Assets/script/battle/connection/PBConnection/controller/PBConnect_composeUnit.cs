using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_composeUnit : PBConnect<ComposeUnitRequest, ComposeUnitResponse> {

	private ComposeUnitRequest _request;

	public PBConnect_composeUnit() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "composeUnit.php"; // 生产
	}

	public override void Send (ComposeUnitRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;
	}

	override protected void DisposeContent(ComposeUnitResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUnit (content.unit);

		DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (_request.unitId);
		InstancePlayer.instance.model_User.model_itemGroup.UseItem (dataUnit.chipId, dataUnit.chipCount);

	}
	
	override protected int ValidateContent(ComposeUnitResponse content)
	{
		return ValidateApiResponse(content.response);
	}




	public enum RESULT
	{
		OK,
		LACK_ITEM,
		LACK_ALREADY_HAVE,
	}

	public static RESULT CheckResult(int unitId)
	{
		bool hasUnit = InstancePlayer.instance.model_User.unlockUnits.ContainsKey (unitId);
		if (hasUnit) {
			return RESULT.LACK_ALREADY_HAVE;
		}

		DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
		int itemCount = InstancePlayer.instance.model_User.model_itemGroup.GetItemCount (dataUnit.chipId);
		if (itemCount < dataUnit.chipCount) {
			return RESULT.LACK_ITEM;
		}

		return RESULT.OK;
	}



}
