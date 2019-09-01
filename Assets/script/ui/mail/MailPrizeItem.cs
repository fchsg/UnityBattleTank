using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class MailPrizeItem : MonoBehaviour {

	private UISprite _bg;
	private UITexture _icon;
	private UILabel _num;
	private UISprite _iconSprite;
	//data 
	PrizeItem _prizeItem;
	void Awake()
	{
		_bg = transform.Find("itembg").GetComponent<UISprite>();
		_icon = transform.Find("Texture").GetComponent<UITexture>();
		_num = transform.Find("Label").GetComponent<UILabel>();
		_num.color = UICommon.FONT_COLOR_GREY;
	}
	public void Init(PrizeItem prizeItem)
	{
		if(prizeItem != null)
		{
			_prizeItem = prizeItem;
		}
		UpdateUI ();
	}
	// Update is called once per frame
	void UpdateUI () {
		if(_prizeItem != null)
		{
//			MultipleItem mulItem = UIHelper.GetMultipleItem(_prizeItem.itemId,_prizeItem.num,_prizeItem.type,_prizeItem.itemLevel,_icon);
			UIDropItem dropitem = UIHelper.GetUIDropItemByPrizeItem(_prizeItem);
			_icon.SetTexturePath(dropitem.icon);
			_num.text = dropitem.count.ToString();
		 	 
		}
	}

}
