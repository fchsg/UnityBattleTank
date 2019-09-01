using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_login : PBConnect<LoginRequest, LoginResponse> {

	public PBConnect_login() :
		base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "login.php";
	}

	override protected void DisposeContent(LoginResponse content)
	{
		InstancePlayer.instance.ticket = content.ticket;

		Model_User user = InstancePlayer.instance.model_User;

		user.Login (content.response.user);

		foreach (SlgPB.Unit slgUnit in content.units)
		{
			user.UpdateUnit (slgUnit);
		}

		user.UpdateRepairUnits (content.units, content.repairEndTime, content.repairTotalTime, true);

		user.UpdateUnitGroup (content.unitGroups);

		user.UpdateLevel (content.missons);

		user.UpdateItems (content.items);

		user.UpdateHeroes (content.heroes);
	}


	override protected int ValidateContent(LoginResponse content)
	{
		return ValidateApiResponse(content.response);
	}
}
