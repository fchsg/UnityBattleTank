using UnityEngine;
using System.Collections;

public class MaxPlayer : MonoBehaviour {

	public BattleConfig.LAYER layer;
	public string animName = "Take 001";
	public float speed = 1;
	
	private Animation _animation;
	private float _palyTime = 0;

	void Start ()
	{
		BattleFactory.AddUnitToLayer (gameObject, layer);

		_animation = GetComponentInChildren<Animation> ();
		if (_animation != null) 
		{
			_animation [animName].speed = speed;
			_palyTime = _animation [animName].length / speed;
			_animation.Play (animName);
		}
	}

	void Update () 
	{
		_palyTime -= TimeHelper.deltaTime;
		if (_palyTime <= 0)
		{
			Destroy(gameObject);
		}
	}

}
