using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 建筑管理
/// </summary>
public class BuildingManager : MonoBehaviour {

	private GameObject _camera;//相机


//	private UIDraggableCamera _uiDraggableCamera;
//	private UIDragCamera _uiDragCamera;


	private GameObject _mainBg; //
	
	//		指挥中心管理类
	private BuildingFunctionItem _controlCenterManager;
	//		军工厂管理类
	private BuildingFunctionItem _militaryFactoryManager;
	//		维修厂管理类
	private BuildingFunctionItem _repairShopsManager;
	//		粮田管理类
	private BuildingFunctionItem _grainfieldManager;
	//		油井管理类
	private BuildingFunctionItem _oilWellManager;
	//		矿井管理类
	private BuildingFunctionItem _mineManager;
	//		稀土矿管理类
	private BuildingFunctionItem _rareEarthManager;
	
	//		指挥中心
	private GameObject _controlCenterItem;
	//		军工厂
	private GameObject _militaryFactoryItem;
	//		维修厂
	private GameObject _repairShopsItem;
	//		粮田
	private GameObject _grainfieldItem;
	//		油井
	private GameObject _oilWellItem;
	//		矿井
	private GameObject _mineItem;
	//		稀土矿
	private GameObject _rareEarthItem;
 	
	List<BuildingFunctionItem> _buildItemManagerList = new List<BuildingFunctionItem>();
	private List<Collider> _colliderList = new List<Collider>();// 所有  BoxCollider 

	//data
	Model_User _model_User;
	Dictionary<Model_Building.Building_Type, Model_Building> _buildings;

	void Start () {
		InitBuilding();
		NotificationCenter.instance.AddEventListener(Notification_Type.RefreshFunctionBtnItem,RefreshFunctionBtnItem);
		UpdateBuilding();
	}
	 
	void Update () {
		
	}

	void UpdateBuilding()
	{
		_buildings = InstancePlayer.instance.model_User.buildings;
		// 更新建筑物状态
		foreach (Model_Building building in _buildings.Values) 
		{
			switch(building.buildingType)
			{
			case Model_Building.Building_Type.ControlCenter:
				_buildItemManagerList[0].UpdateBuildingStatus(building);
				break;
			case Model_Building.Building_Type.WeaponsFactory:
				_buildItemManagerList[1].UpdateBuildingStatus(building);
				break;
			case Model_Building.Building_Type.ScienceCenter:
				_buildItemManagerList[2].UpdateBuildingStatus(building);
				break;
			case Model_Building.Building_Type.FoodFactory:
				_buildItemManagerList[3].UpdateBuildingStatus(building);
				break;
			case Model_Building.Building_Type.OilFactory:
				_buildItemManagerList[4].UpdateBuildingStatus(building);
				break;
			case Model_Building.Building_Type.MetalFactory:
				_buildItemManagerList[5].UpdateBuildingStatus(building);
				break;
			case Model_Building.Building_Type.RareFactory:
				_buildItemManagerList[6].UpdateBuildingStatus(building);
				break;
			}
		}
	}
	//通知刷新功能按钮状态
	public void RefreshFunctionBtnItem(Notification notification)
	{
		Model_Building _model_Building = notification._data as Model_Building;
		for(int i = 0 ; i<_buildItemManagerList.Count ; i++)
		{
			if(_buildItemManagerList[i].GetModelBuilding().buildingType != _model_Building.buildingType)
			{
				_buildItemManagerList[i].AllChildHideOrShow(false);
			}
		}
		
		
	}
	//点击屏幕关闭功能按钮
	void OnClick(GameObject click)
	{
		for(int i = 0 ; i<_buildItemManagerList.Count ; i++)
		{
			_buildItemManagerList[i].AllChildHideOrShow(false);
		}
		
	}
	
	public void  AddBtnListener(){
		if (_colliderList.Count != 0) {
			_colliderList.Clear();
		}
		
		Collider[] triggers = this.transform.parent.GetComponentsInChildren<Collider>(true);
		int count = triggers.Length;
		for (int i = 0; i < count; ++i)
		{
			Collider trigger = triggers[i];
			UIEventListener listener = UIEventListener.Get(trigger.gameObject);
			listener.onClick = OnClick;
			_colliderList.Add(trigger);
		}
	}


	void InitBuilding()
	{
		if(_buildItemManagerList == null)_buildItemManagerList = new List<BuildingFunctionItem>();
		_buildItemManagerList.Add(_controlCenterManager);
		_buildItemManagerList.Add(_militaryFactoryManager);
		_buildItemManagerList.Add(_repairShopsManager);
		_buildItemManagerList.Add(_grainfieldManager);
		_buildItemManagerList.Add(_oilWellManager);
		_buildItemManagerList.Add(_mineManager);
		_buildItemManagerList.Add(_rareEarthManager);

	}

	void OnDestroy() {

		NotificationCenter.instance.RemoveEventListener (Notification_Type.RefreshFunctionBtnItem);
	}
	
	void Awake()
	{
		_mainBg = transform.Find("mian_Bg_sp").gameObject;
		_camera = GameObject.FindWithTag("UICamera");
		if(_camera != null)
		{
//			_camera.AddComponent<UIDraggableCamera>();
//			_uiDraggableCamera = _camera.GetComponent<UIDraggableCamera>();
//			_uiDraggableCamera.rootForBounds = this.transform;
//			_uiDraggableCamera.dragEffect = UIDragObject.DragEffect.Momentum;
//			_uiDraggableCamera.scrollWheelFactor = 100.0f;
//			_uiDraggableCamera.momentumAmount = 100.0f;
//			
//			_uiDragCamera = _mainBg.GetComponent<UIDragCamera>();
//			_uiDragCamera.draggableCamera = _uiDraggableCamera;
		}
		_controlCenterManager = transform.Find("BuildingModel_1").GetComponent<BuildingFunctionItem>();
		_controlCenterItem = transform.Find("BuildingModel_1/Btn_product_Item").gameObject;

		_militaryFactoryManager = transform.Find("BuildingModel_2").GetComponent<BuildingFunctionItem>();
		_militaryFactoryItem = transform.Find("BuildingModel_2/Btn_product_Item").gameObject;

		_repairShopsManager = transform.Find("BuildingModel_3").GetComponent<BuildingFunctionItem>();
		_repairShopsItem = transform.Find("BuildingModel_3/Btn_product_Item").gameObject;

		_grainfieldManager = transform.Find("BuildingModel_4").GetComponent<BuildingFunctionItem>();
		_grainfieldItem = transform.Find("BuildingModel_4/Btn_product_Item").gameObject;

		_oilWellManager = transform.Find("BuildingModel_5").GetComponent<BuildingFunctionItem>();
		_oilWellItem = transform.Find("BuildingModel_5/Btn_product_Item").gameObject;

		_mineManager = transform.Find("BuildingModel_6").GetComponent<BuildingFunctionItem>();
		_mineItem = transform.Find("BuildingModel_6/Btn_product_Item").gameObject;

		_rareEarthManager = transform.Find("BuildingModel_7").GetComponent<BuildingFunctionItem>();
		_rareEarthItem = transform.Find("BuildingModel_7/Btn_product_Item").gameObject;
		AddDragCamera();
		AddBtnListener();
	}

	void AddDragCamera()
	{
//		UIDragCamera controlCamera = _controlCenterItem.GetComponent<UIDragCamera>();
//		controlCamera.draggableCamera = _uiDraggableCamera;
//
//		UIDragCamera militaryCamera = _militaryFactoryItem.GetComponent<UIDragCamera>();
//		militaryCamera.draggableCamera = _uiDraggableCamera;
//
//		UIDragCamera repairCamera = _repairShopsItem.GetComponent<UIDragCamera>();
//		repairCamera.draggableCamera = _uiDraggableCamera;
//
//		UIDragCamera grainCamera = _grainfieldItem.GetComponent<UIDragCamera>();
//		grainCamera.draggableCamera = _uiDraggableCamera;
//
//		UIDragCamera oilCamera = _oilWellItem.GetComponent<UIDragCamera>();
//		oilCamera.draggableCamera = _uiDraggableCamera;
//
//		UIDragCamera rareCamera = _rareEarthItem.GetComponent<UIDragCamera>();
//		rareCamera.draggableCamera = _uiDraggableCamera;
//
//		UIDragCamera mineCamera = _mineItem.GetComponent<UIDragCamera>();
//		mineCamera.draggableCamera = _uiDraggableCamera;

	}
}
