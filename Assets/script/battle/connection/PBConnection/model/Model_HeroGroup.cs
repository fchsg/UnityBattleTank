using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class Model_HeroGroup {

	public Dictionary<int, Hero> heroesMap = new Dictionary<int, Hero> ();

	public Hero GetHero(int id)
	{
		if (heroesMap.ContainsKey (id)) {
			return heroesMap[id];
		}
		return null;
	}

	public bool HasHero(int id)
	{
		return heroesMap.ContainsKey (id);
	}



	public int GetCurrentLevel(int id)
	{
		Hero hero = GetHero (id);
		int level = DataManager.instance.dataHeroGroup.GetHeroLevel (id, hero.exp);
		return level;
	}

	public int GetCurrentStage(int id)
	{
		Hero hero = GetHero (id);
		int stage = hero.stage;
		return stage;
	}

	/*
	public void GetProperty(int id,
	                        out DataHero dataHero, out DataHeroLeadership dataHeroLeadership, out DataHeroUpgrade dataHeroUpgrade)
	{
		Hero hero = GetHero (id);
		DataManager.instance.dataHeroGroup.GetHeroProperty (
			id, hero.exp, hero.stage, out dataHero, out dataHeroLeadership, out dataHeroUpgrade);

	}
	*/

	public void SetHeroes(List<SlgPB.Hero> heroes)
	{
		heroesMap = new Dictionary<int, Hero> ();

		foreach (SlgPB.Hero hero in heroes) {
			heroesMap[hero.heroId] = hero;
		}
	}

	public void AddHero(SlgPB.Hero hero)
	{
		heroesMap[hero.heroId] = hero;
	}

	// =========================================
	// upgrade

	public bool IsHeroArriveMaxStage(int id)
	{
		Hero hero = GetHero (id);
		return hero.stage >= DataHero.STAGE_MAX;
	}
	
	public bool HasEnoughItemToUpgrade(int id)
	{
		Hero hero = GetHero (id);
		DataHeroUpgrade dataHeroUpgrade = DataManager.instance.dataHeroGroup.GetHeroUpgrade (id, hero.stage + 1);
		
		int count = InstancePlayer.instance.model_User.model_itemGroup.GetItemCount(dataHeroUpgrade.itemId);
		return (count >= dataHeroUpgrade.itemCount);
	}

	public bool HasEnoughLevelToUpgrade(int id)
	{
		Hero hero = GetHero (id);
		DataHeroUpgrade dataHeroUpgrade = DataManager.instance.dataHeroGroup.GetHeroUpgrade (id, hero.stage + 1);

		int level = GetCurrentLevel (id);
		return (level >= dataHeroUpgrade.requireLevel);
	}

	// =========================================
	// compose

	public bool HasEnoughItemToCompose(int id)
	{
		DataHeroUpgrade dataHeroUpgrade = DataManager.instance.dataHeroGroup.GetHeroUpgrade (id, 1);

		int count = InstancePlayer.instance.model_User.model_itemGroup.GetItemCount(dataHeroUpgrade.itemId);
		return (count >= dataHeroUpgrade.itemCount);
	}


	// =========================================
	// exp change

	public class ExpChangeResult
	{
		public int heroId;
		public int exp;
		public int expChanged;
		public int level;
		public int levelChanged;
	}

	public ExpChangeResult AddExp(int heroId, int exp)
	{
		Hero hero = GetHero (heroId);

		DataHeroGroup dataHeroGroup = DataManager.instance.dataHeroGroup;
		int maxExp = dataHeroGroup.GetMaxExp (heroId);
		exp = (int)Mathf.Min (exp, maxExp - hero.exp);

		ExpChangeResult r = new ExpChangeResult ();
		r.heroId = heroId;
		r.expChanged = exp;

		int oldLevel = GetCurrentLevel (heroId);
		hero.exp += exp;
		r.exp = hero.exp;
		int newLevel = GetCurrentLevel (heroId);

		r.level = newLevel;
		r.levelChanged = newLevel - oldLevel;

		return r;

	}

}
