using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataUnitPartGroup {

	
	private Dictionary<int, Dictionary<int, DataUnitPart>> dataUnitParts; //index with id / level
	
	private bool isLoad = false;
	
	public DataUnitPart GetPart(int id, int level)
	{
		Dictionary<int, DataUnitPart> parts;
		dataUnitParts.TryGetValue (id, out parts);
		if (parts != null) {
			DataUnitPart part;
			parts.TryGetValue(level, out part);
			return part;
		}
		return null;
	}
	
	public DataUnitPart[] GetAllLevelParts(int id)
	{
		Dictionary<int, DataUnitPart> parts;
		dataUnitParts.TryGetValue (id, out parts);
		if (parts != null) {
			
			DataUnitPart[] allParts = new DataUnitPart[parts.Count];
			
			int i = 0;
			foreach (KeyValuePair<int, DataUnitPart> pair in parts) 
			{
				allParts[i++] = pair.Value;
			}
			
			return allParts;
		}
		return null;


	}

	public int GetPartMaxLevel(int id)
	{
		Dictionary<int, DataUnitPart> parts;
		dataUnitParts.TryGetValue (id, out parts);
		if (parts != null) {
			return parts.Count;
		}
		return 0;

	}
	
	public void Load(string name)
	{
		if (isLoad) 
		{
			return;		
		}
		isLoad = true;


		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		dataUnitParts = new Dictionary<int, Dictionary<int, DataUnitPart>> ();
		
		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataUnitPart data = new DataUnitPart();
			data.Load(subNode);

			Dictionary<int, DataUnitPart> parts;
			dataUnitParts.TryGetValue (data.id, out parts);
			if (parts == null) {
				parts = new Dictionary<int, DataUnitPart>();
				dataUnitParts.Add(data.id, parts);
			}

			parts.Add(data.level, data);
		}

		
	}


}
