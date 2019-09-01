using UnityEngine;
using System.Collections;

public class SpineBillboardCorrect : MonoBehaviour {

	private SkeletonAnimation skinAnim;
	private Camera camera;

	// Use this for initialization
	void Start () {
		skinAnim = gameObject.GetComponent<SkeletonAnimation> ();
		camera = Camera.main;

		AlignWithCamera ();
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 position = skinAnim.transform.position;
//		skinAnim.transform.position = position;
	}

	private void AlignWithCamera()
	{
		skinAnim.transform.rotation = camera.transform.rotation;
	}

}
