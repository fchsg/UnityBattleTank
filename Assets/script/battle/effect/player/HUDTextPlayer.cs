using UnityEngine;
using System.Collections;

public class HUDTextPlayer : MonoBehaviour {
	
	private float _playTime = 0;
	private bool _isStart = false;
	
	void Start ()
	{
	}

	void Update()
	{
		if (_isStart) 
		{
			_playTime -= TimeHelper.deltaTime;
			if(_playTime < 0.0f)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void createHUDText(Transform targetPosition, string text, Color color, float duration)
	{
		if (HUDRoot.go == null) 
		{
			GameObject.Destroy (this.gameObject);
			return;
		}

		_isStart = true;
		_playTime = duration;

		this.transform.parent = HUDRoot.go.transform;

		this.transform.localPosition = Vector3.zero;
		this.transform.localRotation = Quaternion.identity;
		this.transform.localScale = Vector3.one;

		this.gameObject.layer = HUDRoot.go.layer;

		gameObject.GetComponent<UIFollowTarget> ().target = targetPosition;

		HUDText hudText = gameObject.GetComponent<HUDText> ();
		hudText.Add (text, color, duration);
	}

}
