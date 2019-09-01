using UnityEngine;
using System.Collections;

public class DataLeader {
	public int level;
	public int honor;
	public int Leadership;
	public int Energy;

	public void Load(LitJson.JSONNode json)
	{
		level = int.Parse(json["Lv"]);
		honor = int.Parse(json["Honor"]);
		Leadership = int.Parse(json["Leadership"]);
		Energy = int.Parse(json["Energy"]);

	}

}
