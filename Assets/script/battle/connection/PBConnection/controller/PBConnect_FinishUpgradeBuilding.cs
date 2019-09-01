using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_FinishUpgradeBuilding : PBConnect<UpgradeBuildingRequest, UpgradeBuildingResponses>  {

	public PBConnect_FinishUpgradeBuilding() :
	   base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "FinishUpgradeBuilding.php"; // 升级CD结束时调用
	}
	
	override protected void DisposeContent(UpgradeBuildingResponses content)
	{
		InstancePlayer.instance.model_User.UpgradeBuilding (content.response.user, content.unlock);
		InstancePlayer.instance.model_User.UpdateUnlockUnits (content.units);
	}

	override protected int ValidateContent(UpgradeBuildingResponses content)
	{
		return ValidateApiResponse(content.response);
	}
}
