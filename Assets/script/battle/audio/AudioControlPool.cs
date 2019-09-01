using UnityEngine;
using System.Collections.Generic;

public class AudioControlPool {

	private static int _activeCount = 0;
	public static int activeCount
	{
		get { return _activeCount; }
	}

	private static List<AudioSource> _freeList = new List<AudioSource>();

	public static AudioSource Query(AudioSource audioPrimitive)
	{
		++_activeCount;

		if (_freeList.Count > 0) {
			AudioSource audio = _freeList [0];
			_freeList.RemoveAt (0);

			audio.gameObject.name = audioPrimitive.gameObject.name + "(reused)";
			audio.loop = audioPrimitive.loop;
			audio.clip = audioPrimitive.clip;
			return audio;
		} else {
			AudioSource audio = GameObject.Instantiate(audioPrimitive) as AudioSource;
			MonoBehaviour.DontDestroyOnLoad(audio);
			return audio;
		}
	}

	public static void Push(AudioSource audio)
	{
		Assert.assert (_activeCount > 0);
		--_activeCount;

		audio.clip = null;
		_freeList.Add (audio);
	}

}
