using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private Camera _camera;
	public Camera camera
	{
		get { return _camera; }
	}

	private CameraSizeControl _sizeControl;
	public CameraSizeControl sizeControl
	{
		get { return _sizeControl; }
	}


	private Vector3 _orientation;
	public Vector3 orientation
	{
		get { return _orientation; }
	}

	private Vector3 _initialPosition;

	private Vector3 _cameraCenter;
	public Vector3 cameraCenter
	{
		get { return _cameraCenter; }
	}

	private float _shakeLeftSec = 0;
	private Vector3 _shakeOffset = Vector3.zero;
	private const float SHAKE_WAVE = 2;
	private const float SHAKE_DURATION = 0.75f;

	public const float DISPLAY_GRIDS_WIDTH = 7;

	public const float VERTICAL_FLOATING_RANGE = MapGrid.GRID_SIZE * 2;
	public const float VERTICAL_FLOATING_RANGE2 = VERTICAL_FLOATING_RANGE / 2;


	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();
		_sizeControl = GetComponent<CameraSizeControl> ();

		GetOrientation ();
		_initialPosition = transform.position;

//		AdjustCameraToFullScreen ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateShake ();
	}

	public void Shake()
	{
		_shakeLeftSec = Mathf.Max(_shakeLeftSec, SHAKE_DURATION);
	}
	
	private void UpdateShake()
	{
		transform.Translate (-_shakeOffset);
		_shakeOffset = Vector3.zero;
		
		_shakeLeftSec -= TimeHelper.deltaTime;
		if (_shakeLeftSec > 0)
		{
			float wave = Mathf.Pow(_shakeLeftSec / SHAKE_DURATION, 1.2f) * SHAKE_WAVE;
			float rx = RandomHelper.Range(-wave, wave);
			float ry = RandomHelper.Range(-wave, wave);
			_shakeOffset = new Vector3(rx, ry, 0);
			_shakeOffset = transform.TransformDirection(_shakeOffset);
			transform.Translate(_shakeOffset);
		}

	}

	/*
	public void LookAtRange(MapFieldHelper.Range range)
	{
		transform.LookAt (range.center);

		float scale = range.width / 2 / sizeControl.expectScreenWidth2;
		float height2 = sizeControl.expectScreenHeight2 * scale;
		camera.orthographicSize = height2;

	}
	*/


	private void GetOrientation()
	{
		float dist = transform.position.magnitude;
		_orientation = UnitHelper.GetOrientation (transform);
		VectorHelper.ResizeVector (ref _orientation, dist);

	}


	/*
	private void AdjustCameraToFullScreen()
	{
		Vector3 p1 = new Vector3 (0, -MapGrid.GRID_SIZE, 0);
		Vector3 p2 = new Vector3 (0, MapGrid.GetMapHeight() / 2 + MapGrid.GRID_SIZE, 0);

		p1 = transform.TransformPoint (p1);
		p2 = transform.TransformPoint (p2);
		float height = p2.y - p1.y;
		camera.orthographicSize = height / 2;
	}
	*/


	public void FollowPoint(Vector3 center)
	{
		_cameraCenter = LimitCameraCenter (center);

		Vector3 p = _cameraCenter - _orientation;
		transform.position = p;
		transform.LookAt (_cameraCenter);
	}

	public Vector3 LimitCameraCenter(Vector3 center)
	{
		BattleGame game = BattleGame.instance;
		
		float expectW2 = _sizeControl.expectScreenWidth2;
		float expectH2 = _sizeControl.expectScreenHeight2;
		
		float centerMinX = expectW2;
		float centerMaxX = game.mapGrid.GetMapWidth () - expectW2;
		float centerMinZ = game.mapGrid.GetMapHeight () / 2 - VERTICAL_FLOATING_RANGE2;
		float centerMaxZ = game.mapGrid.GetMapHeight () / 2 + VERTICAL_FLOATING_RANGE2;
		
		
		center.x = Mathf.Max (center.x, centerMinX);
		center.x = Mathf.Min (center.x, centerMaxX);
		center.z = Mathf.Max (center.z, centerMinZ);
		center.z = Mathf.Min (center.z, centerMaxZ);
		return center;
	}

	public bool IsPointInsideCamera(Vector3 point)
	{
		float dx = Mathf.Abs (point.x - _cameraCenter.x);
		return dx < MapGrid.GRID_SIZE * DISPLAY_GRIDS_WIDTH / 2;

	}

	public bool ProjectScreenPointToPlane(out Vector3 worldPoint, Vector3 screenPoint, float height)
	{
		bool result;
		float depth;
		
		Plane plane = new Plane (Vector3.up, new Vector3 (0, height, 0));
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
