using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataMission {
	public int magicId;

	public DataConfig.MISSION_DIFFICULTY difficulty;
	public int stageId;
	public int missionId;


//	public DataConfig.MISSION_DIFFICULTY difficulty;

	public int[] teamsId;
	public float playerRotation = 90;
	public float[] teamsRotation = { -90 };
	public float powerScale;

	public string asset;
	public string name;

	public int honor;
	public int exp;
	public int itemGroupId;
	public int evaluate;
//	public int[] itemsId;

	public int EnergyCost;
	public int Star;
	public int PlayerLevel;
	public int[] RateIds;
	
	public DataConfig.CAMPAIGN_ARROW[] arrows = new DataConfig.CAMPAIGN_ARROW[ARROW_COUNT_MAX];
	public float[] arrowsRotation = new float[ARROW_COUNT_MAX];
	
	public const int ARROW_COUNT_MAX = 3;


	public static string[] BK_NAMES = new string[] {
		"city",
		"mountain",
		"suburb",
	};

	public void Load(LitJson.JSONNode json)
	{
		magicId = int.Parse(json["ID"]);
		name = json ["Name"];
		ParseStageMissionId (magicId);
//		difficulty = (DataConfig.MISSION_DIFFICULTY)int.Parse(json["Difficulty"]);

		string str_teamsId = json["TeamsID"];
		teamsId = StringHelper.ReadIntArrayFromString (str_teamsId);

		playerRotation = JsonReader.Float (json, "PlayerRotate");

		string str_teamsRotation = json["TeamsRotation"];
		teamsRotation = StringHelper.ReadFloatArrayFromString (str_teamsRotation);

		Assert.assert (teamsId.Length == teamsRotation.Length);

		powerScale = float.Parse(json["PowerScale"]);

		asset = json ["BattleBackground"];

		honor = JsonReader.Int (json, "Honor");
		exp = JsonReader.Int (json, "Exp");
		itemGroupId = JsonReader.Int (json, "ItemGroupID");
		evaluate = JsonReader.Int (json, "Evaluate");

//		string str_itemsId = json["Item"];
//		itemsId = StringHelper.ReadIntArrayFromString (str_itemsId);

		EnergyCost = JsonReader.Int (json, "EnergyCost");
		Star = JsonReader.Int (json, "Star");
		PlayerLevel = JsonReader.Int (json, "PlayerLevel");

		string str_rateIds = json["RateIDs"];
		RateIds = StringHelper.ReadIntArrayFromString (str_rateIds);
		if (RateIds.Length < 2) {
			RateIds = ArrayHelper.Fill<int> (RateIds, 0, 2 - RateIds.Length);
		}

		for (int i = 0; i < ARROW_COUNT_MAX; ++i) {
			string arrowContent = json ["Arrow" + (i + 1)];
			DataConfig.CAMPAIGN_ARROW arrow;
			float arrowRot;
			ReadArrow (arrowContent, out arrow, out arrowRot);
			
			arrows[i] = arrow;
			arrowsRotation[i] = arrowRot;
		}
		

	}

	public int[] itemsId
	{
		get
		{
			List<int> itemIds = DataManager.instance.dataDropGroup.CollectPossibleDropItemIds (itemGroupId);
			int[] ids = itemIds.ToArray ();
			return ids;
		}
	}

	private void ParseStageMissionId(int magicId)
	{
		difficulty = GetDifficulty (magicId);
		stageId = GetStageId (magicId);
		missionId = GetMissionId (magicId);

		Assert.assert (stageId > 0);
		Assert.assert (missionId > 0);
	}

	public static DataConfig.MISSION_DIFFICULTY GetDifficulty(int magicId)
	{
		if (magicId >= 20000 && magicId < 30000) {
			return DataConfig.MISSION_DIFFICULTY.ELITE;
		} else {
			return DataConfig.MISSION_DIFFICULTY.NORMAL;
		}
		
	}

	public static int GetStageId(int magicId)
	{
		return (magicId / 100) % 100;
	}

	public static int GetMissionId(int magicId)
	{
		return magicId % 100;
	}
	
	private void ReadArrow(string s, out DataConfig.CAMPAIGN_ARROW arrow, out float rotation)
	{
		if (s == null) {
			arrow = DataConfig.CAMPAIGN_ARROW.UNKNOWN;
			rotation = 0;
			return;
		}
		
		s = s.ToLower ();
		char t = s [0];
		
		switch (t) {
		case 'a':
			arrow = DataConfig.CAMPAIGN_ARROW.A_UPWARD;
			break;
			
		case 'b':
			arrow = DataConfig.CAMPAIGN_ARROW.B_UPWARD_TURN_LEFT;
			break;
			
		case 'c':
			arrow = DataConfig.CAMPAIGN_ARROW.C_UPWARD_TURN_RIGHT;
			break;
			
		case 'd':
			arrow = DataConfig.CAMPAIGN_ARROW.D_UPWARD_BIG;
			break;
			
		case 'e':
			arrow = DataConfig.CAMPAIGN_ARROW.E_UPWARD_TURN_LEFT_BIG;
			break;
			
		case 'f':
			arrow = DataConfig.CAMPAIGN_ARROW.F_UPWARD_TURN_RIGHT_BIG;
			break;
			
		default:
			arrow = DataConfig.CAMPAIGN_ARROW.UNKNOWN;
			break;
		}
		
		//
		if (arrow != DataConfig.CAMPAIGN_ARROW.UNKNOWN) {
			string r = s.Substring (1);
			rotation = -float.Parse (r); //CLOCK WISE
		} else {
			rotation = 0;
		}
		
	}

	public int[] GetUnitsId(int index)
	{
		int teamId = Mathf.Abs (teamsId [index]);
		if (teamId > 0) {
			return DataManager.instance.dataTeamGroup.GetTeam(teamId).unitId;
		}

		Assert.assert (false);
		return null;
	}

	public int[] GetUnitsCount(int index)
	{
		int teamId = Mathf.Abs (teamsId [index]);
		if (teamId > 0) {
			return DataManager.instance.dataTeamGroup.GetTeam(teamId).unitCount;
		}

		Assert.assert (false);
		return null;
	}

	public int GetMemberCount(int index)
	{
		int teamId = Mathf.Abs (teamsId [index]);
		if (teamId > 0) {
			return DataManager.instance.dataTeamGroup.GetTeam(teamId).memberCount;
		}

		Assert.assert (false);
		return 0;
	}



}
