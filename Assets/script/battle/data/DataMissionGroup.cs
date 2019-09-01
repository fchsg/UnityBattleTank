using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataMissionGroup {

	public class DataCampaign
	{
		public DataConfig.MISSION_DIFFICULTY difficulty;
		public int stageId;
		public List<DataMission> missions = new List<DataMission>();

		public DataCampaign(DataConfig.MISSION_DIFFICULTY difficulty, int stageId)
		{
			this.difficulty = difficulty;
			this.stageId = stageId;
		}
	}

	private Dictionary<int, DataMission> dataMissions; //magicId, mission
	private List<DataCampaign> campaignsNormal;
	private List<DataCampaign> campaignsElite;


	private bool isLoad = false;


	public DataMission GetMission(int magicId)
	{
		DataMission mission = null;
		dataMissions.TryGetValue (magicId, out mission);
		return mission;
	}

	public DataMission[] GetAllMissions()
	{
		DataMission[] allMissions = new DataMission[dataMissions.Count];
		
		int i = 0;
		foreach (KeyValuePair<int, DataMission> pair in dataMissions) 
		{
			allMissions[i++] = pair.Value;
		}
		
		return allMissions;
	}


	public void Load(string name)
	{
		if (isLoad) 
		{
			return;		
		}
		isLoad = true;

		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		dataMissions = new Dictionary<int, DataMission> ();

		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataMission data = new DataMission();
			data.Load(subNode);

			/*
			Dictionary<int, DataMission> missions;
			dataMissions.TryGetValue((int)data.difficulty, out missions);
			if(missions == null)
			{
				missions = new Dictionary<int, DataMission>();
				dataMissions.Add((int)data.difficulty, missions);
			}
			*/

			dataMissions.Add(data.magicId, data);
		}

		CollectChapterData ();

	}

	private void CollectChapterData()
	{
		campaignsNormal = new List<DataCampaign> ();
		campaignsElite = new List<DataCampaign> ();

		foreach (DataMission m in dataMissions.Values) {
			List<DataCampaign> targetCampaigns = GetCampaigns(m.difficulty);

			int stageIndex = m.stageId - 1;
			DataCampaign targetCampaign;
			if(targetCampaigns.Count > stageIndex)
			{
				targetCampaign = targetCampaigns[stageIndex];
			}
			else
			{
				Assert.assert(stageIndex == targetCampaigns.Count, "stageId is not continuous = " + m.stageId);

				targetCampaign = new DataCampaign(m.difficulty, m.stageId);
				targetCampaigns.Add(targetCampaign);
			}

			int missionIndex = m.missionId - 1;
			Assert.assert(targetCampaign.missions.Count == missionIndex);
			targetCampaign.missions.Add(m);

		}

	}

	public List<DataCampaign> GetCampaigns(DataConfig.MISSION_DIFFICULTY difficulty)
	{
		if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			return campaignsNormal;
		} else {
			return campaignsElite;
		}
	}

	public DataCampaign GetCampaign(DataConfig.MISSION_DIFFICULTY difficulty, int stageId)
	{
		List<DataCampaign> targetCampaigns = GetCampaigns(difficulty);
		return targetCampaigns [stageId - 1];
	}

	public DataMission GetMission(DataConfig.MISSION_DIFFICULTY difficulty, int stageId, int missionId)
	{
		DataCampaign campaign = GetCampaign (difficulty, stageId);
		return campaign.missions[missionId - 1];
	}

}
