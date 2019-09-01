using UnityEngine;

public class BulletPathLinear : Bullet {

	private float duration;
	private float life;
	
	public const float SPEED = 200;
	
	protected override void ShootBegan()
	{
		base.ShootBegan ();
		
		Vector3 destination = GetCurrentDestination ();
		Vector3 line = destination - startPosition;
		duration = line.magnitude / SPEED;
		life = duration;
	}
	
	protected override void ShootEnd()
	{		
		base.ShootEnd ();
	}
	
	
	void Update () {
		CorrectDirection ();
		
		life -= TimeHelper.deltaTime;
		life = Mathf.Max (life, 0);

		Move ();

		if (life <= 0) {
			ShootEnd ();
		} else {
		}

	}
	
	private void Move()
	{
		float k01 = 1 - life / duration;

		Vector3 destination = GetCurrentDestination ();
		Vector3 line = destination - startPosition;
		line *= k01;
		Vector3 p = line + startPosition;

		transform.position = p;
	}

}
