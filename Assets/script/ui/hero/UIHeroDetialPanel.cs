using UnityEngine;
using System.Collections;

public class UIHeroDetialPanel : MonoBehaviour {

	public  GameObject ployPlane;
	 
	void Start()
	{ 
	}

	public  void Init (HeroDataManager.HeroData _heroData)
	{
		GameObject polygon = UIHelper.FindChildInObject (gameObject, "Sprite");

		UIHeroPolyController polyController = 	gameObject.AddComponent<UIHeroPolyController> ();
		polyController.Init (polygon,_heroData);

		ployPlane = polyController.foreground;
	}
}
