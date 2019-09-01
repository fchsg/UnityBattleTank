using UnityEngine;
using System.Collections;

public class TestHUDText : MonoBehaviour {

	public Transform m_target;//HUD字体出现的位置
	public GameObject m_hudTextPrefab;//HUD字体 prefab，不可为空

	private HUDText m_hudText = null;//HUD字体
	// 初始化时调用
	void Start ()
	{
		if (HUDRoot.go == null) {
			GameObject.Destroy (this);
			return;
		}

		if (m_target == null) 
		{
			m_target = this.transform;
			Vector3 mpos = this.transform.position;
			mpos.y += 2;
			m_target.position = mpos;
		}

		//添加hud text到HUDRoot结点下
		GameObject child = NGUITools.AddChild (HUDRoot.go, m_hudTextPrefab);
		//获取HUDText
		m_hudText = child.GetComponent<HUDText> ();
		//添加UIFollow脚本
		child.AddComponent<UIFollowTarget> ().target = m_target;
	}
	
	// 每帧调用此函数一次
	void Update ()
	{
		if (Input.GetMouseButton (0)) {
			m_hudText.Add ("+100", Color.red, 0);
		}
		if (Input.GetMouseButton (1)) {
			m_hudText.Add ("-30", Color.green, 0);
		}
		if (Input.GetMouseButton (2)) {
			m_hudText.Add ("Miss", Color.cyan, 0);    
		}
	}
	
	void OnClick ()
	{
		if (m_hudText != null) {
			m_hudText.Add ("HUD TEXT", Color.red, 1.0f);
		}
	}

	
}
