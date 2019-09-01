using UnityEngine;
using System.Collections;

public class SettlementInstructorItem : MonoBehaviour {

	private UISprite _bg_Sprite;
	private UISprite _Icon_Sprite;
	private UILabel _Level_Label;
	private UILabel _exp_Value_Label;

	private UILabel _label_Tips;
	private TweenPosition tweenPos;

	void Awake()
	{
		_bg_Sprite = transform.Find("bg_Sprite").GetComponent<UISprite>();
		_Icon_Sprite = transform.Find("bg_Sprite/Icon_Sprite").GetComponent<UISprite>();
		_Level_Label = transform.Find("Level_Label").GetComponent<UILabel>();
		_exp_Value_Label = transform.Find("exp_Sprite/exp_Value_Label").GetComponent<UILabel>();


		_label_Tips = transform.Find("LevelUp_Label").GetComponent<UILabel>();  
		tweenPos = transform.Find("LevelUp_Label").GetComponent<TweenPosition>(); 
	}

	public void InitData(Model_HeroGroup.ExpChangeResult heroExp)
	{
		_Level_Label.text = "LV" + heroExp.level;
		_exp_Value_Label.text = "" +  heroExp.expChanged;

		if (heroExp.levelChanged > 0) 
		{
			_label_Tips.gameObject.SetActive (true);

			_label_Tips.text = "等级提升" + heroExp.levelChanged;
			tweenPos.PlayForward ();
			tweenPos.SetOnFinished (FinishCallBack);
		}
		else
		{
			_label_Tips.gameObject.SetActive (false);
		}
	}

	public void FinishCallBack()
	{
		DestroyImmediate (_label_Tips.gameObject);
	}
}
