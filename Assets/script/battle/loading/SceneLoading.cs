using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {

	private AsyncOperation async;
	private uint _nowprocess;

	UISceneLoading uiSceneLoading;

	void Start () 
	{
		_nowprocess = 0;
		StartCoroutine(loadScene());

		uiSceneLoading = UIController.instance.CreateScene (UICommon.UI_SCENE_LOADING) as UISceneLoading;
	}

	IEnumerator loadScene()
	{
		async = SceneManager.LoadSceneAsync(AppConfig.SCENE_NAME_BATTLE);
		async.allowSceneActivation = false;

		yield return async;
	}

	void Update()
	{
		if(async == null)
		{
			return;
		}

		uint toProcess;
		if(async.progress < 0.9f) //最多到0.9f
		{
			toProcess = (uint)(async.progress * 100);
		}
		else
		{
			toProcess = 100;
		}

		if(_nowprocess < toProcess)
		{
			_nowprocess++;
		}

		 float value = _nowprocess/100f;

		uiSceneLoading.UpdateUI (value);

		if (_nowprocess == 100)  //async.isDone应该是在场景被激活时才为true
		{
			async.allowSceneActivation = true;
		}
	}

}