using UnityEngine;
using System.Collections;

public class TestInitData : MonoBehaviour {

	void Awake()
	{
		DataManager.instance.InitData ();
	}
}
