using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 奖励道具Item
/// </summary>

public class SignInItem : MonoBehaviour {
	 
	public enum SignInItemType
	{
		SIGININ = 1,//30签到
		CUMULATIVE = 2,//累计签到
		DAYSITEM = 3,//七日登录累计
	}

	private Transform _parent;
	private UIButton _bg_btn;
	private UITexture _iconTexture;
	private UISprite _angle_Sprite;
	private UISprite _Vip_Sprite;
	private UISprite _Double_Sprite;
	private UILabel _num_Label;
	private UILabel _can_Label;
	private UISprite _have_Sprite;
	private UILabel _num_right_Label;
	private UISprite _receive_Sprite;
	List<GameObject> _gameObject = new List<GameObject>(); 
	//data
	private SignInItemType _signInItemType;
	void Awake()
	{
		_gameObject.Clear();
		_parent = this.transform; 
		_bg_btn = _parent.Find("bg_btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_bg_btn,OnClickItem);
		_iconTexture = _parent.Find("iconTexture").GetComponent<UITexture>();
		_angle_Sprite = _parent.Find("angle_Sprite").GetComponent<UISprite>();
		_Vip_Sprite = _parent.Find("angle_Sprite/Vip_Sprite").GetComponent<UISprite>();
		_Double_Sprite = _parent.Find("angle_Sprite/Sprite").GetComponent<UISprite>();
		_num_Label = _parent.Find("num_Label").GetComponent<UILabel>();
		_num_Label.color = UICommon.FONT_COLOR_GREY;
		_can_Label = _parent.Find("can_Label").GetComponent<UILabel>();
		_can_Label.color = UICommon.FONT_COLOR_GREEN;
		_have_Sprite = _parent.Find("have_Sprite").GetComponent<UISprite>();
		_num_right_Label = _parent.Find("num_right_Label").GetComponent<UILabel>();
		_num_right_Label.color = UICommon.FONT_COLOR_GREY;
		_receive_Sprite = _parent.Find("receive_Sprite").GetComponent<UISprite>();
		_gameObject.Add(_angle_Sprite.gameObject);
		_gameObject.Add(_num_Label.gameObject);
		_gameObject.Add(_can_Label.gameObject);
		_gameObject.Add(_have_Sprite.gameObject);
		_gameObject.Add(_num_right_Label.gameObject);
		_gameObject.Add(_receive_Sprite.gameObject);
		foreach(GameObject g in _gameObject)
		{
			g.SetActive(false);
		}
	}
	public void UpdateData(int data,SignInItemType type)
	{
		_signInItemType = type;
	}
	void Start () {
		switch(_signInItemType)
		{
		case SignInItemType.CUMULATIVE:
			CumulativeItemUI();
			break;
		case SignInItemType.DAYSITEM:
			DaysItemUI();
			break;
		case SignInItemType.SIGININ:
			SiginInItemUI();
			break;
		}
	}
	//累计签到
	void CumulativeItemUI()
	{
		_num_right_Label.gameObject.SetActive(true);
		_receive_Sprite.gameObject.SetActive(true);
	}
	//七日签到
	void DaysItemUI()
	{
		_num_Label.gameObject.SetActive(true);
	}
	//30签到
	void SiginInItemUI()
	{
		_num_Label.gameObject.SetActive(true);
	}
	void Update () {
	
	}
	void OnClickItem()
	{
		
	}
}
