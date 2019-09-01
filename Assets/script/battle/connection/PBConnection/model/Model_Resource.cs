using UnityEngine;
using System.Collections;

using SlgPB;

public class Model_Resource {

	public float cash;//主要需要付费的虚拟货币
	public float food;//主要需要付费的虚拟货币
	public float oil;//主要需要付费的虚拟货币
	public float metal;//主要需要付费的虚拟货币
	public float rare;//主要需要付费的虚拟货币
	public float combat;//主要需要付费的虚拟货币
	
	// 资源收获
	public int gainCash;
	public int gainFood;
	public int gainOil;
	public int gainMetal;
	public int gainRare;
	public int gainCombat;
	
	// 资源消耗
	public int costCash;
	public int costFood;
	public int costOil;
	public int costMetal;
	public int costRare;
	public int costCombat;



	public int GetIntCash()
	{
		return Mathf.FloorToInt (cash);
	}
	
	public int GetIntFood()
	{
		return Mathf.FloorToInt (food);
	}
	
	public int GetIntOil()
	{
		return Mathf.FloorToInt (oil);
	}
	
	public int GetIntMetal()
	{
		return Mathf.FloorToInt (metal);
	}
	
	public int GetIntRare()
	{
		return Mathf.FloorToInt (rare);
	}

	public int GetIntCombat()
	{
		return Mathf.FloorToInt (combat);
	}



	public void UpdateUserResources(User user, bool isLogin = false)
	{
		if (user == null) 
		{
			return;
		}
		int diffCash = Mathf.FloorToInt ((float)user.cash - cash);
		cash = user.cash;
		if (diffCash > 0) {
			gainCash = diffCash;
			costCash = 0;
		} else if (diffCash < 0) {
			gainCash = 0;
			costCash = diffCash;		
		} else {
			gainCash = costCash = 0;
		}
		
		int diffFood = Mathf.FloorToInt ((float)user.food - food);
		food = user.food;
		if (diffFood > 0) {
			gainFood = diffFood;
			costFood = 0;
		} else if (diffFood < 0) {
			gainFood = 0;
			costFood = diffFood;
		} else {
			gainFood = costFood = 0;		
		}
		
		int diffOil = Mathf.FloorToInt ((float)user.oil - oil);
		oil = user.oil;
		if (diffOil > 0) {
			gainOil = diffOil;
			costOil = 0;
		} else if (diffOil < 0) {
			gainOil = 0;
			costOil = diffOil;
		} else {
			gainOil = costOil = 0;
		}
		
		int diffMetal = Mathf.FloorToInt ((float)user.metal - metal);
		metal = user.metal;
		if (diffMetal > 0) {
			gainMetal = diffMetal;
			costMetal = 0;
		} else if (diffMetal < 0) {
			gainMetal = 0;
			costMetal = diffMetal;		
		} else {
			gainMetal = costMetal = 0;
		}
		
		int diffRare = Mathf.FloorToInt ((float)user.rare - rare);
		rare = user.rare;
		if (diffRare > 0) {
			gainRare = diffRare;
			costRare = 0;
		} else if (diffRare < 0) {
			gainRare = 0;
			costRare = diffRare;
		} else {
			gainRare = costRare = 0;
		}

		int diffCombat = Mathf.FloorToInt ((float)user.combat - combat);
		combat = user.combat;
		if (diffCombat > 0) {
			gainCombat = diffCombat;
			costCombat = 0;
		} else if (diffCombat < 0) {
			gainCombat = 0;
			costCombat = diffCombat;
		} else {
			gainCombat = costCombat = 0;
		}


		if (isLogin) 
		{
			gainCash = costCash = 0;
			gainFood = costFood = 0;
			gainOil = costOil = 0;
			gainMetal = costMetal = 0;
			gainRare = costRare = 0;
			gainCombat = costCombat = 0;
		}
	}

}
