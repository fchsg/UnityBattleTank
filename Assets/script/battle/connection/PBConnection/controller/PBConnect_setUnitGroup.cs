using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_setUnitGroup : PBConnect<SetUnitGroupRequest, CommonResponse> {
	
	public PBConnect_setUnitGroup() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "setUnitGroup.php";  //设置阵型
	}
	
	override protected void DisposeContent(CommonResponse content)
	{
	
	}
	
	override protected int ValidateContent(CommonResponse content)
	{
		return ValidateApiResponse(content.response);
	}

	public enum RESULT
	{
		OK,
	}

	public static RESULT SetFormation(PBConnect_setUnitGroup.DelegateConnectCallback OnSetFormation)
	{
		RESULT result = RESULT.OK;

		SetUnitGroupRequest request = new SetUnitGroupRequest ();
		Model_Formation model_Formation = InstancePlayer.instance.model_User.model_Formation;
		model_Formation.CreateUnitGroupsResquest (request.unitGroup);	
		request.api = new Model_ApiRequest ().api;

		(new PBConnect_setUnitGroup ()).Send (request, OnSetFormation);

		return result;
	}
	
}