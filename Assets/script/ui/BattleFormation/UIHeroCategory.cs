using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIHeroCategory {

	public UIHeroPanel heroPanel = null;

	public int HERO_BASIC_WEIGHT = 1000;  // 上阵基础权重 

	public class Hero : IComparable<Hero>{
		public int id;
		public bool isOnDuty;   // 是否上阵

		public SlgPB.Hero pb_Hero;

		public int weight = 0;

		// 根据hero id 大小排序
		public int CompareTo(Hero hero)
		{
			if (weight > hero.weight) {
				return -1;
			}
			else if (weight < hero.weight) {
				return 1;
			}
			else  {
				return 0;
			}
		}
	}

	public List<Hero> hero_List = new List<Hero>();

	public UIHeroCategory(Dictionary<int, SlgPB.Hero> userHeros)
	{
		if (userHeros != null && userHeros.Count == 0) 
		{
			 CreateLocalHeroList ();		
		}
		UpdateData ();
	}

	public List<Hero> GetSortHero()
	{
		UpdateHerosOnDuty (hero_List);
		hero_List.Sort ();
		return hero_List;
	}

	private void UpdateData()
	{
		Dictionary<int, SlgPB.Hero> heroesMap = InstancePlayer.instance.model_User.model_heroGroup.heroesMap;

		foreach (SlgPB.Hero slgPB_Hero in heroesMap.Values) 
		{
			Hero hero = new Hero ();
			hero.id = slgPB_Hero.heroId;
			hero.pb_Hero = slgPB_Hero;
			hero_List.Add (hero);
		}
	}

	private void UpdateHerosOnDuty(List<Hero> heros)
	{
		foreach (Hero hero in heros) 
		{
			Model_Formation model_Formation =InstancePlayer.instance.model_User.model_Formation;
			int teamId = model_Formation.GetSelectTeamId();
			hero.isOnDuty = model_Formation.IsTeamContaninHero(teamId ,hero.id);
			if(hero.isOnDuty)
			{
				hero.weight =  HERO_BASIC_WEIGHT + hero.id;
			}
			else
			{
				hero.weight =  hero.id;
			}
		}
	}
		
	// 创建离线测试数据
	public void CreateLocalHeroList()
	{
		Dictionary<int, SlgPB.Hero> heroesMap = InstancePlayer.instance.model_User.model_heroGroup.heroesMap;

		DataHero[] heros = DataManager.instance.dataHeroGroup.GetAllHeroPrimitive ();

		for (int i = 0; i < heros.Length; ++i) 
		{
			SlgPB.Hero hero = new SlgPB.Hero ();
			hero.heroId = heros [i].id;
			hero.exp = 1;
			hero.stage = 1;

			heroesMap.Add (hero.heroId, hero);
		}
	}
		
}
