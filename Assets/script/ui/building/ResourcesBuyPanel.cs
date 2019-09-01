using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public enum ResourcesBuyType
{
	UpLevelType = 0,//建筑升级
	BugQueueUpLevelType = 1,//购买队列升级
	TrainTankType = 2,//训练坦克
	RepairTankType = 3,//维修坦克
	StrengThenPartType = 4,//升级部件
}
/// <summary>
/// 购买资源统一面板
/// </summary>
public class ResourcesBuyPanel : PanelBase {


	private BoxCollider _collider;
	private UIButton _closeBtn;
	private UIButton _buyBtn;
	private UILabel _cashLabel;
	private UILabel _remainingLabel;
	private UILabel _res_1_label;
	private UILabel _res_2_label;
	private UILabel _res_3_label;
	private UILabel _res_4_label;

	private UISprite _1_bg;
	private UISprite _2_bg;
	private UISprite _3_bg;
	private UISprite _4_bg;
	ResourcesBuyType _resourcesBuyType;

	//data  
	Model_Building _model_building;
	Model_Unit _model_unit;
	int _StrengthenTimes;

	int[] _resArr = new int[]{};
	Dictionary<int,string> _spritedic = new Dictionary<int, string>();
	Dictionary<int,int> _newdic = new Dictionary<int, int>();
	List<UILabel> _labelList = new List<UILabel>();
	List<UISprite> _spriteList = new List<UISprite>();
	override public void Init ()
	{
		base.Init ();

	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		_resArr = parameters[0] as int[]; 
		_resourcesBuyType = (ResourcesBuyType)parameters[1];
		switch(_resourcesBuyType)
		{
			case ResourcesBuyType.TrainTankType:
				_model_unit = parameters[2] as Model_Unit;
				break;
			case ResourcesBuyType.StrengThenPartType:
			_StrengthenTimes = (int)parameters[2];
					break;
		}
		InitUI();
		UpdateUI();
	}

	void UpdateUI()
	{
		int index = 0;
		for(int i = 0;i < _resArr.Length ;i++)
		{

			int arrValue = _resArr[i];
			if(arrValue != 0)
			{
				_newdic.Add(index,_resArr[i]);
				string resName = "";
				if(i == 0)
				{
					resName = "Food";
				}
				else if(i == 1)
				{
					resName = "Accel";
				}
				else if(i == 2)
				{
					resName = "Metal";
				}
				else if(i == 3)
				{
					resName = "Rare";
				}
				_spritedic.Add(index,resName);
				index += 1;
			}
		}
		index = 0;
		for(int i = 0 ; i < _newdic.Count ; i++)
		{
			_spriteList[i].gameObject.SetActive(true);
			_spriteList[i].spriteName = _spritedic[i];
			_labelList[i].text = UIHelper.SetStringColor( _newdic[i].ToString());
		}

		int needCash = Model_Helper.GetResourcesNeedCash((float)_resArr[0],(float)_resArr[1],(float)_resArr[2],(float)_resArr[3]);
		_cashLabel.text = needCash + "";

		_spritedic.Clear();
		_newdic.Clear();
	}

	public void OnBuy()
	{
		QuickBuy();
	}

	public void InitUI()
	{
		_closeBtn = transform.Find("UpRequire/bg_Sprite/BtnClose").GetComponent<UIButton>();
		_buyBtn = transform.Find("UpRequire/Btn_speed_Sprite").GetComponent<UIButton>();
		
		EventDelegate ev = new EventDelegate (OnDeletePanel);
		_closeBtn.onClick.Add (ev);
		
		EventDelegate evsp = new EventDelegate (OnBuy);
		_buyBtn.onClick.Add (evsp);
		
		_cashLabel = _buyBtn.transform.Find("cash_Label").GetComponent<UILabel>();
		
		_res_1_label = transform.Find("UpRequire/1_bg/res_1_Label").GetComponent<UILabel>();
		_res_2_label = transform.Find("UpRequire/2_bg/res_2_Label").GetComponent<UILabel>();
		_res_3_label = transform.Find("UpRequire/3_bg/res_3_Label").GetComponent<UILabel>();
		_res_4_label = transform.Find("UpRequire/4_bg/res_4_Label").GetComponent<UILabel>();
		_1_bg = transform.Find("UpRequire/1_bg").GetComponent<UISprite>();
		_2_bg = transform.Find("UpRequire/2_bg").GetComponent<UISprite>();
		_3_bg = transform.Find("UpRequire/3_bg").GetComponent<UISprite>();
		_4_bg = transform.Find("UpRequire/4_bg").GetComponent<UISprite>();
		_labelList.Add(_res_1_label);
		_labelList.Add(_res_2_label);
		_labelList.Add(_res_3_label);
		_labelList.Add(_res_4_label);
		_spriteList.Add(_1_bg);
		_spriteList.Add(_2_bg);
		_spriteList.Add(_3_bg);
		_spriteList.Add(_4_bg);
		foreach(UISprite sp in _spriteList)
		{
			sp.gameObject.SetActive(false);
		}
	}

	public void OnDeletePanel()
	{
		Delete();
	}

	void OnDestroy()
	{
		_resArr = new int[]{};
		_spritedic.Clear();
		_newdic.Clear();
		_labelList.Clear();
		_spriteList.Clear();
	}
	//==================================================================

	void QuickBuy()
	{

		QuickBuyRequest request = new QuickBuyRequest ();
		request.api = new Model_ApiRequest ().api;

		int [] types = new []{
			(int)Model_Production.Production_Type.Food,
			(int)Model_Production.Production_Type.Oil,
			(int)Model_Production.Production_Type.Metal,
			(int)Model_Production.Production_Type.Rare,
			
		};
		
		for (int i = 0; i < 4; i++) 
		{
			Production product = new Production ();
			product.resourceType = types[i];
			product.num = _resArr[i];
			if(_resArr[i] != 0)
			{
				request.buy.Add(product);
			}
		}
		int needCash = Model_Helper.GetResourcesNeedCash((float)_resArr[0],(float)_resArr[1],(float)_resArr[2],(float)_resArr[3]);
		if(ConnectionValidateHelper.IsEnoughCashBuyResources(needCash) == 0)
		{
			UIHelper.LoadingPanelIsOpen(true);
			(new PBConnect_quickBuy ()).Send (request, OnQuickBuy);
		}
		else
		{
			UIHelper.BuyCashUI();
		}



	}
	void OnQuickBuy(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			if(_resourcesBuyType == ResourcesBuyType.BugQueueUpLevelType )
			{
				NotificationCenter.instance.DispatchEvent(Notification_Type.RequestBuilingBugQueue,new Notification(""));
			}
			else if(_resourcesBuyType == ResourcesBuyType.UpLevelType )
			{
				NotificationCenter.instance.DispatchEvent(Notification_Type.RequestBuilingUpLevel,new Notification(""));
			}
			else if(_resourcesBuyType == ResourcesBuyType.TrainTankType )
			{
				NotificationCenter.instance.DispatchEvent(Notification_Type.RequestTrainTank,new Notification(_model_unit));
			}
			else if(_resourcesBuyType == ResourcesBuyType.RepairTankType)
			{
				NotificationCenter.instance.DispatchEvent(Notification_Type.RequestRepairTank,new Notification("RequestRepairTank"));
			}
			else if(_resourcesBuyType == ResourcesBuyType.StrengThenPartType)
			{
				NotificationCenter.instance.DispatchEvent(Notification_Type.RequestStrengthenPart,new Notification(_StrengthenTimes));
			}

			OnDeletePanel();
			Trace.trace("OnQuickBuy success",Trace.CHANNEL.UI);

		} else {
			Trace.trace("OnQuickBuy failed",Trace.CHANNEL.UI);
		}
	}


}
