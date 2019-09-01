using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_getPvpLog : PBConnect<CommonRequest, GetPVPLogResponse> {

	public PBConnect_getPvpLog() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "GetPVPLog.php"; // 升级CD结束时调用
	}


	override protected void DisposeContent(GetPVPLogResponse content)
	{
		InstancePlayer.instance.model_User.model_pvpUser.pvpLogs = content.fightLogs;
	}

	override protected int ValidateContent(GetPVPLogResponse content)
	{
		return ValidateApiResponse(content.response);
	}



	public void GetLog(PBConnect_getPvpLog.DelegateConnectCallback callback)
	{
		CommonRequest request = new CommonRequest ();
		request.api = (new Model_ApiRequest ()).api;

		(new PBConnect_getPvpLog ()).Send (request, callback);

	}


}
