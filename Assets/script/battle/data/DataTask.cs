using UnityEngine;
using System.Collections;

public class DataTask {

	public enum TYPE
	{
		UNKNOWN = 0,
		NORMAL = 1,
		DAILY,
		OTHER,
	}

	public TYPE taskType;
	public int id;
	public int preTaskId;
	public int itemGroupID;

	public string command;
	public float source;
	public float destination;
	public float additional;


	public int totalProgress;

	public string name;
	public string descript;

	public int icon;

	public void Load(LitJson.JSONNode json)
	{
		taskType = (TYPE)JsonReader.Int (json, "TaskType");
		id = JsonReader.Int(json, "Id");
		preTaskId = JsonReader.Int(json, "PreTaskId");
		itemGroupID = JsonReader.Int(json, "ItemGroupID");

		command = json["Command"];
		source = JsonReader.Float(json, "Source");
		destination = JsonReader.Float(json, "Destination");
		additional = JsonReader.Float(json, "Additional");

		totalProgress = JsonReader.Int(json, "TotalProgress");

		name = json["Name"];
		descript =GetDescription(json["Descript"]) ;

		icon = JsonReader.Int (json, "Icon");

		//Trace.trace ("task" + " command: " + command  + " description: "  + descript, Trace.CHANNEL.IO);

	}

	private string GetDescription(string descript)
	{
		switch (command) 
		{
		case "CommanderLv": //指挥官提升到Ｎ级
			{
				descript = StringHelper.GenerateCompleteDescription (descript, source);
			}
			break;

		case "PassMission":    //完成系统指定的关卡
			{
				int magicId = (int)source;
				DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (magicId);
				descript = StringHelper.GenerateCompleteDescription (descript, dataMission.name, destination);
			}
			break;

		case "SpecifiedUnitPartLv":    //指定unitpart强化到N级
			{
				int unitId = (int)source;
				DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);

				int partId = (int)destination;
				DataUnitPart dataUnitPart = DataManager.instance.dataUnitPartGroup.GetPart (partId, 1);

				descript = StringHelper.GenerateCompleteDescription (descript, dataUnit.name, dataUnitPart.name, additional);
			}
			break;

		case "SpecifiedHeroLv":    //指定hero升级到N级
			{
				int heroId = (int)source;
				DataHero dataHero = DataManager.instance.dataHeroGroup.GetHeroPrimitive (heroId);

				descript = StringHelper.GenerateCompleteDescription (descript, dataHero.name, destination);
			}
			break;

		case "SpecifiedHeroStage":    //指定hero升衔到N阶
			{
				int heroId = (int)source;
				DataHero dataHero = DataManager.instance.dataHeroGroup.GetHeroPrimitive (heroId);

				descript = StringHelper.GenerateCompleteDescription (descript, dataHero.name, destination);
			}
			break;

		case "ConsumeStar": //获得N颗星星
			{
				descript = StringHelper.GenerateCompleteDescription (descript, source);
			}

			break;

		case "GainUnit": //获得指定unit
			{
				int unitId = (int)source;
				DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);

				descript = StringHelper.GenerateCompleteDescription (descript, dataUnit.name, destination);
			}
			break;

		case "GainHero": //获得指定hero
			{
				int heroId = (int)source;
				DataHero dataHero = DataManager.instance.dataHeroGroup.GetHeroPrimitive (heroId);

				descript = StringHelper.GenerateCompleteDescription (descript, dataHero.name);
			}
			break;

		case "ArenaRankings": //在竞技场达到指定排名
			{
				descript = StringHelper.GenerateCompleteDescription (descript, source);
			}
			break;

		case "BuildingLv": //相应建筑提升到N级
			{
				int buildingId = (int)source;
				DataBuilding dataBuilding = DataManager.instance.dataBuildingGroup.GetBuilding (buildingId, 1);

				descript = StringHelper.GenerateCompleteDescription (descript, dataBuilding.name, destination);
			}
			break;
		}

		return descript;
	}



}
