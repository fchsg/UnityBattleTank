using UnityEngine;
using System.Collections;

public class CameraSizeControl : MonoBehaviour {

	public float designAspectWidth = 16;
	public float designAspectHeight = 9;

	private float _expectScreenWidth2;
	public float expectScreenWidth2
	{
		get { return _expectScreenWidth2; }
	}
	

	private float _expectScreenHeight2;
	public float expectScreenHeight2
	{
		get { return _expectScreenHeight2; }
	}

	// Use this for initialization
	void Start () {

		float aspectRatio = (float)Screen.width / Screen.height;

		Camera camera = GetComponent<Camera> ();
		_expectScreenWidth2 = camera.orthographicSize / designAspectHeight * designAspectWidth;
		_expectScreenHeight2 = _expectScreenWidth2 / aspectRatio;
		camera.orthographicSize = _expectScreenHeight2;


		/*
		float screenHeight = Screen.height;
		
		Debug.Log ("screenHeight = " + screenHeight);
		
		//this.GetComponent<Camera>().orthographicSize = screenHeight / 200.0f;
		
		float orthographicSize = this.GetComponent<Camera>().orthographicSize;
		

		float cameraWidth = orthographicSize * 2 * aspectRatio;
		
		Debug.Log ("cameraWidth = " + cameraWidth);
		
		if (cameraWidth < devWidth)
		{
			orthographicSize = devWidth / (2 * aspectRatio);
			Debug.Log ("new orthographicSize = " + orthographicSize);
			this.GetComponent<Camera>().orthographicSize = orthographicSize;
		}
		*/
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
