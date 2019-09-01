using UnityEngine;
using System.Collections;

public class DataUnitPart {

	public int id;

	public int level;
	public int mainLevel;

	public DataUnit.BasicBattleParam battleParam;

	public DataUnit.BasicCost cost;
	public DataUnit.ItemCost itemCost1;
	public DataUnit.ItemCost itemCost2;

	public string name
	{
		get
		{
			switch (id) 
			{
			case 1:
				return "车体系统";
			case 2:
				return "武器系统";
			case 3:
				return "悬架系统";
			case 4:
				return "动力系统";
			}
			return "";
		}
	}

	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int (json, "ID");

		level = JsonReader.Int (json, "Level");
		mainLevel = JsonReader.Int (json, "MainLevel");

		battleParam = new DataUnit.BasicBattleParam ();
		battleParam.Load (json);

		cost = new DataUnit.BasicCost ();
		cost.Load (json);

		itemCost1 = new DataUnit.ItemCost ();
		itemCost1.Load (json ["Item_1"]);

		itemCost2 = new DataUnit.ItemCost ();
		itemCost2.Load (json ["Item_2"]);
		
	}

}
