using UnityEngine;
using System.Collections;

public enum FunctionBtnItem_Type
{
	ProductionBuildingType = 1,
	FunctionBuildingType = 2,
	DiamondType = 3,
}
public class FunctionBtnItem {

	public enum BtnItem_Type
	{
		DetailsType = 1,
		SpeedResourcesType = 2,
		UpgradeSpeedType = 3,
		GainType = 4,
		ManageProductionType = 5,
	}
	 
	public BtnItem_Type btnItemType;
	public string btnItemName;
	public string btnItemIcon;
	public string btnItemId;

	public FunctionBtnItem(string btnItemId,BtnItem_Type btnItemType,string btnItemName,string btnItemIcon)
	{
		this.btnItemId = btnItemId;
		this.btnItemType = btnItemType;
		this.btnItemName = btnItemName;
		this.btnItemIcon = btnItemIcon; 
	}

}
