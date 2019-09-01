using UnityEngine;
using System.Collections;
using System;

public class DataHero : ICloneable {
	public int id;
	public string name;
//	public int level;
	public DataUnit.BasicBattleParam basicParam;
	public int leadership;
	public int type;
//	public int exp;
//	public DataSkill.TYPE skill;
//	public int quality;


	public const int STAGE_MAX = 11;

	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int (json, "ID");
		name = json ["Name"];
//		level = JsonReader.Int (json, "Level");

		basicParam = new DataUnit.BasicBattleParam ();
		basicParam.Load (json);

		leadership = JsonReader.Int (json, "Leadership");
		type = JsonReader.Int(json,"Arms");
//		exp = JsonReader.Int (json, "Exp");

//		skill = (DataSkill.TYPE)JsonReader.Int (json, "Skill");
//		quality = JsonReader.Int (json, "Quality");

		
	}

	public object Clone()
	{
		DataHero clone = new DataHero ();
		clone.id = id;	
		clone.name 	 = name;		
//		clone.level  = level;	
		clone.basicParam = basicParam.Clone() as DataUnit.BasicBattleParam;
		clone.leadership = leadership; 

		return clone;
	}

	public void AddUpgradeEffect(DataHeroUpgrade upgrade, int level)
	{
//		int level_1 = level - 1;
		basicParam.damage *= (1 + upgrade.kAP * level);
		basicParam.ammo *= (1 + upgrade.kDP * level);
		basicParam.hp *= (1 + upgrade.kHP * level);
	}


}
