using UnityEngine;
using System.Collections;
using SlgPB;
public class LoginScene : SceneBase {
 
	private static string _uerName;
	public static string userName
	{
		set { _uerName = value;}
		get { return _uerName;}
	
	}
	private static string _password;
	public static string PassWord
	{
		set {_password = value;}
		get { return _password;}
	}

	public UIInput userInput;
	public UIInput passWord;
	public UIButton _LoginBtn;

	override public void Init()
	{
		base.Init ();

		if (DataPersistent.name != "") 
		{
			userInput.value  = DataPersistent.name;
			passWord.value = DataPersistent.password;
		}

		EventDelegate ev = new EventDelegate (ButtonClick);
		_LoginBtn.onClick.Add (ev);

		// test
		GameBase.instance.eventController.AddHandler (GameEvent.EVENT.BASE_BUILDING_LEVELUP, onBuildingLevelUp);
	}

	private void onBuildingLevelUp(GameEvent e)
	{
	}

	void ButtonClick()
	{
		if (userInput != null)
		{
			_uerName = userInput.value;
			_password = passWord.value;
		}
	
		DataPersistent.name = _uerName;
		DataPersistent.password = _password;

		Login ();
	}

	public void Login(){
		UIHelper.LoadingPanelIsOpen(true);
		LoginRequest loginReqe = new LoginRequest ();
		loginReqe.userName = _uerName;
		loginReqe.api = new Model_ApiRequest ().api;
		(new PBConnect_login ()).Send (loginReqe,OnLogin);
	}
	private void OnLogin(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
//			StartGame ();

			PBConnect_getNotification.GetNotification (OnLoadNotification);
		} else {
			Trace.trace("Login Request Error " ,Trace.CHANNEL.UI);
		}
	}


	public void OnLoadNotification(bool success, System.Object content)
	{
		StartGame ();
	}


	private void StartGame()
	{
		// test
		GameBase.instance.eventController.RemoveHandler (GameEvent.EVENT.BASE_BUILDING_LEVELUP, onBuildingLevelUp);

		GameBase.instance.eventController.RemoveAllHandleBelongTo (this);

		UIController.instance.CreateScene (UICommon.UI_MAIN_SCENE);


	}

}
