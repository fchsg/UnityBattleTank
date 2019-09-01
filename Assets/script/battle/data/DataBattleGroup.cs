using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataBattleGroup {

//	private Dictionary<int, DataBattle> dataBattlesNormal;
//	private Dictionary<int, DataBattle> dataBattlesElite;

	private List<DataBattle> dataBattles;


	private bool isLoad = false;

	public void Load(string name)
	{
		if (isLoad) {
			return;		
		}
		isLoad = true;


		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);


		dataBattles = new List<DataBattle> ();
//		dataBattlesNormal = new Dictionary<int, DataBattle> ();
//		dataBattlesElite = new Dictionary<int, DataBattle> ();
		
		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataBattle data = new DataBattle();
			data.Load (subNode);

			dataBattles.Add(data);
//			if(data.difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL)
//			{
//				dataBattlesNormal.Add (data.chapter, data);
//			}
//			else
//			{
//				dataBattlesElite.Add (data.chapter, data);
//			}

		}
	}

	public DataBattle GetBattle(DataConfig.MISSION_DIFFICULTY difficulty, int stageId)
	{
		foreach (DataBattle battle in dataBattles) {
			if(battle.difficulty == difficulty && battle.chapter == stageId)
			{
				return battle;
			}
		}

		return null;
	}

	/*
	public Dictionary<int, DataBattle> GetAllDataBattlesMap()
	{
		Dictionary<int, DataBattle> map = new Dictionary<int, DataBattle> ();

		foreach (KeyValuePair<int, DataBattle> pair in dataBattlesNormal) {
			Assert.assert(!map.ContainsKey(pair.Key));
			map.Add(pair.Key, pair.Value);
		}

		foreach (KeyValuePair<int, DataBattle> pair in dataBattlesElite) {
			Assert.assert(!map.ContainsKey(pair.Key));
			map.Add(pair.Key, pair.Value);
		}
		
		return map;
	}

	public Dictionary<int, DataBattle> GetDataBattlesMap(DataConfig.MISSION_DIFFICULTY difficulty)
	{
		if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			return dataBattlesNormal;
		} else {
			return dataBattlesElite;
		}
	}

	public DataBattle GetDataBattle(DataConfig.MISSION_DIFFICULTY difficulty, int stage)
	{
		Dictionary<int, DataBattle> map = GetDataBattlesMap (difficulty);
		return map [stage];
	}
	*/
	
}
