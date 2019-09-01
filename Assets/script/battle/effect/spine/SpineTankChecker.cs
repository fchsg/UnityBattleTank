using UnityEngine;
using System.Collections;

public class SpineTankChecker : MonoBehaviour {

	public int bodyLevel = 90;
	public bool bodyFlip = false;
	public bool cannonFlip = false;

	private string spineName;
	private SkeletonAnimation _skeletonAnim;

	private bool initialized = false;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (!initialized) {
			initialized = true;

			_skeletonAnim = GetComponentInChildren<SkeletonAnimation> ();
			
			int index = _skeletonAnim.skeletonDataAsset.name.IndexOf("_SkeletonData");
			spineName = _skeletonAnim.skeletonDataAsset.name.Substring (0, index);
			
			GetCannonLevel ();
			ShowCorrectImage ();
			AdjustMouse ();

		}

	}
	
	private int cannonLevel = 0;
	private void GetCannonLevel()
	{
		string animName = _skeletonAnim.AnimationName;
		if (animName == null) {
			return;
		}

		cannonLevel = 90;
		if (animName.IndexOf ("60") >= 0) {
			cannonLevel = 60;
		}
		if (animName.IndexOf ("70") >= 0) {
			cannonLevel = 70;
		}
		if (animName.IndexOf ("80") >= 0) {
			cannonLevel = 80;
		}
		if (animName.IndexOf ("90") >= 0) {
			cannonLevel = 90;
		}
		if (animName.IndexOf ("100") >= 0) {
			cannonLevel = 100;
		}
		if (animName.IndexOf ("110") >= 0) {
			cannonLevel = 110;
		}
		if (animName.IndexOf ("120") >= 0) {
			cannonLevel = 120;
		}
	}

	private void ShowCorrectImage()
	{
		string bodyName = spineName + "_Base_" + bodyLevel;
		string cannonName = spineName + "_Barrel_" + cannonLevel;
		
		foreach (Spine.Slot slot in _skeletonAnim.skeleton.slots) {
			bool show = false;
			
			if(slot.Data.Name == cannonName)
			{
				show = true;
			}
			
			if(slot.Data.Name == bodyName)
			{
				show = true;
				slot.Bone.FlipX = bodyFlip ^ cannonFlip;
			}
			
			if(show)
			{
				slot.A = 1;
			}
			else
			{
				slot.A = 0;
			}
			
		}

		Vector3 s = transform.localScale;
		s.x = Mathf.Abs(s.x);
		if (cannonFlip) {
			transform.localScale = new Vector3(-s.x, s.y, s.z);
		} else {
			transform.localScale = new Vector3(s.x, s.y, s.z);
		}


	}

	private void AdjustMouse()
	{
		string boneName = "mouse_" + cannonLevel;
		Spine.Bone boneMouse = _skeletonAnim.Skeleton.FindBone (boneName);
		
		Vector3 bPoint = new Vector3 (boneMouse.WorldX, boneMouse.WorldY, 0);
		Vector3 wPoint = transform.TransformPoint(bPoint);
		
		float height = 2.4f;
		
		CameraControl cameraControl = GameObject.Find ("Main Camera").GetComponent<CameraControl> ();
		Vector3 position = GeometryHelper.ProjectPointToPlane(wPoint, height, cameraControl.orientation);
		
		GameObject.Find ("Mouse").transform.position = wPoint;

	}

	
}
