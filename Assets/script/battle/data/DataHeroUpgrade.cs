using UnityEngine;
using System.Collections;

public class DataHeroUpgrade {

	public int id;
	public int stage;
	public int requireLevel;
	public float kAP;
	public float kDP;
	public float kHP;
	public int aLeadership;
	public int itemId;
	public int itemCount;

	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int (json, "ID");
		stage = JsonReader.Int (json, "Stage");
		requireLevel = JsonReader.Int (json, "Lv");

		kAP = JsonReader.Float (json, "AP");
		kDP = JsonReader.Float (json, "DP");
		kHP = JsonReader.Float (json, "HP");

		aLeadership = JsonReader.Int (json, "Leadership");

		itemId = JsonReader.Int (json, "ItemID");
		itemCount = JsonReader.Int (json, "Count");

	}


}
