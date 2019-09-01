using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DataUnitsGroup {
	
	private Dictionary<int, DataUnit> dataUnits; //index with id

	private bool isLoad = false;
	/*
	public void SetDataUnits(DataUnit[][] units)
	{
		//this.units = units;
	}
	*/

	/*
	public DataUnit[] GetUnitsAllLevelData(int id)
	{
		if (units.Count > 0) 
		{	
			Dictionary<int, DataUnit> allLevelData;
			units.TryGetValue(id, out allLevelData);
			if(allLevelData != null)
			{
				DataUnit[] allDataUnit = new DataUnit[allLevelData.Count];
				int index = 0;
				foreach(DataUnit unit in allLevelData.Values)
				{
					allDataUnit[index++] = unit;
				}
				return allDataUnit;
			}
		}
		return null;
	}
	*/

	public DataUnit GetUnit(int id)
	{
		if (dataUnits.ContainsKey (id))
		{
			DataUnit data;
			dataUnits.TryGetValue(id, out data);
			return data;
		}

		return null;
	}

	public DataUnit[] GetAllUnits()
	{
		DataUnit[] allUnits = new DataUnit[dataUnits.Count];

		int i = 0;
		foreach (KeyValuePair<int, DataUnit> pair in dataUnits) {
			allUnits[i++] = pair.Value;
		}

		return allUnits;
	}

	public void Load(string name)
	{
		if (isLoad) {
			return;
		}
		isLoad = true;

		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		dataUnits = new Dictionary<int, DataUnit> ();

		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataUnit unit = new DataUnit();
			unit.Load(subNode);
		
			Assert.assert(GetUnitUnlockWithItem(unit.chipId) == null, "one chip link to multi items");

			dataUnits.Add(unit.id, unit);
		}
	}


	public DataUnit GetUnitUnlockWithItem(int itemId)
	{
		foreach (DataUnit unit in dataUnits.Values) {
			if(unit.chipId == itemId)
			{
				return unit;
			}
		}
		return null;
	}

}
