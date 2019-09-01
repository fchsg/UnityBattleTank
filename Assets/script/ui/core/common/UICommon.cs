using UnityEngine;
using System.Collections;

public class UICommon
{
	// depth
	public  static int UI_PANEL_DEPTH = 1000;
	public  static int UI_TIPS_DEPTH  = 10000;
	public  static int UI_DEPTH_INCREMENTAL = 100; // depth每次增量 100

	// scene prefab path
	public static string UI_LOGIN_SCENE = "login/LoginScene";
	public static string UI_MAIN_SCENE = "main/MainScene";
	public static string UI_SCENE_LOADING = "loading/SceneLoading";
	
	// panel prefab path
	public static string UI_PANEL_LOADING = "loading/LoadingPanel";
	public static string UI_PANEL_BUILDINGUPLEVEL = "building/BuildingUpLevelPanel";
	public static string UI_PANEL_BUILDINGDETAILS = "building/BuildingDetailsPanel";

	public static string UI_PANEL_UPLEVEL = "building/UpLevelPanel";
	public static string UI_PANEL_REPAIRFACTORY = "RepairFactory/RepairFactoryPanel";
	public static string UI_PANEL_TANKFACTORY = "TankFactory/TankFactoryPanel";
	public static string UI_PANEL_TANKINFO = "TankFactory/TankInfoPanel";
	public static string UI_PANEL_TANKPRODUCT = "TankFactory/TankProductionPanel";
	public static string UI_PANEL_RESEARCHSUCC = "TankFactory/ResearchSuccessPanel";
	public static string UI_PANEL_FORMATION = "BattleFormation/BattleFormationPanel";
	public static string UI_PANEL_BATTLE  = "battle/BattlePanelPrefab";
	public static string UI_PANEL_BATTLE_SETTLEMENT  = "battleSettlement/BattleSettlementPanel";
	public static string UI_PANEL_INSTRUCTOR_UPLEVEL = "battleSettlement/InstructorUpLevelPanel";

	public static string UI_PANEL_CAMPAIGN = "campaign/CampaignPanel";
	public static string UI_PANEL_MISSION = "mission/MissionPanel";
	public static string UI_PANEL_MISSION_ClEAR = "mission/MissionClearPanel";
	public static string UI_PANEL_MISSION_ClEAR_RESULT = "mission/MissionClearResultPanel";



	public static string UI_PANEL_WAREHOUSE = "warehouse/WarehousePanel";
	public static string UI_PANEL_NORMALPROP = "warehouse/NormalPropPanel";
	public static string UI_PANEL_CONSUMEPROP = "warehouse/ConsumePropPanel";
	public static string UI_PANEL_HERO = "hero/HeroPanel";
	public static string UI_PANEL_HEROINFO = "hero/HeroInfoPanel";
	public static string UI_PANEL_HEROUPLEVEL = "hero/HeroUpLevelPanel";
	public static string UI_PANEL_HEROADVANCED = "hero/HeroAdvancedPanel";
	public static string UI_PANEL_HEROPROMPT = "hero/PromptPanel";
	public static string UI_PANEL_PVP = "pvp/PVPPanel";
	public static string UI_PANEL_PVPRANK = "pvp/RankPanel";
	public static string UI_PANEL_PVPREPORT = "pvp/ReportPanel";
	public static string UI_PANEL_PVPEXCHANGE = "pvp/ExchangePanel";
	public static string UI_PANEL_PVPENEMYFORMATION = "pvp/EnemyFormationPanel";
	public static string UI_PANEL_MAIL = "mail/MailPanel";


	public static string UI_PANEL_TASK = "Task/TaskPanel";





	public static string UI_PANEL_STRENGTHEN = "strengthen/StrengthenPanel";

	public static string UI_PANEL_MESSAGEERROR = "public/MessageErrorPanel";


	// tips prefab path
	public static string UI_TIPS_COMMON = "tips/TipsCommon";

	public static string UI_TIPS_BUYRES = "building/ResourcesBuyPanel";
	public static string UI_TIPS_SPEEDUPLEVEL = "building/UpLevelSpeedPanel";
	
	// unit icon
	public static string UNIT_SMALL_ICON_PATH = "unit_small_";
	public static string UNIT_ICON_BG = "unit_iconbg_";
	public static string MISSION_ICON_PATH = "mission_icon_";
	public static string HERO_HEAD_ICON_PATH = "hero_head_";
	public static string HERO_BODY_ICON_PATH = "hero_body_";
	public static string HERO_TEXTURE_ICON_PATH = "icon_hero_";
	public static string HERO_RANK_ICON_PATH = "rank_";

	// item icon
	public const string ITEM_PATH_PREFIX 				= "profab/icon/item/item_icon_";
	public const string UNIT_BIG_PATH_PREFIX 		= "profab/icon/unit_big/unit_big_icon_";
	public const string UNIT_SMALL_PATH_PREFIX = "profab/icon/unit_small/unit_small_icon_";
	public const string HERO_SMALL_PATH_PREFIX = "profab/icon/hero/icon_hero_";
	public const string DROP_ITEM_PATH_PREFIX = "profab/icon/dropItem/ItemIcon_";
	public const string TASK_ITEM_ICON= "task_icon_";


	public static Color UNIT_NAME_COLOR_0 = new Color(255.0f/255.0f,255.0f/255.0f,255.0f/255.0f,255.0f/255.0f);// 白色
	public static Color UNIT_NAME_COLOR_1 = new Color(100.0f/255.0f,171.0f/255.0f,24.0f/255.0f,255.0f/255.0f);// 绿色
	public static Color UNIT_NAME_COLOR_2 = new Color(27.0f/255.0f,172.0f/255.0f,198.0f/255.0f,255.0f/255.0f);// 蓝色
	public static Color UNIT_NAME_COLOR_3 = new Color(180.0f/255.0f,30.0f/255.0f,202.0f/255.0f,255.0f/255.0f);// 紫色
	public static Color UNIT_NAME_COLOR_4 = new Color(210.0f/255.0f,153.0f/255.0f,0.0f/255.0f,255.0f/255.0f);// 橙色
	public static Color UNIT_NAME_COLOR_5 = new Color(200.0f/255.0f,19.0f/255.0f,19.0f/255.0f,255.0f/255.0f);// 红色
	// 字体颜色 
	public static Color FONT_COLOR_GREEN = new Color(100.0f/255.0f,171.0f/255.0f,24.0f/255.0f,255.0f/255.0f);// 字体 绿色
	public static Color FONT_COLOR_GREY = new Color(208.0f/255.0f,206.0f/255.0f,191.0f/255.0f,255.0f/255.0f);// 字体 浅灰色
	public static Color FONT_COLOR_GOLDEN = new Color(252.0f/255.0f,226.0f/255.0f,176.0f/255.0f,255.0f/255.0f);// 字体 按钮金色
	public static Color FONT_COLOR_ORANGE = new Color(210.0f/255.0f,153.0f/255.0f,0.0f/255.0f,255.0f/255.0f);// 字体 橙色
	public static Color FONT_COLOR_RED = new Color(210.0f/255.0f,19.0f/255.0f,19.0f/255.0f,255.0f/255.0f);// 字体  红色
	public static Color FONT_COLOR_BROWN = new Color(167.0f/255.0f,137.0f/255.0f,110.0f/255.0f,255.0f/255.0f);// 字体 褐色

	public static Color FONT_COLOR_BANGREY = new Color(118.0f/255.0f,118.0f/255.0f,118.0f/255.0f,255.0f/255.0f);// 字体 禁止点击置灰色
	//图片置灰
	public static Color IMAGE_COLOR_BANGREY = new Color(0.0f/255.0f,255.0f/255.0f,255.0f/255.0f,255.0f/255.0f);// 图片 置灰色

	//16 color  
	public static string SIXTEEN_RED = "[C81313FF]";
	public static string SIXTEEN_GREEN = "[23AB18FF]";
	public static string SIXTEEN_ORANGE = "[D29900FF]";
	public static string SIXTEEN_GREY = "[D0CEBFFF]";
}
