using UnityEngine;
using System.Collections;

public class ReportItem : MonoBehaviour {

	private UILabel _iswin_Label;
	private UILabel _info_Label;
	private UILabel _btnLabel;
	private UIButton _view_Btn;

	private SlgPB.FightLog _fightLog;
	void Awake()
	{
		_iswin_Label = transform.Find("iswin_Label").GetComponent<UILabel>();
		_info_Label = transform.Find("info_Label").GetComponent<UILabel>();
		_btnLabel = transform.Find("view_Btn/Label").GetComponent<UILabel>();
		_btnLabel.color = UICommon.FONT_COLOR_GOLDEN;
		_view_Btn = transform.Find("view_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_view_Btn,ONView);
	}
	public void Init(SlgPB.FightLog fightLog)
	{
		if(fightLog != null)
		{
			_fightLog = fightLog;
		}
	}
	 
	void Update () {
		UpdateUI ();
	}
	void UpdateUI () {
		if(_fightLog != null)
		{
			if(_fightLog.fightResult != 0)
			{
				_iswin_Label.color = UICommon.FONT_COLOR_GREEN;
				_iswin_Label.text = "胜利";
			}
			else
			{
				_iswin_Label.SetColorGrey(true);
				_iswin_Label.text = "失败";
			}
			string timeStr = SetTime(_fightLog.timeToNow);
			string youStr = "你";
			string challengeStr = "挑战了";
			string userName = _fightLog.oppUserName;
			string winStr = "战胜了";
			string youwinStr = "你胜利了";
			string youfailedStr = "你失败了";
			string rankUpStr = "排名上升";
			string rankDownStr = "排名下降";
			string rankStr = "排名保持不变!";
			string rankValueStr = _fightLog.rankChange.ToString();
			string str  = "名!";

			if(_fightLog.direct != 0)//你挑战其他人
			{
				if(_fightLog.fightResult != 0)
				{
					_info_Label.text = UIHelper.SetStringSixteenColor(timeStr,UICommon.SIXTEEN_GREY) + 
						UIHelper.SetStringSixteenColor(youStr,UICommon.SIXTEEN_RED) +
						UIHelper.SetStringSixteenColor(challengeStr,UICommon.SIXTEEN_GREY) +
						UIHelper.SetStringSixteenColor(userName,UICommon.SIXTEEN_ORANGE) + ","+
						UIHelper.SetStringSixteenColor(youwinStr,UICommon.SIXTEEN_GREEN) + ","+
						UIHelper.SetStringSixteenColor(rankUpStr,UICommon.SIXTEEN_GREY) + 
						UIHelper.SetStringSixteenColor(rankValueStr,UICommon.SIXTEEN_RED) + UIHelper.SetStringSixteenColor(str,UICommon.SIXTEEN_GREY) ;

				}
				else
				{
					_info_Label.text = UIHelper.SetStringSixteenColor(timeStr,UICommon.SIXTEEN_GREY) + 
						UIHelper.SetStringSixteenColor(youStr,UICommon.SIXTEEN_RED) +
						UIHelper.SetStringSixteenColor(challengeStr,UICommon.SIXTEEN_GREY) +
						UIHelper.SetStringSixteenColor(userName,UICommon.SIXTEEN_ORANGE) + ","+
						UIHelper.SetStringSixteenColor(youfailedStr,UICommon.SIXTEEN_GREY) + ","+
						UIHelper.SetStringSixteenColor(rankStr,UICommon.SIXTEEN_GREY) ;
				}
			}
			else
			{
				if(_fightLog.fightResult != 0)
				{
					_info_Label.text = UIHelper.SetStringSixteenColor(timeStr,UICommon.SIXTEEN_GREY) +
						UIHelper.SetStringSixteenColor(userName,UICommon.SIXTEEN_ORANGE) +
						UIHelper.SetStringSixteenColor(challengeStr,UICommon.SIXTEEN_GREY) +
						UIHelper.SetStringSixteenColor(youStr,UICommon.SIXTEEN_RED) + ","+
						UIHelper.SetStringSixteenColor(youwinStr,UICommon.SIXTEEN_GREEN) + ","+
						UIHelper.SetStringSixteenColor(rankStr,UICommon.SIXTEEN_GREY);
				}
				else
				{
					_info_Label.text = UIHelper.SetStringSixteenColor(timeStr,UICommon.SIXTEEN_GREY) +
						UIHelper.SetStringSixteenColor(userName,UICommon.SIXTEEN_ORANGE) +
						UIHelper.SetStringSixteenColor(challengeStr,UICommon.SIXTEEN_GREY) +
						UIHelper.SetStringSixteenColor(youStr,UICommon.SIXTEEN_RED) + ","+
						UIHelper.SetStringSixteenColor(youfailedStr,UICommon.SIXTEEN_GREY) + ","+
						UIHelper.SetStringSixteenColor(rankDownStr,UICommon.SIXTEEN_GREY) + 
						UIHelper.SetStringSixteenColor(rankValueStr,UICommon.SIXTEEN_RED) + UIHelper.SetStringSixteenColor(str,UICommon.SIXTEEN_GREY) ;
				}
			}
		}
	}
	string SetTime(int second)
	{
		string str = "";
		if(second > 0 && second <= 60)
		{
			str = second + "秒前";
		}
		else if(second > 60 && second <= 3600)
		{
			str = (second/60) + "分钟前";
		}
		else if(second > 3600 && second <= 86400)
		{
			str = (second/3600) + "小时前";
		}
		else if(second > 86400 && second <= 86400 * 7)
		{
			str = (second/86400) + "天前";
		}
		else if(second > 86400 * 7 && second <= 86400 * 30)
		{
			str = (second/86400/7) + "周前";
		}
		else
		{
			str = "刚刚";
		}
		return str;
	}
	void ONView()
	{
		
	}


}
