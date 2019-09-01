using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_addUnit : PBConnect<ProcessUnitRequest, ProcessUnitResponse> {
	
	public PBConnect_addUnit() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "addUnit.php"; // 生产
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

}

