using UnityEngine;
using System.Collections;

public class DataStarRate {


	public enum COMMAND
	{
		UNKNOWN,
		PassMission,
		BattleDamage,
		BattleTime,
	}

	public int id;

	public COMMAND command;
	public float A;
	public float B;

	public string descript;


	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int(json, "ID");

		string cmdStr = json["Command"];
		command = GetCmdCode (cmdStr);

		A = JsonReader.Float(json, "A");
		B = JsonReader.Float(json, "B");

		descript = json["Descript"];

	}

	private static COMMAND GetCmdCode(string cmd)
	{
		if (cmd == "PassMission") {
			return COMMAND.PassMission;
		}
		if (cmd == "BattleDamage") {
			return COMMAND.BattleDamage;
		}
		if (cmd == "BattleTime") {
			return COMMAND.BattleTime;
		}

		Assert.assert (false);
		return COMMAND.UNKNOWN;
	}


}
