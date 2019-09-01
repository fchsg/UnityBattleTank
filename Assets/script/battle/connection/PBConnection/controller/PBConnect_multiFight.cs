using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_multiFight : PBConnect<MultiFightRequest, MultiFightResponse> {

	public PBConnect_multiFight() :
	base(false)
	{
	}

	private MultiFightRequest _multiFightRequest;

	override protected string GetUrl()
	{
		return URL_BASE + "multiFight.php";
	}

	public override void Send (MultiFightRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		InstancePlayer.instance.multiFightPrizeItems.Clear ();

		Model_Mission mission = InstancePlayer.instance.model_User.model_level.GetMission (content.missionId);
		Assert.assert (mission.actived);

		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (content.missionId);
		InstancePlayer.instance.model_User.model_Energy.SubEnergy (dataMission.EnergyCost * content.num);

		InstancePlayer.instance.multiFightHeroGotExp.Clear ();

		_multiFightRequest = content;
	}


	override protected void DisposeContent(MultiFightResponse content)
	{
		InstancePlayer.instance.multiFightPrizeItems = content.prizeItems;

		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);

		Model_Mission modelMission = InstancePlayer.instance.model_User.model_level.GetMission (_multiFightRequest.missionId);
		modelMission.remainFightNum -= _multiFightRequest.num;

		CalcWinResult ();

	}



	private void CalcWinResult()
	{
		DataMission mission = DataManager.instance.dataMissionGroup.GetMission (_multiFightRequest.missionId);
		int gotExp = mission.exp;
		int gotHonor = mission.honor;

		InstancePlayer.instance.model_User.UpdateHonor (gotHonor * _multiFightRequest.num);

		Model_HeroGroup heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
		Model_Formation formation = InstancePlayer.instance.model_User.model_Formation;

		int[] HeroesId = formation.GetSelectTeamHeroesId ();

		for (int i = 0; i < _multiFightRequest.num; ++i) {
			List<Model_HeroGroup.ExpChangeResult> result = new List<Model_HeroGroup.ExpChangeResult> ();
			InstancePlayer.instance.multiFightHeroGotExp.Add (result);

			foreach (int heroId in HeroesId) {
				if (heroId > 0) {
					Model_HeroGroup.ExpChangeResult r = heroGroup.AddExp (heroId, gotExp);
					result.Add (r);
				}
			}
		}


	}



	override protected int ValidateContent(MultiFightResponse content)
	{
		return ValidateApiResponse(content.response);
	}



	public enum RESULT
	{
		OK,
		LACK_STAR,
		LACK_TIMES,
		LACK_ENERGY,

	}

	public static RESULT CheckMultyFight(int missionMagicId, int times)
	{
		Model_Mission modelMission = InstancePlayer.instance.model_User.model_level.GetMission (missionMagicId);
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (missionMagicId);

		if (modelMission.remainFightNum < times) {
			return RESULT.LACK_TIMES;
		}

		if (modelMission.starCount < 3) {
			return RESULT.LACK_STAR;
		}

		int totalEnergyCost = dataMission.EnergyCost * times;
		if (InstancePlayer.instance.model_User.model_Energy.energy < totalEnergyCost) {
			return RESULT.LACK_ENERGY;
		}

		return RESULT.OK;
	}


	private static PBConnect_multiFight.DelegateConnectCallback _callback = null;

	public static RESULT MultiFight(int missionMagicId, int times, PBConnect_multiFight.DelegateConnectCallback callback)
	{
		/*
		Model_Mission modelMission = InstancePlayer.instance.model_User.model_level.GetMission (missionMagicId);
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (missionMagicId);

		if (modelMission.remainFightNum < times) {
			return RESULT.LACK_TIMES;
		}

		if (modelMission.starCount < 3) {
			return RESULT.LACK_STAR;
		}

		int totalEnergyCost = dataMission.EnergyCost * times;
		if (InstancePlayer.instance.model_User.model_Energy.energy < totalEnergyCost) {
			return RESULT.LACK_ENERGY;
		}
		*/


		RESULT r = CheckMultyFight (missionMagicId, times);
		if (r != RESULT.OK) {
			return r;
		}

		Assert.assert (_callback == null);
		_callback = callback;


		MultiFightRequest request = new MultiFightRequest ();
		request.api = new Model_ApiRequest ().api;

		request.missionId = missionMagicId;
		request.num = times;

		(new PBConnect_multiFight ()).Send (request, OnMultiFight);

		return r;
	}



	private static void OnMultiFight(bool success, System.Object content)
	{
		if (success) {
			SyncRequest request = new SyncRequest ();
			request.api = new Model_ApiRequest ().api;

			PBConnect_sync.FillSyncRequest (InstancePlayer.instance.multiFightPrizeItems, request);

			(new PBConnect_sync ()).Send (request, OnSyncUser);

		} else {
			PBConnect_multiFight.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

	private static void OnSyncUser(bool success, System.Object content)
	{
		if (success) {
			PBConnect_multiFight.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		} else {
			PBConnect_multiFight.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}


}
