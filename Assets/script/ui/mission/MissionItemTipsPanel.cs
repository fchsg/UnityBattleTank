using UnityEngine;
using System.Collections;

public class MissionItemTipsPanel : UTooltip {

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

		DataItem dataItem = data as DataItem;
		if (dataItem != null) 
		{
			name_Label.text = dataItem.name;
		}
	}
}
