using UnityEngine;
using System.Collections;

public class UnitHelper {

	/*
	public static GameObject GetTank(AttackQueue.MEMBER member)
	{
		GameObject tank = BattleGame.instance.simulationPlayer.GetTank (member.team, member.slotId);
		return tank;
	}

	public static GameObject[] GetAllTanks()
	{
		return BattleGame.instance.simulationPlayer.allTanks;
	}

	public static GameObject[] GetMyTanks()
	{
		return BattleGame.instance.simulationPlayer.myTanks;
	}

	public static GameObject[] GetEnemyTanks()
	{
		return BattleGame.instance.simulationPlayer.enemyTanks;
	}
	*/	


	public static Vector3 GetOrientation(Transform transform)
	{
		Vector3 orientation = transform.TransformDirection (AppConfig.DEFAULT_DIRECTION);
		return orientation;
	}

	public static bool IsTouchEdge(Unit tank, Vector3 c, float r)
	{
		Vector3 v = tank.transform.position - c;
		if (v.magnitude <= r + tank.unit.dataUnit.GetRadius ()) {
			return true;
		} else {
			return false;
		}
	}
	
}
