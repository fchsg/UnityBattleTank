using UnityEngine;
using System.Collections;

public class SpinePlayer : MonoBehaviour {

	public BattleConfig.LAYER layer;
	public float speed = 1;

	private SkeletonAnimation _skeleton;
	private Spine.Animation _animation;
	private float _palyTime = 0;

	// Use this for initialization
	void Start () {
		BattleFactory.AddUnitToLayer (gameObject, layer);

		_skeleton = GetComponentInChildren<SkeletonAnimation>();
		if (_skeleton != null) {
			_skeleton.state.TimeScale = speed;
			_animation = _skeleton.Skeleton.Data.FindAnimation(_skeleton.AnimationName);
			_palyTime = _animation.Duration;
		}

	}
	
	// Update is called once per frame
	void Update () {
		_palyTime -= TimeHelper.deltaTime;
		if (_palyTime <= 0) {
			Destroy(gameObject);
		}
	}
}
