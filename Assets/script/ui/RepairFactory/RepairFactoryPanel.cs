using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

/// <summary>
/// 维修工厂界面
/// </summary>

public class RepairFactoryPanel : PanelBase {

	public class RepairItemData
	{
		public Model_Unit modelUnit;
		public float slider;
	}

	private Transform _RepairFactory;
	private GameObject _TankListContainer;
	private UIButton _closeBtn;
	private UIScrollView _repairScroll;
	private UIGrid _repairGrid;
//	private UIWrapContent _wrapContent;
	private UIButton _immediatelyRepair_btn;
	private UIButton _allChoose_Btn;
	private UIButton _Repair_btn;
	private Transform _speed_Container;
	private UIButton _speedBtn;
	public UIButton _finish_Btn;

	private UILabel _immediatelyRepair_CoinValue;
	private UILabel _RepairTime_Value;
	private UILabel _speedCoinLabel;

	private UISprite _resSprite_1;
	private UISprite _resSprite_2;
	private UISprite _resSprite_3;
	private UISprite _resSprite_4;
	private UILabel _resLabel_1;
	private UILabel _resLabel_2;
	private UILabel _resLabel_3;
	private UILabel _resLabel_4;

	private UISlider _Timer_Colored_Slider;
	private UILabel _timerLabel;

	private UILabel _TankDamage_ValueLabel;//战损
	private UILabel _TankDamageIngLabel; 
	private UILabel _TankDamageIng_ValueLabel;//正在维修
	//data
	Model_RepairUnit _model_RepairUnit;
	List<SlgPB.Unit> _repairUnitsList;
	List<Model_Unit> _needRepairUnitsList = new List<Model_Unit>();//待维修的坦克列表
	Dictionary<int, Model_Unit> _repairUnits;
	DataUnit _dataUnit;

	Dictionary<int, int> _allResCountDic = new Dictionary<int, int>();
	List<UISprite> _resSpriteList = new List<UISprite>();
	List<UILabel> _resLabelList = new List<UILabel>();
	Dictionary<int,TankRepairItem> _tankRepairItemDic = new Dictionary<int, TankRepairItem>();
	int _needUnitCount = 0;//初始记录需要维修的坦克数量
	void Start(){
		
		NotificationCenter.instance.AddEventListener( Notification_Type.RequestRepairTank,RequestRepairTank);
	}

	override public void Init ()
	{
		base.Init ();
		UpdateItem();
//		CreateTankItem(3);
		CreateWrapTankItem(_needRepairUnitsList);
	}
 
	
	void Update () {
		
		UpdateUI();
		 
	}

	void UpdateItem()
	{
		_repairUnits = InstancePlayer.instance.model_User.unlockUnits;
		if(_needRepairUnitsList == null)
		{
			_needRepairUnitsList = new List<Model_Unit>();
		}
		else
		{
			_needRepairUnitsList.Clear();
		}
		foreach (KeyValuePair<int, Model_Unit> kv in _repairUnits)	
		{
			Model_Unit model_Unit = kv.Value;
			if(model_Unit.onDamaged > 0 || model_Unit.onRepair > 0)
			{
				_needRepairUnitsList.Add(model_Unit);
				Trace.trace("_needRepairUnitsList  " +  model_Unit.unitId ,Trace.CHANNEL.UI);
			}	
		}

//		SetWrapContent(_repairGrid,_repairScroll,_needRepairUnitsList,OnUpdateItemMain);
	}
	 
	void UpdateUI()
	{
		ResData();
		BattleDamage();
		UpdateSlider();
	}

	//更新维修时间进度条
	void UpdateSlider()
	{
		_model_RepairUnit = InstancePlayer.instance.model_User.model_RepairUnit;
		_repairUnitsList = _model_RepairUnit.GetRepairUnits();
		int repairTotalTimeSec = _model_RepairUnit.repairTotalTimeSec;
		int repairLeftTime = _model_RepairUnit.repairLeftTime;

		if(repairLeftTime > 0)
		{
			_speed_Container.gameObject.SetActive(true);
			_immediatelyRepair_btn.gameObject.SetActive(false);
			_allChoose_Btn.gameObject.SetActive(false);
			_Repair_btn.gameObject.SetActive(false);
			_finish_Btn.gameObject.SetActive(false);
		}
		else if(repairLeftTime == 0 && repairTotalTimeSec > 0)
		{
			_speed_Container.gameObject.SetActive(false);
			_immediatelyRepair_btn.gameObject.SetActive(false);
			_allChoose_Btn.gameObject.SetActive(false);
			_Repair_btn.gameObject.SetActive(false);
			_finish_Btn.gameObject.SetActive(true);
		}
		else
		{
			_speed_Container.gameObject.SetActive(false);
			_immediatelyRepair_btn.gameObject.SetActive(true);
			_allChoose_Btn.gameObject.SetActive(true);
			_Repair_btn.gameObject.SetActive(true);
			_finish_Btn.gameObject.SetActive(false);
		}
		_Timer_Colored_Slider.value = (float)repairLeftTime / repairTotalTimeSec;
		_timerLabel.text = UIHelper.setTimeDHMS(repairLeftTime);
		// Clear CD Need Cash
	
		int needCash = InstancePlayer.instance.model_User.model_InitialConfig.GetClearUnitCDCash ((float)repairLeftTime);
		_speedCoinLabel.text = needCash + "";

		//正在维修数量显示
		int repairIngCount = 0;
		foreach(SlgPB.Unit unit in _repairUnitsList)
		{
			repairIngCount += unit.onRepair;
		}
		_TankDamageIng_ValueLabel.text = repairIngCount.ToString();
	}
	//计算战斗损耗
	void BattleDamage()
	{
		int damageCount = 0;
		if(_tankRepairItemDic != null)
		{
			_repairUnits = InstancePlayer.instance.model_User.unlockUnits;
			foreach (KeyValuePair<int, Model_Unit> kv in _repairUnits)	
			{
				Model_Unit model_Unit = kv.Value;
				int damage = model_Unit.onDamaged + model_Unit.onRepair;
				damageCount = damage + damageCount;
			}
			_TankDamage_ValueLabel.text = damageCount.ToString();
			if(damageCount <= 0)
			{
				_immediatelyRepair_btn.isEnabled = false;
				_Repair_btn.isEnabled = false;
			}
			else
			{
				_immediatelyRepair_btn.isEnabled = true;
				_Repair_btn.isEnabled = true;
			}
		}

	}

	void ResData()
	{
		if(_tankRepairItemDic != null)
		{
			int foodCount = 0;
			int oilCount = 0;
			int matelCount = 0;
			int rareCount = 0;
			int timeCount = 0;
			int cashCount = 0;
			
			foreach (KeyValuePair<int,TankRepairItem> kv in _tankRepairItemDic)	
			{
				TankRepairItem tankItem = kv.Value;
				Dictionary<int, int> resCountDic = tankItem.GetResCountDic();
				int res_1 = 0;
				resCountDic.TryGetValue(1,out res_1);
				foodCount = foodCount + res_1;
				
				int res_2 = 0;
				resCountDic.TryGetValue(2,out res_2);
				oilCount = oilCount + res_2;
				
				int res_3 = 0;
				resCountDic.TryGetValue(3,out res_3);
				matelCount = matelCount + res_3;
				
				int res_4 = 0;
				resCountDic.TryGetValue(4,out res_4);
				rareCount = rareCount + res_4;
				
				int res_5 = 0;
				resCountDic.TryGetValue(5,out res_5);
				timeCount = timeCount + res_5;

				int res_6 = 0;
				resCountDic.TryGetValue(6,out res_6);
				cashCount = cashCount + res_6;
			}
			
			if(_allResCountDic != null)
			{
				_allResCountDic.Clear();
				_allResCountDic.Add(1,foodCount);
				_allResCountDic.Add(2,oilCount);
				_allResCountDic.Add(3,matelCount);
				_allResCountDic.Add(4,rareCount);
				_allResCountDic.Add(5,timeCount);
				_allResCountDic.Add(6,cashCount);

			}
		}

		if(_allResCountDic != null)
		{	
			int allTime;
			_allResCountDic.TryGetValue(5,out allTime);
			_RepairTime_Value.text = UIHelper.setTimeDHMS(Mathf.RoundToInt(allTime)).ToString();

			int allCash;
			_allResCountDic.TryGetValue(6,out allCash);
			_immediatelyRepair_CoinValue.text = (int)allCash + "";
			
			for(int i = 1 ;i <= 4;i++)
			{
				int resNum = 0;
				_allResCountDic.TryGetValue(i,out resNum);
				int requst = -1;
				if(i == 1)
				{
					requst = ConnectionValidateHelper.IsEnoughFoodUse((int)resNum);
					if(requst == 0)
					{
						_resLabelList[i-1].text = Mathf.RoundToInt(resNum).ToString() + "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaFoodRes());
					}
					else
					{
						_resLabelList[i-1].text = UIHelper.SetStringColor(Mathf.RoundToInt(resNum).ToString()) + "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaFoodRes()) ;
					}
				}
				else if(i == 2)
				{
					requst = ConnectionValidateHelper.IsEnoughOilUse((int)resNum);
					if(requst == 0)
					{
						_resLabelList[i-1].text = Mathf.RoundToInt(resNum).ToString()+ "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaOilRes());
					}
					else
					{
						_resLabelList[i-1].text = UIHelper.SetStringColor(Mathf.RoundToInt(resNum).ToString()) + "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaOilRes());
					}
				}
				else if(i == 3)
				{
					requst = ConnectionValidateHelper.IsEnoughMetalUse((int)resNum);
					if(requst == 0)
					{
						_resLabelList[i-1].text = Mathf.RoundToInt(resNum).ToString()+ "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaMatelRes());
					}
					else
					{
						_resLabelList[i-1].text = UIHelper.SetStringColor( Mathf.RoundToInt(resNum).ToString()) + "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaMatelRes());
					}
				}
				else if(i == 4)
				{
					requst = ConnectionValidateHelper.IsEnoughRareUse((int)resNum);
					if(requst == 0)
					{
						_resLabelList[i-1].text = Mathf.RoundToInt(resNum).ToString()+ "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaRareRes());
					}
					else
					{
						_resLabelList[i-1].text = UIHelper.SetStringColor(Mathf.RoundToInt(resNum).ToString()) + "/" + UIHelper.SetResourcesShowFormat(Model_Helper.GetPlayerHavaRareRes());
					}
				}
			}
		}
	}

	void OnUpdateItemMain(GameObject go, int index, int realIndex)
	{
		
		OnUpateItem(go,index,realIndex,_needRepairUnitsList,_tankRepairItemDic);
	}
	// 
	void OnUpateItem(GameObject go, int index, int realIndex,List<Model_Unit> dataList,Dictionary<int,TankRepairItem> dataDic)
	{
		int index_ = 0;
		int indexList = Mathf.Abs(realIndex);
		int tankCount = dataList.Count;
		TankRepairItem Item1 = go.transform.Find("TankRepairItem1").GetComponent<TankRepairItem>();
		TankRepairItem tankItem2 = go.transform.Find("TankRepairItem2").GetComponent<TankRepairItem>();

		index_ = indexList * 2;
		if(index_ > (tankCount - 1) )
		{
			Item1.gameObject.SetActive(false);
			tankItem2.gameObject.SetActive(false);
			return;
		}
		else
		{
			Item1.gameObject.SetActive(true);
			Item1.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].unitId))
			{
				dataDic.Add(dataList[index_].unitId,Item1);
			}
			else
			{	
				dataDic.Remove(dataList[index_].unitId);
				dataDic.Add(dataList[index_].unitId,Item1);
			}
		}
		index_ = indexList * 2 + 1;
		if(index_ > (tankCount - 1))
		{
			tankItem2.gameObject.SetActive(false);
			return;
		}
		else
		{
			tankItem2.gameObject.SetActive(true);
			tankItem2.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].unitId))
			{
				dataDic.Add(dataList[index_].unitId,tankItem2);
			}
			else
			{	
				dataDic.Remove(dataList[index_].unitId);
				dataDic.Add(dataList[index_].unitId,tankItem2);
			}
		}

	


	}
	//
	void SetWrapContent(UIGrid grid,UIScrollView scrollView,List<Model_Unit> dataList,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		int tankCont = dataList.Count;
		int itemCount = 0;
		if(tankCont <= 4)
		{
			scrollView.enabled = false;
		}
		else
		{
			scrollView.enabled = true;
		}
		if(tankCont % 2 == 0)
		{
			itemCount = tankCont / 2;
		}
		else
		{
			itemCount = tankCont / 2 + 1;
		}
	 
		UIWrapContent wrap = null;
		if(grid.gameObject.GetComponent<UIWrapContent>())
		{
			wrap = grid.gameObject.GetComponent<UIWrapContent>();
		}
		else
		{
			grid.gameObject.AddComponent<UIWrapContent>();
			wrap = grid.gameObject.GetComponent<UIWrapContent>();

		}
		if(wrap != null)
		{
			//绑定方法
			wrap.itemSize = (int)grid.cellHeight;
			wrap.minIndex = -(itemCount -1);
			wrap.maxIndex = 0;

			wrap.onInitializeItem = OnUpdateItemMain;
			wrap.enabled = true;
			wrap.SortAlphabetically();
		}
	}

	void CreateWrapTankItem (List<Model_Unit> dataList)
	{
		int count = dataList.Count;
		if (_repairGrid != null) {
			_repairGrid.DestoryAllChildren();
		}
		_tankRepairItemDic.Clear();
		for(int i = 0 ;i < count ;i++)
		{
			if(_repairGrid.gameObject != null){
				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "RepairFactory/TankRepairItem");
				GameObject item = NGUITools.AddChild(_repairGrid.gameObject,tankItem);
				TankRepairItem repairItem = item.GetComponent<TankRepairItem>();
				repairItem.Init(dataList[i]);
				_tankRepairItemDic.Add(dataList[i].unitId,repairItem);
			}
		}
		_repairGrid.Reposition();

	}
	void CreateTankItem (int count)
	{
		if (_repairGrid != null) {
			_repairGrid.DestoryAllChildren();
		}
		for(int i = 0 ;i < count ;i++)
		{
			if(_repairGrid.gameObject != null){
				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "RepairFactory/TwoTankItemContainer");
				GameObject item = NGUITools.AddChild(_repairGrid.gameObject,tankItem);
			}
		}
		_repairGrid.Reposition();

	}


	public void RequestRepairTank(Notification notification)
	{
		if(notification._data == "RequestRepairTank")
		{
			RepairUnit();
		}
		else
		{
			Model_Unit unit = notification._data as Model_Unit;
			_tankRepairItemDic.Remove(unit.unitId);
			_repairGrid.Reposition();
		}

	}
	//  立即维修 
	void OnImmediatelyRepair()
	{
		int allCash;
		_allResCountDic.TryGetValue(6,out allCash);
		int requst = ConnectionValidateHelper.IsEnoughCashBuyResources((int)allCash);
		if(requst == 0)
		{
			ImmediatelyRepairUnit();
		}
		else
		{
			UIHelper.BuyCashUI();
		}

	}
	//	全选
	void OnAllChoose()
	{
		if(_tankRepairItemDic != null)
		{
			foreach (KeyValuePair<int,TankRepairItem> kv in _tankRepairItemDic)	
			{
				Trace.trace("全选" +  kv.Key,Trace.CHANNEL.UI);
				TankRepairItem tankItem = kv.Value;
				tankItem.SetRepairAllTank();	 
			}
		}
	}
	//	维修 
	void OnRepair()
	{
		int foodCount = 0;
		int oilCount = 0;
		int matelCount = 0;
		int rareCount = 0;
		_allResCountDic.TryGetValue(1,out foodCount);
		_allResCountDic.TryGetValue(2,out oilCount);
		_allResCountDic.TryGetValue(3,out matelCount);
		_allResCountDic.TryGetValue(4,out rareCount);

		int[] _resArr = Model_Helper.GetPlayerNeedBuyRes((int)foodCount,(int)oilCount,(int)matelCount,(int)rareCount);
		bool isNeedBuyRes = ConnectionValidateHelper.IsResourcesEnoughToUse(_resArr);
		if(isNeedBuyRes)
		{
			RepairUnit();
		}
		else
		{
			UIController.instance.CreatePanel(UICommon.UI_TIPS_BUYRES, _resArr,ResourcesBuyType.RepairTankType);
		}

	}
	//	加速
	void OnSpeed()
	{
		int requst = ConnectionValidateHelper.IsEnoughCashClearRepairUnitCD(InstancePlayer.instance.model_User);
		if(requst == 0)
		{
			FinishRepairUnit();
		}
		else
		{
			UIHelper.BuyCashUI();
		}

	}
	void OnFinish()
	{
		OnRepairTimer();
	}
	public void OnClose()
	{
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RequestRepairTank);
		Delete();

	}



	//==============================================
	// 向服务器请求数据

	void RepairUnit() // 正常维修
	{
		UIHelper.LoadingPanelIsOpen(true);
		RepairUnitRequest request = new RepairUnitRequest ();
		request.api = new Model_ApiRequest ().api;


		if(_tankRepairItemDic != null)
		{
			foreach (KeyValuePair<int,TankRepairItem> kv in _tankRepairItemDic)	
			{
				TankRepairItem tankItem = kv.Value;
				Model_Unit modelUnit = tankItem.GetCurrentModelUnit();
				int tankNum = tankItem.GetCurrentRepairTankNum();
				if( tankNum != null && tankNum > 0)
				{
					SlgPB.Unit unit = new SlgPB.Unit ();
					unit.unitId = modelUnit.unitId; 
					unit.num = tankItem.GetCurrentRepairTankNum();
					request.units.Add (unit);
				}
			}
		}
		request.buyCd = 0;
		(new PBConnect_repairUnit ()).Send (request, OnRepairUnit);
		
		InstancePlayer.instance.model_User.model_Queue.AddUnitRepairQueue();


	}
	void OnRepairUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success){

			Trace.trace("OnRepairUnit success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnRepairUnit failure",Trace.CHANNEL.UI);
		}
	}

	void ImmediatelyRepairUnit() // 立即完成维修
	{
		UIHelper.LoadingPanelIsOpen(true);
		RepairUnitRequest request = new RepairUnitRequest ();
		request.api = new Model_ApiRequest ().api;
		
		
		if(_tankRepairItemDic != null)
		{
			foreach (KeyValuePair<int,TankRepairItem> kv in _tankRepairItemDic)	
			{
				TankRepairItem tankItem = kv.Value;
				Model_Unit modelUnit = tankItem.GetCurrentModelUnit();
				int tankNum = tankItem.GetCurrentRepairTankNum();
				if( tankNum != null && tankNum > 0)
				{
					SlgPB.Unit unit = new SlgPB.Unit ();
					unit.unitId = modelUnit.unitId; 
					unit.num = tankItem.GetCurrentRepairTankNum();
					request.units.Add (unit);
				}
			}
		}
		request.buyCd = 1;
		(new PBConnect_repairUnit ()).Send (request, OnImmediatelyRepairUnit);
	}
	void OnImmediatelyRepairUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success){
//			InitUI();
			Trace.trace("OnImmediatelyRepairUnit success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnImmediatelyRepairUnit failure",Trace.CHANNEL.UI);
		}
	}
	
	void FinishRepairUnit() //免CD修理Unit
	{
		UIHelper.LoadingPanelIsOpen(true);
		RepairUnitRequest request = new RepairUnitRequest ();
		request.api = new Model_ApiRequest ().api;
		
		request.buyCd = 1;
		(new PBConnect_finishRepairUnit ()).Send (request, OnFinishRepairUnit);
	}
	void OnFinishRepairUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {		
			Trace.trace("OnFinishRepairUnit success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnFinishRepairUnit failure",Trace.CHANNEL.UI);
		}
	}
	//完成收获
	void OnRepairTimer()
	{
		SlgPB.RepairUnitRequest request = new SlgPB.RepairUnitRequest ();	
		request.api = new Model_ApiRequest ().api;
		request.buyCd = 0;
		(new PBConnect_finishRepairUnit ()).Send (request,OnFinishRepair);
	}

	void OnFinishRepair(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) 
		{
			InstancePlayer.instance.model_User.model_Queue.RemoveUnitRepairQueue();
//			Delete();
//			//Trace.trace("OnRequestFinishUpgradeBuilding success", Trace.CHANNEL.INTEGRATION);
		} 
		else
		{
			//Trace.trace("OnRequestFinishUpgradeBuilding failed", Trace.CHANNEL.INTEGRATION);
		}
	}

	void Awake()
	{
		_RepairFactory = transform.Find("RepairFactoryContainer");
		_TankListContainer = transform.Find("RepairFactoryContainer/TankListContainer").gameObject;
		_repairScroll = _RepairFactory.Find("TankListContainer/Scroll View").GetComponent<UIScrollView>();
		_repairGrid = _repairScroll.gameObject.transform.Find("TankListGrid").GetComponent<UIGrid>();
//		_wrapContent = _repairGrid.gameObject.GetComponent<UIWrapContent>();
		_closeBtn = _RepairFactory.Find("BtnClose").GetComponent<UIButton>();
		_TankDamage_ValueLabel = _RepairFactory.Find("TankDamageLabel/TankDamage_ValueLabel").GetComponent<UILabel>();

		_TankDamageIngLabel = _RepairFactory.Find("TankDamageIngLabel").GetComponent<UILabel>();
		_TankDamageIng_ValueLabel = _RepairFactory.Find("TankDamageIngLabel/TankDamageIng_ValueLabel").GetComponent<UILabel>();//正在维修
		_resSprite_1 = _RepairFactory.Find("ResourceContainer/FoodLabel").GetComponent<UISprite>();
		_resSprite_2 = _RepairFactory.Find("ResourceContainer/OilLabel").GetComponent<UISprite>();
		_resSprite_3 = _RepairFactory.Find("ResourceContainer/MetalLabel").GetComponent<UISprite>();
		_resSprite_4 = _RepairFactory.Find("ResourceContainer/RareLabel").GetComponent<UISprite>();
		if(_resSpriteList != null)
		{
			_resSpriteList.Clear();
			_resSpriteList.Add(_resSprite_1);
			_resSpriteList.Add(_resSprite_2);
			_resSpriteList.Add(_resSprite_3);
			_resSpriteList.Add(_resSprite_4);
		}
		_resLabel_1 = _RepairFactory.Find("ResourceContainer/FoodLabel/FoodValueLabel").GetComponent<UILabel>();
		_resLabel_2 = _RepairFactory.Find("ResourceContainer/OilLabel/OilValueLabel").GetComponent<UILabel>();
		_resLabel_3 = _RepairFactory.Find("ResourceContainer/MetalLabel/MetalValueLabel").GetComponent<UILabel>();
		_resLabel_4 = _RepairFactory.Find("ResourceContainer/RareLabel/RareValueLabel").GetComponent<UILabel>();
		if(_resLabelList != null)
		{
			_resLabelList.Clear();
			_resLabelList.Add(_resLabel_1);
			_resLabelList.Add(_resLabel_2);
			_resLabelList.Add(_resLabel_3);
			_resLabelList.Add(_resLabel_4);
		}
		
		_immediatelyRepair_btn = _RepairFactory.Find("immediatelyRepair_btn").GetComponent<UIButton>();
		_allChoose_Btn = _RepairFactory.Find("allChoose_Btn").GetComponent<UIButton>();
		_Repair_btn = _RepairFactory.Find("Repair_btn").GetComponent<UIButton>();
		_speed_Container = _RepairFactory.Find("speed_Container");
		_Timer_Colored_Slider = _speed_Container.Find("Timer_Colored_Slider").GetComponent<UISlider>();
		_timerLabel = _speed_Container.Find("Timer_Colored_Slider/Label").GetComponent<UILabel>();
		_speedBtn = _speed_Container.Find("speedBtn").GetComponent<UIButton>();
		
		_immediatelyRepair_CoinValue = _immediatelyRepair_btn.gameObject.transform.Find("immediatelyRepair_CoinValue").GetComponent<UILabel>();
		_RepairTime_Value = _Repair_btn.gameObject.transform.Find("RepairTime_Value").GetComponent<UILabel>();
		_speedCoinLabel = _speedBtn.gameObject.transform.Find("speedCoinLabel").GetComponent<UILabel>();
		_finish_Btn = _RepairFactory.Find("finish_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_finish_Btn,OnFinish);
		EventDelegate evClose = new EventDelegate (OnClose);
		_closeBtn.onClick.Add (evClose);
		EventDelegate evimmediatelyRepair = new EventDelegate (OnImmediatelyRepair);
		_immediatelyRepair_btn.onClick.Add (evimmediatelyRepair);
		EventDelegate evallChoose = new EventDelegate (OnAllChoose);
		_allChoose_Btn.onClick.Add (evallChoose);
		EventDelegate evRepair = new EventDelegate (OnRepair);
		_Repair_btn.onClick.Add (evRepair);
		EventDelegate evspeed = new EventDelegate (OnSpeed);
		_speedBtn.onClick.Add (evspeed);
		
	}
}
