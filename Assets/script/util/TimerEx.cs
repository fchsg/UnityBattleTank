using UnityEngine;
using System.Collections;
using System.Timers;

public class TimerEx : MonoBehaviour {

	public delegate void CALLBACK(System.Object parameter);


	private float _interval;
	private CALLBACK _callback;
	private bool _loop;
	private System.Object _parameter;
	
	private long _timestamp = 0;
	private bool _pause = false;

	private const string TIMEREX_NAME_PREFIX = "_TimerExObject";
	
	private void Init (float interval, CALLBACK callback, bool loop = false, System.Object parameter = null) 
	{
		_interval = interval;
		_callback = callback;
		_loop = loop;
		_parameter = parameter;

		_timestamp = TimeHelper.GetCurrentRealTimestamp ();
	}

	public void Pause()
	{
		_pause = true;
	}

	public void Resume()
	{
		_pause = false;
	}

	public void Stop()
	{
		Destroy(gameObject);
	}

	void Update () 
	{
		if (_pause) {
			return;
		}

		long ct = TimeHelper.GetCurrentRealTimestamp();
		float diff = (float)(ct - _timestamp) / 1000.0f;
		if(diff >= _interval)
		{
			if(_callback != null)
			{
				_callback(_parameter);
			}
			//Trace.trace(gameObject.name, Trace.CHANNEL.INTEGRATION);
			
			if(_loop)
			{
				_timestamp = ct;
			}
			else
			{
				Stop();
			}
		}

	}

	// =====================================================
	// helper

	public static GameObject _timerRoot = null;

	public static TimerEx Init(string timerName, float interval, CALLBACK callback, bool loop = false, System.Object parameter = null)
	{
		if (_timerRoot == null) 
		{
			_timerRoot = new GameObject("TimerSpawn");
			MonoBehaviour.DontDestroyOnLoad(_timerRoot);
		}

		GameObject gameObject = new GameObject(TIMEREX_NAME_PREFIX + timerName);
		MonoBehaviour.DontDestroyOnLoad(gameObject);
		gameObject.transform.parent = _timerRoot.transform;

		TimerEx t = gameObject.AddComponent<TimerEx>();
		if (t != null) {
			t.Init(interval, callback, loop, parameter);
			return t;
		}
		return null;
	}
	

}
