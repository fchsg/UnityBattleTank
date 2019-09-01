using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHelper {

	private static string currentSceneName;


	public static void SwitchScene(string sceneName)
	{
		switch (currentSceneName) {
		case AppConfig.SCENE_NAME_BATTLE:
			break;

		}

		UISceneManager.instance.DeleteUIScene ();
		Resources.UnloadUnusedAssets();
		currentSceneName = sceneName;

		if (sceneName == AppConfig.SCENE_NAME_BATTLE) 
		{
			SceneManager.LoadScene (AppConfig.SCENE_NAME_LOADING);
		}
		else
		{
			SceneManager.LoadScene (sceneName);
		}
	}

}
