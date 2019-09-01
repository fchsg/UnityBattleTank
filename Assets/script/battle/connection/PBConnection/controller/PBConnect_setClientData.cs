using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using SlgPB;

public class PBConnect_setClientData : PBConnect<SetClientDataRequest, CommonResponse> {


	public PBConnect_setClientData() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "setClientData.php";
	}

	public override void Send (SetClientDataRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

	}

	override protected void DisposeContent(CommonResponse content)
	{

	}




	override protected int ValidateContent(CommonResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	public static void Send(PBConnect_setClientData.DelegateConnectCallback callback)
	{
		List<int> stream = InstancePlayer.instance.model_User.customData.GetCombineData ();

		SetClientDataRequest request = new SetClientDataRequest ();
		request.api = (new Model_ApiRequest ()).api;

		ListHelper.Push<int> (request.clientData, stream);

		(new PBConnect_setClientData ()).Send (request, callback);
	}


}