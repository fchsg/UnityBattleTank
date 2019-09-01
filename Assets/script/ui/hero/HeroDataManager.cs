using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class HeroDataManager   
{
	public enum RecruitType
	{
		ALREADYRECRUIT = 0,//已招募
		CANRECRUIT = 1,//能招募
		UNABLERECRUIT = 2,//不能招募
	}
	public enum TEAM
	{
		NOTEAM = 0,//没有上阵 
		FIRSTTEAM = 1,//一队
		SECONDTEAM = 2,//二队
		THIRDTEAM = 3,//三队
	}
	public enum HeroType
	{
		TANK = 1,//坦克
		GUN = 2,//装甲
		MISSILE = 3,//导弹
		CANNON = 4,//火炮
		UNKNOWN = 5,//反坦

	}
	public class HeroData
	{
		public int id;
		public RecruitType recruitType;
		public HeroType heroType;
		public int quality;
		public int rank;
		public TEAM team;
		public int level;
		public int skillId;
		public int exp;
		public Color nameColor;
		public string IconBgName;
		public string IconName;
		public string rankName;
		public DataHero data;
		public DataHeroUpgrade heroUpgradeData;
	}
	//data 
	private DataHeroGroup heroGroupData;
	private Dictionary<int, DataHero> heroesMap ;

	List<HeroData> allHeroDataList = new List<HeroData>();
	List<int> typeList = new List<int>();
	List<int> colorList = new List<int>();
	public HeroDataManager()
	{
		Init();
	}

	public void Init()
	{
		allHeroDataList.Clear();
		heroGroupData = DataManager.instance.dataHeroGroup;
		heroesMap = heroGroupData.heroesMap;
		foreach(KeyValuePair<int,DataHero> kvp in heroesMap)
		{
			HeroData heroData = new HeroData();
			heroData = InitHeroData(kvp.Value);
			allHeroDataList.Add(heroData);
		}
	}


	public List<HeroData> ShowHeroData(List<int> typeList,List<int> colorList)
	{
		List<HeroData> needHeroDataList = new List<HeroData>();
		needHeroDataList.Clear();
		foreach(HeroData hero in allHeroDataList)
		{
			if(typeList.Count == 0 && colorList.Count == 0)
			{
				needHeroDataList.Add(hero);
				continue;
			}
			if(typeList.Count == 0 && colorList.Count != 0)
			{
				bool isAdd = false;
				foreach(int color in colorList)
				{
					if(hero.quality == color)
					{
						isAdd = false;
						break;
					}
					else
					{
						isAdd = true;
					}
				}
				if(isAdd)
				{
					needHeroDataList.Add(hero);
				}
				continue;
			}
			if(typeList.Count != 0 && colorList.Count == 0)
			{
				bool isAdd = false;
				foreach(int type in typeList)
				{
					if((int)hero.heroType == type)
					{
						isAdd = false;
						break;
					}
					else
					{
						isAdd = true;
					}
				}
				if(isAdd)
				{
					needHeroDataList.Add(hero);
				}
				continue;
			}
		}
		needHeroDataList = SortingData(needHeroDataList);

		return needHeroDataList;
	}

	public List<HeroData> SortingData(List<HeroData> heroDataList)
	{
		List<HeroData> alreadyRecruitList = new List<HeroData>();
		List<HeroData> canRecruitList = new List<HeroData>();
		List<HeroData> unableRecruitList = new List<HeroData>();
		alreadyRecruitList.Clear();
		canRecruitList.Clear();
		unableRecruitList.Clear();
		List<HeroData> formationHeroDataList = new List<HeroData>();
		List<HeroData> notFormationHeroDataList = new List<HeroData>();
		formationHeroDataList.Clear();
		notFormationHeroDataList.Clear();
		List<HeroData> newHeroDataList = new List<HeroData>();
		newHeroDataList.Clear();

		foreach(HeroData hero in heroDataList)
		{
			switch(hero.recruitType)
			{
			case RecruitType.ALREADYRECRUIT:
				alreadyRecruitList.Add(hero);
				break;
			case RecruitType.CANRECRUIT:
				canRecruitList.Add(hero);
				break;
			case RecruitType.UNABLERECRUIT:
				unableRecruitList.Add(hero);
				break;
			default:
				break;
			}


		}
		foreach(HeroData hero in alreadyRecruitList)
		{
			switch(hero.team)
			{
			case TEAM.NOTEAM:
				notFormationHeroDataList.Add(hero);
				break;
			case TEAM.FIRSTTEAM:	 
			case TEAM.SECONDTEAM:
			case TEAM.THIRDTEAM:
				formationHeroDataList.Add(hero);
				break;
			default:
				break;
			}
		}
		//上阵英雄 品质排序
		formationHeroDataList.Sort(QualitySortCompareDESC);
		formationHeroDataList.Sort(QualityAfterRankSort);
		formationHeroDataList.Sort(RankAfterLevelSort);
		//未上阵英雄 品质排序
		notFormationHeroDataList.Sort(QualitySortCompareDESC);
		notFormationHeroDataList.Sort(QualityAfterRankSort);
		notFormationHeroDataList.Sort(RankAfterLevelSort);

		//	能招募英雄排序
		canRecruitList.Sort(QualitySortCompareDESC);
		canRecruitList.Sort(IDSortCompare);
		//	不能招募英雄排序
		unableRecruitList.Sort(QualitySortCompareASCE);
		unableRecruitList.Sort(IDSortCompare);

		newHeroDataList.AddRange(formationHeroDataList);
		newHeroDataList.AddRange(notFormationHeroDataList);
		newHeroDataList.AddRange(canRecruitList);
		newHeroDataList.AddRange(unableRecruitList);

		return newHeroDataList;
	}
	public HeroData InitHeroData(DataHero data)
	{
		if(data != null)
		{ 
			DataHeroLeadership heroLeadershipData = heroGroupData.GetHeroLeadership(data.id);

			Model_HeroGroup model_heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
			Model_ItemGroup model_itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
			Model_Formation model_formation = InstancePlayer.instance.model_User.model_Formation;
			HeroData heroData = new HeroData();
			heroData.id = data.id;
			heroData.heroType = (HeroType)data.type;
			heroData.quality = heroLeadershipData.quality;
			heroData.IconName = UICommon.HERO_TEXTURE_ICON_PATH + data.id;
			heroData.skillId = (int)heroLeadershipData.skill;
			heroData.data =  data;
			DataHeroUpgrade heroUpgradeData = heroGroupData.GetHeroUpgrade(data.id,1);
			heroData.heroUpgradeData = heroUpgradeData;
			if(model_heroGroup.heroesMap.ContainsKey(data.id))
			{
				heroData.level = model_heroGroup.GetCurrentLevel(data.id);
				heroData.recruitType = RecruitType.ALREADYRECRUIT;
				heroData.rank = model_heroGroup.GetCurrentStage(data.id);
				heroData.rankName = UICommon.HERO_RANK_ICON_PATH + model_heroGroup.GetCurrentStage(data.id);
				heroData.exp = model_heroGroup.GetHero(data.id).exp;
			}
			else
			{
				heroData.level = 1;
				heroData.rank = 1;//默认军衔为下士
				heroData.rankName = UICommon.HERO_RANK_ICON_PATH + "1";
				heroData.exp = 0;
				
				//isCanUnlock 是否能解锁
				Item item = model_itemGroup.QueryItem(heroUpgradeData.itemId);
				if(item.num >= heroUpgradeData.itemCount)
				{
					heroData.recruitType = RecruitType.CANRECRUIT;
				}
				else
				{
					heroData.recruitType = RecruitType.UNABLERECRUIT;
				}
			}
			//英雄是否上阵
			for(int i = 1 ;i < 4 ;i ++)
			{
				heroData.team = (TEAM)0;
				if(model_formation.IsTeamContaninHero(i,data.id))
				{
					heroData.team = (TEAM)i;
					break;
				}

			}
 
			switch((int)heroLeadershipData.quality)
			{
			case 1:
				heroData.nameColor = UICommon.UNIT_NAME_COLOR_0;
				heroData.IconBgName = UICommon.UNIT_ICON_BG + "0";
				break;
			case 2:
				heroData.nameColor = UICommon.UNIT_NAME_COLOR_1;
				heroData.IconBgName = UICommon.UNIT_ICON_BG + "1";
				break;
			case 3:
				heroData.nameColor = UICommon.UNIT_NAME_COLOR_2;
				heroData.IconBgName = UICommon.UNIT_ICON_BG + "2";
				break;
			case 4:
				heroData.nameColor = UICommon.UNIT_NAME_COLOR_3;
				heroData.IconBgName = UICommon.UNIT_ICON_BG + "3";
				break;
			case 5:
				heroData.nameColor = UICommon.UNIT_NAME_COLOR_4;
				heroData.IconBgName = UICommon.UNIT_ICON_BG + "4";
				break;
			default :
				heroData.nameColor = UICommon.UNIT_NAME_COLOR_0;
				heroData.IconBgName = UICommon.UNIT_ICON_BG + "0";
				break;
			}
			 
			return heroData;
		}
		return null;
	}


	/// <summary>
	/// 按品质升序或者降序
	/// </summary>
	/// <returns>The sorting.</returns>
	/// <param name="dataList">Data list.</param>
	/// <param name="isLowHigh">If set to <c>true</c> is low high.</param>
	List<HeroData>  QualitySorting(List<HeroData> dataList,bool isLowHigh)
	{
		if(dataList != null)
		{
			if(isLowHigh)
			{
				dataList.Sort(QualitySortCompareASCE);
			}
			else
			{
				dataList.Sort(QualitySortCompareDESC);
			}
			return dataList;

		}
		return null;
	}
	/// <summary>
	/// Qualities the sort compare 升序 .
	/// </summary>
	/// <returns>The sort compare ASC.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	private static int QualitySortCompareASCE(HeroData obj1, HeroData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality > obj2.quality)
		{
			res = 1;
		}
		else if (obj1.quality < obj2.quality)
		{
			res = -1;
		}
		return res;
	}
	/// <summary>
	/// Qualities the sort compare  降序.
	/// </summary>
	/// <returns>The sort compare DES.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	private static int QualitySortCompareDESC(HeroData obj1, HeroData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality > obj2.quality)
		{
			res = -1;
		}
		else if (obj1.quality < obj2.quality)
		{
			res = 1;
		}
		return res;
	}

	/// <summary>
	/// 按照id大小
	/// </summary>
	/// <returns>The sort compare.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	private static int IDSortCompare(HeroData obj1, HeroData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality == obj2.quality)
		{
			if (obj1.id > obj2.id)
			{
				res = 1;
			}
			else if (obj1.id < obj2.id)
			{
				res = -1;
			}
		}
		return res;
	}

	private static int QualityAfterRankSort(HeroData obj1, HeroData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality == obj2.quality)
		{
			if (obj1.rank > obj2.rank)
			{
				res = 1;
			}
			else if (obj1.rank < obj2.rank)
			{
				res = -1;
			}
		}
		return res;
	}
	private static int RankAfterLevelSort(HeroData obj1, HeroData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.rank == obj2.rank)
		{
			if (obj1.level > obj2.level)
			{
				res = 1;
			}
			else if (obj1.level < obj2.level)
			{
				res = -1;
			}
		}
		return res;
	}

}
