using UnityEngine;
using System.Collections;

public class MessageErrorPanel : PanelBase {
	
//-------------------- data

	private DataErrorCode _errorCode;
	
//--------------------- ui

	public UIButton _confirm_btn;
	public UIButton _bg_btn;
	public UILabel _message_text;


	public override void Init ()
	{
		base.Init ();

		_confirm_btn = UIHelper.FindChildInObject (gameObject, "cofirm_btn").GetComponent<UIButton>();
		_bg_btn = UIHelper.FindChildInObject (gameObject, "bg_btn").GetComponent<UIButton>();
		_message_text = UIHelper.FindChildInObject(gameObject, "message_text").GetComponent<UILabel>();

		UIHelper.AddBtnClick (_confirm_btn, ConfirmBtnCallBack);
		UIHelper.AddBtnClick (_bg_btn, BgBtnCallBack);
	}


	public override void Open (params object[] parameters)
	{
		base.Open (parameters);

		int errorcodeId = (int)parameters [0];
		_errorCode = DataManager.instance.dataErrorCodeGroup.GetErrorCode (errorcodeId);

		if (_errorCode != null) {
			_message_text.text = _errorCode.message;
		}
		else 
		{
			_message_text.text = "" + errorcodeId;
		}

	}

	private void ConfirmBtnCallBack()
	{
		base.Delete ();

		ProcessError ();
	}
	
	private void BgBtnCallBack()
	{
		base.Delete ();

		ProcessError ();
	}
	
	private void ProcessError()
	{
		if (_errorCode != null) 
		{
			switch (_errorCode.operation) 
			{
			case 0:  // 重启客户端
				UIController.instance.CreateScene(UICommon.UI_LOGIN_SCENE);
				break;
			}
		}
	}


}
