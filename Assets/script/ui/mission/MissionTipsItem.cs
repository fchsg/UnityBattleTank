using UnityEngine;
using System.Collections;

public class MissionTipsItem : MonoBehaviour {

	private DataItem _dataItem;
	private GameObject _ItemTipsPanel;

	public void InitData(GameObject itemTipsPanel, DataItem dataItem)
	{
		_ItemTipsPanel = itemTipsPanel;
		_dataItem = dataItem;
	}
		
	void OnPress(bool isPressed)
	{
		// 调用鼠标提示
		if (isPressed)
		{
			UTooltipManager.setTooltip (this.gameObject, _ItemTipsPanel, _dataItem);
		}
		else
		{
			UTooltipManager.Hidden();
		}
	}
}
