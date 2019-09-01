using UnityEngine;
using System;
using System.Collections;

public class TimerItem : MonoBehaviour
{
	private string timerKey;
	private int totalNum;
	private float delayTime;
	private Action<int> callback;
	/// <summary>
	/// 回调函数
	/// </summary>
	private Action<string> endCallback;
	/// <summary>
	/// 是否倒计时
	/// </summary>
	private bool isCountCown;
	private int currentIndex;
	/// <summary>
	/// 当前时间
	/// </summary>
	public float currentTime;

	 
	public void Init(string timerKey,float time, float delayTime, Action<string> callback)
	{
		this.timerKey = timerKey;
		this.currentTime = time;
		this.delayTime = delayTime;
		this.endCallback = callback;
	}
	
	public void Run(string timerKey,int totalNum, float delayTime, Action<int> callback, Action<string> endCallback,bool isCountCown)
	{
		this.Stop ();
		this.timerKey = timerKey;
		this.currentIndex = 0;
		
		this.totalNum = totalNum;
		this.delayTime = delayTime;
		this.callback = callback;
		this.endCallback = endCallback;
		this.isCountCown = isCountCown;
		
		this.StartCoroutine ("EnumeratorAction");
	}

	public void RunCoolingTime(float time)
	{
		// 计算差值
		float offsetTime = time - this.currentTime;
		
		// 如果差值大等于延迟时间
		if(offsetTime >= this.delayTime)
		{
			float count = offsetTime / this.delayTime - 1;
			float mod = offsetTime % this.delayTime;
			
			for(int index = 0; index < count; index ++)
			{
				this.endCallback(this.timerKey);
			}
			
			this.currentTime = time - mod;
		}
	}

	public void Stop()
	{
		this.StopCoroutine ("EnumeratorAction");
	}
	
	private IEnumerator EnumeratorAction()
	{
		yield return new WaitForSeconds (this.delayTime);
		
		this.currentIndex ++;
		if (this.callback != null) {
			if (this.isCountCown) {
				this.callback(this.totalNum - this.currentIndex);
			}else{
				this.callback(this.currentIndex);
			}

		}

		if(this.totalNum != -1)
		{
			if(this.currentIndex >= this.totalNum)
			{
				if(this.endCallback != null) this.endCallback(this.timerKey);
				yield break;
			}
		}
		this.StartCoroutine("EnumeratorAction");
	}
}
