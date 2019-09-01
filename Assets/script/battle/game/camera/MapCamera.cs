using UnityEngine;
using System.Collections;

public class MapCamera {

	private GameObject _camera;

	private CameraControl _cameraControl;
	public CameraControl cameraControl
	{
		get { return _cameraControl; }
	}

	private CameraAutoFollow _cAutoFollow;
	public CameraAutoFollow cAutoFollow
	{
		get { return _cAutoFollow; }
	}

	private CameraAutoScale _cAutoScale;
	public CameraAutoScale cAutoScale
	{
		get { return _cAutoScale; }
	}

	private CameraSmoothMove _cSmoothMove;
	public CameraSmoothMove cSmoothMove
	{
		get { return _cSmoothMove; }
	}

	private CameraShowTeam _cShowTeam;
	public CameraShowTeam cShowTeam
	{
		get { return _cShowTeam; }
	}

	private CameraLookAt _cLookAt;
	public CameraLookAt cLookAt
	{
		get { return _cLookAt; }
	}

	private CameraManualMove _cManualMove;
	public CameraManualMove cManualMove
	{
		get { return _cManualMove; }
	}

	private Vector3 _cameraCenter;
	public Vector3 cameraCenter
	{
		get { return _cameraCenter; }
	}

	public enum STATE
	{
		FREE,
		AUTO_FOLLOW,
		AUTO_SCALE,
		SMOOTH_MOVE,
		SHOW_TEAM,
		LOOK_AT,
		MANUAL_MOVE
	}
	private STATE _state = STATE.FREE;
	public STATE state
	{
		get { return _state; }
	}

	public MapCamera()
	{
		_camera = GameObject.FindGameObjectWithTag (AppConfig.TAB_MAIN_CAMERA);
		_cameraControl = _camera.GetComponent<CameraControl> ();
		
		_cAutoFollow = new CameraAutoFollow (this);
		_cAutoScale = new CameraAutoScale (this);
		_cSmoothMove = new CameraSmoothMove (this);
		_cShowTeam = new CameraShowTeam (this);
		_cLookAt = new CameraLookAt (this);
		_cManualMove = new CameraManualMove (this);

	}

	// Update is called once per frame
	public void Update () {
	
		switch (_state) {
		case STATE.FREE:
			UpdateFree();
			break;
		case STATE.AUTO_FOLLOW:
			UpdateAutoFollow();
			break;
		case STATE.AUTO_SCALE:
			UpdateAutoScale();
			break;
		case STATE.SMOOTH_MOVE:
			UpdateSmoothMove();
			break;
		case STATE.SHOW_TEAM:
			UpdateShowTeam();
			break;
		case STATE.LOOK_AT:
			UpdateLookAt();
			break;
		case STATE.MANUAL_MOVE:
			UpdateManualMove();
			break;
		}
	}

	public void SetState(STATE s)
	{
		_state = s;
	}

	private void UpdateFree()
	{
	}

	private void UpdateAutoFollow()
	{
		_cAutoFollow.Update ();
		_cameraCenter = _cAutoFollow.cameraCenter;
	}

	private void UpdateAutoScale()
	{
		_cAutoScale.Update ();
		_cameraCenter = _cAutoScale.cameraCenter;
	}
	
	private void UpdateSmoothMove()
	{
		_cSmoothMove.Update ();
		_cameraCenter = _cSmoothMove.cameraCenter;
	}

	private void UpdateShowTeam()
	{
		_cShowTeam.Update ();
		_cameraCenter = _cShowTeam.cameraCenter;
	}
	
	private void UpdateLookAt()
	{
		_cLookAt.Update ();
		_cameraCenter = _cLookAt.cameraCenter;
	}

	private void UpdateManualMove()
	{
		_cManualMove.Update ();
		_cameraCenter = _cManualMove.cameraCenter;
	}
	
}
