using UnityEngine;
using System.Collections;

public class CampaignCamera : MonoBehaviour {

	private Camera _camera;
	public Camera camera
	{
		get { return _camera; }
	}


	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public bool ProjectScreenPointToPlane(out Vector3 worldPoint, Vector3 screenPoint)
	{
		bool result;
		float depth;
		
		Plane plane = new Plane (Vector3.back, new Vector3 (0, 0, 0));
		Ray ray = _camera.ScreenPointToRay (screenPoint);
		
		if (plane.Raycast (ray, out depth)) {
			worldPoint = ray.origin + ray.direction * depth;
			result = true;
		} else {
			worldPoint = Vector3.zero;
			result = false;
		}
		
		return result;
		
	}

}
