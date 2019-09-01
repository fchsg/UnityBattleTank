using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataHeroGroup {

	public Dictionary<int, DataHero> heroesMap = new Dictionary<int, DataHero>(); //id, hero
	public Dictionary<int, DataHeroLeadership> heroesLeadershipMap = new Dictionary<int, DataHeroLeadership>();
	public Dictionary<int, Dictionary<int, DataHeroUpgrade>> heroesUpgradeMap = new Dictionary<int, Dictionary<int, DataHeroUpgrade>> (); //id, stage
	public Dictionary<int, DataHeroExp> heroesExpMap = new Dictionary<int, DataHeroExp> ();

	public DataHero GetHero(int id, int exp, int stage)
	{
		DataHero dataHero = heroesMap [id];
		dataHero = dataHero.Clone () as DataHero;

		/*
		SlgPB.Hero hero = InstancePlayer.instance.model_User.model_heroGroup.GetHero ();
		int stage = hero.stage;
		*/

		int level = GetHeroLevel (id, exp);
		DataHeroUpgrade dataUpgrade = GetHeroUpgrade (id, stage);
		dataHero.AddUpgradeEffect (dataUpgrade, level);

		return dataHero;

		/*
		Dictionary<int, DataHero> levels = heroesMap [id];
		return levels [level];
		*/

	}

	public DataHero[] GetAllHeroPrimitive()
	{
		DataHero[] heroes = new DataHero[heroesMap.Values.Count];
		heroesMap.Values.CopyTo (heroes, 0);
		return heroes;

		/*
		DataHero[] heros = new DataHero[heroesMap.Count];
		int i = 0;
		foreach( Dictionary<int, DataHero> levels in heroesMap.Values)
		{
			heros[i]  = levels [level];
			++i;
		}
		return heros;
		*/
	}

	public DataHero GetHeroPrimitive(int heroId)
	{
		return heroesMap [heroId];

	}

		public int GetHeroLevel(int id, int exp)
	{
		int quality = GetHeroQuality (id);

		int n = heroesExpMap.Count;
		for (int i = 2; i <= n; ++i) {
			DataHeroExp dataHeroExp = heroesExpMap[i];
			int needExp = dataHeroExp.qualityExp[quality - 1];
			if(exp < needExp)
			{
				return i - 1;
			}
		}
		return n;
	}

	public int GetHeroExpToNextLevel(int id, int exp)
	{
		int level = GetHeroLevel (id, exp);
		int maxLevel = GetMaxLevel (id);
		if (level >= maxLevel) {
			return 0;
		}

		int nextLevel = level + 1;
		DataHeroExp dataHeroExp = heroesExpMap[nextLevel];

		int quality = GetHeroQuality (id);
		int needExp = dataHeroExp.qualityExp[quality - 1];
		return needExp - exp;

	}

	public int GetHeroTotalExpToNextLevel(int id, int exp)
	{
		int level = GetHeroLevel (id, exp);
		int maxLevel = GetMaxLevel (id);
		if (level >= maxLevel) {
			return 0;
		}

		int nextLevel = level + 1;
		DataHeroExp dataHeroExp1 = heroesExpMap[level];
		DataHeroExp dataHeroExp2 = heroesExpMap[nextLevel];

		int quality = GetHeroQuality (id);
		int exp1 = dataHeroExp1.qualityExp[quality - 1];
		int exp2 = dataHeroExp2.qualityExp[quality - 1];
		return exp2 - exp1;

	}

	public int GetMaxExp(int id)
	{
		int quality = GetHeroQuality (id);

		int n = heroesExpMap.Count;
		DataHeroExp dataHeroExp = heroesExpMap [n];
		int exp = dataHeroExp.qualityExp[quality - 1];
		return exp;

	}
	
	public int GetMaxLevel(int id)
	{
		int quality = GetHeroQuality (id);

		int n = heroesExpMap.Count;
		return n;

	}

	public DataHeroLeadership GetHeroLeadership(int id)
	{
		return heroesLeadershipMap [id];
	}

	public int GetHeroQuality(int id)
	{
		int quality = heroesLeadershipMap [id].quality;
		return quality;
	}
	
	public DataHeroUpgrade GetHeroUpgrade(int id, int stage)
	{
		Dictionary<int, DataHeroUpgrade> stagesMap = heroesUpgradeMap[id];
		return stagesMap [stage];
	}

	/*
	public void GetHeroProperty(int id, int exp, int stage,
	                            out DataHero hero, out DataHeroLeadership leadership, out DataHeroUpgrade upgrade)
	{
		int level = GetHeroLevel (id, exp);
		hero = GetHero (id, level);
		leadership = GetHeroLeadership (id);
		upgrade = GetHeroUpgrade (id, stage);
	}
	*/
	
	public void LoadHeros(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		foreach (LitJson.JSONNode subNode in json.Childs) 
		{
			DataHero data = new DataHero();
			data.Load(subNode);

			heroesMap[data.id] = data;

			/*
			if(!heroesMap.ContainsKey(data.id))
			{
				heroesMap[data.id] = new Dictionary<int, DataHero>();
			}
			Dictionary<int, DataHero> levelsMap = heroesMap[data.id];
			levelsMap[data.level] = data;
			*/

		}
	}

	public void LoadHerosLeadership(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		foreach (LitJson.JSONNode subNode in json.Childs) 
		{
			DataHeroLeadership data = new DataHeroLeadership();
			data.Load(subNode);

			heroesLeadershipMap[data.id] = data;
		}
	}


	public void LoadHerosUpgrade(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		foreach (LitJson.JSONNode subNode in json.Childs) 
		{
			DataHeroUpgrade data = new DataHeroUpgrade();
			data.Load(subNode);

			if(!heroesUpgradeMap.ContainsKey(data.id))
			{
				Assert.assert(GetHeroUnlockWithItem(data.itemId) <= 0, "one item link to multi heroes");

				heroesUpgradeMap[data.id] = new Dictionary<int, DataHeroUpgrade>();
			}
			Dictionary<int, DataHeroUpgrade> stagesMap = heroesUpgradeMap[data.id];
			stagesMap[data.stage] = data;
		}
	}


	public int GetHeroUnlockWithItem(int itemId)
	{
		foreach (Dictionary<int, DataHeroUpgrade> upgrades in heroesUpgradeMap.Values) {
			DataHeroUpgrade firstUpgrade = upgrades [1];

			foreach (DataHeroUpgrade upgrade in upgrades.Values) {
				Assert.assert (upgrade.itemId == firstUpgrade.itemId, "not all stage use same chip item");
			}

			if (firstUpgrade.itemId == itemId) {
				return firstUpgrade.id;
			}
		}

		return 0;
	}


	public void LoadHerosExp(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		foreach (LitJson.JSONNode subNode in json.Childs) 
		{
			DataHeroExp data = new DataHeroExp();
			data.Load(subNode);

			heroesExpMap[data.level] = data;
		}
	}

}
