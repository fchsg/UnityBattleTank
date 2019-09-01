using UnityEngine;
using System.Collections;

/// <summary>
/// 每日登录奖励item
/// </summary>
public class DaysItem : MonoBehaviour {

	private Transform _parent;
	private UILabel _tieleLabel;
	private UISprite _NO_Sprite;
	private UISprite _have_Sprite;
	private UIButton _btn;
	private UIGrid _Grid;
	int _day;
	void Awake()
	{
		_parent = this.transform;
		_tieleLabel = _parent.Find("Label").GetComponent<UILabel>();
		_NO_Sprite = _parent.Find("NO_Sprite").GetComponent<UISprite>();
		_have_Sprite = _parent.Find("have_Sprite").GetComponent<UISprite>();
		_btn = _parent.Find("btn").GetComponent<UIButton>();
		_Grid = _parent.Find("Grid").GetComponent<UIGrid>();
		_NO_Sprite.gameObject.SetActive(false);
		_have_Sprite.gameObject.SetActive(false);
		CreateItem(_Grid,6,SignInItem.SignInItemType.DAYSITEM);
	}
	public void UpdateData(int data)
	{
		_day = data + 1;

	}
	void Start () {
		UpdateUI();
	}
	void UpdateUI()
	{
		_tieleLabel.text = UIHelper.SetStringSixteenColor("登录第",UICommon.SIXTEEN_GREY) + UIHelper.SetStringSixteenColor(_day.ToString(),UICommon.SIXTEEN_ORANGE) + UIHelper.SetStringSixteenColor("天",UICommon.SIXTEEN_GREY) ;
	}
	// Update is called once per frame
	void Update () {
	
	}
	void CreateItem(UIGrid grid,int num,SignInItem.SignInItemType type){
		grid.DestoryAllChildren();
		for(int i = 0; i < num; i++)
		{
			if(grid.gameObject != null)
			{
				GameObject profab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "SignIn/SignInItem");
				GameObject item = NGUITools.AddChild(grid.gameObject,profab);
				SignInItem classItem = item.GetComponent<SignInItem>();
				classItem.UpdateData(i,type);
				item.name = "0" + i;		 
			}
		}
		grid.Reposition();
	}


}
