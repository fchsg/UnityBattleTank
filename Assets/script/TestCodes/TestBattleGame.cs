using UnityEngine;
using System.Collections;

public class TestBattleGame : MonoBehaviour {

	void Start () 
	{

		float f1 = BattleGameHelper.GetEnemyTeamAmountHP ();
		float f2 = BattleGameHelper.GetEnemyTeamCurrentHP ();
		float f3 = BattleGameHelper.GetPlayerTeamAmountHP ();
		float f4 = BattleGameHelper.GetPlayerTeamCurrentHP ();
		
	}
	
	void Update()
	{
		float f1 = BattleGameHelper.GetEnemyTeamAmountHP ();
		float f2 = BattleGameHelper.GetEnemyTeamCurrentHP ();
		float f3 = BattleGameHelper.GetPlayerTeamAmountHP ();
		float f4 = BattleGameHelper.GetPlayerTeamCurrentHP ();
		
		Debug.Log ("enemy HP------------------ " + f2 + "     player HP ++++++++++++++ " + f4);
	}
}
