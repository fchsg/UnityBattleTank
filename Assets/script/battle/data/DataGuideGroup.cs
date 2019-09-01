using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataGuideGroup {

	public class Guide
	{
		public int id;
		public string title;
		public List<DataGuide> steps = new List<DataGuide>();
		public int task;
	}


	public List<DataGuide> guidesGroup = new List<DataGuide>();
	public Dictionary<int, Guide> guideMap = new Dictionary<int, Guide>(); //id, guide

	public void LoadHeros(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		foreach (LitJson.JSONNode subNode in json.Childs) 
		{
			DataGuide data = new DataGuide();
			data.Load(subNode);

			guidesGroup.Add (data);

		}

		CollectGuide ();

	}

	private void CollectGuide()
	{
		foreach (DataGuide dataGuide in guidesGroup) {
			Guide guide = null;
			if (guideMap.ContainsKey (dataGuide.id)) {
				guide = guideMap [dataGuide.id];
			} else {
				guide = new Guide ();
				guide.id = dataGuide.id;
				guide.title = dataGuide.title;
				guide.task = dataGuide.task;
				guideMap.Add (dataGuide.id, guide);
			}

			guide.steps.Add (dataGuide);

		}
	}


	//======================================================
	// helper

	private PBConnect_setClientData.DelegateConnectCallback checkingCallback = null;

	public int CheckAllGuide(PBConnect_setClientData.DelegateConnectCallback callback)
	{
		Assert.assert (checkingCallback == null);

		CustomData cData = InstancePlayer.instance.model_User.customData;

		foreach (KeyValuePair<int, Guide> pair in guideMap) {
			int guideId = pair.Key;
			Guide guide = pair.Value;

			if (cData.IsGuideComplete (guideId)) {
				continue;
			}

			if (TryActiveGuide (guide)) {
				checkingCallback = callback;
				cData.SetGuideComplete (guideId);
				PBConnect_setClientData.Send (OnCheck);

				return guideId;
			}

		}

		return -1;
	}

	private void OnCheck(bool success, System.Object content)
	{
		PBConnect_setClientData.DelegateConnectCallback callback = checkingCallback;
		checkingCallback = null;
		callback (success, content);
	}

	private bool TryActiveGuide(Guide guide)
	{
		foreach (DataGuide dataGuide in guide.steps) {
			if (!CheckCommand (dataGuide)) {
				return false;
			}
		}

		return true;
	}

	private bool CheckCommand(DataGuide dataGuide)
	{
		switch(dataGuide.command)
		{
		case "CommanderLv":
			return CheckCommand_CommanderLv (dataGuide);

		case "BuildingLv":
			return CheckCommand_BuildingLv (dataGuide);

		case "PassMission":
			return CheckCommand_PassMission (dataGuide);

		case "GetHero":
			return CheckCommand_GetHero (dataGuide);

		case "UnitLv":
			return CheckCommand_UnitLv (dataGuide);


		}

		Assert.assert (false);
		return false;
	}

	private bool CheckCommand_CommanderLv(DataGuide dataGuide)
	{
		int level = (int)dataGuide.A;
		return InstancePlayer.instance.model_User.honorLevel >= level;
	}

	private bool CheckCommand_BuildingLv(DataGuide dataGuide)
	{
		int buildingId = (int)dataGuide.A;
		int buildingLevel = (int)dataGuide.B;

		Model_Building building = InstancePlayer.instance.model_User.buildings [(Model_Building.Building_Type)buildingId];
		return building.level >= buildingLevel;
	}

	private bool CheckCommand_PassMission(DataGuide dataGuide)
	{
		int missionId = (int)dataGuide.A;

		Model_Mission mission = InstancePlayer.instance.model_User.model_level.GetMission (missionId);
		return mission.starCount > 0;
	}

	private bool CheckCommand_GetHero(DataGuide dataGuide)
	{
		int heroId = (int)dataGuide.A;

		return InstancePlayer.instance.model_User.model_heroGroup.HasHero (heroId);
	}

	private bool CheckCommand_UnitLv(DataGuide dataGuide)
	{
		//TO DO
		return false;
	}


}
