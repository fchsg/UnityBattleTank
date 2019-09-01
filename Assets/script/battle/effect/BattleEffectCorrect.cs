using UnityEngine;

public class BattleEffectCorrect : MonoBehaviour {

	public float imageWidth = 1334 * 2 + 20;
	public float imageHeight = 1000 + 20;

	// Use this for initialization
	void Start () {
		AdjustPosition ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void AdjustPosition()
	{
		float ox = (imageWidth - AppConfig.DESIGN_WIDTH * 2) / 2;
		float oy = (imageHeight - AppConfig.DESIGN_HEIGHT) / 2;

		float scale = CameraControl.DISPLAY_GRIDS_WIDTH * MapGrid.GRID_SIZE / AppConfig.DESIGN_WIDTH;
//		Vector3 pos = new Vector3 (-ox * scale, 0, -oy * scale);
//
//		Quaternion q = new Quaternion ();
//		q.eulerAngles = new Vector3(-AppConfig.CAMERA_DEGREE, 0, 0);

		transform.Translate (-ox * scale, 0, -oy * scale);
//		transform.Rotate (-AppConfig.CAMERA_DEGREE, 0, 0);
//		transform.localPosition = pos;
//		transform.localRotation = q;

	}

}
