using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_FinishUnit : PBConnect<ProcessUnitRequest, ProcessUnitResponse> {
	
	public PBConnect_FinishUnit() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "getUnit.php";  // 在生产,维修CD结束时调用
	}
	
	override protected void DisposeContent(ProcessUnitResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUnit (content.unit);
	}
	
	override protected int ValidateContent(ProcessUnitResponse content)
	{
		return ValidateApiResponse(content.response);
	}
	
}