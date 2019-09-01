using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataDiscountGroup {

	private List<DataDiscount> _discount = new List<DataDiscount> ();
	
	public DataDiscount GetDiscount(float resourcesCount)
	{
		int selectIndex = 0;
		int count = _discount.Count;
		for (int i = 1; i < count; ++i) 
		{	
			DataDiscount discount = _discount[i];
			if(resourcesCount >= discount.resourcesMin)
			{
				++selectIndex;
			}
		}

		return _discount [selectIndex];
	}
	
	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
	
		foreach (LitJson.JSONNode subNode in json.Childs) 
		{
			DataDiscount data = new DataDiscount();
			data.Load(subNode);
			_discount.Add(data);
		}
	}

}
