using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
// 主城建筑按钮

public class BuildingFunctionItem : MonoBehaviour {

	bool isOpen = false;//记录当前菜单状态
	bool _canplay = false; 
	int _playNum = 0;
	private int chlidCount = 3;//按钮数
	private float anglecheap = -25;//每个方块间的角度偏移  
	private float firstangle = 90;//记录第一个角度，用以左右对称  
	private float r = 80;//椭圆的两个弦长  
	private float R = 240;  
	private UIPlayTween pt;
	
	private UIButton button;//主按钮
	
	private UIButton _btnIcon;
	private GameObject parent_;
	private GameObject timerSlider;
	private GameObject productIcon;
	private UILabel _sliderLabel;
	private UISlider _slider;
	private int totalTimer_;
	private string timerName_;

	private UILabel _buingName;
	List<GameObject> childList = new List<GameObject>();//要显示的按钮列表
	private List<Collider> _colliderList = new List<Collider>();// 所有 Button BoxCollider 

// Data

	Model_Building _model_Building;
	DataBuilding _data_Building;
	MainCityEffectPanel _effectPanel = null;
	void Awake()
	{
		parent_ = transform.Find("Btn_product_Item").gameObject;
		productIcon = transform.Find("Btn_product_Item/Btn_info").gameObject;
		button = transform.Find ("Btn_product_Item").GetComponent<UIButton> ();
		if (button != null) {
			EventDelegate btnEven = new EventDelegate (OnMainButtonClick);
			button.onClick.Add (btnEven);

			pt = transform.Find ("Btn_product_Item").GetComponent<UIPlayTween>();

			_btnIcon = button.transform.Find("Btn_info").GetComponentInChildren<UIButton>();
			EventDelegate evicon = new EventDelegate(OnClickIcon);
			_btnIcon.onClick.Add(evicon);


			_slider = transform.Find("Btn_product_Item/Timer_Colored_Slider").GetComponent<UISlider>();
			
			timerSlider = _slider.gameObject;
			_sliderLabel = transform.Find("Btn_product_Item/Timer_Colored_Slider/Label").GetComponent<UILabel>();
			_slider.value = 1f ;
			_sliderLabel.text =  "00:00";	
			
			_buingName = transform.Find("Btn_product_Item/ProName").GetComponent<UILabel>();
			productIcon.SetActive(false);


		}
	}

	void Start()
	{
		_effectPanel = GameObject.Find("main_Panel").transform.Find("effect_Panel").GetComponent<MainCityEffectPanel>();
	}
	
	// 更新建筑物状态
	public void UpdateBuildingStatus(Model_Building building)
	{
		_model_Building = building;
	}

	// Update is called once per frame
	void Update () {
		if(_model_Building != null)
		{
			_model_Building = InstancePlayer.instance.model_User.buildings[_model_Building.buildingType];
		}

		UpdateUI();
	}

	void UpdateUI()
	{

		if(_model_Building != null)
		{
			if(_model_Building.id == 0 )
			{
				_data_Building = DataManager.instance.dataBuildingGroup.GetBuilding((int)_model_Building.buildingType,1);
			}
			else
			{
				_data_Building = DataManager.instance.dataBuildingGroup.GetBuilding(_model_Building.id,_model_Building.level);
			}
			if(_model_Building.buildingLevelUpTime == 0)
			{
				timerSlider.SetActive(false);
			}
			else
			{
				timerSlider.SetActive(true);
				_sliderLabel.text = UIHelper.setTimeDHMS(_model_Building.buildingLevelUpTime);
				_slider.value = (float)_model_Building.buildingLevelUpTime / _data_Building.cost.costTime ;
				if(_model_Building.buildingLevelUpTime == 1)
				{
					timerSlider.SetActive(false);
					_canplay = true;
					if(_canplay && _playNum <1)
					{
						PlayFarmAni();
					}
				}
			}
		}
		
		if(_model_Building != null && _model_Building.model_Production != null)
		{
			if(_model_Building.model_Production.num >= 100)
			{
				productIcon.SetActive(true);
			}
			else
			{
				productIcon.SetActive(false);
			}
		}
		
		if(_buingName != null && _data_Building != null)
		{
			if(_model_Building.id == 0)
			{
				_buingName.text = _data_Building.name + "(1)";
			}
			else
			{
				_buingName.text = _data_Building.name + "(" +  _model_Building.level + ")";
			}
			
		}

		if (_model_Building != null && _model_Building.isUnlocked) 
		{
			button.isEnabled = true;
		}
		else 
		{
			button.isEnabled = false;
			return;
		}
	}
	public void OnClick(GameObject click) {
		AllChildHideOrShow(false);
		if (click.name.Equals ("Btn_Func_Itme_0")) 
		{
			//建筑升级界面 
			UIController.instance.CreatePanel(UICommon.UI_PANEL_BUILDINGDETAILS, _model_Building);

		} 
		else if (click.name.Equals ("Btn_Func_Itme_1"))
		{
			if(_model_Building.isUpgrading)
			{
				UIController.instance.CreatePanel(UICommon.UI_TIPS_SPEEDUPLEVEL,_model_Building);
			}
			else
			{
				UIController.instance.CreatePanel(UICommon.UI_PANEL_BUILDINGUPLEVEL,_model_Building);
			}

		}
		else if (click.name.Equals ("Btn_Func_Itme_2"))
		{
			if(_model_Building.buildingType == Model_Building.Building_Type.FoodFactory ||
			   _model_Building.buildingType == Model_Building.Building_Type.OilFactory ||
			   _model_Building.buildingType == Model_Building.Building_Type.MetalFactory ||
			   _model_Building.buildingType == Model_Building.Building_Type.RareFactory )
			{
				//收获按钮响应事件
				OnClickIcon();
			}
			else if(_model_Building.buildingType == Model_Building.Building_Type.ControlCenter) //指挥中心
			{

			}
			else if(_model_Building.buildingType == Model_Building.Building_Type.ScienceCenter) //维修厂 
			{
				UIController.instance.CreatePanel (UICommon.UI_PANEL_REPAIRFACTORY);
			}
			else if(_model_Building.buildingType == Model_Building.Building_Type.WeaponsFactory) //军工厂
			{
				//test  parameters
				int integer = 123;
				string str = "asdfasf";
				DataUnit dataUnit = new DataUnit ();
				dataUnit.asset = "tank111";
				int[] arr = new int[]{23,56,78,4,5}; 
				
				UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKFACTORY, integer, str, dataUnit, arr);
			}
		}
		else if (click.name.Equals ("Btn_Func_Itme_3"))
		{
			//收获按钮响应事件
			OnClickIcon();
		}
	}

	public void OnClickIcon(){
//		GameObject resType = null;
//		switch (GetModelBuilding().buildingType) 
//		{
//		case Model_Building.Building_Type.FoodFactory:
//			resType = GameObject.Find("AddFoodBtn");
//			break;
//		case Model_Building.Building_Type.OilFactory:
//			resType = GameObject.Find("AddOilBtn");
//			break;
//		case Model_Building.Building_Type.MetalFactory:
//			resType = GameObject.Find("AddMetalBtn");
//			break;
//		case Model_Building.Building_Type.RareFactory:
//			resType = GameObject.Find("AddRareBtn");
//			break;
//		}
//		CoinsManager.PlayCoinsEffect(button.gameObject,resType,_model_Building);
//		UIHelper.PlayCoinsEffect(button.gameObject,resType,_model_Building);
		if(productIcon.activeSelf)
		{
			productIcon.SetActive (false);
		}

		DrawProduction();
	}
	//正向反向移动响应事件
	public void OnMainButtonClick()
	{
		if(_model_Building.model_Production != null && _model_Building.model_Production.num >= 100)
		{
			OnClickIcon();
		}
		else
		{
			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshFunctionBtnItem,new Notification(_model_Building));
			UpdateItems();
			SetChildPos ();
			AddBtnListener ();
			if (!isOpen)
			{
				//正向移动
				AllChildHideOrShow(true);
			}
			else
			{
				//反向移动
				AllChildHideOrShow(false);
			}
			button.isEnabled = false;
			StartCoroutine(SetTrue());
		}
	}

	public Model_Building GetModelBuilding()
	{
		if(_model_Building != null)
		{
			return _model_Building;
		}
		return null;
	}


	public void PlayFarmAni()
	{
		Trace.trace("PlayFarmAni()" ,Trace.CHANNEL.UI);
		_playNum = 1;
		FrameAniManager.PlayFrameAni(this.gameObject);
		_canplay = false;
		StartCoroutine(CanAgain());

	}
	IEnumerator CanAgain()
	{
		yield return new WaitForSeconds(3.0f);
		_playNum = 0;
	}


	public GameObject  CreateFunctionBtn (string itemName,FunctionBtnItem btnItem)
	{
		if (parent_ != null) {
			GameObject buildingItem = (GameObject)Resources.Load (AppConfig.FOLDER_PROFAB_UI + "building/Btn_Func_Itme");
			GameObject item_ = NGUITools.AddChild (parent_, buildingItem);	
			UIButton btnitem = item_.transform.Find("Btn_Func_Itme").GetComponent<UIButton>();
			UILabel labelName = btnitem.transform.Find("product_Function_Label").GetComponent<UILabel>();
			UISprite icon = btnitem.transform.Find("icon").GetComponent<UISprite>();
			btnitem.name = itemName;
			labelName.text = btnItem.btnItemName;
			icon.spriteName = btnItem.btnItemIcon;
			
			return item_;
		}
		return null;
	}
	
	public void SetChildPos()
	{
		chlidCount = FunctionBtnItemManger.dict.Count;
		float currentFirstAngel = firstangle - (anglecheap * (chlidCount - 1) ) / 2;
		
		if (childList.Count != chlidCount) {
			//存好按钮列表
			for (int i = 0; i < chlidCount; i++) {		
				//取出椭圆的中心点  
				Vector3 center = button.transform.position;  
				float currentAngel = currentFirstAngel + anglecheap * i;
				int id = i + 1;
				string str = id + "";
				GameObject item_ = CreateFunctionBtn ("Btn_Func_Itme_" + i,FunctionBtnItemManger.GetItem(str));
				Vector3 v = getPosition (currentAngel, center);
				item_.transform.Find ("Btn_Func_Itme_" + i).GetComponent<TweenPosition> ().to = v;
				childList.Add (item_);
			}
		}
	}
	
	
	public void UpdateItems()
	{
		FunctionBtnItemManger.Init();
		FunctionBtnItemManger.RemoveAllItem();
		//更新最新的建筑数据
		_model_Building = InstancePlayer.instance.model_User.buildings[_model_Building.buildingType];
		
		FunctionBtnItemManger.AddItem(new FunctionBtnItem("1",FunctionBtnItem.BtnItem_Type.DetailsType,"详情","details"));
		if(_model_Building.isUpgrading)
		{
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("2",FunctionBtnItem.BtnItem_Type.UpgradeSpeedType,"加速","upgrade"));
			return;
		}
		else
		{
			
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("2",FunctionBtnItem.BtnItem_Type.UpgradeSpeedType,"升级","speedup"));
		}
		
		if(_model_Building.buildingType == Model_Building.Building_Type.FoodFactory )
		{
			//			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.SpeedResourcesType,"资源加速","Foodtospeedup"));
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.GainType,"收获","Food"));
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.OilFactory )
		{
			//			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.SpeedResourcesType,"资源加速","Acceleratedtheoil"));
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.GainType,"收获","Accel"));
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.MetalFactory )
		{
			//			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.SpeedResourcesType,"资源加速","Metalspeed"));
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.GainType,"收获","Metal"));
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.RareFactory )
		{
			//			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.SpeedResourcesType,"资源加速","Rarearthtospeedup"));
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.GainType,"收获","Rare"));
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.ScienceCenter)
		{
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.ManageProductionType,"维修","speedup"));
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.WeaponsFactory)
		{
			FunctionBtnItemManger.AddItem(new FunctionBtnItem("3",FunctionBtnItem.BtnItem_Type.ManageProductionType,"生产","speedup"));
		}
	}
	
	public void AllChildHideOrShow(bool isShow){
		isOpen = isShow;
		if (childList.Count != 0) {
			for(int i = 0;i < childList.Count ;i++){
				childList[i].SetActive(isShow);
				if(!isOpen)
				{
					NGUITools.Destroy(childList[i]);
				}
			}
		}
		if(!isOpen)
		{
			if(childList != null)
			{
				childList.Clear();
			}
		}
	}
	
	//获取当前角度的坐标  
	Vector3 getPosition(float _angle,Vector3 _centerposition)  
	{  
		float hudu = (_angle/180f) * Mathf.PI;  
		float cosx = Mathf.Cos(hudu);  
		float sinx = Mathf.Sin(hudu);  
		float x = _centerposition.x + R * cosx;  
		float y = _centerposition.y + r * sinx; 
		Vector3 point = new Vector3(x, y, 0);   
		return  point;  
	}  
	
	IEnumerator SetTrue()
	{
		//0.5秒后可再次点击
		yield return new WaitForSeconds(0.2f);
		button.isEnabled = true;
	}
	
	public void  AddBtnListener(){
		if (_colliderList.Count != 0) {
			_colliderList.Clear();
		}
		
		Collider[] triggers = this.GetComponentsInChildren<Collider>(true);
		int count = triggers.Length;
		for (int i = 0; i < count; ++i)
		{
			Collider trigger = triggers[i];
			if (trigger.gameObject.name.StartsWith("Btn") == true) // 以"Btn"开头命名的按钮才会触发OnClick
			{
				UIEventListener listener = UIEventListener.Get(trigger.gameObject);
				listener.onClick = OnClick;
				_colliderList.Add(trigger);
			}
		}
	}
	


//	================================================================
//	向服务器请求数据

	void DrawProduction()
	{
		UIHelper.LoadingPanelIsOpen(true);
		if (InstancePlayer.instance.model_User.buildings.Count > 0)
		{
			DrawProductionRequest request = new DrawProductionRequest ();
			
			Model_Building building	= InstancePlayer.instance.model_User.buildings[_model_Building.buildingType];
			if(building != null)
			{
				Model_Production production = building.model_Production;
				if(production != null)
				{
					request.resourceType = production.resourceType; //要收取的资源类型	
					request.api = new Model_ApiRequest ().api;			
					(new PBConnect_drawProduction ()).Send (request, OnDrawProduction);
					return;
				}
			}
		}

	}
	void OnDrawProduction(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			_effectPanel.OnPlay(_model_Building,this.gameObject.transform.localPosition);
			Trace.trace("OnDrawProduction OnDrawProduction OnDrawProduction",Trace.CHANNEL.UI);

		} else {
			Trace.trace("OnDrawProduction OnDrawProduction" ,Trace.CHANNEL.UI);
		}
	}


}
