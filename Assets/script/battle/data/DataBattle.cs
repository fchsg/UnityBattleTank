using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataBattle {

	public int id;
	public int chapter;
//	public int preChapter;
//	public int[] missionIds;

	public DataConfig.MISSION_DIFFICULTY difficulty;


	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int (json, "ID");
		chapter = JsonReader.Int (json, "Chapter");
//		preChapter = JsonReader.Int (json, "PreChapter");
		difficulty = (DataConfig.MISSION_DIFFICULTY)JsonReader.Int (json, "Difficulty");

//		string missionsStr = json["MissionIDs"];
//		missionIds = StringHelper.ReadIntArrayFromString (missionsStr);

//		difficulty = (preChapter > 0) ? DataConfig.MISSION_DIFFICULTY.ELITE : DataConfig.MISSION_DIFFICULTY.NORMAL;

	}

//	public DataMission GetMission(int index)
//	{
//		int id = missionIds[index];
//		return DataManager.instance.dataMissionGroup.GetMission (id);
//	}


}
