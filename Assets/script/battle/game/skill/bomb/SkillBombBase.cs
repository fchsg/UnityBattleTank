using UnityEngine;
using System.Collections;

public class SkillBombBase : MonoBehaviour {

	protected InstanceSkill _skill;
	protected Vector3 _targetPosition;
	
	protected bool _isOver = false;
	public bool isOver
	{
		get { return _isOver; }
	}
	
	public const float HEIGHT = 15;

	virtual protected float speed
	{
		get { return -15; }
	}
	

	private float inertiaSpeed;
	private const float INERTIA_OFFSET = 5;

	// Use this for initialization
	virtual protected void Start () {
		BattleFactory.AddUnitToLayer (gameObject, BattleConfig.LAYER.BULLET);


//		float time = Mathf.Sqrt (HEIGHT * 2 / G);
		float time = HEIGHT / Mathf.Abs(speed);
		inertiaSpeed = INERTIA_OFFSET / time;

		transform.Translate (-INERTIA_OFFSET, 0, 0);

	}

	// Update is called once per frame
	virtual protected void Update () {
		if (_isOver) {
			return;
		}

		transform.Translate (inertiaSpeed * TimeHelper.deltaTime, 0, 0);

//		_speed -= G * TimeHelper.deltaTime;
		transform.Translate (0, speed * TimeHelper.deltaTime, 0);

		float s = CalcScaleWithHeight ();
		transform.localScale = new Vector3 (s, s, s);
		
		if (transform.position.y <= 0) {
			_isOver = true;
			Bomb ();
		} else {
		}

	}

	private float CalcScaleWithHeight()
	{
		float s = transform.position.y / HEIGHT + 1;
		return s;
	}

    virtual public void Init(InstanceSkill skill, Vector3 targetPosition)
	{
		_skill = skill;
		_targetPosition = targetPosition;
		
		transform.position = targetPosition + new Vector3 (0, HEIGHT, 0);
	}

	virtual protected void Bomb()
	{
	}

}
