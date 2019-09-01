using UnityEngine;
using System.Collections;
using System;

public class TimeHelper {

	public static float deltaTime
	{
		get {
			float FIX_FPS = 60;
			
			if (AppConfig.USE_FIXED_FRAMERATE) {
				return Time.timeScale / FIX_FPS;
			} else {
				float t = Time.deltaTime;
				return t;
			}
		}

	}

	public static float unscaledDeltaTime 
	{
		get {
			float FIX_FPS = 60;
			
			if (AppConfig.USE_FIXED_FRAMERATE) {
				return 1 / FIX_FPS;
			} else {
				float t = Time.unscaledDeltaTime;
				return t;
			}
		}
	}

	public static long GetTimestamp(DateTime cTime)
	{
		TimeSpan timeSpan = cTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);  
		long timestamp = Convert.ToInt64 (timeSpan.TotalMilliseconds);
		return timestamp;
	}

	public static long GetCurrentRealTimestamp()
	{
		return GetTimestamp(DateTime.UtcNow);
	}

	public static float GetLeftSecondsToEndTimestamp(long endTime)
	{
		long currentTime = GetCurrentRealTimestamp();
		float dTime = (float)(endTime - currentTime) / 1000;

		return Mathf.Max (0.0f, dTime);
	}


	// ===================================================
	// timescale

	private static long _timestampAtFrameStart = 0;

	public static void SynchronizeTimestampScaled()
	{
		if (_timestampAtFrameStart == 0) {
			_timestampAtFrameStart = GetCurrentRealTimestamp();
		} else {
			_timestampAtFrameStart += (long)(deltaTime * 1000);
		}

	}

	public static long GetCurrentTimestampScaled()
	{
		//ensure call this AFTER timestamp is synchronized,
		//that means SynchronizeTimestampScaled is called at the beginning of every frame
		Assert.assert (_timestampAtFrameStart > 0);
		return _timestampAtFrameStart;
	}
	

	public static void SetTimeScale(float scale)
	{
		Assert.assert(scale > 0);

		Time.timeScale = scale;

	}

}
