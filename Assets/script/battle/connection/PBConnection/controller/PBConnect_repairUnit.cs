using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_repairUnit : PBConnect<RepairUnitRequest, RepairUnitResponse> {
	
	public PBConnect_repairUnit() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "repairUnit.php";  //维修
	}
	
	override protected void DisposeContent(RepairUnitResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);

		foreach (SlgPB.Unit unit in content.units) 
		{
			InstancePlayer.instance.model_User.UpdateUnit (unit);
		}

		InstancePlayer.instance.model_User.UpdateRepairUnits (content.units, content.repairEndTime, content.repairTotalTime);
	}
	
	override protected int ValidateContent(RepairUnitResponse content)
	{
		return ValidateApiResponse(content.response);
	}
	
}