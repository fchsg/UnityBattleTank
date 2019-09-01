using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_Helper {

	public static void PauseAllTimer()
	{
		SetModelTimer (true);
	}

	public static void ResumeAllTimer()
	{
		SetModelTimer (false);
	}


	//获取加速生产所需的金币数
	public static int GetClearUnitCDNeedCash(Model_User model_user, int unitId)
	{
		int result = -1;
		result = ConnectionValidateHelper.IsEnoughCashClearBuildUnitCD(model_user,unitId);

		if(result != 0)
		{
			Model_Unit model_Unit;
			model_user.unlockUnits.TryGetValue(unitId, out model_Unit);
			float needCash = model_user.model_InitialConfig.GetClearUnitCDCash (model_Unit.produceLeftTime);
			float userCash = model_user.model_Resource.cash;
			if(userCash < needCash)
			{
				result = Mathf.CeilToInt(needCash - userCash);
			}
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public static bool HasEnoughResource(DataUnit.BasicCost cost)
	{
		if (cost.costFood > InstancePlayer.instance.model_User.model_Resource.GetIntFood ()) {
			return false;
		}

		if (cost.costOil > InstancePlayer.instance.model_User.model_Resource.GetIntOil ()) {
			return false;
		}

		if (cost.costRare > InstancePlayer.instance.model_User.model_Resource.GetIntRare ()) {
			return false;
		}

		if (cost.costMetal > InstancePlayer.instance.model_User.model_Resource.GetIntMetal ()) {
			return false;
		}

		if (cost.costCash > InstancePlayer.instance.model_User.model_Resource.GetIntCash ()) {
			return false;
		}

		return true;
	}


	public static bool HasEnoughResource(int type, int cost)
	{
		if (type == (int)DataConfig.DATA_TYPE.Food) {
			return cost <= InstancePlayer.instance.model_User.model_Resource.GetIntFood ();
		}

		if (type == (int)DataConfig.DATA_TYPE.Metal) {
			return cost <= InstancePlayer.instance.model_User.model_Resource.GetIntMetal ();
		}

		if (type == (int)DataConfig.DATA_TYPE.Rare) {
			return cost <= InstancePlayer.instance.model_User.model_Resource.GetIntRare ();
		}

		if (type == (int)DataConfig.DATA_TYPE.Oil) {
			return cost <= InstancePlayer.instance.model_User.model_Resource.GetIntOil ();
		}

		if (type == (int)DataConfig.DATA_TYPE.Cash) {
			return cost <= InstancePlayer.instance.model_User.model_Resource.GetIntCash ();
		}

		if (type == (int)DataConfig.DATA_TYPE.Combat) {
			return cost <= InstancePlayer.instance.model_User.model_Resource.GetIntCombat ();
		}

		Assert.assert (false);
		return false;
	}


	//获取玩家缺少的资源种类及数量
	public static int[] GetPlayerNeedBuyRes(int needFood,int needOil,int needMetal,int needRare)
	{
		int[] resArr = new int[4];
		int food = 0;
		int oil = 0;
		int metal = 0;
		int rare = 0;

		food = Model_Helper.GetNeedBuyFoodCount(needFood);
		oil = Model_Helper.GetNeedBuyOilCount(needOil);
		metal = Model_Helper.GetNeedBuyMetalCount(needMetal);
		rare = Model_Helper.GetNeedBuyRareCount(needRare);

		if(resArr != null)
		{
			resArr[0] = food;
			resArr[1] = oil;
			resArr[2] = metal;
			resArr[3] = rare;
		}
		return resArr;
	}
	//获取玩家当前拥有的资源量
	public static int GetPlayerHavaFoodRes()
	{
		return InstancePlayer.instance.model_User.model_Resource.GetIntFood();
	}
	//获取玩家当前拥有的资源量
	public static int GetPlayerHavaOilRes()
	{
		return InstancePlayer.instance.model_User.model_Resource.GetIntOil();
	}
	//获取玩家当前拥有的资源量
	public static int GetPlayerHavaMatelRes()
	{
		return InstancePlayer.instance.model_User.model_Resource.GetIntMetal();
	}
	//获取玩家当前拥有的资源量
	public static int GetPlayerHavaRareRes()
	{
		return InstancePlayer.instance.model_User.model_Resource.GetIntRare();
	}

	//获取需要购买的资源数量
	public static int GetNeedBuyFoodCount(int resCount)
	{
		int result = -1;
		result = ConnectionValidateHelper.IsEnoughFoodUse(resCount);
		if(result != 0)
		{
			result = resCount - InstancePlayer.instance.model_User.model_Resource.GetIntFood();
		}else
		{
			result = 0;
		}
		return result;
	}
	//获取需要购买的资源数量
	public static int GetNeedBuyOilCount(int resCount)
	{
		int result = -1;
		result = ConnectionValidateHelper.IsEnoughOilUse(resCount);
		if(result != 0)
		{
			result = resCount - InstancePlayer.instance.model_User.model_Resource.GetIntOil();
		}else
		{
			result = 0;
		}
		return result;
	}
	//获取需要购买的资源数量
	public static int GetNeedBuyMetalCount(int resCount)
	{
		int result = -1;
		result = ConnectionValidateHelper.IsEnoughMetalUse(resCount);
		if(result != 0)
		{
			result = resCount - InstancePlayer.instance.model_User.model_Resource.GetIntMetal();
		}else
		{
			result = 0;
		}
		return result;
	}
	//获取需要购买的资源数量
	public static int GetNeedBuyRareCount(int resCount)
	{
		int result = -1;
		result = ConnectionValidateHelper.IsEnoughRareUse(resCount);
		if(result != 0)
		{
			result = resCount - InstancePlayer.instance.model_User.model_Resource.GetIntRare();
		}else
		{
			result = 0;
		}
		return result;
	}
	//购买资源需要花费的cash数量
	public static int GetResourcesNeedCash(float foodCount, float metalCount, float oilCount, float rareCount)
	{
		int needCash = 0;
		
		if (foodCount > 0.0f) {
			needCash += GetBuyFoodNeedCash(foodCount);
		}

		if (metalCount > 0.0f) {
			needCash += GetBuyMetalNeedCash(metalCount);
		}
		
		if (oilCount > 0.0f) {
			needCash += GetBuyOilNeedCash(oilCount);
		}

		if (rareCount > 0.0f) {
			needCash += GetBuyRareNeedCash(rareCount);
		}
		
		return needCash;
	}


	private static void SetModelTimer(bool isPause)
	{
		Model_User model_user = InstancePlayer.instance.model_User;

		// energy
		Model_Energy model_Energy = model_user.model_Energy;
		if (isPause) 
		{
			model_Energy.PauseRecoverEnergyTimer();
		} 
		else 
		{
			model_Energy.ResumeRecoverEnergyTimer();
		}

	}

	// 资源与钻石的换算方式：
	// 玩家实际需要花费的钻石数 =（当前购买的资源量*资源换算比）*当前购买的资源区间折扣数(Discount表格)
	private static int GetBuyFoodNeedCash(float foodCount)
	{
		DataInitialConfig initialConfig = DataManager.instance.dataInitialConfigGroup.GetDataInitialConfig ();
		DataDiscount discount = DataManager.instance.dataDiscountGroup.GetDiscount (foodCount);
		
		return Mathf.CeilToInt(foodCount * initialConfig.foodToCashRate * discount.foodDiscount);
	}
	
	private static int GetBuyOilNeedCash(float oilCount)
	{
		DataInitialConfig initialConfig = DataManager.instance.dataInitialConfigGroup.GetDataInitialConfig ();
		DataDiscount discount = DataManager.instance.dataDiscountGroup.GetDiscount (oilCount);
		
		return Mathf.CeilToInt(oilCount * initialConfig.oilToCashRate * discount.oilDiscount);
	}
	
	private static int GetBuyMetalNeedCash(float metalCount)
	{
		DataInitialConfig initialConfig = DataManager.instance.dataInitialConfigGroup.GetDataInitialConfig ();
		DataDiscount discount = DataManager.instance.dataDiscountGroup.GetDiscount (metalCount);
		
		return Mathf.CeilToInt(metalCount * initialConfig.metalToCashRate * discount.metalDiscount);
	}
	
	private static int GetBuyRareNeedCash(float rareCount)
	{
		DataInitialConfig initialConfig = DataManager.instance.dataInitialConfigGroup.GetDataInitialConfig ();
		DataDiscount discount = DataManager.instance.dataDiscountGroup.GetDiscount (rareCount);
		
		return Mathf.CeilToInt(rareCount * initialConfig.rareToCashRate * discount.rareDiscount);
	}

}
