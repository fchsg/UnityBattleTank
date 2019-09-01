using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_finishRepairUnit : PBConnect<RepairUnitRequest, RepairUnitResponse> {
	
	public PBConnect_finishRepairUnit() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "finishRepairUnit.php";  // 读秒时间结束或者加速完成 修理Unit
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
