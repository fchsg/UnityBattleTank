using UnityEngine;
using System.Collections;

public class DataBuilding {

	public int type;
	public int id;
	public int level;
	public int openLevel; //指挥所升级后 其他建筑解锁相应等级
	public int playerLevel;
	public int categoryId;
	public string name;
	public string assset;

	public int[] buildingID; //指挥所升级所需其他建筑id
	public int[] buildingLevel;//指挥所升级所需其他建筑level
	public float upgradeCash;

	public string description;

	public DataUnit.BasicCost cost;

	public void Load(LitJson.JSONNode json)
	{
		type = JsonReader.Int (json, "Type");
		id = JsonReader.Int (json, "ID");
		level = JsonReader.Int (json, "Level");
		playerLevel = JsonReader.Int (json, "PlayerLevel");
		openLevel = JsonReader.Int (json, "OpenLevel");

		string str_buildingID = json ["BuildingID"];
		buildingID = StringHelper.ReadIntArrayFromString (str_buildingID);
		string str_buildingLevel = json ["BuildingLevel"];
		buildingLevel = StringHelper.ReadIntArrayFromString (str_buildingLevel);			
		upgradeCash = JsonReader.Float (json, "UpgradeCash");

		categoryId = JsonReader.Int(json, "CategoryID");
		name = json ["Name"];
		assset = json ["Asset"];

		description = json["Description"];

		cost = new DataUnit.BasicCost ();
		cost.Load (json);
	}

}
