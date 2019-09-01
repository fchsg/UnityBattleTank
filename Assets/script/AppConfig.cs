using UnityEngine;
using System.Collections;

public class AppConfig {

	//global setting
	public const bool DEBUGGING = true;
	public const bool USE_FIXED_FRAMERATE = DEBUGGING && true;
	public const bool USE_DYNAMIC_CONFIG = !DEBUGGING || false;

	public const bool SHOW_TILE = DEBUGGING && false;
	public const bool SHOW_PATH = SHOW_TILE && false;
	public const bool SHOW_HOTFIELD = SHOW_TILE && false;


	//table
	public const string TAB_MAIN_CAMERA = "MainCamera";
	public const string TAB_UI_CAMERA = "UICamera";
	public const string TAB_HTTP = "HTTP";
	public const string TAB_BATTLE_GAME = "battle_game";
	public const string TAB_BATTLE_FIELD = "battle_field";


	//scene name
	public const string SCENE_NAME_UI = "SceneMainUI";
	public const string SCENE_NAME_BATTLE = "SceneBattle";
	public const string SCENE_NAME_LOADING = "SceneLoading";
	public const string SCENE_NAME_BATTLE_OFFLINE = "SceneBattleOffline";


	//folder
	public const string FOLDER_PROFAB = "profab/";

	public const string FOLDER_PROFAB_TANK = FOLDER_PROFAB + "tank/";

	public const string FOLDER_PROFAB_EFFECT 			  = FOLDER_PROFAB + "effect/";
	public const string FOLDER_PROFAB_EFFECT_EXPLODE 	  = FOLDER_PROFAB_EFFECT + "explode/";

	public const string FOLDER_PROFAB_CAMPAIGN = FOLDER_PROFAB + "campaign/";

	public const string FOLDER_PROFAB_UI = FOLDER_PROFAB + "ui/";
	public const string FOLDER_PROFAB_UI_HUDTEXT = FOLDER_PROFAB_UI + "hudtext/";

	public const string FOLDER_DATACONFIG = "DataConfig/";

	//orientation
	public static Vector3 DEFAULT_DIRECTION = new Vector3(0, 0, 1);
	public static Vector3 DEFAULT_ANGLE = new Vector3 (40, 0, 0);
	public static Vector3 SHADOW_DIRECTION = new Vector3 (0.2f, -1, -0.5f);


	//design resolution
	public const float DESIGN_WIDTH  = 1334.0f;
	public const float DESIGN_HEIGHT = 750.0f;

	// Unity单位1 对应 UI 缩放比例 
	public static float UI_ASPECT_HEIGHT = DESIGN_HEIGHT / Screen.height;
	public static Vector3 UNITY_SCALE_UI = new Vector3((Screen.height / UI_ASPECT_HEIGHT / 2.0f), (Screen.height / UI_ASPECT_HEIGHT / 2.0f), 0.0f);

	//camera direction
	public const float CAMERA_DEGREE = 50;
	public const float CAMERA_RADIAN = Mathf.PI * CAMERA_DEGREE / 180;

	// sortingOrder 
	public const int SORTINGORDER_UI_POPUP = 10;



	// mission
	public const int FIRST_MISSION_MAGICID = 10101;

}
