using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEvaluate {


	public class EVALUATION
	{
		public int[] starRateIds = new int[3];
		public bool[] star = new bool[3];

		public int starMask
		{
			get {
				int mask = 0;
				for (int i = 0; i < star.Length; ++i) {
					if (star [i]) {
						mask |= (1 << i);
					}
				}
				return mask;
			}
		}

	}

	public void EvaluateStar()
	{
		GameEvaluate.EVALUATION evaluate = new GameEvaluate.EVALUATION ();
		InstancePlayer.instance.battleEvaluation = evaluate;

		evaluate.starRateIds [0] = (int)DataStarRate.COMMAND.PassMission;
		ArrayHelper.Fill<int> (
			InstancePlayer.instance.battle.mission.RateIds,
			ref evaluate.starRateIds,
			1,
			InstancePlayer.instance.battle.mission.RateIds.Length);

		/*
		bool win = battle.IsPlayerWin ();
		if (win) {
			evaluate.star [0] = true;
		}
		*/

		for (int i = 0; i < 3; ++i) {
			evaluate.star [i] = Evaulate (evaluate.starRateIds [i]);
		}

	}

	private bool Evaulate(int rateId)
	{
		if (rateId == 0) {
			return true;
		}

		DataStarRate rate = DataManager.instance.dataStarRateGroup.GetStarRate (rateId);
		switch (rate.command) {
		case DataStarRate.COMMAND.PassMission:
			return Evaulate_PassMission (rate);

		case DataStarRate.COMMAND.BattleDamage:
			return Evaulate_BattleDamage (rate);

		case DataStarRate.COMMAND.BattleTime:
			return Evaulate_BattleTime (rate);

		}

		return true;
	}

	private bool Evaulate_PassMission(DataStarRate rate)
	{
		bool win = InstancePlayer.instance.battle.IsPlayerWin ();
		return win;
	}

	private bool Evaulate_BattleDamage(DataStarRate rate)
	{
		bool win = InstancePlayer.instance.battle.IsPlayerWin ();
		if (win) {
			float ratio = rate.A;
			float deadRatio = BattleGame.instance.unitGroup.GetPlayerDeadUnitsRatio ();

			return deadRatio <= ratio;
		}

		return false;
	}

	private bool Evaulate_BattleTime(DataStarRate rate)
	{
		bool win = InstancePlayer.instance.battle.IsPlayerWin ();
		if (win) {
			float sec = rate.A;
			float usedSec = BattleGame.instance.battleUsedSeconds;

			return usedSec <= sec;
		}

		return false;
	}

}
