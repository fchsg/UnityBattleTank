using UnityEngine;
using System.Collections;

public class CoinsEffctManager : MonoBehaviour {
	public UIButton _btn;
	// Use this for initialization
	void Awake()
	{
		
	}
	void Start () {
		UIHelper.AddBtnClick(_btn,OnClick_);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick_()
	{
		StartCoroutine(PlayEffect(this.gameObject));
	}
	public const string DUST_PARTICLE_PATH = "profab/ui/effect/Accel03";
	// 播放粒子效果 
	IEnumerator PlayEffect(GameObject go)
	{ 
//		go = go.GetComponent<FormationDragItemUI> ().attachSlotUI.gameObject;
		GameObject prefab = Resources.Load(DUST_PARTICLE_PATH) as GameObject;
		GameObject instanceObj  = NGUITools.AddChild(go, prefab);

		instanceObj.transform.localScale = Vector3.one;

//		battlePanel.particleList.Add (instanceObj);

		float leftTime = ParticleHelper.GetlifeCycleSec (instanceObj.transform);
		yield return new WaitForSeconds (leftTime);
//
		if (instanceObj != null) 
		{
//			battlePanel.particleList.Remove (instanceObj);
			DestroyImmediate (instanceObj);
		} 
	}
}
