using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class UIHeroPolyController : MonoBehaviour
{
	public const string HERO_PLANE_PATH = "profab/ui/hero/HeroPolyPlane";
	public const string HERO_LINE_PATH = "profab/ui/hero/HeroLineRender";

	public float[] curValue;
	public float[] foreValue;

	public GameObject _root;
	public GameObject background;
	public GameObject foreground;

	UIHeroPolyPlane pHandle;

	public void Init(GameObject parent,HeroDataManager.HeroData heroData)
	{
		_root = new GameObject ("PolyRoot");
		_root.transform.parent = parent.transform;
		_root.layer = parent.layer;
		 
//		_root.transform.localPosition   = Vector3.zero;
//		_root.transform.localRotation  = Quaternion.identity;
		_root.transform.localScale        = Vector3.one;

//		background = ResourceHelper.Load(HERO_PLANE_PATH);
//		background.layer = _root.layer;
//		background.transform.parent = _root.transform;
//		background.transform.position = new Vector3(0, 0, 5);
//		background.AddComponent<UIHeroPolyPlane>().CreateMesh(Color.yellow);

		foreground = ResourceHelper.Load(HERO_PLANE_PATH);
		foreground.layer = _root.layer;
		foreground.transform.parent = _root.transform;

		pHandle = foreground.AddComponent<UIHeroPolyPlane> ();
		pHandle.CreateMesh(Color.red);
		DataHeroLeadership heroLeadership = DataManager.instance.dataHeroGroup.GetHeroLeadership(heroData.id);
		curValue = new float[5];
		curValue[0] = heroLeadership.kTank;
		curValue[1] = heroLeadership.kMissile;
		curValue[2] = heroLeadership.kUnknown;
		curValue[3] = heroLeadership.kGun;
		curValue[4] = heroLeadership.kCannon;
		for (int i = 0; i < curValue.Length; i++)
		{
			pHandle.SetSectionValue(i, curValue[i]);
		}
		_root.transform.localPosition = parent.transform.localPosition;
		_root.transform.localScale *= 0.21f;

//		CreateLineRender ();

	}

	public void CreateLineRender()
	{
		GameObject line = ResourceHelper.Load (HERO_LINE_PATH);
		UIHeroLineRender heroLine = line.GetComponent<UIHeroLineRender> ();
		heroLine.SetPoints (pHandle.m_Vertices);
	}


	// test 
//	void OnGUI()
//	{
//		for (int i = 0; i < 5; i++)
//		{
//			GUILayout.Label("ability"+i);
//			curValue[i] = GUILayout.HorizontalSlider(curValue[i], 0.0F, 1.0F, GUILayout.Width(100));
//			if (curValue[i] != foreValue[i])
//			{
//				pHandle.SetSectionValue(i, curValue[i]);
//				foreValue[i] = curValue[i];
//			}
//		}
//	}

}

