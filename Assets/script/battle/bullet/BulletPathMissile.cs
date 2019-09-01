using UnityEngine;

public class BulletPathMissile : Bullet {

	public const float SPEED_ROTATE = 180;
	
	public const float SPEED_START = 10;
	public const float SPEED_MAX = 80;
	public const float ACCELERATE = 30;
	
	private float speed = SPEED_START;
	
	private float heightInitOffset;
	
	protected override void ShootBegan()
	{
		base.ShootBegan ();
		
		heightInitOffset = Mathf.Abs (_shoot.target.transform.position.y - startPosition.y);
		
		transform.rotation = _shoot.attacker.launcher.transform.rotation;
		//		transform.Rotate (new Vector3 (0, 90, 0));
		
	}
	
	protected override void ShootEnd()
	{		
		base.ShootEnd ();
	}
	
	
	void Update () {
		CorrectDirection ();
		
		bool arrive = Move ();
		if (arrive) {
			ShootEnd();
		}

	}
	
	private bool Move()
	{
		speed += ACCELERATE * TimeHelper.deltaTime;
		speed = Mathf.Min (speed, SPEED_MAX);
		
		float rotSpeed = speed / SPEED_MAX * SPEED_ROTATE;
		
		
		Vector3 destination = GetCurrentDestination ();
		AimmingControl.RESULT rotateR = AimmingControl.Rotate (
			transform.position, destination, transform.rotation.eulerAngles.y, rotSpeed, 0.01f);
		transform.Rotate (new Vector3 (0, rotateR.turnDegree, 0));
		
		
		Vector3 line = destination - transform.position;
		float dist = line.magnitude;
		
		Vector3 orientation = UnitHelper.GetOrientation (transform);
		VectorHelper.ResizeVector (ref orientation, dist);
		Vector3 p = orientation + transform.position;
		
		float radius = _shoot.target.unit.dataUnit.GetHurtRadius ();
		MovingControl.RESULT moveR = MovingControl.MoveTo (transform.position, p, speed, radius + heightInitOffset);
		transform.position = moveR.destination;
		
		
		return moveR.arrived;
	}


}
