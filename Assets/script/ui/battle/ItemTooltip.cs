using UnityEngine;
using System.Collections;

public class ItemTooltip : UTooltip
{
	public UILabel name_Label;

	void Awake()
	{
		name_Label = UIHelper.FindChildInObject (this.gameObject, "tip_Container/name_Label").GetComponent<UILabel>();  
	}
		
	public override void setTipData(object data)
	{
		if (data == null) 
		{
			return;
		}

		string itemName = data as string;
		if (itemName == null) 
		{
			return;
		}
		name_Label.text = itemName;

	}
}
