using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataBuildingGroup {
	
	private Dictionary<int, Dictionary<int, DataBuilding>> dataBuildings; 
	
	private bool isLoad = false;
	
	public DataBuilding GetBuilding(int id, int level)
	{
		Dictionary<int, DataBuilding> buildings;
		dataBuildings.TryGetValue (id, out buildings);
		if (buildings != null) 
		{
			DataBuilding building = null;
			buildings.TryGetValue (level, out building);
			if(building != null)
			{
				return building;
			}
		}	
		return null;
	}
	
	public DataBuilding[] GetAllBuildings(int id)
	{
		Dictionary<int, DataBuilding> bulidings;
		dataBuildings.TryGetValue (id, out bulidings);
		if (bulidings != null) {
			DataBuilding[] allBuildings = new DataBuilding[bulidings.Count];
			
			int i = 0;
			foreach (KeyValuePair<int, DataBuilding> pair in bulidings) 
			{
				allBuildings[i++] = pair.Value;
			}
			
			return allBuildings;
		}
		
		return null;
	}

	public int[] GetAllBuildingsType()
	{
		int[] buildingsType = new int[dataBuildings.Count];

		int i = 0;
		foreach (int key in dataBuildings.Keys) 
		{
			buildingsType[i++] = key;
		}

		return buildingsType;
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
		
		dataBuildings = new Dictionary<int, Dictionary<int, DataBuilding>> ();
		
		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataBuilding data = new DataBuilding ();
			data.Load (subNode);
			
			Dictionary<int, DataBuilding> bulidings;
			dataBuildings.TryGetValue (data.id, out bulidings);
			if (bulidings == null) 
			{
				bulidings = new Dictionary<int, DataBuilding> ();
				dataBuildings.Add (data.id, bulidings);
			}
			
			bulidings.Add (data.level, data);
		}
	}

}
