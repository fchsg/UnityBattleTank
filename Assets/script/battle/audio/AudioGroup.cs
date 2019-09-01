using UnityEngine;

public class AudioGroup : MonoBehaviour {

	public AudioSource[] move;
	public AudioSource[] fire;
	public AudioSource[] miss;
	public AudioSource[] hit;
	public AudioSource[] dead;
	public AudioSource[] music;
	public AudioSource[] win;
	public AudioSource[] loss;

	public enum TYPE
	{
		MUSIC,
		EFFECT,
	}

	private static AudioSource _playingMusic = null;

	private const int MAX_PLAYING_COUNT = 8;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static AudioSource Play(AudioSource[] candidates, GameObject target, TYPE type = TYPE.EFFECT)
	{
		if (type == TYPE.MUSIC) {
			if (_playingMusic != null) {
				_playingMusic.GetComponent<AudioControl> ().FadeOut ();
				_playingMusic = null;
			}
		} else {
			if(AudioControlPool.activeCount >= MAX_PLAYING_COUNT)
			{
				return null;
			}
		}

		if (candidates.Length > 0) {
			int index = (int)RandomHelper.Range (0, candidates.Length);
			AudioSource audioPrimitive = candidates[index];

			AudioSource audio = AudioControlPool.Query(audioPrimitive);
			audio.transform.parent = GetAudioRoot().transform;

			bool destroyOnStop = (type != TYPE.MUSIC);
			audio.GetComponent<AudioControl>().Init(target, destroyOnStop);
			audio.Play();

			if (type == TYPE.MUSIC) {
				_playingMusic = audio;
			}
		}

		return null;
	}


	// ========================================================
	// audio

	private static GameObject _audioRoot = null;
	
	private static GameObject GetAudioRoot()
	{
		if (_audioRoot == null) {
			_audioRoot = new GameObject();
			_audioRoot.name = "AudioObjectsRoot";
			MonoBehaviour.DontDestroyOnLoad(_audioRoot);
		}

		return _audioRoot;
	}

}
