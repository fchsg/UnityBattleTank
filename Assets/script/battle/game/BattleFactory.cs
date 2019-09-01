using UnityEngine;
using System.Collections;

public class BattleFactory {

	private const float HINT_TEXT_HEIGHT = 2;

	public static GameObject[][] CreateBattleUnits(InstanceBattle battle, DataMap dataMap)
	{
		int totalGroup = 1 + battle.extraTeamCount;
		GameObject[][] objects = new GameObject[totalGroup][];

		GameObject[] myObjects = CreateBattleTeam (battle.myTeam, DataConfig.TEAM.MY, dataMap.startPosition, dataMap.startPosition, battle.mission.playerRotation);
		objects [0] = myObjects;

		for (int i = 1; i < totalGroup; ++i) {
			int teamIndex = i - 1;
			COORD location = dataMap.GetExtraPosition(teamIndex);

			InstanceTeam enemyTeam = battle.enemyTeams[teamIndex];
			InstanceTeam friendTeam = battle.friendTeams[teamIndex];
			Assert.assert(enemyTeam == null || friendTeam == null);

			float teamRotation = battle.mission.teamsRotation[teamIndex];

			GameObject[] members = null;
			if(enemyTeam != null)
			{
				Assert.assert(location != null);
				members = CreateBattleTeam (enemyTeam, DataConfig.TEAM.ENEMY, location, dataMap.startPosition, teamRotation);
			}
			if(friendTeam != null)
			{
				Assert.assert(location != null);
				members = CreateBattleTeam (friendTeam, DataConfig.TEAM.FRIEND, location, dataMap.startPosition, teamRotation);
			}
			objects [i] = members;

		}

		return objects;
	}

	public static GameObject[] CreateBattleTeam(InstanceTeam team, DataConfig.TEAM teamSide,
	                                            COORD location, COORD playerLocation, float rotationDegree)
	{
		int n = team.units.Length;
		GameObject[] objects = new GameObject[n];

		for (int i = 0; i < n; ++i) {
			InstanceUnit unit = team.units[i];
			if(unit != null)
			{
				GameObject obj = CreateBattleUnit(unit, i, teamSide, location, playerLocation, rotationDegree);
				objects[i] = obj;
			}
		}

		return objects;
	}

	public static GameObject CreateBattleUnit(InstanceUnit instUnit, int slotIndex, DataConfig.TEAM teamSide,
	                                          COORD location, COORD playerLocation, float rotationDegree)
	{
		DataUnit dataUnit = instUnit.dataUnit;

		string asset = AppConfig.FOLDER_PROFAB_TANK + "tankPrimitive"; //dataUnit.asset
		GameObject obj = ResourceHelper.Load(asset);
//		obj.transform.localScale = new Vector3 (2, 2, 2);

		Unit unit = obj.GetComponent<Unit> ();
		unit.transform.Rotate(new Vector3(0, rotationDegree, 0));
//		unit.body.transform.Rotate(new Vector3(0, rotationDegree, 0));
//		unit.launcher.transform.Rotate(new Vector3(0, rotationDegree, 0));

		/*
		if(teamSide == DataConfig.TEAM.MY)
		{
			unit.body.transform.Rotate(new Vector3(0, 90, 0));
			unit.launcher.transform.Rotate(new Vector3(0, 90, 0));
		}
		else
		{
			Vector3 o = new Vector3(
				playerLocation.x - location.x,
				0,
				playerLocation.z - location.z);

			unit.body.transform.LookAt(unit.body.transform.position + o);
			unit.launcher.transform.LookAt(unit.launcher.transform.position + o);
		}
		*/


		Vector3 offset = CalcUnitPosOffset (slotIndex);
		offset = unit.body.transform.TransformDirection (offset);

		Vector2 center = MapGrid.GetTileCenter (location.x, location.z);
		float x = center.x + offset.x;
		float z = center.y + offset.z;

		float randomRange = 0.5f;
		x += RandomHelper.Range (-randomRange, randomRange);
		z += RandomHelper.Range (-randomRange, randomRange);
		
		Vector3 fightPos = new Vector3 (x, 0, z);
		unit.Init (instUnit, teamSide, fightPos, slotIndex);

		AddUnitToLayer (obj, BattleConfig.LAYER.TANK);
		return obj;
	}


	public static Vector3 CalcUnitPosOffset(int slotIndex)
	{
		COORD[] offset = new COORD[]{
			/*
			new COORD(-1, 0),
			new COORD(0, 0),
			new COORD(1, 0),
			new COORD(-1, -1),
			new COORD(0, -1),
			new COORD(1, -1),
			*/

			new COORD(-1, 0),
			new COORD(0, 0),
			new COORD(1, 0),
			new COORD(2, 0),
			new COORD(0, -1),
			new COORD(1, -1),

			/*
			new COORD(0, 1),
			new COORD(-2, 0),
			new COORD(-1, 0),
			new COORD(0, 0),
			new COORD(1, 0),
			new COORD(2, 0),
			*/

		};

		Vector3 v = offset [slotIndex].ToVector3();
		v *= MapGrid.GRID_SIZE;

		return v;
	}

	/*
	public static Vector3 CalcUnitPos(DataConfig.TEAM team, int slotIndex)
	{
		BattleGame game = BattleGame.instance;

		float FIELD_WIDTH = game.mapGrid.GetMapWidth ();
		float FIELD_HEIGHT = game.mapGrid.GetMapHeight ();

		float FIELD_MIN_X = MapGrid.GRID_SIZE;
		float FIELD_MAX_X = FIELD_MIN_X + MapGrid.GRID_SIZE * 2;
		float FIELD_MIN_Y = FIELD_HEIGHT / 2 - MapGrid.GRID_SIZE * 2;
		float FIELD_MAX_Y = FIELD_HEIGHT / 2 + MapGrid.GRID_SIZE * 2;

		float A0 = FIELD_MIN_X / FIELD_WIDTH;
		float A1 = FIELD_MAX_X / FIELD_WIDTH;
		float A = A1 - A0;

		float B0 = FIELD_MIN_Y / FIELD_HEIGHT;
		float B1 = FIELD_MAX_Y / FIELD_HEIGHT;
		float B = B1 - B0;


		//
		int col = DataConfig.CalcSlotCol (slotIndex);
		int row = DataConfig.CalcSlotRow (slotIndex);

		float kx = A1 - (float)row / (DataConfig.FORMATION_TOTAL_LINES - 1) * A;
		float ky = B1 - (float)col / (DataConfig.FORMATION_LINE_SLOT - 1) * B;

		Vector3 pos = new Vector3 ();
		pos.z = FIELD_HEIGHT * (ky);

		if (team == DataConfig.TEAM.MY) {
			pos.x = FIELD_WIDTH * (kx);
		} else {
			pos.x = FIELD_WIDTH * (1 - kx);
		}

		return pos;
	}
	*/


	public static void AddUnitToLayer(GameObject obj, BattleConfig.LAYER layer)
	{
		GameObject battleField = GameObject.FindWithTag(AppConfig.TAB_BATTLE_FIELD);
		if (battleField != null) {
			string layerName = BattleConfig.GetLayerName(layer);
			if(layerName != null)
			{
				Transform layerUnits = battleField.transform.Find(layerName);
				obj.transform.parent = layerUnits.transform;
			}
		}
	}


	public static void CreateBackground(string asset)
	{
		string resName = AppConfig.FOLDER_PROFAB + "background/" + asset;
		string resNameTop = resName + "-top";
		string resNameEffect = resName + "-effect";

		Vector3 cameraDir = UnitHelper.GetOrientation (BattleGame.instance.camera.transform);
		VectorHelper.ResizeVector (ref cameraDir, 50);

		GameObject background = ResourceHelper.Load(resName);
		background.transform.position += cameraDir;
		AddUnitToLayer (background, BattleConfig.LAYER.OTHER);

		GameObject backgroundTop = ResourceHelper.Load(resNameTop);
		backgroundTop.transform.position += cameraDir;
		AddUnitToLayer (backgroundTop, BattleConfig.LAYER.OTHER);

		GameObject backgroundEffect = ResourceHelper.Load(resNameEffect);
		backgroundEffect.transform.position += cameraDir;
		AddUnitToLayer (backgroundEffect, BattleConfig.LAYER.OTHER);

	}


	public static void CreateBattleBullet(DataConfig.BULLET_TYPE type, UnitFire.ShootParam shoot)
	{
		string resName = AppConfig.FOLDER_PROFAB + "bullet/bullet" + (int)type;
		
		GameObject obj = ResourceHelper.Load(resName);
		
		AddUnitToLayer (obj, BattleConfig.LAYER.BULLET);
		
		obj.GetComponent<Bullet> ().Create (shoot);
	}

	// ==================================================
	// HUD

	public static void CreateHUDDamageText(UnitFire.ShootParam shoot)
	{
//		GameObject miss = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_EFFECT + "Ctrl");
//		miss.transform.position = shoot.target.transform.position + new Vector3 (0, HINT_TEXT_HEIGHT, 0);

		string text = "-" + Mathf.FloorToInt(shoot.damage);
		Vector3 position = shoot.target.transform.position + new Vector3(0, HINT_TEXT_HEIGHT, 0);
		float scale = shoot.isDoubleDamage ? 1.5f : 1;
		new PopupNumber(text, PopupNumber.COLOR.RED, position, scale);
		
	}

	public static void CreateHUDHealText(GameObject target, float hp)
	{
		/*
		string resName = AppConfig.FOLDER_PROFAB_UI_HUDTEXT + "SimpleHudText";
		GameObject hubTextobj = ResourceHelper.Load(resName);
		
		Transform targetTransform = target.transform.FindChild ("hudtext").transform;
		
		float duration = 1.0f;
		string damageText = "+" +  Mathf.FloorToInt(hp);
		Color color = Color.green;

		HUDTextPlayer hudController = hubTextobj.GetComponent<HUDTextPlayer> ();
		hudController.createHUDText (targetTransform, damageText, color, duration);
		*/

		string s = "+" + Mathf.FloorToInt(hp);
		Vector3 p = target.transform.position + new Vector3(0, HINT_TEXT_HEIGHT, 0);
		new PopupNumber(s, PopupNumber.COLOR.GREEN, p, 1);

		
	}

	public static void CreateHUDMissText(GameObject target)
	{

		/*
		string resName = AppConfig.FOLDER_PROFAB_UI_HUDTEXT + "SimpleHudText";
		GameObject hubTextobj = ResourceHelper.Load(resName);
		
		Transform targetTransform = target.transform.FindChild ("hudtext").transform;
		
		float duration = 1.0f;
		string damageText = "MISS";
		Color color = Color.blue;
		
		HUDTextPlayer hudController = hubTextobj.GetComponent<HUDTextPlayer> ();
		hudController.createHUDText (targetTransform, damageText, color, duration);
		*/

		return;
		GameObject miss = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_EFFECT + "Miss");
		miss.transform.position = target.transform.position + new Vector3 (0, HINT_TEXT_HEIGHT, 0);
		
	}
	


}
