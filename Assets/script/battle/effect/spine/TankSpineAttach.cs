using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankSpineAttach : MonoBehaviour {

	public class RENDER_STATUS
	{
		public float degree;
		public int level;
		public int displayLevel;
		public bool displayFlipX;
	}

	public enum ANIMATION
	{
		NONE,
		STAND,
		RUN,
		FIRE,
	}

	private bool _attached = false;
	private GameObject _target;
	private Unit _unit;

	private string _spineName;
	private float _spineScale;

	private GameObject _spine;
	public GameObject spine
	{
		get { return _spine; }
	}

	private SkeletonAnimation _skeletonAnim;
	public SkeletonAnimation skeletonAnim
	{
		get { return _skeletonAnim; }
	}


	private bool _needSwitchAnim = false;
	private ANIMATION _anim = ANIMATION.NONE;
	private bool _animLoop;
	private RENDER_STATUS _bodyStatus = null;
	private RENDER_STATUS _cannonStatus = null;
	private Spine.Animation _spineAnim;
	private long _animStartTimestamp;

	private Dictionary<string, Spine.Attachment> _attachmentMap = new Dictionary<string, Spine.Attachment>();
	

//	public const int DEGREE_INTERVAL = 30;
//	public const int DEGREE_INTERVAL2 = DEGREE_INTERVAL / 2;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePoser ();

		
	}


	private void UpdatePoser()
	{
		if (!_attached) {
			return;
		}

		float bodyDegree = _unit.body.transform.rotation.eulerAngles.y;
		RENDER_STATUS bodyStatus = GetStatus (bodyDegree);
		if (_bodyStatus == null) {
			_bodyStatus = bodyStatus;
		} else {
			if(Mathf.Abs(_bodyStatus.degree - bodyStatus.degree) < 1)
			{
//				bodyStatus = _bodyStatus;
			}
		}

		float cannonDegree = _unit.launcher.transform.rotation.eulerAngles.y;
		RENDER_STATUS cannonStatus = GetStatus (cannonDegree);
		if (_cannonStatus == null) {
			_cannonStatus = cannonStatus;
		} else {
			if(Mathf.Abs(_cannonStatus.degree - cannonStatus.degree) < 1)
			{
//				cannonStatus = _cannonStatus;
			}
		}

		_needSwitchAnim |= (_bodyStatus.level != bodyStatus.level);
		_needSwitchAnim |= (_cannonStatus.level != cannonStatus.level);

		_bodyStatus = bodyStatus;
		_cannonStatus = cannonStatus;

		if (_needSwitchAnim) {
			_needSwitchAnim = false;
			RefreshAnim();
		}

		ShowCorrectImage ();


	}

	public static RENDER_STATUS GetStatus(float degree)
	{
		RENDER_STATUS status = new RENDER_STATUS ();

		degree = AngleHelper.LimitAngleIn0_360 (degree);
		degree = AngleHelper.LimitAngleIn0_360 (180 - degree);
		status.degree = degree;

		if (degree >= 0 && degree < 65) {
			status.level = 60;
			status.displayLevel = 60;
			status.displayFlipX = false;
		}
		if (degree >= 65 && degree < 75) {
			status.level = 70;
			status.displayLevel = 70;
			status.displayFlipX = false;
		}
		if (degree >= 75 && degree < 85) {
			status.level = 80;
			status.displayLevel = 80;
			status.displayFlipX = false;
		}
		if (degree >= 85 && degree < 95) {
			status.level = 90;
			status.displayLevel = 90;
			status.displayFlipX = false;
		}
		if (degree >= 95 && degree < 105) {
			status.level = 100;
			status.displayLevel = 100;
			status.displayFlipX = false;
		}
		if (degree >= 105 && degree < 115) {
			status.level = 110;
			status.displayLevel = 110;
			status.displayFlipX = false;
		}
		if (degree >= 115 && degree < 180) {
			status.level = 120;
			status.displayLevel = 120;
			status.displayFlipX = false;
		}

		if (degree >= 295 && degree < 360) {
			status.level = -60;
			status.displayLevel = 60;
			status.displayFlipX = true;
		}
		if (degree >= 285 && degree < 295) {
			status.level = -70;
			status.displayLevel = 70;
			status.displayFlipX = true;
		}
		if (degree >= 275 && degree < 285) {
			status.level = -80;
			status.displayLevel = 80;
			status.displayFlipX = true;
		}
		if (degree >= 265 && degree < 275) {
			status.level = -90;
			status.displayLevel = 90;
			status.displayFlipX = true;
		}
		if (degree >= 255 && degree < 265) {
			status.level = -100;
			status.displayLevel = 100;
			status.displayFlipX = true;
		}
		if (degree >= 245 && degree < 255) {
			status.level = -110;
			status.displayLevel = 110;
			status.displayFlipX = true;
		}
		if (degree >= 180 && degree < 245) {
			status.level = -120;
			status.displayLevel = 120;
			status.displayFlipX = true;
		}


		return status;

	}

	private void RefreshAnim()
	{
		string name = "";
		
		switch (_anim) {
		case ANIMATION.STAND:
			name = "Stand_";
			break;
		case ANIMATION.RUN:
			name = "Run_";
			break;
		case ANIMATION.FIRE:
			name = "Shoot_";
			break;
		}
		
		name += _cannonStatus.displayLevel;
		_skeletonAnim.state.SetAnimation (0, name, _animLoop);
		_spineAnim = _skeletonAnim.Skeleton.Data.FindAnimation(name);
		_animStartTimestamp = TimeHelper.GetCurrentTimestampScaled ();

	}

	public bool IsAnimFinished()
	{
		if (_animLoop) {
			return false;
		}

		long ts = TimeHelper.GetCurrentTimestampScaled ();
		if (ts - _animStartTimestamp >= _spineAnim.Duration * 1000) {
			return true;
		} else {
			return false;
		}
	}

	private void ShowCorrectImage()
	{
		string bodyName = GetBodyName (_bodyStatus.displayLevel);
		string cannonName = GetBarralName (_cannonStatus.displayLevel);

		foreach (Spine.Slot slot in _skeletonAnim.skeleton.slots) {
			bool show = false;

			if(slot.Data.Name == cannonName)
			{
				show = true;
//				slot.Bone.FlipX = _cannonStatus.displayFlipX;
			}

			if(slot.Data.Name == bodyName)
			{
				show = true;
				slot.Bone.FlipX = _bodyStatus.displayFlipX ^ _cannonStatus.displayFlipX;
			}

			if(show)
			{
				if(_attachmentMap.ContainsKey(slot.Data.Name))
				{
					Spine.Attachment attachment = _attachmentMap[slot.Data.Name];
					slot.Data.AttachmentName = attachment.Name;
					slot.Attachment = attachment;
				}
			}
			else
			{
				if(slot.Attachment != null)
				{
					_attachmentMap[slot.Data.Name] = slot.Attachment;
					slot.Data.AttachmentName = null;
					slot.Attachment = null;
				}
			}

		}
		
		if (_cannonStatus.displayFlipX) {
			_spine.transform.localScale = new Vector3(-_spineScale, _spineScale, _spineScale);
		} else {
			_spine.transform.localScale = new Vector3(_spineScale, _spineScale, _spineScale);
		}

	}
	
	private string[] _bodyNames = new string[36];
	private string GetBodyName(int displayLevel)
	{
		int level = displayLevel / 10;
		if (_bodyNames [level] == null) {
			_bodyNames [level] = _spineName + "_Base_" + displayLevel;
		}
		return _bodyNames [level];
	}
	
	private string[] _barrelNames = new string[36];
	private string GetBarralName(int displayLevel)
	{
		int level = displayLevel / 10;
		if (_barrelNames [level] == null) {
			_barrelNames [level] = _spineName + "_Barrel_" + displayLevel;
		}
		return _barrelNames [level];
	}
	

	public void ChangeAnimation(ANIMATION anim, bool loop)
	{
		if (_anim != anim || _animLoop != loop) {
			_needSwitchAnim = true;
			_anim = anim;
			_animLoop = loop;
		}

	}

	public void Attach(GameObject target, string spineName)
	{
		Assert.assert (!_attached);
		_attached = true;

		_target = target;
		_unit = target.GetComponent<Unit> ();

		_spineName = spineName;
		_spineScale = (_unit.unit.dataUnit.length / 10) * (10f / 3f);

		_spine = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_TANK + spineName);
		_spine.transform.parent = _target.transform;
		RenderHelper.ResetLocalTransform (_spine);
//		_spine.transform.localScale = new Vector3 (_spineScale, _spineScale, _spineScale);

		_skeletonAnim = _spine.GetComponent<SkeletonAnimation> ();

		ChangeAnimation (ANIMATION.STAND, true);
	}

	public void Deattach()
	{
		_attached = false;

		DestroyImmediate (_spine);
		_spine = null;
	}


	public Spine.Bone GetBoneMouse()
	{
		string boneName = "mouse_";
		
		if (_unit.unit.dataUnit.bodyType == DataConfig.BODY_TYPE.CAR) {
			boneName += _bodyStatus.displayLevel;
		} else if (_unit.unit.dataUnit.bodyType == DataConfig.BODY_TYPE.CAR_WITH_CANNON) {
			boneName += _cannonStatus.displayLevel;
		}

		Spine.Bone bone = GetBone (boneName);
		return bone;
	}

	public Spine.Bone GetBone(string boneName)
	{
		Spine.Bone bone = _skeletonAnim.Skeleton.FindBone (boneName);
		return bone;
	}

}
