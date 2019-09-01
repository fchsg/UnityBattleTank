using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataMapGroup {

	private Dictionary<int, DataMap> dataMaps = new Dictionary<int, DataMap> ();

	public DataMap QueryMap(int id)
	{
		if (dataMaps.ContainsKey (id)) {
			return dataMaps [id];
		}

		string name = AppConfig.FOLDER_DATACONFIG + "Map" + id + ".json";
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		DataMap map = new DataMap();
		map.Load(json);

		dataMaps [id] = map;
		return map;
	}

}
