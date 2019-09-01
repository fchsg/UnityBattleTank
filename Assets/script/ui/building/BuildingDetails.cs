using UnityEngine;
using System.Collections;
using SlgPB;
// 建筑详情

public class BuildingDetails : PanelBase 
{
	
// UI

	private UIButton _btnClose;	// 关闭按钮

	private Transform _buildingInfo;
	private UILabel _buildingName_left_Label;// 当前建筑名称
	private UILabel _currentLevel;	// 当前等级
	private UILabel _descriptionInfo_Label;	// 建筑描述信息

	private UISprite _buildingName;
	private UILabel _propertyInfo_1_Label;	// 功能文字描述
	private UILabel _propertyValue_1_Label;	// 功能文字描述值
	private UILabel _propertyInfo_2_Label;
	private UILabel _propertyValue_2_Label;
	
// data	
	Model_Building _model_Building;
	DataBuilding _data_Building;
	DataProduct _data_Product;
	


	override public void Init()
	{
		base.Init ();
	}


	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		_model_Building = parameters [0] as Model_Building;
		_data_Building = DataManager.instance.dataBuildingGroup.GetBuilding (_model_Building.id, _model_Building.level);

		InitUI();
		UpdateUI();

	}

	void Update()
	{

	}


	public void UpdateUI()
	{
		_data_Building = DataManager.instance.dataBuildingGroup.GetBuilding (_model_Building.id, _model_Building.level);

		//当前等级 
		_currentLevel.text = "" + _model_Building.level;
		// 当前建筑名称
		_buildingName_left_Label.text = _data_Building.name;
		_descriptionInfo_Label.text = _data_Building.description;

		DataProduct dataProduct = new DataProduct();

		if(_model_Building.buildingType == Model_Building.Building_Type.FoodFactory )
		{
			dataProduct = DataManager.instance.dataProductFoodGroup.GetProduct(_model_Building.level);
			ShowSpeedNum(dataProduct);
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.OilFactory )
		{
			dataProduct = DataManager.instance.dataProductOilGroup.GetProduct(_model_Building.level);
			ShowSpeedNum(dataProduct);
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.MetalFactory )
		{
			dataProduct = DataManager.instance.dataProductMetalGroup.GetProduct(_model_Building.level);
			ShowSpeedNum(dataProduct);
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.RareFactory )
		{
			dataProduct = DataManager.instance.dataProductRareGroup.GetProduct(_model_Building.level);
			ShowSpeedNum(dataProduct);
		}

	}

	public void ShowSpeedNum(DataProduct dataProduct)
	{

		if(dataProduct != null)
		{
			_propertyInfo_1_Label.gameObject.SetActive(true);
			_propertyValue_1_Label.gameObject.SetActive(true);
			_propertyInfo_2_Label.gameObject.SetActive(true);
			_propertyValue_2_Label.gameObject.SetActive(true);
			_propertyValue_1_Label.text = dataProduct.produceSpeed * 3600 + "";
			_propertyValue_2_Label.text = dataProduct.capacity + "";
		}
	}

	public void InitUI()
	{
		_buildingInfo = transform.Find("building_bg");
		if (_buildingInfo != null) 
		{
			_buildingName_left_Label = _buildingInfo.Find("buildingName_left_Label").GetComponent<UILabel>();
			_currentLevel = _buildingInfo.Find("currentLevel").GetComponent<UILabel>();
			_descriptionInfo_Label = _buildingInfo.Find("descriptionInfo_Label").GetComponent<UILabel>();
			
			_propertyInfo_1_Label = _buildingInfo.Find("propertyInfo_1_Label").GetComponent<UILabel>();
			_propertyValue_1_Label = _buildingInfo.Find("propertyValue_1_Label").GetComponent<UILabel>();
			_propertyInfo_2_Label = _buildingInfo.Find("propertyInfo_2_Label").GetComponent<UILabel>();
			_propertyValue_2_Label = _buildingInfo.Find("propertyValue_2_Label").GetComponent<UILabel>();
			_buildingName = _buildingInfo.Find("building_icon").GetComponent<UISprite>();
			
		}
		_btnClose = transform.Find("building_bg/BtnClose").GetComponent<UIButton>();
		
		if (_btnClose != null) 
		{
			EventDelegate evenBtnClose = new EventDelegate(OnDestoryPanel);
			_btnClose.onClick.Add(evenBtnClose);
		}
		_propertyInfo_1_Label.gameObject.SetActive(false);
		_propertyValue_1_Label.gameObject.SetActive(false);
		_propertyInfo_2_Label.gameObject.SetActive(false);
		_propertyValue_2_Label.gameObject.SetActive(false);
	}


	public void OnDestoryPanel()
	{
		Delete ();
	}

	
}
