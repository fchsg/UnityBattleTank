using UnityEngine;
using System.Collections;

public class TestEffectCreate : MonoBehaviour {

	public int count = 1;
	public GameObject[] primitives;

	private float fps = 0;
	private float _fps = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			foreach(GameObject primitive in primitives)
			{
				GameObject.Instantiate(primitive);
			}
		}

		fps = 1 / Time.deltaTime;
		_fps = (_fps * 99 + fps * 1) / 100;


	}

	void OnGUI() {
		string msg = "fps = " + fps;
		msg += ", ~fps = " + _fps;

		GUI.Label (new Rect (1, 1, 200, 20), msg);

	}
}
