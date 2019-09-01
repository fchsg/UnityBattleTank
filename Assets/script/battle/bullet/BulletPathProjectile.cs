using UnityEngine;

public class BulletPathProjectile : Bullet{

	private float duration;
	private float life;
	
	public const float A = 9.8f * 5;
	public const float SPEED = 100;
	
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
		float k010 = k01 * 2;
		if (k010 > 1) {
			k010 = 2 - k010;
		}
		
		Vector3 destination = GetCurrentDestination ();
		Vector3 line = destination - startPosition;
		line *= k01;
		Vector3 p = line + startPosition;
		
		float mt = duration / 2;
		float mh = 0.5f * A * mt * mt;
		
		float t = mt * (1 - k010);
		float s = 0.5f * A * t * t;
		
		p.y += mh - s;
		
		transform.position = p;
	}

}
