using UnityEngine;
using System.Collections;

public class DataHeroLeadership {
	public int id;

	public float kTank;
	public float kGun;
	public float kMissile;
	public float kCannon;
	public float kUnknown;

	public int quality;
	public DataSkill.TYPE skill;

	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int (json, "ID");
		
		kTank = JsonReader.Float (json, "Tank");
		kGun = JsonReader.Float (json, "Gun");
		kMissile = JsonReader.Float (json, "Missile");
		kCannon = JsonReader.Float (json, "Cannon");
		kUnknown = JsonReader.Float (json, "Unknown");
		quality = JsonReader.Int (json, "Quality");
		skill = (DataSkill.TYPE)JsonReader.Int (json, "Skill");

	}


}
