using UnityEngine;
using System.Collections;

public class AimmingControl {

	public class RESULT
	{
		public bool arrived;
		public float turnDegree;
		public float leftDegree;
		public float destinationAngleY;
	}


	private bool _running = true;

	private GameObject _owner;

	private Vector3 targetPosition;
	private bool _arrived = true;
	public bool arrived
	{
		get { return _arrived; }
	}


	private float _speed;
	public float speedScale = 1;


	public AimmingControl (GameObject owner, float speed = 60) {
		this._owner = owner;
		this._speed = speed;
	}

	public void Stop()
	{
		_running = false;
	}

	public void Resume()
	{
		_running = true;
	}

	// Update is called once per frame
	public void Update () {

		if (!_running) {
			return;
		}

		if (!_arrived) {
			Rotate();
		}
	}

	public void TrunToTarget(Vector3 targetPosition)
	{
		this.targetPosition = targetPosition;
		_arrived = false;
	}

	public void ClearTarget()
	{
		_arrived = true;
	}

	private bool Rotate()
	{
		float speed = _speed * speedScale;
		RESULT r = Rotate (_owner.transform.position, targetPosition, _owner.transform.rotation.eulerAngles.y, speed, 0.01f);
		_owner.transform.Rotate(new Vector3(0, r.turnDegree, 0));
		return r.arrived;
	}

	public static RESULT Rotate(Vector3 a, Vector3 b, float currentAngleY, float speed, float arriveThreshold)
	{
		float targetAngleY = CalcRotationAngleY (a, b);
		RESULT r = Rotate (currentAngleY, targetAngleY, speed, arriveThreshold);
		return r;
	}

	public static RESULT Rotate(float currentAngleY, float targetAngleY, float speed, float arriveThreshold)
	{
		float dAngle = targetAngleY - currentAngleY;
		dAngle = AngleHelper.LimitAngleInNPI_PI (dAngle);

		float turnLimitAngle = speed * TimeHelper.deltaTime;
		float cAngle = 0;
		if(dAngle > 0)
		{
			cAngle = Mathf.Min(turnLimitAngle, dAngle);
		}
		else if(dAngle < 0)
		{
			cAngle = Mathf.Max(-turnLimitAngle, dAngle);
		}
		
		float leftDegree = AngleHelper.LimitAngleInNPI_PI (dAngle - cAngle);
		
		RESULT r = new RESULT ();
		r.arrived = Mathf.Abs (leftDegree) <= arriveThreshold;
		r.turnDegree = cAngle;
		r.leftDegree = leftDegree;
		r.destinationAngleY = currentAngleY + cAngle;
		return r;
	}
	
	public static float CalcRotationTo(Vector3 a, Vector3 b, float currentAngleY)
	{
		float targetAngle = CalcRotationAngleY (a, b);

		float dAngle = (targetAngle - currentAngleY);
		dAngle = AngleHelper.LimitAngleInNPI_PI (dAngle);
		return dAngle;
	}

	public static float CalcRotationAngleY(Vector3 a, Vector3 b)
	{
		Vector3 orientation = b - a;
		if (orientation.x == 0 && orientation.z == 0) {
			return 0;
		}
		
		float targetAngle = (-Mathf.Atan2(orientation.z, orientation.x) + Mathf.PI / 2) / Mathf.PI * 180;
		return targetAngle;
	}

}
