using UnityEngine;
using System.Collections;
using SlgPB;
public class SettlementDropItem : MonoBehaviour {

	private UISprite _bg_Sprite;
	private UITexture _Icon_Texture;
	private UILabel _res_Value_Label; 
	public string equipType;

	private PrizeItem _prizeItem;

	private GameObject _ItemTipsPanel;
	private string _ItemName;

	void Awake()
	{
		_bg_Sprite = transform.Find("bg_Sprite").GetComponent<UISprite>();
		_Icon_Texture = transform.Find("bg_Sprite/Icon_Sprite").GetComponent<UITexture>();
		_res_Value_Label = transform.Find("res_Value_Label").GetComponent<UILabel>();
	}

	public void InitData(PrizeItem data, GameObject itemTipsPanel)
	{
		_prizeItem = data;


//		string iconName = "settlement_icon_food";
//
//		switch((DataConfig.DATA_TYPE)data.type)
//		{
//		case DataConfig.DATA_TYPE.Unit:
//			break;
//		case DataConfig.DATA_TYPE.UnitPart:
//			break;
//		case DataConfig.DATA_TYPE.Building: 
//			break;
//		case DataConfig.DATA_TYPE.Mission:
//			break;
//		case DataConfig.DATA_TYPE.Battle: 
//			break;
//		case DataConfig.DATA_TYPE.DropGroup: 
//			break;
//		case DataConfig.DATA_TYPE.Food:
//			iconName = "settlement_icon_food";
//			_ItemName = "食物";
//			break;
//		case DataConfig.DATA_TYPE.Oil:
//			iconName = 	"settlement_icon_oil";
//			_ItemName = "石油";
//			break;
//		case DataConfig.DATA_TYPE.Metal:
//			iconName = 	"settlement_icon_metal";
//			_ItemName = "矿产";
//			break;
//		case DataConfig.DATA_TYPE.Rare: 
//			iconName = 	"settlement_icon_rare";
//			_ItemName = "稀土";
//			break;
//		case DataConfig.DATA_TYPE.Cash:
//			iconName = 	"settlement_icon_food";
//			_ItemName = "金币";
//			break;
//		case DataConfig.DATA_TYPE.Exp:
//			break;
//		case DataConfig.DATA_TYPE.Energy: 
//			break;
//		case DataConfig.DATA_TYPE.Equipment: 
//			break;
//		case DataConfig.DATA_TYPE.Item:
//			_ItemName = "物品";
//			break;
//		}

//		MultipleItem muitem = UIHelper.GetMultipleItem(_prizeItem.itemId,_prizeItem.num,_prizeItem.type,_prizeItem.itemLevel,_Icon_Texture);
		UIDropItem dropitem = UIHelper.GetUIDropItemByPrizeItem(_prizeItem);
		_Icon_Texture.SetTexturePath(dropitem.icon);
		_res_Value_Label.text = dropitem.count.ToString();

		_ItemTipsPanel = itemTipsPanel;
		_ItemName = dropitem.name;
	}

	public void InitHonor(GameObject itemTipsPanel)
	{
		_Icon_Texture.SetDropItemTexture((int)DataConfig.DATA_TYPE.Combat);

		SlgPB.PVPUser pvpUser = InstancePlayer.instance.model_User.model_pvpUser.selfPvpUser;
		DataLadder ladder = DataManager.instance.dataLadderGroup.GetLadder(pvpUser.rank);
		 
		// 获得的honor
		int honorChanged = InstancePlayer.instance.model_User.honorChanged;
		_res_Value_Label.text = ladder.honor.ToString();

		_ItemTipsPanel = itemTipsPanel;
		_ItemName = "荣誉";
	}
 	
	void OnPress(bool isPressed)
	{
		// 调用鼠标提示
		if (isPressed)
		{
			UTooltipManager.setTooltip (this._Icon_Texture.gameObject, _ItemTipsPanel, _ItemName);
		}
		else
		{
			UTooltipManager.Hidden();
		}
	}
}
