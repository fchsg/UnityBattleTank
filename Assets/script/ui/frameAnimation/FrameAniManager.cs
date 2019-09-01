using UnityEngine;
using System.Collections;

public class FrameAniManager
{
	public static void PlayFrameAni(GameObject parent)
	{
		CreateFrameAni(parent);
	}


	public static void CreateFrameAni (GameObject parent)
	{
		if(parent != null){
			GameObject Item = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "main/upLevelAni");
			GameObject item = NGUITools.AddChild(parent,Item);
			

			
		}
	}
}
