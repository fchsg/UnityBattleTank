using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Camera))]

public class CameraAdjust : Editor {

	//1334x750
	public const float CAMERA_RADIAN = AppConfig.CAMERA_RADIAN;
	public const float CAMERA_DISTANCE = 100;

	public const float DISPLAY_GRIDS_WIDTH = CameraControl.DISPLAY_GRIDS_WIDTH;

	public const float SCREEN_RATIO_WIDTH = 16;
	public const float SCREEN_RATIO_HEIGHT = 9;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		if (GUILayout.Button("Adjust Camera Position")) {
			Camera camera = target as Camera;

			float z = -Mathf.Sin(CAMERA_RADIAN) * CAMERA_DISTANCE;
			float y = Mathf.Cos(CAMERA_RADIAN) * CAMERA_DISTANCE;

			camera.transform.position = new Vector3(0, y, z);
			camera.transform.LookAt(new Vector3(0, 0, 0));
		}

		if (GUILayout.Button("Adjust Camera Range")) {
			Camera camera = target as Camera;

			float k = Mathf.Cos(CAMERA_RADIAN);
			float ratio = SCREEN_RATIO_WIDTH / SCREEN_RATIO_HEIGHT;
			float displayGridHeight = DISPLAY_GRIDS_WIDTH / ratio / k;

			Vector3 p1 = new Vector3 (0, 0, 0);
			Vector3 p2 = new Vector3 (0, 0, displayGridHeight * MapGrid.GRID_SIZE);
			
			p1 = camera.transform.InverseTransformPoint (p1);
			p2 = camera.transform.InverseTransformPoint (p2);
			float height = p2.y - p1.y;
			camera.orthographicSize = height / 2;
		}

		if (GUILayout.Button("Adjust Campaign Camera")) {
			Camera camera = target as Camera;

			camera.transform.position = new Vector3(AppConfig.DESIGN_WIDTH / 2, AppConfig.DESIGN_HEIGHT / 2, -500);
			camera.orthographicSize = AppConfig.DESIGN_HEIGHT / 2;
		}

	}

}
