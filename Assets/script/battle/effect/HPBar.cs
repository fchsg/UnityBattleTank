using UnityEngine;
using System.Collections;

public class HPBar : MonoBehaviour {

	private Unit _unit;

	private SkeletonAnimation _skeletonAnim;
	private string _animName;
	private float _duration;
	private float _rate = 1;

	private const float CHANGE_SPEED = 1.5f;
	private const float ALPHA_SPEED = 0.3f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (_unit.isDead) {
			DestroyImmediate(gameObject);
			return;
		}

		float targetRate = CalcRate ();
		float d = targetRate - _rate;
		if (d != 0) {
			float c = CHANGE_SPEED * TimeHelper.deltaTime;
			if (d > 0) {
				d = Mathf.Min (d, c);
			} else {
				d = Mathf.Max(d, -c);
			}
			
			_rate += d;
			Refresh(d);
			
			_skeletonAnim.Skeleton.A = 1;
		} else {
			float alpha = Mathf.Max(0, _skeletonAnim.Skeleton.A - ALPHA_SPEED * TimeHelper.deltaTime);
			_skeletonAnim.Skeleton.A = alpha;
		}

	}

	private float CalcRate()
	{
		float rate = _unit.unit.currentHp / _unit.unit.complexBattleParam.hp;
		rate = MathHelper.Clip (rate, 0, 1);
		return rate;
	}
	
	public void Init(Unit unit)
	{
		_unit = unit;

		_skeletonAnim = GetComponent<SkeletonAnimation> ();
		_skeletonAnim.Skeleton.A = 0;

		if (_unit.team == DataConfig.TEAM.ENEMY) {
			_animName = "Tank_blood_blue";
		} else {
			_animName = "Tank_blood_red";
		}
		_duration = _skeletonAnim.Skeleton.Data.FindAnimation (_animName).Duration;

		_skeletonAnim.state.SetAnimation (0, _animName, false);
		_skeletonAnim.state.TimeScale = 0;
		
	}


	private void Refresh(float dif)
	{
		float delta = -dif * _duration;

		_skeletonAnim.state.TimeScale = 1;
		_skeletonAnim.state.Update (delta);
		_skeletonAnim.state.TimeScale = 0;
	}

}
