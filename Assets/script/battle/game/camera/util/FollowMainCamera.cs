using UnityEngine;
using System.Collections;

public class FollowMainCamera : MonoBehaviour {

	public GameObject followingMainCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Camera mainCamera = followingMainCamera.GetComponent<Camera> ();
		Camera camera = GetComponent<Camera> ();

		camera.transform.position = mainCamera.transform.position;
		camera.transform.rotation = mainCamera.transform.rotation;
		camera.transform.localScale = mainCamera.transform.localScale;
		camera.orthographic = mainCamera.orthographic;
		camera.orthographicSize = mainCamera.orthographicSize;

	}

}
