using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataProductGroup {

	private Dictionary<int, DataProduct> _products; // level
	
	public DataProduct GetProduct(int level)
	{
		DataProduct product;
		_products.TryGetValue (level, out product);
		if (product != null) {
			return product;
		}
		return null;
	}

	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		_products = new Dictionary<int, DataProduct>();
		
		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataProduct data = new DataProduct();
			data.Load(subNode);

			_products.Add(data.level, data);
		}
	}

}
