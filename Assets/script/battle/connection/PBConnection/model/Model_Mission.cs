using UnityEngine;
using System.Collections;

public class Model_Mission {

	public int magicId;
	public DataConfig.MISSION_DIFFICULTY difficulty;
	public int stageId;
	public int missionId;

	private int _remainFightNum = 0;
	public int remainFightNum
	{
		get { return _remainFightNum; }
		set 
		{
			_remainFightNum = value;
			Assert.assert (value >= 0);
		}
	}

	public int starMask = 0;

	private DataMission _referenceMission;
	public DataMission referenceMission
	{
		get { return _referenceMission; }
	}

	public bool actived = false;


	public const int FIGHT_NUM_NORMAL = 50;
	public const int FIGHT_NUM_ELITE = 20;

	/*
	public Model_Mission(DataConfig.MISSION_DIFFICULTY difficulty, int stageId, int missionId)
	{
		magicId = (int)(difficulty + 1) * 10000 + stageId * 100 + missionId;

		this.difficulty = difficulty;
		this.stageId = stageId;
		this.missionId = missionId;

		InitRemainFightNum ();
	}
	*/

	public Model_Mission(int magicId)
	{
		this.magicId = magicId;

		difficulty = DataMission.GetDifficulty (magicId);
		stageId = DataMission.GetStageId (magicId);
		missionId = DataMission.GetMissionId (magicId);

		InitRemainFightNum ();

		_referenceMission = DataManager.instance.dataMissionGroup.GetMission (magicId);
		Assert.assert (_referenceMission != null);
	}

	private void InitRemainFightNum()
	{
		if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
			_remainFightNum = FIGHT_NUM_NORMAL;
		} else {
			_remainFightNum = FIGHT_NUM_ELITE;
		}

	}


	public void Parse(SlgPB.Mission m)
	{
		starMask = m.star;
		_remainFightNum = m.remainFightNum;

		actived = true;

	}

//	public void RemoveStar(int star)
//	{
//		starMask &= !star;
//	}

	// =========================================================
	// star

	public int starCount
	{
		get {
			int c = 0;
			
			int mask = starMask;
			for(int i = 0; i < 3; ++i)
			{
				if((mask & 1) != 0)
				{
					++c;
				}
				mask >>= 1;
			}
			
			return c;
		}
	}
	
	public bool HasStar(int index)
	{
		int mask = (starMask >> index) & 1;
		return mask != 0;
	}
	
	public int GetStarMask(int index)
	{
		return 1 << index;
	}
	
	public void SetStar(int star)
	{
		starMask |= star;
	}



}
