using UnityEngine;
using System.Collections;

public class ConnectionValidateHelper {

	// 是否拥有足够金币免CD加速完成生产Unit
	public static int IsEnoughCashClearBuildUnitCD(Model_User model_user, int unitId)
	{
		int result = -1;

		Model_Unit model_Unit;
		model_user.unlockUnits.TryGetValue(unitId, out model_Unit);
		if (model_Unit != null && model_Unit.onProduce > 0) 
		{
			float needCash = model_user.model_InitialConfig.GetClearUnitCDCash (model_Unit.produceLeftTime);
			float userCash = model_user.model_Resource.cash;

			if(userCash >= needCash)
			{
				result = 0;
			}
		
		}

		return result;
	}

	// 是否拥有足够金币免CD加速完成修理Unit
	public static int IsEnoughCashClearRepairUnitCD(Model_User model_user)
	{
		int result = -1;
		
		Model_RepairUnit model_RepairUnit = model_user.model_RepairUnit;

		if (model_RepairUnit != null && model_RepairUnit.repairLeftTime > 0) 
		{
			float needCash = model_user.model_InitialConfig.GetClearUnitCDCash (model_RepairUnit.repairLeftTime);
			float userCash = model_user.model_Resource.cash;
			
			if(userCash >= needCash)
			{
				result = 0;
			}
		}
		
		return result;
	}

	// 是否拥有足够金币免CD加速完成建筑升级
	public static int IsEnoughCashClearBuildingCD(Model_User model_user, Model_Building.Building_Type buildType)
	{
		int result = -1;

		Model_Building model_Building;
		model_user.buildings.TryGetValue (buildType, out model_Building);
		if (model_Building != null && model_Building.isUpgrading) 
		{
			float needCash = model_user.model_InitialConfig.GetClearBuildingCDCash (model_Building.buildingLevelUpTime);
			float userCash = model_user.model_Resource.cash;
			
			if(userCash >= needCash)
			{
				result = 0;
			}
		}

		return result;
	}

	// 是否拥有足够金币使建筑立即升级
	public static int IsEnoughCashImmediateUpgradeBuinding(Model_User model_user, int buildingId)
	{
		int result = -1;
		
		Model_Building model_Building;
		Model_Building.Building_Type buildType = (Model_Building.Building_Type)buildingId;
		model_user.buildings.TryGetValue (buildType, out model_Building);
		if (model_Building != null) 
		{
			DataBuilding dataBuilding = DataManager.instance.dataBuildingGroup.GetBuilding(model_Building.id, model_Building.level);

			float needCash = dataBuilding.upgradeCash;
			float userCash = model_user.model_Resource.cash;
			
			if(userCash >= needCash)
			{
				result = 0;
			}
		}
		
		return result;
	}
 
	public enum CostCheck
	{
		OK,
		NEED_CASH,
		NEED_FOOD,
		NEED_OIL,
		NEED_METAL,
		NEED_RARE,
	}
	public static CostCheck IsEnoughCost(DataUnit.BasicCost cost)
	{
		int r;

		r = IsEnoughCashBuyResources (cost.costCash);
		if (r < 0) {
			return CostCheck.NEED_CASH;
		}

		r = IsEnoughFoodUse (cost.costFood);
		if (r < 0) {
			return CostCheck.NEED_FOOD;
		}
		
		r = IsEnoughOilUse (cost.costOil);
		if (r < 0) {
			return CostCheck.NEED_OIL;
		}

		r = IsEnoughMetalUse (cost.costMetal);
		if (r < 0) {
			return CostCheck.NEED_METAL;
		}
		
		r = IsEnoughRareUse (cost.costRare);
		if (r < 0) {
			return CostCheck.NEED_RARE;
		}

		return CostCheck.OK;
		
	}


	/// <summary>
	/// 是否拥有足够的金币供玩家使用
	/// </summary>
	/// <returns><c>true</c> if is enough cash buy resources the specified cashCount; otherwise, <c>false</c>.</returns>
	/// <param name="cashCount">Cash count.</param>  
	public static int IsEnoughCashBuyResources(int cashCount)
	{
		int result = 0;
		float userCash = InstancePlayer.instance.model_User.model_Resource.cash;
		
		if(userCash < cashCount)
		{
			result = (int)Mathf.Ceil(userCash - cashCount);
		}
		return result;
	}

	//粮食是否足够使用
	public static int IsEnoughFoodUse(int resNum)
	{
		int result = 0;
		Model_User model_user = InstancePlayer.instance.model_User;
		if(model_user != null)
		{	
			int havaResCount = model_user.model_Resource.GetIntFood();
			if(resNum > havaResCount)
			{
				result = havaResCount - resNum;
			}
		}
		return result;
	}

	//石油是否足够使用
	public static int IsEnoughOilUse(int resNum)
	{
		int result = 0;
		Model_User model_user = InstancePlayer.instance.model_User;
		if(model_user != null)
		{	
			int havaResCount = model_user.model_Resource.GetIntOil();
			if(resNum > havaResCount)
			{
				result = havaResCount - resNum;
			}
		}
		return result;
	}

	//  铁矿是否足够使用
	public static int IsEnoughMetalUse(int resNum)
	{
		int result = 0;
		Model_User model_user = InstancePlayer.instance.model_User;
		if(model_user != null)
		{	
			int havaResCount = model_user.model_Resource.GetIntMetal();
			if(resNum  >  havaResCount)
			{
				result = havaResCount - resNum;
			}
		}
		return result;
	}

	// 稀土是否足够使用
	public static int IsEnoughRareUse(int resNum)
	{
		int result = 0;
		Model_User model_user = InstancePlayer.instance.model_User;
		if(model_user != null)
		{	
			int havaResCount = model_user.model_Resource.GetIntRare();
			if(resNum > havaResCount)
			{
				result = havaResCount - resNum;
			}
		}
		return result;
	}

	// 资源是否足够使用
	public static bool IsResourcesEnoughToUse(int[] resCheckResult)
	{
		if(resCheckResult != null)
		{
			int n = resCheckResult.Length;
			for(int i = 0; i < n; ++i)
			{
				if(resCheckResult[i] > 0)
				{
					return false;
				}
			}
		}
		return true;
	}

}
