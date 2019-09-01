using UnityEngine;
using System.Collections;

public class TestEffect : MonoBehaviour {
	
	public GameObject plane;


	void Start () {
	//	TestAnimation ();

	//	TestParticle ();

		TestUIParticle ();
	}	

	void Update () 
	{
	}


	void TestAnimation()
	{
		Object prefab =   Resources.Load (AppConfig.FOLDER_PROFAB_EFFECT_EXPLODE + "test/TestAnimation");
		Object.Instantiate (prefab);

	}

	void TestParticle()
	{
		Object prefab =   Resources.Load (AppConfig.FOLDER_PROFAB_EFFECT_EXPLODE + "test/ExplodeParticle");
		Object.Instantiate (prefab);
	}

	void TestUIParticle()
	{
		GameObject go = ResourceHelper.Load ("profab/ui/effect/Rare03");
	}
}
