using UnityEngine;
using System.Collections;

public class DataDiscount {

	public int resourcesMin;

	public float foodDiscount;
	public float oilDiscount;
	public float metalDiscount;
	public float rareDiscount;


	public void Load(LitJson.JSONNode json)
	{
		resourcesMin = JsonReader.Int (json, "ResourcesMin");

		foodDiscount = JsonReader.Float (json, "FoodDiscount");
		oilDiscount = JsonReader.Float (json, "OilDiscount");
		metalDiscount = JsonReader.Float (json, "MetalDiscount");
		rareDiscount = JsonReader.Float (json, "RareDiscount");
	}

}
