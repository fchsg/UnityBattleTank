using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_finishAddUnit : PBConnect<ProcessUnitRequest, ProcessUnitResponse> {
	
	public PBConnect_finishAddUnit() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "finishAddUnit.php";  //免CD建造Unit
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
