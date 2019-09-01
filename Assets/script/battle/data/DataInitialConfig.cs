using UnityEngine;
using System.Collections;

public class DataInitialConfig {

	// 初始化资源数量
	public float initCash;
	public float initFood;
	public float intiOil;
	public float initMetal;
	public float initRare;

	// 加速所需cash
	public float unitCashFactor;
	public float buildingCashFactor;
	public float skillCashFactor;

	// 资源金钱换算比
	public float foodToCashRate;
	public float oilToCashRate;
	public float metalToCashRate;
	public float rareToCashRate;


	// TODO
	public int[] items;
	public int[] itemNum;
	
	public int[] units;
	public int[] unitNum;


	public void Load(LitJson.JSONNode json)
	{
		initCash = JsonReader.Float (json, "Cash");
		initFood = JsonReader.Float (json, "Food");
		intiOil = JsonReader.Float (json, "Oil");
		initMetal = JsonReader.Float (json, "Metal");
		initRare = JsonReader.Float (json, "Rare");

		unitCashFactor = JsonReader.Float (json, "UnitCash");
		buildingCashFactor = JsonReader.Float (json, "BuildingCash");
		skillCashFactor = JsonReader.Float (json, "SkillCash");

		foodToCashRate = JsonReader.Float (json, "FoodCash");
		oilToCashRate = JsonReader.Float (json, "OilCash");
		metalToCashRate = JsonReader.Float (json, "MetalCash");
		rareToCashRate = JsonReader.Float (json, "RareCash"); 

	}

}
