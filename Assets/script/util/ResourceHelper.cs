using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceHelper {

	public static GameObject Load(string name)
	{
		Object profab  = Resources.Load(name);

		Assert.assert (profab != null, "wrong prefab path:" + name);

		GameObject obj = GameObject.Instantiate(profab) as GameObject;
		return obj;
	}

}
