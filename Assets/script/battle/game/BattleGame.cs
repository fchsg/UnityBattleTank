using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class BattleGame : MonoBehaviour, IEvent {

	private static BattleGame _instance;
	public static BattleGame instance
	{
		get { return _instance; }
	}

	private EventController _eventController = new EventController ();

	public enum STATE
	{
		INIT,
		ENTERING,
		BATTLE,
		OVER,
		UI,
	}
	private STATE _state = STATE.INIT;
	public STATE state
	{
		get { return _state; }
	}


	private GameObject _camera;
	public GameObject camera
	{
		get { return _camera; }
	}

	private UnitGroup _unitGroup;
	public UnitGroup unitGroup
	{
		get { return _unitGroup; }
	}

	private MapGrid _mapGrid;
	public MapGrid mapGrid
	{
		get { return _mapGrid; }
	}

	private MapAStar _mapAStar;
	public MapAStar mapAStar
	{
		get { return _mapAStar; }
	}

	private MapCamera _mapCamera;
	public MapCamera mapCamera
	{
		get { return _mapCamera; }
	}

	private BattleGameEntering _gameEntering;

	private MouseControl _mouseContorl;
	public MouseControl mouseContorl
	{
		get { return _mouseContorl; }
	}

	private MouseStatus _mouseStatus = new MouseStatus ();
	public MouseStatus mouseStatus
	{
		get { return _mouseStatus; }
	}

	private GameSkill _gameSkill;
	public GameSkill gameSkill
	{
		get { return _gameSkill; }
	}

	private GameSkillControl _gameSkillControl;

	private int _battleSpeed = 1;
	public int battleSpeed
	{
		set { _battleSpeed = value; }
		get { return _battleSpeed; }
	}

	private bool _isPause = false;
	private bool _isOver = false;


	private int _gameTick = 0;
	public int gameTick
	{
		get { return _gameTick; }
	}

	private long _lastSychronizeBattleProgressTimestamp = 0;
	private const float SYCHRONIZE_BATTLE_PROGRESS_SECONDS = 5;

	private GameEvaluate _evaluation = new GameEvaluate ();
	public GameEvaluate evaluation
	{
		get { return _evaluation; }
	}

	private long _battleStartTimeStamp = 0;
	public long battleStartTimeStamp
	{
		get
		{
			Assert.assert (_battleStartTimeStamp > 0);
			return _battleStartTimeStamp;
		}
	}



	public enum RESULT
	{
		UNKNOWN,
		WIN,
		LOSS,
		NOTIME,
	}
	public RESULT battleResult = RESULT.UNKNOWN;

	public float battleUsedSeconds = 0;
	public const float BATTLE_DURATION_SEC = 90;


	// Use this for initialization
	void Start () {
		Assert.assert (_instance == null);
		_instance = this;

		if (AppConfig.DEBUGGING) {
			new MyTest();
		}

		CreateBattleData(); // SceneBattle

		_camera = GameObject.FindGameObjectWithTag (AppConfig.TAB_MAIN_CAMERA);

		//todo, use define in mission
		_unitGroup = new UnitGroup ();
		_mapGrid = new MapGrid (InstancePlayer.instance.battle.dataMap);
		_mapAStar = new MapAStar (_mapGrid);
		_mapCamera = new MapCamera ();

		_gameEntering = new BattleGameEntering (this);

		_mouseContorl = new MouseControl (this);

		_gameSkill = new GameSkill ();
		_gameSkillControl = new GameSkillControl (this);

//		MapAStar.PATH path = _mapAStar.Calc (5, 2, 15, 2);
//		_mapGrid.AddPath (path);

		AudioGroup.Play (GetComponent<AudioGroup> ().music, _camera, AudioGroup.TYPE.MUSIC);

		UnitTrackControl.ClearTrackTiles ();

		BattleGameHelper.PreloadAssets ();

	}


	private void CreateBattle(DataMission mission)
	{
		//CreateTestPlayerTeam ();

		Model_Formation model_Formation = InstancePlayer.instance.model_User.model_Formation;
		if (GameOffine.START_OF_OFFLINE || !InstancePlayer.instance.model_User.isLogin) 
		{
			int batteUnitCount = model_Formation.GetCurrentTeamUnitCount ();
			if (batteUnitCount <= 0) 
			{
				model_Formation.CreateLocalData();  //创建本地测试Team1数据
			}

			if (false) {
				CreateTestPvp ();
			}

		}
		model_Formation.CreatePlayerArmy();

		InstancePlayer.instance.battle = new InstanceBattle ();
		InstancePlayer.instance.battle.ImportFromPlayer ();
		if (InstancePlayer.instance.pvpUser != null) {
			InstancePlayer.instance.battle.ImportFromPvp (InstancePlayer.instance.pvpUser);
		} else {
			InstancePlayer.instance.battle.ImportFromLevel (mission);
		}
		InstancePlayer.instance.battle.ImportMap ();

	}

	private void CreateTestPvp()
	{
		SlgPB.PVPUser pvp = new SlgPB.PVPUser ();

		SlgPB.UnitGroup unitGroup = new SlgPB.UnitGroup ();
		pvp.unitGroups.Add (unitGroup);
		unitGroup.posId = 1;
		unitGroup.unitId = 1;
		unitGroup.num = 10;
		unitGroup.heroId = 2;

		SlgPB.Hero pbHero = new SlgPB.Hero ();
		pvp.heroes.Add (pbHero);
		pbHero.heroId = 2;
		pbHero.exp = 100;
		pbHero.stage = 1;

		SlgPB.Unit pbUnit = new SlgPB.Unit ();
		pvp.units.Add (pbUnit);
		pbUnit.unitId = 1;
		pbUnit.unitPartLevel.Add (1);
		pbUnit.unitPartLevel.Add (1);
		pbUnit.unitPartLevel.Add (1);
		pbUnit.unitPartLevel.Add (1);

		InstancePlayer.instance.pvpUser = pvp;
	}

	/*
	private void CreateTestPlayerTeam()
	{
		//add player test data
		InstancePlayer.instance.playerArmy = new InstancePlayerArmy ();
		InstancePlayer.instance.playerArmy.memberCount = 6;  // test player member conunt
		int memberCount = InstancePlayer.instance.playerArmy.memberCount;
		
		InstancePlayer.instance.playerArmy.unitId = new int[memberCount];
		InstancePlayer.instance.playerArmy.unitCount = new int[memberCount];
		InstancePlayer.instance.playerArmy.heroId = new int[memberCount];
		
		for (int i = 0; i < memberCount; i++) 
		{
			InstancePlayer.instance.playerArmy.unitId [i] = (int)RandomHelper.Range(1, 7);
			InstancePlayer.instance.playerArmy.unitCount [i] = 5;
			InstancePlayer.instance.playerArmy.heroId [i] = 0;
		}
	}
	*/

	private void CreateBattleData()
	{

		DataManager.instance.InitData ();

		int battleId = InstancePlayer.instance.missionMagicId;
//		DataConfig.MISSION_DIFFICULTY battleDifficulty = InstancePlayer.instance.battleDifficulty;
		DataMission mission = DataManager.instance.dataMissionGroup.GetMission (battleId);
		CreateBattle (mission);

	}

	void OnDestroy() {
		Assert.assert (_instance == this);
		_instance = null;

		TimeHelper.SetTimeScale(1);

	}
	
	
	// Update is called once per frame
	void Update () {
		//ensure game is the first instance in scane, and call this at the begin of update
		TimeHelper.SynchronizeTimestampScaled ();

		if (_state == STATE.UI) {
			TimeHelper.SetTimeScale(1);
		} else {
			TimeHelper.SetTimeScale(_battleSpeed);
		}

		//
		if (!_isPause) {
			_mouseStatus.Update ();

			switch (state) {

			case STATE.INIT:
				UpdateInit();
				break;

			case STATE.ENTERING:
				UpdateEntering();
				break;

			case STATE.BATTLE:
				UpdateBattle();
				break;


			case STATE.OVER:
				UpdateOver();
				break;

			}

			_mouseContorl.Update ();
			_mapCamera.Update ();
			_mapGrid.UpdateShowTile ();
			_gameSkillControl.Update ();

			SychronizeBattleProgress ();

			++_gameTick;
		}

	}

	public void PauseGame()
	{
		if (!_isPause) {
			_isPause = true;

		}

	}

	private void SychronizeBattleProgress()
	{
		if (!_isOver) {
			long timestamp = TimeHelper.GetCurrentTimestampScaled ();
			if (_lastSychronizeBattleProgressTimestamp <= 0) {
				_lastSychronizeBattleProgressTimestamp = timestamp;
			}

			if (timestamp - _lastSychronizeBattleProgressTimestamp >= SYCHRONIZE_BATTLE_PROGRESS_SECONDS * 1000) {
				_lastSychronizeBattleProgressTimestamp = timestamp;

				BattleConnection.instance.SendBattleProgress ();
			}
		}
	}


	private void SwitchState(STATE state)
	{
//		Trace.trace ("game enter state = " + state, Trace.CHANNEL.FIGHTING);
		_state = state;
	}

	public void UpdateInit()
	{
		GameObject[][] tanks = BattleFactory.CreateBattleUnits (InstancePlayer.instance.battle,
		                                                        InstancePlayer.instance.battle.dataMap);
		_unitGroup.AddUnit (tanks);

		BattleFactory.CreateBackground (InstancePlayer.instance.battle.mission.asset);



		_gameEntering.Start ();

		SwitchState(STATE.ENTERING);

	}

	public void UpdateEntering()
	{
		_gameEntering.Update ();
		if (_gameEntering.state == BattleGameEntering.STATE.OVER) {

			List<Unit> units = unitGroup.allUnits;
			foreach (Unit unit in units) {
				unit.StartFight();
			}

			float avarageSpeed = 0;
			List<Unit> myUnits = _unitGroup.myUnits;
			foreach(Unit unit in myUnits)
			{
				avarageSpeed += unit.unit.dataUnit.speed;
			}
			avarageSpeed /= myUnits.Count;

			//			UnitLayout layout = CameraHelper.CalcAllAliveTanksLayout ();
			Vector3 targetCenter = new Vector3(
				_mapGrid.GetMapWidth() / 2,// - MapGrid.GRID_SIZE,
				0,
				_mapGrid.GetMapHeight() / 2);
			_mapCamera.cSmoothMove.Init(_mapCamera.cameraControl.cameraCenter, targetCenter, 10 / avarageSpeed);
//			_mapCamera.cSmoothMove.Init(_mapCamera.cLookAt.cameraCenter, targetCenter, 10 / avarageSpeed);
			_mapCamera.SetState (MapCamera.STATE.SMOOTH_MOVE);

			_battleStartTimeStamp = TimeHelper.GetCurrentTimestampScaled ();

			SwitchState(STATE.BATTLE);
		}

	}


	public void UpdateBattle()
	{
		long battleEndTimeStamp = TimeHelper.GetCurrentTimestampScaled ();
		float battleUsedSeconds = (float)(battleEndTimeStamp - battleStartTimeStamp) / 1000.0f;
		battleUsedSeconds = battleUsedSeconds;

		if (InstancePlayer.instance.battle.IsPlayerWin ()) {
			battleResult = RESULT.WIN;

			AudioGroup.Play (GetComponent<AudioGroup> ().win, _camera, AudioGroup.TYPE.MUSIC);

		}
		else if (InstancePlayer.instance.battle.IsPlayerLoss ()) {
			battleResult = RESULT.LOSS;

			AudioGroup.Play (GetComponent<AudioGroup> ().loss, _camera, AudioGroup.TYPE.MUSIC);
		}
		else if (battleUsedSeconds >= 1000 * BATTLE_DURATION_SEC) {
			battleResult = RESULT.NOTIME;

			AudioGroup.Play (GetComponent<AudioGroup> ().loss, _camera, AudioGroup.TYPE.MUSIC);
		}

		if (battleResult != RESULT.UNKNOWN) {

			SwitchState(STATE.OVER);

			if(battleResult != RESULT.NOTIME)
			{
				List<Unit> units = _unitGroup.allUnits;
				foreach (Unit unit in units) {
					if(!unit.isDead)
					{
						unit.WinAndExit();
					}
				}
			}

		}
	}


	private float _toOverDelay = 5;
	public void UpdateOver()
	{
		_toOverDelay -= TimeHelper.deltaTime;
		if (_toOverDelay < 0 && !_isOver)
		{
			_isOver = true;

			if(InstancePlayer.instance.pvpUser == null)
			{
				_evaluation.EvaluateStar ();
				BattleConnection.instance.EndFight();
			}
			else
			{
				BattleConnection.instance.EndPvpFight();
			
			}

			_state = STATE.UI;

		}
	}



	/*
	IEnumerator TransitionScene()
	{
		yield return new WaitForSeconds(3);
		//Object persistentObj = GameObject.Find ("BattleDebug") as Object;
		//DontDestroyOnLoad (persistentObj);
		Application.LoadLevel ("SceneBattleDebug");
	}
	*/



	public void DispatchEvent (GameEvent e)
	{
		_eventController.Dispatch (e);
	}



}
