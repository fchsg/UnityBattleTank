using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//兵工厂

public class TankFactoryPanel : PanelBase {

	private UIButton _close ;

	private UIButton _MainTank;
	private UIButton _ArmoredVehicles;
	private UIButton _SelfPropellGuns;
	private UIButton _SpecialVehicles;

	private UISprite _Prompt_1;
	private UISprite _Prompt_2;
	private UISprite _Prompt_3;
	private UISprite _Prompt_4;

	private Transform  _RightTankInfoContainer;
	private UIGrid _MainTankGrid;
	private UIGrid _ArmoredVehiclesGrid;
	private UIGrid _SelfPropellGunsGrid;
	private UIGrid _SpecialVehiclesGrid;

	private UIScrollView _MainTankScroll;
	private UIScrollView _ArmoredVehiclesScroll;
	private UIScrollView _SelfPropellGunsScroll;
	private UIScrollView _SpecialVehiclesScroll;
	private UIToggle _MainToggle;
	private UIToggle _ArmoredToggle;
	private UIToggle _SelfPropellToggle;
	private UIToggle _SpecialToggle;
	//		0 主战坦克 GUN,
	//		1 装甲战车 CANNON,
	//		2 特殊战车 MISSILE,
	//		3 自行火炮 HOWITZER,
	//data
	List<TankDataManager.UnitData> _gunList = new List<TankDataManager.UnitData>();
	List<TankDataManager.UnitData> _cannonList = new List<TankDataManager.UnitData>();
	List<TankDataManager.UnitData> _missileList = new List<TankDataManager.UnitData>();
	List<TankDataManager.UnitData> _howitzerList = new List<TankDataManager.UnitData>();

	Dictionary<int,TankCurrentInfoItem> _gunDic = new Dictionary<int, TankCurrentInfoItem>();
	Dictionary<int,TankCurrentInfoItem> _cannonDic = new Dictionary<int, TankCurrentInfoItem>();
	Dictionary<int,TankCurrentInfoItem> _missileDic = new Dictionary<int, TankCurrentInfoItem>();
	Dictionary<int,TankCurrentInfoItem> _howitzerDic = new Dictionary<int, TankCurrentInfoItem>();

	List<UIButton> _unitTypeBtnList = new List<UIButton>();
	List<UIGrid> _unitGridList = new List<UIGrid>();
	List<UIWrapContent> _unitWrapList = new List<UIWrapContent>();
	List<UIScrollView> _unitScrollViewList = new List<UIScrollView>();
	List<UISprite> _promptList = new List<UISprite>();
	List<UIWrapContent> _wrapList = new List<UIWrapContent>();

	void Start(){

		NotificationCenter.instance.AddEventListener( Notification_Type.RefreshProductTank,RefreshProductTank);

	}

	public void RefreshProductTank(Notification notification)
	{
		TankDataManager tankDataManager = new TankDataManager();
		_gunList.Clear();
		_cannonList.Clear();
		_missileList.Clear();
		_howitzerList.Clear();
		_gunList =  tankDataManager.GetGunSortData();
		_cannonList =  tankDataManager.GetCannonSortData();
		_missileList =  tankDataManager.GetMissileSortData();
		_howitzerList =  tankDataManager.GetHowitzerSortData();
//		int dicCount = _gunDic.Count;
//		for(int i = 0;i < dicCount ;i++)
//		{
//			_cannonDic[i].UpdateData(_cannonList[i]);
//		}
		UpdataTankInfo(_gunList,_gunDic);
		UpdataTankInfo(_cannonList,_cannonDic);
		UpdataTankInfo(_missileList,_missileDic);
		UpdataTankInfo(_howitzerList,_howitzerDic);

	}
	void UpdataTankInfo(List<TankDataManager.UnitData> dataList,Dictionary<int,TankCurrentInfoItem> dataDic)
	{
		int i = 0;
		foreach(KeyValuePair<int,TankCurrentInfoItem>kvp in dataDic)
		{
			
			kvp.Value.UpdateData(dataList[i]);

//			Trace.trace(" _gunDic  " + kvp.Key + "  " + kvp.Value + " i  = "+ i ,Trace.CHANNEL.UI); 
			i++;
		}
		foreach(UIWrapContent wrap in _wrapList)
		{
			wrap.SortAlphabetically();
		}
	}
	void OnDestroy()
	{
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RefreshProductTank);
	}

	override public void Open(params System.Object[] parameters)
	{
		base.Open (parameters);
	}

	void Update()
	{
		UpdateScroll();  
	}

	void UpdateScroll()
	{
		for(int i = 0; i <_unitTypeBtnList.Count; i++)
		{
			if(_unitScrollViewList[i] != null)
			{
//				int gridCount = _unitGridList[i].GetChildList().Count;
				int gridCount = 0;
				switch(i)
				{
					case 0:
					gridCount = _gunList.Count;
						break;
					case 1:
					gridCount = _cannonList.Count;
						break;
					case 2:
					gridCount = _missileList.Count;
						break;
					case 3:
					gridCount = _howitzerList.Count;
						break;
				}
				if(gridCount <= 6)
				{
					_unitScrollViewList[i].enabled = false;
				}
				else if(gridCount == 0)
				{
					_unitTypeBtnList[i].isEnabled = false;
					_unitScrollViewList[i].enabled = false;
				}
				else
				{
					_unitTypeBtnList[i].isEnabled = true;
					_unitScrollViewList[i].enabled = true;
				}
			}
		}

	}

	override public void Init()
	{
		base.Init ();
		_gunList.Clear();
		_cannonList.Clear();
		_missileList.Clear();
		_howitzerList.Clear();

		_gunDic.Clear();
		_cannonDic.Clear();
		_missileDic.Clear();
		_howitzerDic.Clear();
	
		_wrapList.Clear();
		InitData ();

	}
	public void InitData()
	{

		TankDataManager tankDataManager = new TankDataManager();
		//		0 主战坦克 GUN,
		//		1 装甲战车 CANNON,
		//		2 特殊战车 MISSILE,
		//		3 自行火炮 HOWITZER,
		_gunList =  tankDataManager.GetGunSortData();
		_cannonList =  tankDataManager.GetCannonSortData();
		_missileList =  tankDataManager.GetMissileSortData();
		_howitzerList =  tankDataManager.GetHowitzerSortData();
		foreach(TankDataManager.UnitData unit in _gunList)
		{
//			Trace.trace("_gunList  " + unit.id + "  " + unit.quality + unit.unitData.name,Trace.CHANNEL.UI);
		}
		foreach(TankDataManager.UnitData unit in _cannonList)
		{
//			Trace.trace("_cannonList  " + unit.id + "  " + unit.quality + unit.unitData.name,Trace.CHANNEL.UI);
		}
		foreach(TankDataManager.UnitData unit in _missileList)
		{
//			Trace.trace("_missileList  " + unit.id + "  " + unit.quality + unit.unitData.name,Trace.CHANNEL.UI);
		}
		foreach(TankDataManager.UnitData unit in _howitzerList)
		{
//			Trace.trace("_howitzerList  " + unit.id + "  " + unit.quality + unit.unitData.name,Trace.CHANNEL.UI);
		}
	}
	
	IEnumerator CreateUnit(UIGrid grid, List<TankDataManager.UnitData> unitData,Dictionary<int,TankCurrentInfoItem> dataDic){
		yield return new WaitForSeconds (0.01f);	
		if (grid != null) {
			DelTankItem(grid);
		}

		int tankCount = unitData.Count;
		int itemCount = 0;
		if(tankCount % 2 == 0)
		{
			itemCount = tankCount / 2;
		}
		else
		{
			itemCount = tankCount / 2 + 1;
		}
		for(int i = 0; i < itemCount; i++)
		{
			if(grid.gameObject != null)
			{
				int index_ = 0;
				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "TankFactory/TwoTank_Container");
				GameObject item = NGUITools.AddChild(grid.gameObject,tankItem);
				item.name = "0" + i;
				TankCurrentInfoItem tankItem1 = item.transform.Find("TankCurrentInfoItem1").GetComponent<TankCurrentInfoItem>();
				tankItem1.Init(unitData[i*2]);
				dataDic.Add(unitData[i*2].id,tankItem1);
				TankCurrentInfoItem tankItem2 = item.transform.Find("TankCurrentInfoItem2").GetComponent<TankCurrentInfoItem>();
				
				index_ = i*2 + 1;
				if(index_ > (tankCount - 1) )
				{
					NGUITools.Destroy(tankItem2.gameObject);
				}
				else
				{
					tankItem2.Init(unitData[index_]);
					dataDic.Add(unitData[index_].id,tankItem2);
				}
				
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}

	void CreateWrapUnit(UIGrid grid,int num){
		if (grid != null) {
			grid.DestoryAllChildren();
		}
		for(int i = 0; i < num; i++)
		{
			if(grid.gameObject != null)
			{
				 
				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "TankFactory/TwoTank_Container");
				GameObject item = NGUITools.AddChild(grid.gameObject,tankItem);
				item.name = "0" + i;
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}


	void DelTankItem (UIGrid parent){
		if(parent != null)
		{
			parent.DestoryAllChildren();
		}
		 
	}

	void OnMainTankToggle()
	{
		bool check = UIToggle.current.value;
		if(check)
		{
			_gunDic.Clear();
			SetWrapContent(_MainTankGrid,_gunList,_gunDic,OnUpdateItemMain);
		}
	}

	void OnArmoredToggle()
	{
		bool check = UIToggle.current.value;
		if(check)
		{
			_cannonDic.Clear();
			SetWrapContent(_ArmoredVehiclesGrid,_cannonList,_cannonDic,OnUpdateItemArmoredVehicles);
		}
	}

	void OnSpecialToggle()
	{
		bool check = UIToggle.current.value;
		if(check)
		{
			_missileDic.Clear();
			SetWrapContent(_SpecialVehiclesGrid,_missileList,_missileDic,OnUpdateItemSpecialVehicles);
		}	 
	}

	void OnSelfPropellToggle()
	{
		bool check = UIToggle.current.value;
		if(check)
		{
			_howitzerDic.Clear();
			SetWrapContent(_SelfPropellGunsGrid,_howitzerList,_howitzerDic,OnUpdateItemSelfPropellGuns);
		}	 
	}

	void SetWrapContent(UIGrid grid,List<TankDataManager.UnitData> dataList,Dictionary<int,TankCurrentInfoItem> dataDic ,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		grid.enabled = true;
		int tankCont = dataList.Count;
		int gridCont = grid.GetChildList().Count;
		int itemCount = 0;
		if(tankCont % 2 == 0)
		{
			itemCount = tankCont / 2;
		}
		else
		{
			itemCount = tankCont / 2 + 1;
		}
		if(itemCount <= 3)
		{
			
			if(grid.gameObject.GetComponent<UIWrapContent>())
			{
				NGUITools.Destroy(grid.gameObject.GetComponent<UIWrapContent>());
			}
			if(gridCont != itemCount)
			{
				StartCoroutine(CreateUnit(grid,dataList,dataDic));
				
			}
		}
		else
		{
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
			_wrapList.Add(wrap);
			if(gridCont != 4)
			{
				if (grid != null) {
					grid.DestoryAllChildren();
				}
				CreateWrapUnit(grid,4);
				if(wrap != null)
				{
					//绑定方法
					wrap.itemSize = (int)grid.cellHeight;
					wrap.minIndex = -(itemCount - 1);
					wrap.maxIndex = 0;
					wrap.onInitializeItem = OnUpdateItemMain;
					wrap.enabled = true;
					
				}
			} 
		}
	}

	void OnUpdateItemMain(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_gunList,_gunDic);
	}
	void OnUpdateItemArmoredVehicles(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_cannonList,_cannonDic);
	}
	void OnUpdateItemSpecialVehicles(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_missileList,_missileDic);
	}
	void OnUpdateItemSelfPropellGuns(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_howitzerList,_howitzerDic);
	}

	void OnUpateItem(GameObject go, int index, int realIndex,List<TankDataManager.UnitData> dataList,Dictionary<int,TankCurrentInfoItem> dataDic)
	{
		int index_ = 0;
		int indexList = Mathf.Abs(realIndex);
		TankCurrentInfoItem tankItem1 = go.transform.Find("TankCurrentInfoItem1").GetComponent<TankCurrentInfoItem>();
		tankItem1.Init(dataList[indexList * 2]);
		if(!dataDic.ContainsKey(dataList[indexList * 2].id))
		{
			dataDic.Add(dataList[indexList * 2].id,tankItem1);
		}
		
		index_ = indexList * 2 + 1;
		int tankCount = dataList.Count;
		if(index_ > (tankCount - 1) )
		{
			TankCurrentInfoItem tankItem2 = go.transform.Find("TankCurrentInfoItem2").GetComponent<TankCurrentInfoItem>();
			tankItem2.gameObject.SetActive(false);
		}
		else
		{
			TankCurrentInfoItem tankItem2 = go.transform.Find("TankCurrentInfoItem2").GetComponent<TankCurrentInfoItem>();
			tankItem2.gameObject.SetActive(true);
			tankItem2.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].id))
			{
				dataDic.Add(dataList[index_].id,tankItem2);
			}
		}	 
	}

	public void OnClose()
	{
		if(this.gameObject != null)
		{
			Delete();
		}
	}
	
	void Awake()
	{
		_close = transform.Find("TankFactoryContainer/CloseBtn").GetComponent<UIButton>();
		EventDelegate evClose = new EventDelegate (OnClose);
		_close.onClick.Add (evClose);
		
		_MainTank = transform.Find("TankFactoryContainer/LeftBtnContainer/MainBattleTankBtn").GetComponent<UIButton>();
		_ArmoredVehicles = transform.Find("TankFactoryContainer/LeftBtnContainer/ArmoredVehiclesBtn").GetComponent<UIButton>();
		_SelfPropellGuns = transform.Find("TankFactoryContainer/LeftBtnContainer/SelfPropellGunBtn").GetComponent<UIButton>();
		_SpecialVehicles = transform.Find("TankFactoryContainer/LeftBtnContainer/SpecialVehiclesBtn").GetComponent<UIButton>();
		if(_unitTypeBtnList !=null)
		{
			_unitTypeBtnList.Clear();
			_unitTypeBtnList.Add(_MainTank);
			_unitTypeBtnList.Add(_ArmoredVehicles);
			_unitTypeBtnList.Add(_SelfPropellGuns);
			_unitTypeBtnList.Add(_SpecialVehicles);
		}
		_MainToggle = _MainTank.transform.GetComponent<UIToggle>();
		_ArmoredToggle = _ArmoredVehicles.transform.GetComponent<UIToggle>();
		_SelfPropellToggle = _SelfPropellGuns.transform.GetComponent<UIToggle>();
		_SpecialToggle = _SpecialVehicles.transform.GetComponent<UIToggle>();

		EventDelegate.Add(_MainToggle.onChange,OnMainTankToggle);
		EventDelegate.Add(_ArmoredToggle.onChange,OnArmoredToggle);
		EventDelegate.Add(_SpecialToggle.onChange,OnSpecialToggle);
		EventDelegate.Add(_SelfPropellToggle.onChange,OnSelfPropellToggle);

		_Prompt_1 = _MainTank.gameObject.transform.Find("Prompt_Sprite").GetComponent<UISprite>();
		_Prompt_2 = _ArmoredVehicles.gameObject.transform.Find("Prompt_Sprite").GetComponent<UISprite>();
		_Prompt_3 = _SelfPropellGuns.gameObject.transform.Find("Prompt_Sprite").GetComponent<UISprite>();
		_Prompt_4 = _SpecialVehicles.gameObject.transform.Find("Prompt_Sprite").GetComponent<UISprite>();
		if(_promptList != null)
		{
			_promptList.Clear();
			_promptList.Add(_Prompt_1);
			_promptList.Add(_Prompt_2);
			_promptList.Add(_Prompt_3);
			_promptList.Add(_Prompt_4);
		}
		for(int i = 0 ;i< _promptList.Count;i++)
		{
			_promptList[i].gameObject.SetActive(false);
		}
		_RightTankInfoContainer = transform.Find("TankFactoryContainer/RightTankInfoContainer");
		_MainTankGrid = _RightTankInfoContainer.Find("MainTankContainer/MainTankScroll View/MainTankGrid").GetComponent<UIGrid>();
		_ArmoredVehiclesGrid = _RightTankInfoContainer.Find("ArmoredVehiclesContainer/ArmoredVehiclesScroll View/ArmoredVehiclesGrid").GetComponent<UIGrid>();
		_SelfPropellGunsGrid = _RightTankInfoContainer.Find("SelfPropellGunsContainer/SelfPropellGunsScroll View/SelfPropellGunsGrid").GetComponent<UIGrid>();
		_SpecialVehiclesGrid = _RightTankInfoContainer.Find("SpecialVehiclesContainer/SpecialVehiclesScroll View/SpecialVehiclesGrid").GetComponent<UIGrid>();
		if(_unitGridList != null)
		{
			_unitGridList.Clear();
			_unitGridList.Add(_MainTankGrid);
			_unitGridList.Add(_ArmoredVehiclesGrid);
			_unitGridList.Add(_SelfPropellGunsGrid);
			_unitGridList.Add(_SpecialVehiclesGrid);
		}

		_MainTankScroll = _RightTankInfoContainer.Find("MainTankContainer/MainTankScroll View").GetComponent<UIScrollView>();
		_ArmoredVehiclesScroll = _RightTankInfoContainer.Find("ArmoredVehiclesContainer/ArmoredVehiclesScroll View").GetComponent<UIScrollView>();
		_SelfPropellGunsScroll = _RightTankInfoContainer.Find("SelfPropellGunsContainer/SelfPropellGunsScroll View").GetComponent<UIScrollView>();
		_SpecialVehiclesScroll = _RightTankInfoContainer.Find("SpecialVehiclesContainer/SpecialVehiclesScroll View").GetComponent<UIScrollView>();
		if(_unitScrollViewList != null)
		{
			_unitScrollViewList.Clear();
			_unitScrollViewList.Add(_MainTankScroll);
			_unitScrollViewList.Add(_ArmoredVehiclesScroll);
			_unitScrollViewList.Add(_SelfPropellGunsScroll);
			_unitScrollViewList.Add(_SpecialVehiclesScroll);
		}



	}


}
