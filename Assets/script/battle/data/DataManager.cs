using UnityEngine;
using System.Collections;

public class DataManager {

	private static DataManager _instance = null;
	public static DataManager instance
	{
		get
		{
			if (_instance == null) {
				_instance = new DataManager();
			}
			return _instance;
		}
	}

	private bool _isLoaded = false;

	public void InitData()
	{
		if (!_isLoaded) 
		{
			dataInitialConfigGroup.Load (AppConfig.FOLDER_DATACONFIG + "InitialConfig.json");
			dataUnitsGroup.Load(AppConfig.FOLDER_DATACONFIG + "Unit.json");
			dataMissionGroup.Load(AppConfig.FOLDER_DATACONFIG + "Mission.json");
			dataUnitPartGroup.Load(AppConfig.FOLDER_DATACONFIG + "UnitPart.json");
			dataProductCashGroup.Load (AppConfig.FOLDER_DATACONFIG + "CashProduct.json");
			dataProductFoodGroup.Load (AppConfig.FOLDER_DATACONFIG + "FoodProduct.json");
			dataProductMetalGroup.Load (AppConfig.FOLDER_DATACONFIG + "MetalProduct.json");
			dataProductOilGroup.Load (AppConfig.FOLDER_DATACONFIG + "OilProduct.json");
			dataProductRareGroup.Load (AppConfig.FOLDER_DATACONFIG + "RareProduct.json");
			dataBuildingGroup.Load(AppConfig.FOLDER_DATACONFIG + "Building.json");
			dataTeamGroup.Load(AppConfig.FOLDER_DATACONFIG + "Team.json");
			dataSkillGroup.Load(AppConfig.FOLDER_DATACONFIG + "Skill.json");
			dataDiscountGroup.Load(AppConfig.FOLDER_DATACONFIG + "Discount.json");
			_dataBattleGroup.Load(AppConfig.FOLDER_DATACONFIG + "Battle.json");
			_dataErrorCodeGroup.Load(AppConfig.FOLDER_DATACONFIG + "ErrorCode.json");
			_dataItemGroup.Load(AppConfig.FOLDER_DATACONFIG + "Item.json");

			_dataHeroGroup.LoadHeros(AppConfig.FOLDER_DATACONFIG + "Hero.json");
			_dataHeroGroup.LoadHerosExp(AppConfig.FOLDER_DATACONFIG + "HeroExp.json");
			_dataHeroGroup.LoadHerosLeadership(AppConfig.FOLDER_DATACONFIG + "HeroLeadership.json");
			_dataHeroGroup.LoadHerosUpgrade(AppConfig.FOLDER_DATACONFIG + "HeroUpgrade.json");

			_dataLeaderGroup.Load(AppConfig.FOLDER_DATACONFIG + "Leader.json");
			_dataDropGroup.Load(AppConfig.FOLDER_DATACONFIG + "DropGroup.json");
			_dataLadderGroup.Load(AppConfig.FOLDER_DATACONFIG + "Ladder.json");

			_dataTaskGroup.Load(AppConfig.FOLDER_DATACONFIG + "Task.json");
			_dataStarRateGroup.Load(AppConfig.FOLDER_DATACONFIG + "StarRate.json");

			_isLoaded = true;
		}
	}

	private DataUnitsGroup _dataUnitsGroup = new DataUnitsGroup ();
	public DataUnitsGroup dataUnitsGroup
	{
		get { return _dataUnitsGroup; }
	}

	private DataMissionGroup _dataMissionGroup = new DataMissionGroup();
	public DataMissionGroup dataMissionGroup
	{
		get { return _dataMissionGroup; }
	}

	private DataUnitPartGroup _dataUnitPartGroup = new DataUnitPartGroup();
	public DataUnitPartGroup dataUnitPartGroup
	{
		get { return _dataUnitPartGroup; }
	}

	private DataProductGroup _dataProductCashGroup = new DataProductGroup();
	public DataProductGroup dataProductCashGroup 
	{
		get { return _dataProductCashGroup; }
	}

	private DataProductGroup _dataProductFoodGroup = new DataProductGroup();
	public DataProductGroup dataProductFoodGroup 
	{
		get { return _dataProductFoodGroup; }
	}
	
	private DataProductGroup _dataProductMetalGroup = new DataProductGroup();
	public DataProductGroup dataProductMetalGroup 
	{
		get { return _dataProductMetalGroup; }
	}
	
	private DataProductGroup _dataProductOilGroup = new DataProductGroup();
	public DataProductGroup dataProductOilGroup 
	{
		get { return _dataProductOilGroup; }
	}
	
	private DataProductGroup _dataProductRareGroup = new DataProductGroup();
	public DataProductGroup dataProductRareGroup 
	{
		get { return _dataProductRareGroup; }
	}
	

	private DataBuildingGroup _dataBuildingGroup = new DataBuildingGroup ();
	public DataBuildingGroup dataBuildingGroup
	{
		get { return _dataBuildingGroup; }
	}

	private DataMapGroup _dataMapGroup = new DataMapGroup();
	public DataMapGroup dataMapGroup
	{
		get { return _dataMapGroup; }
	}

	private DataTeamGroup _dataTeamGroup = new DataTeamGroup();
	public DataTeamGroup dataTeamGroup
	{
		get { return _dataTeamGroup; }
	}

	private DataSkillGroup _dataSkillGroup = new DataSkillGroup();
	public DataSkillGroup dataSkillGroup
	{
		get { return _dataSkillGroup; }
	}

	private DataInitialConfigGroup _dataInitialConfigGroup = new DataInitialConfigGroup();
	public DataInitialConfigGroup dataInitialConfigGroup
	{
		get { return _dataInitialConfigGroup; }
	}

	private DataDiscountGroup _dataDiscountGroup = new DataDiscountGroup ();
	public DataDiscountGroup dataDiscountGroup
	{
		get { return _dataDiscountGroup; }
	}

	private DataBattleGroup _dataBattleGroup = new DataBattleGroup ();
	public DataBattleGroup dataBattleGroup
	{
		get { return _dataBattleGroup; }
	}

	private DataErrorCodeGroup _dataErrorCodeGroup = new DataErrorCodeGroup ();
	public DataErrorCodeGroup dataErrorCodeGroup
	{
		get { return _dataErrorCodeGroup; }
	}

	private DataItemGroup _dataItemGroup = new DataItemGroup ();
	public DataItemGroup dataItemGroup
	{
		get { return _dataItemGroup; }
	}
	
	private DataHeroGroup _dataHeroGroup = new DataHeroGroup ();
	public DataHeroGroup dataHeroGroup
	{
		get { return _dataHeroGroup; }
	}
	
	private DataLeaderGroup _dataLeaderGroup = new DataLeaderGroup ();
	public DataLeaderGroup dataLeaderGroup
	{
		get { return _dataLeaderGroup; }
	}

	private DataDropGroup _dataDropGroup = new DataDropGroup ();
	public DataDropGroup dataDropGroup
	{
		get { return _dataDropGroup; }
	}


	private DataLadderGroup _dataLadderGroup = new DataLadderGroup();
	public DataLadderGroup dataLadderGroup
	{
		get { return _dataLadderGroup; }
	}


	private DataTaskGroup _dataTaskGroup = new DataTaskGroup();
	public DataTaskGroup dataTaskGroup
	{
		get { return _dataTaskGroup; }
	}


	private DataStarRateGroup _dataStarRateGroup = new DataStarRateGroup();
	public DataStarRateGroup dataStarRateGroup
	{
		get { return _dataStarRateGroup; }
	}

	private DataGuideGroup _dataGuideGroup = new DataGuideGroup();
	public DataGuideGroup dataGuideGroup
	{
		get { return _dataGuideGroup; }
	}

}
