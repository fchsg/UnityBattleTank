using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_startFight : PBConnect<StartFightRequest, StartFightResponse> {

	private StartFightRequest _request;

	public PBConnect_startFight() :
		base(false)
	{
	}

	public override void Send (StartFightRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		Model_Mission mission = InstancePlayer.instance.model_User.model_level.GetMission (content.missionId);
		Assert.assert (mission.actived);

		_request = content;
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "StartFight.php";
	}
	
	override protected void DisposeContent(StartFightResponse content)
	{
		InstancePlayer.instance.currentFightId = content.fightId;

		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);

		Model_Mission modelMission = InstancePlayer.instance.model_User.model_level.GetMission (_request.missionId);
		modelMission.remainFightNum--;

		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (_request.missionId);
		InstancePlayer.instance.model_User.model_Energy.SubEnergy (dataMission.EnergyCost);

	}
	
	override protected int ValidateContent(StartFightResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	public enum RESULT
	{
		OK,
		LACK_ENERGY,
		LACK_TIMES,
	}

	public static RESULT CheckStartFight(int missionMagicId)
	{
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (missionMagicId);

		int totalEnergyCost = dataMission.EnergyCost * 1;
		if (InstancePlayer.instance.model_User.model_Energy.energy < totalEnergyCost) {
			return RESULT.LACK_ENERGY;
		}

		Model_Mission modelMission = InstancePlayer.instance.model_User.model_level.GetMission (missionMagicId);
		if (modelMission.remainFightNum < 1) {
			return RESULT.LACK_TIMES;
		}

		return RESULT.OK;
	}


	public static RESULT StartFight(PBConnect_startFight.DelegateConnectCallback callback)
	{
		/*
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (InstancePlayer.instance.missionMagicId);

		int totalEnergyCost = dataMission.EnergyCost * 1;
		if (InstancePlayer.instance.model_User.model_Energy.energy < totalEnergyCost) {
			return RESULT.LACK_ENERGY;
		}
		*/

		RESULT r = CheckStartFight (InstancePlayer.instance.missionMagicId);
		if (r != RESULT.OK) {
			return r;
		}


		StartFightRequest request = new StartFightRequest ();
		request.api = new Model_ApiRequest ().api;
		
		request.missionId = InstancePlayer.instance.missionMagicId;
		request.teamId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamId ();
		
		(new PBConnect_startFight ()).Send (request, callback);

		return r;

	}

}
