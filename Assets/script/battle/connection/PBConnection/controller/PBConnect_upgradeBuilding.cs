using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_upgradeBuilding : PBConnect<UpgradeBuildingRequest, UpgradeBuildingResponses>  {

	public PBConnect_upgradeBuilding() :
		base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "upgradeBuilding.php"; //建筑升级
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

