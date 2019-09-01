using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_Level {

	public class Campaign
	{
		public int stageId;
		public List<Model_Mission> list = new List<Model_Mission>();

		public Campaign(int id)
		{
			this.stageId = id;
		}
	}

	public class Chapters
	{
		public List<Campaign> list = new List<Campaign> ();
	}


	private Chapters _chaptersNormal = new Chapters ();
	private Chapters _chaptersElite = new Chapters ();



	public void Init(List<SlgPB.Mission> missions)
	{
		InitChaptersNormal ();
		InitChaptersElite ();

		SlgPB.Mission lastNormalMission;
		SlgPB.Mission lastEliteMission;
		ParseMissions (missions, out lastNormalMission, out lastEliteMission);

		if (lastNormalMission != null) {
			TryUnlockNextMission (lastNormalMission.missonId);
		}

		if (lastEliteMission != null) {
			TryUnlockNextMission (lastEliteMission.missonId);
		}

	}

	private void InitChaptersNormal()
	{
		_chaptersNormal = new Chapters ();
		
		DataConfig.MISSION_DIFFICULTY difficulty = DataConfig.MISSION_DIFFICULTY.NORMAL;

		DataMissionGroup missionGroup = DataManager.instance.dataMissionGroup;

		List<DataMissionGroup.DataCampaign> dataCampaigns = missionGroup.GetCampaigns (difficulty);
		foreach(DataMissionGroup.DataCampaign dataCampaign in dataCampaigns)
		{
			Campaign campaign = new Campaign(dataCampaign.stageId);
			_chaptersNormal.list.Add(campaign);

			foreach(DataMission dataMission in dataCampaign.missions)
			{
				Model_Mission modelMission = new Model_Mission(dataMission.magicId);
//				modelMission.type = difficulty;
//				modelMission.stageId = stageId;
//				modelMission.missionId = m.missionId;
//				modelMission.starMask = 0;
//				modelMission.remainFightNum = DEFAULT_FIGHT_NUM;

//				modelMission.referenceMission = DataManager.instance.dataMissionGroup.GetMission(m.magicId);
//				Assert.assert(modelMission.referenceMission != null);

				campaign.list.Add(modelMission);

			}
		}

	}

	private void InitChaptersElite()
	{
		_chaptersElite = new Chapters ();
		
		DataConfig.MISSION_DIFFICULTY difficulty = DataConfig.MISSION_DIFFICULTY.ELITE;
		
		DataMissionGroup missionGroup = DataManager.instance.dataMissionGroup;
		
		List<DataMissionGroup.DataCampaign> dataCampaigns = missionGroup.GetCampaigns (difficulty);
		foreach(DataMissionGroup.DataCampaign dataCampaign in dataCampaigns)
		{
			Campaign campaign = new Campaign(dataCampaign.stageId);
			_chaptersElite.list.Add(campaign);
			
			foreach(DataMission dataMission in dataCampaign.missions)
			{
				Model_Mission modelMission = new Model_Mission(dataMission.magicId);
				campaign.list.Add(modelMission);
				
			}
		}

		
	}
	

	public Model_Mission GetMission(DataConfig.MISSION_DIFFICULTY difficulty, int stageId, int missionId)
	{
		Campaign campaign = GetCampaign (difficulty, stageId);
		foreach(Model_Mission m in campaign.list)
		{
			if(m.missionId == missionId)
			{
				return m;
			}
		}
		return null;
	}
	
	public Model_Mission GetMission(int missionMagicId)
	{
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (missionMagicId);
		int stageId = DataMission.GetStageId (missionMagicId);
		int missionId = DataMission.GetMissionId (missionMagicId);

		return GetMission (difficulty, stageId, missionId);
	}

	public Campaign GetCampaign(DataConfig.MISSION_DIFFICULTY difficulty, int stageId)
	{
		Chapters chapters = GetChapters (difficulty);
		foreach(Campaign c in chapters.list)
		{
			if(c.stageId == stageId)
			{
				return c;
			}
		}
		return null;
	}

	public Chapters GetChapters(DataConfig.MISSION_DIFFICULTY difficulty)
	{
		if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			return _chaptersNormal;
		} else {
			return _chaptersElite;
		}
	}


	private void ParseMissions(List<SlgPB.Mission> missions, out SlgPB.Mission lastNormalMission, out SlgPB.Mission lastEliteMission)
	{
		lastNormalMission = null;
		lastEliteMission = null;

		foreach (SlgPB.Mission mission in missions) {
			Model_Mission modelMission = new Model_Mission(mission.missonId);
			modelMission.Parse(mission);

//			modelMission.referenceMission = DataManager.instance.dataMissionGroup.GetMission(modelMission.missionId);
//			Assert.assert(modelMission.referenceMission != null);
			
			ReplaceMission(modelMission);

			if (modelMission.starCount > 0) {
				if (modelMission.difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
					lastNormalMission = mission;
				} else if (modelMission.difficulty == DataConfig.MISSION_DIFFICULTY.ELITE) {
					lastEliteMission = mission;
				}
			}

		}
	}

	public int GetLastUnlockMissionMagicId(DataConfig.MISSION_DIFFICULTY difficult)
	{
		Model_Mission mission = GetLastUnlockMission (difficult);
		if (mission != null) {
			return mission.magicId;
		}

		return 0;
	}

	public Model_Mission GetLastUnlockMission(DataConfig.MISSION_DIFFICULTY difficult, int minStarCount = 0)
	{
		Chapters chapters = null;
		if (difficult == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			chapters = _chaptersNormal;
		} else {
			chapters = _chaptersElite;
		}

		Model_Mission findMission = null;
		foreach (Campaign campaign in chapters.list) {
			foreach (Model_Mission mission in campaign.list) {
				if (mission.actived && mission.starCount >= minStarCount) {
					findMission = mission;
				}
			}
		}

		return findMission;
	}

	public void ReplaceMission(Model_Mission modelMission)
	{
		Campaign campaign = GetCampaign (modelMission.difficulty, modelMission.stageId);
		for(int i = 0; i < campaign.list.Count; ++i)
		{
			if(campaign.list[i].missionId == modelMission.missionId)
			{
				campaign.list[i] = modelMission;
				return;
			}
		}

		Assert.assert (false);
	}


	public void TryUnlockNextMission(int battleMissionId)
	{
		/*
		if (battleMissionId == 0) {
			return TryUnlockFirstMission ();
		}
		*/



		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (battleMissionId);

		Model_Mission lastUnlockMission = GetLastUnlockMission (difficulty, 1);
		int stageId = lastUnlockMission.stageId;
		int missionId = lastUnlockMission.missionId;

		Campaign campaign = GetCampaign (difficulty, stageId);
		if (campaign.list.Count == missionId) {
			//it is last mission in campaign

			Chapters chapters = GetChapters (difficulty);
			if(stageId < chapters.list.Count)
			{
				if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
					Campaign nextCampaign = GetCampaign (difficulty, stageId + 1);
					Model_Mission nextMission = nextCampaign.list [0];
					if (CheckMissionUnlock (nextMission)) {
						nextMission.actived = true;
					}
				}


//				return nextMission.referenceMission.magicId;
			}

		} else {
			int nextMissionIndex = missionId;
			Model_Mission nextMission = campaign.list[nextMissionIndex];
			if (CheckMissionUnlock (nextMission)) {
				nextMission.actived = true;
			}
//			return nextMission.magicId;
		}

		if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			TryUnlockEliteFirstMission (battleMissionId);
		}

	}


	public bool CheckMissionUnlock(Model_Mission modelMission)
	{
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (modelMission.magicId);
		int totalStar = GetAllMissionsStar (modelMission.difficulty);
		if (totalStar < dataMission.Star) {
			return false;
		}

		return true;
	}

	public void TryUnlockEliteFirstMission(int battleMissionId)
	{
		Assert.assert (DataMission.GetDifficulty (battleMissionId) == DataConfig.MISSION_DIFFICULTY.NORMAL);

		DataConfig.MISSION_DIFFICULTY difficulty = DataConfig.MISSION_DIFFICULTY.ELITE;
		int stageId = DataMission.GetStageId (battleMissionId);
		int missionId = DataMission.GetMissionId (battleMissionId);

		Campaign campaign = GetCampaign (difficulty, stageId);
		if (campaign.list.Count == missionId) {
			Model_Mission mission = GetMission (difficulty, stageId, 1);
			mission.actived = true;
		}

	}

	/*
	public int TryUnlockFirstMission()
	{
		DataConfig.MISSION_DIFFICULTY difficulty = DataConfig.MISSION_DIFFICULTY.NORMAL;
		int stageId = 1;
		int missionId = 1;

		Campaign campaign = GetCampaign (difficulty, stageId);
		Model_Mission mission = campaign.list [missionId];
		mission.actived = true;
		return mission.magicId;
	}
	*/

	public void SetStar(int missionMagicId, int star)
	{
		Model_Mission mission = new Model_Mission (missionMagicId);
		mission = GetMission (mission.difficulty, mission.stageId, mission.missionId);
		mission.SetStar (star);

	}

	public bool IsNextNormalCampaignUnlock(int nextStageId)
	{
		Campaign campaign = GetCampaign (DataConfig.MISSION_DIFFICULTY.NORMAL, nextStageId);
		Model_Mission first_Mission = campaign.list [0];
		bool isUnlcok = first_Mission.actived;

		return isUnlcok;
	}

	public bool IsNextEliteCampaignUnlock(int nextStageId)
	{
		Campaign campaign = GetCampaign (DataConfig.MISSION_DIFFICULTY.NORMAL, nextStageId);
		Model_Mission last_Mission = campaign.list [campaign.list.Count - 1];
		bool isUnlcok = last_Mission.actived;

		return isUnlcok;
	}

	public bool IsMissionUnlock(int magicId)
	{
		int stageId = DataMission.GetStageId (magicId);
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (magicId);
		int missionId = DataMission.GetMissionId (magicId);

		Model_Mission model_Mission = GetMission (difficulty, stageId, missionId);
		bool isUnlcok = model_Mission.actived;

		return isUnlcok;
	}


	public int GetAllMissionsStar(DataConfig.MISSION_DIFFICULTY difficult)
	{
		int star = 0;

		Chapters chapters = null;
		if (difficult == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			chapters = _chaptersNormal;
		} else {
			chapters = _chaptersElite;
		}

		foreach (Campaign campaign in chapters.list) {
			foreach (Model_Mission mission in campaign.list) {
				star += mission.starCount;
			}
		}

		return star;
	}

	public int GetAllMissionsStar(DataConfig.MISSION_DIFFICULTY difficult, int stageId)
	{
		int star = 0;

		Chapters chapters = null;
		if (difficult == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			chapters = _chaptersNormal;
		} else {
			chapters = _chaptersElite;
		}

		foreach (Campaign campaign in chapters.list) {
			if (campaign.stageId == stageId) {
				foreach (Model_Mission mission in campaign.list) {
					star += mission.starCount;
				}
			}
		}

		return star;
	}

		
}
