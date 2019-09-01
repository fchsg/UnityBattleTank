using UnityEngine;
using System.Collections;

public class DataItem {

	public int id;
	public DataConfig.ITEM_CATEGORY category;

	public int uiAltasType;
	public int uiAltasIndex;

	public string name;
	public int quality;
	public string icon;
	public string desc;
	public int dropGroup;
	public string script;

	public int missionId1;
	public int missionId2;
	public int missionId3;

	public void Load(LitJson.JSONNode json)
	{
		id = int.Parse(json["ID"]);
		category = (DataConfig.ITEM_CATEGORY)JsonReader.Int(json, "Category");

		uiAltasType = GetAtlasType (id);
		uiAltasIndex = GetAtlasIndex (id);

		name = json["Name"];
		quality = JsonReader.Int (json, "Quality");
		icon = json["Icon"];
		desc = json["Desc"];
		dropGroup = JsonReader.Int (json, "DropGroup");
		script = json["Script"];

		missionId1 = JsonReader.Int (json, "Mission_1");
		missionId2 = JsonReader.Int (json, "Mission_2");
		missionId3 = JsonReader.Int (json, "Mission_3");

	}

	public int GetAtlasType(int id)
	{
		return (id / 10000);
	}

	public int GetAtlasIndex(int id)
	{
		return (id / 1000) % 10;
	}

}
