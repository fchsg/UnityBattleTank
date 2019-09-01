using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class InstancePlayer {

	private static InstancePlayer _instance = null;
	public static InstancePlayer instance
	{
		get
		{
			if (_instance == null) {
				_instance = new InstancePlayer();
				_instance.Init();
			}
			return _instance;
		}
	}

	// =====================================================
	// field

	public string ticket = null;
	
	
	private Model_User _model_User;
	public Model_User model_User
	{
		get { return _model_User; }
	}
	
	public InstancePlayerArmy playerArmy; //units in format

	private UIDataStatus _uiDataStatus;
	public UIDataStatus uiDataStatus
	{
		get {  return _uiDataStatus; }
	}


	public void Init()
	{
		_model_User = new Model_User();

		_uiDataStatus = new UIDataStatus();

		CreateOfflineData ();
	}

	// =====================================================
	// offline data
	
	private void CreateOfflineData()
	{

	}

	public static bool IsOffLine()
	{
		return instance.ticket != null;
	}

	// =====================================================
	// shared battle data

	public int _currentFightId;
	public int currentFightId
	{
		get { return _currentFightId; }
		set {
			_currentFightId = value;
			_currentPvpFightId = 0;
		}
	}

	public int _currentPvpFightId;
	public int currentPvpFightId
	{
		get { return _currentPvpFightId; }
		set {
			_currentPvpFightId = value;
			_currentFightId = 0;
		}
	}

	private int _missionMagicId = AppConfig.FIRST_MISSION_MAGICID;
	public int missionMagicId
	{
		get { return _missionMagicId; }
		set
		{
			_missionMagicId = value;
			_pvpUser = null;
		}
	}




	public InstanceBattle battle = null;
	public GameEvaluate.EVALUATION battleEvaluation = null;
	public List<SlgPB.PrizeItem> battleGotPrizeItems = new List<PrizeItem> ();
	public List<Model_HeroGroup.ExpChangeResult> battleHeroGotExp = new List<Model_HeroGroup.ExpChangeResult>();
	public List<List<Model_HeroGroup.ExpChangeResult>> multiFightHeroGotExp = new List<List<Model_HeroGroup.ExpChangeResult>>();

	private SlgPB.PVPUser _pvpUser = null;
	public SlgPB.PVPUser pvpUser
	{
		get { return _pvpUser; }
		set {
			_pvpUser = value;
			_missionMagicId = 0;
		}
	}


	public List<SlgPB.MultiPrizeItem> multiFightPrizeItems = new List<MultiPrizeItem>();



}
