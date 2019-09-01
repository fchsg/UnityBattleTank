using UnityEngine;
using System.Collections;

public class DataTeam {
	public int id;

	public int memberCount;
	public int[] unitId;
	public int[] unitCount;

	public void Load(LitJson.JSONNode json)
	{
		id = int.Parse(json["ID"]);

		string str_unitId = json["EnemiesID"];
		unitId = StringHelper.ReadIntArrayFromString (str_unitId);
		
		string str_unitCount = json["EnemiesCount"];
		unitCount = StringHelper.ReadIntArrayFromString (str_unitCount);
		
		Assert.assert (unitId.Length == unitCount.Length);
		memberCount = unitId.Length;
		Assert.assert (memberCount > 0);

	}

}
