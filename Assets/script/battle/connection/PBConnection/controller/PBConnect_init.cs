using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_init : PBConnect<InitRequest, InitResponse> {

	public PBConnect_init() :
		base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "Init.php";
	}
	
	override protected void DisposeContent(InitResponse content)
	{
	}
	
	override protected int ValidateContent(InitResponse content)
	{
		return ValidateApiResponse(content.response);
	}

}
