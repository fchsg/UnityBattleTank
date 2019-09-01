using UnityEngine;
using System.Collections;

public class DataSkill {

	public enum TYPE
	{
		NONE,
		BOMB = 1,
		AIR_STRIKE = 2,
		HEAL = 3,
	}

	public TYPE type;
	public int level;
	public string name;
	public float effect; //damage, heal
	public float effectRange;
	public float hintRange;
	public float duration;

	public void Load(LitJson.JSONNode json)
	{
		type = (TYPE)int.Parse(json["ID"]);
		level = int.Parse(json["Level"]);

		effect = JsonReader.Float(json, "Effect");
		effectRange = JsonReader.Float(json, "EffectRange");
		hintRange = JsonReader.Float(json, "HintRange");
		duration = JsonReader.Float(json, "Duration");
		name =  json["Name"];
	}

}
