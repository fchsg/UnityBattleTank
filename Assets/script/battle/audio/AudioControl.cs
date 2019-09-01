using UnityEngine;

public class AudioControl : MonoBehaviour {

	private bool _isActive = false;
	public bool isActive
	{
		get { return _isActive; }
	}

	private AudioSource _audio = null;
	private GameObject _target = null;

	private bool _destroyOnStop = true;

	// Use this for initialization
	void Start () {
		_audio = GetComponent<AudioSource> ();
		_audio.minDistance = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (!_isActive) {
			return;
		}

		if (_target != null) {
			transform.position = _target.transform.position;
		}

		if (_audio != null) {
			if (_destroyOnStop && !_audio.isPlaying) {
				Destroy();
			}
		} else {
			Destroy();
		}
	}

	public void Init(GameObject target, bool destroyOnStop = true)
	{
		_target = target;
		_destroyOnStop = destroyOnStop;

		_isActive = true;
	}

	public void FadeOut()
	{
		//to do, fade out
		_audio.Stop ();
		_destroyOnStop = true;
	}

	private void Destroy()
	{
		_isActive = false;
		AudioControlPool.Push (_audio);
	}

}
