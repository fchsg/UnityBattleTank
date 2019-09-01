using UnityEngine;
using System.Collections;

public class DataHeroExp {

	public int level;
	public int[] qualityExp = new int[MAX_QUALITY];

	public const int MAX_QUALITY = 5;

	public void Load(LitJson.JSONNode json)
	{
		level = JsonReader.Int (json, "Lv");
		Assert.assert (level > 0);

		for (int i = 0; i < qualityExp.Length; ++i) {
			qualityExp[i] = JsonReader.Int (json, "Quality_" + (i + 1));
		}

	}

}
