using UnityEngine;
using System.Collections;
using System;

public class DataUnit {

	public class BasicBattleParam : ICloneable
	{
		public float damage;
		public float ammo;
		public float hp;
		public float hitRate; //percentage of attack NOT miss chance, miss will produce half damage
//		public float missRate;
		public float dodgeRate;
		public float doubleDamageRate; //percentage of produce double damage chance

		public object Clone()
		{
			BasicBattleParam clone = new BasicBattleParam ();
			clone.damage = damage;	
			clone.ammo 	 = ammo;		
			clone.hp  = hp;	
			clone.hitRate = hitRate;	
			clone.dodgeRate = dodgeRate; 
			clone.doubleDamageRate = doubleDamageRate;

			return clone;
		}

		public void Load(LitJson.JSONNode json)
		{
			damage = float.Parse (json ["AP"]);
			ammo = float.Parse (json ["DP"]);
			hp = float.Parse (json ["HP"]);
//			speed = 10; //test
			hitRate = float.Parse (json ["HitRate"]);
//			missRate = 1 - hitRate;
			dodgeRate = float.Parse (json ["DodgeRate"]);
			doubleDamageRate = float.Parse (json ["DoubleDamageRate"]);

	
			Assert.assert (hitRate >= 0 && hitRate <= 1, "Assert BasicBattleParam hitRate " + hitRate);
		}

		public void Copy(BasicBattleParam from)
		{
			damage = from.damage;
			ammo = from.ammo;
			hp = from.hp;
			hitRate = from.hitRate;
//			missRate = from.missRate;
			dodgeRate = from.dodgeRate;
			doubleDamageRate = from.doubleDamageRate;
		}

		public void Add(BasicBattleParam from)
		{
			damage += from.damage;
			ammo += from.ammo;
			hp += from.hp;
			hitRate += from.hitRate;
			dodgeRate += from.dodgeRate;
			doubleDamageRate += from.doubleDamageRate;
			
			Assert.assert (hitRate >= 0 && hitRate <= 1);
			Assert.assert (dodgeRate >= 0 && dodgeRate <= 1);
			Assert.assert (doubleDamageRate >= 0 && doubleDamageRate <= 1);
		}


		public int CalcPower()
		{
			float kAP = 0.06f;
			float kDP = 0.2f;
			float kHP = 0.004f;
			float kHitRate = 0.2f;
			float kDodgeRate = 0.1f;
			float kDoubleDamageRate = 0.05f;

			float p =
				kAP * damage +
				kDP * ammo +
				kHP * hp +
				kHitRate * hitRate +
				kDodgeRate * dodgeRate +
				kDoubleDamageRate * doubleDamageRate;
			return (int)p;

		}

	}


	public class BasicCost
	{
		public int costTime;
		public int costFood;
		public int costOil;
		public int costMetal;
		public int costRare;
		public int costCash;

		public void Load(LitJson.JSONNode json)
		{
			costTime = JsonReader.Int(json, "TimeCost");
			costFood = JsonReader.Int(json, "FoodCost");
			costOil = JsonReader.Int(json, "OilCost");
			costMetal = JsonReader.Int(json, "MetalCost");
			costRare = JsonReader.Int(json, "RareCost");
			costCash = JsonReader.Int(json, "CashCost");
		}
		
	}

	public class ItemCost
	{
		public int id;
		public int count;

		public void Load(string c)
		{
			int[] array = StringHelper.ReadIntArrayFromString (c);
			id = array [0];
			count = array [1];
		}

	}


	public int id;

	public BasicBattleParam battleParam;

	public string name;
	public string asset;
	public DataConfig.BULLET_TYPE bulletType;
	public DataConfig.BODY_TYPE bodyType;
	public int quality;

	public DataConfig.TARGET_SELECT targetSelect;
	public float shootCD;
	public float shootRange;
	public float closeRange;
	public float fightRange;
	public bool stopToFire;
	public bool canStandTurn;
	public float firePrepareTime;
	public float damageRange;
	public float length;
	public float breakTime;
	public float speed;
	public float costCdCash;
	public int fireCount;
	public float fireInterval; //interval seconds between each fire

	//to be removed
//	public float shootInterval; //interval seconds between each shooting on target in once fire
//	public int targetsPerFire;
//	public int targetsSelectType;

	public int[] partsId;

	public BasicCost cost;
	public int buildingLevel;

	public float repairRate;

	public int chipId;
	public int chipCount;

	public void Load(LitJson.JSONNode json)
	{
		id = int.Parse (json["ID"]);

		battleParam = new BasicBattleParam ();
		battleParam.Load (json);

		name = json["Name"];
		asset = json["Asset"];
		bulletType = (DataConfig.BULLET_TYPE)int.Parse (json ["CannonType"]);
		bodyType = (DataConfig.BODY_TYPE)int.Parse (json ["BodyType"]);
		quality = JsonReader.Int (json, "Quality");

		targetSelect = (DataConfig.TARGET_SELECT)JsonReader.Int(json, "targetSelect");
		shootCD = JsonReader.Float(json, "shootCD"); //5;

		shootRange = JsonReader.Float (json, "shootRange"); //150;
		closeRange = JsonReader.Float (json, "closeRange"); //80;
		fightRange = JsonReader.Float (json, "fightRange"); //40;
//		Assert.assert (shootRange >= closeRange);
		Assert.assert (closeRange >= fightRange);

		stopToFire = true; //int.Parse (json ["stopToFire"]) != 0;
		canStandTurn = JsonReader.Int (json, "canStandTurn") != 0;
		firePrepareTime = JsonReader.Float (json, "firePrepareTime");//1;
		damageRange = JsonReader.Float (json, "damageRange");
		length = JsonReader.Float (json, "length");//7;
		Assert.assert (length <= MapGrid.GRID_SIZE);
		breakTime = JsonReader.Float (json, "breakTime");//2;
		speed = float.Parse (json ["Speed"]);

		costCdCash = JsonReader.Float(json,"CdCash");

		fireCount = int.Parse (json ["FireCount"]);
		fireInterval = float.Parse (json ["FireInterval"]);
//		shootInterval = float.Parse (json ["ShootInterval"]);
//		targetsPerFire = int.Parse (json ["TargetsPerFire"]);
//		targetsSelectType = (DataConfig.TARGETS_SELECT_TYPE)int.Parse (json ["TargetSelectType"]);

		string str_partsId = json["PartsID"];
		partsId = StringHelper.ReadIntArrayFromString (str_partsId);

		cost = new BasicCost ();
		cost.Load (json);

		buildingLevel = int.Parse (json ["BuildingLevel"]);

		repairRate = float.Parse (json ["RepairRate"]);

		chipId = JsonReader.Int (json, "ChipID");
		chipCount = JsonReader.Int (json, "ChipCount");
	}

	public float GetCollisionRadius()
	{
		return length / 2;
	}
	
	public float GetRadius()
	{
		return length / ((2 + 4.747f) / 2);
	}

	public float GetHurtRadius()
	{
		return length / 4.747f;
	}
	
}
