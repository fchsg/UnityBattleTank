using UnityEngine;
using System.Collections;

public class MainSceneUI : MonoBehaviour {

	private UIButton _AddStrengthBtn;
	private UILabel _AddStrength_Label;
	
	private UIButton _AddFoodBtn;
	private UILabel _AddFood_Label;
	
	private UIButton _AddOilBtn;
	private UILabel _AddOilBtn_Label;
	
	private UIButton _AddMetalBtn;
	private UILabel _AddMetalBtn_Label;
	
	private UIButton _AddRareBtn;
	private UILabel _AddRare_Label;
	
	private UIButton _AddCashBtn;
	private UILabel _AddCash_Label;
	
	private Transform _mian_Panel;
	private UILabel _PlayerName_Label;
	private UILabel _Fighting_Label;
	private UILabel _PlayerLevel_Label;
	private UILabel _PlayerVip_Label;
	private UISprite _head_Sprite;
	private UIButton _PVPBtn;


	private bool _refreCoins = true;
	
	//data
	Model_User _model_user;


	// Use this for initialization
	void Start () {
		// test NotificationCenter
		NotificationCenter.instance.AddEventListener (Notification_Type.RefreshGold, RefershGold);
		NotificationCenter.instance.AddEventListener (Notification_Type.RefreshCash, RefershCash);
		
		NotificationCenter.instance.AddEventListener (Notification_Type.RefreshFood, RefreshFood);
		NotificationCenter.instance.AddEventListener (Notification_Type.RefreshOil, RefreshOil);
		NotificationCenter.instance.AddEventListener(Notification_Type.RefreshMetal,RefreshMetal);
		NotificationCenter.instance.AddEventListener(Notification_Type.RefreshRare,RefreshRare);
	}



	// Update is called once per frame
	void Update () {
		UpdateUI();

	}

	void UpdateUI()
	{
		_model_user = InstancePlayer.instance.model_User;

		if(_model_user != null && _refreCoins)
		{
			int energy = _model_user.model_Energy.energy;
			_AddStrength_Label.text = energy  + "/200";
			_AddFood_Label.text = UIHelper.SetResourcesShowFormat( _model_user.model_Resource.GetIntFood()) + "";
			_AddOilBtn_Label.text = UIHelper.SetResourcesShowFormat( _model_user.model_Resource.GetIntOil()) + "";
			
			_AddMetalBtn_Label.text = UIHelper.SetResourcesShowFormat(  _model_user.model_Resource.GetIntMetal()) + "";
			_AddRare_Label.text = UIHelper.SetResourcesShowFormat(  _model_user.model_Resource.GetIntRare()) + "";
			_AddCash_Label.text = _model_user.model_Resource.GetIntCash() + "";
			 
			 
			_PlayerName_Label.text = _model_user.userName.ToString();
			_Fighting_Label.text = _model_user.model_Formation.CalcPower().ToString();
			_PlayerLevel_Label.text = _model_user.honorLevel.ToString();
			_PlayerVip_Label.text = "8";
//			_head_Sprite ;

		} 
	}
	
	public void RefershGold(Notification notification)
	{
//		_AddFood_Label.text = "12345" + notification._data.ToString();
	}

	public void RefershCash(Notification notification)
	{
//		_AddCash_Label.text = "9999" + notification._data.ToString();
	}
	
	public void RefreshFood(Notification notification)
	{
		_refreCoins = false;
		StartCoroutine(RefreCois());
		int foodlNum = Model_Helper.GetPlayerHavaFoodRes();
		int refreNum = int.Parse(notification._data.ToString());
		int curreNum = foodlNum + refreNum;
		_AddFood_Label.text = UIHelper.SetResourcesShowFormat(curreNum);
	}
	public void RefreshOil(Notification notification)
	{
		_refreCoins = false;
		StartCoroutine(RefreCois());
		int OillNum = Model_Helper.GetPlayerHavaOilRes();
		int refreNum = int.Parse(notification._data.ToString());
		int curreNum = OillNum + refreNum;
		_AddOilBtn_Label.text = UIHelper.SetResourcesShowFormat(curreNum);
	}
	public void RefreshMetal(Notification notification)
	{
		_refreCoins = false;
		StartCoroutine(RefreCois());
		int matalNum = Model_Helper.GetPlayerHavaMatelRes();
		int refreNum = int.Parse(notification._data.ToString());
		int curreNum = matalNum + refreNum;
		_AddMetalBtn_Label.text = UIHelper.SetResourcesShowFormat(curreNum);
	}
	public void RefreshRare(Notification notification)
	{
		_refreCoins = false;
		StartCoroutine(RefreCois());
		int RarelNum = Model_Helper.GetPlayerHavaRareRes();
		int refreNum = int.Parse(notification._data.ToString());
		int curreNum = RarelNum + refreNum;
		_AddRare_Label.text = UIHelper.SetResourcesShowFormat(curreNum);
	}

	IEnumerator RefreCois()
	{
		yield return new WaitForSeconds(2.0f);
		_refreCoins = true;
	}



	public void OnClickEquipment(){
		Trace.trace ("OnClickEquipment",Trace.CHANNEL.UI);
		UIController.instance.CreatePanel (UICommon.UI_PANEL_REPAIRFACTORY);
		//		UIController.instance.OpenPanelLoading (55);
	}
	public void OnClickTankFactory(){
		Trace.trace ("OnClickTankFactory",Trace.CHANNEL.UI);
		
		//test  parameters
		int integer = 123;
		string str = "asdfasf";
		DataUnit dataUnit = new DataUnit ();
		dataUnit.asset = "tank111";
		int[] arr = new int[]{23,56,78,4,5}; 
		
		UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKFACTORY, integer, str, dataUnit, arr);
	}

	
	// 增加粮食 
	public void OnClickAddFood(){
		//Trace.trace ("// 增加粮食" ,Trace.CHANNEL.UI);
		//		_addFoodValue.text = "00000";
	}
	// 增加石油
	public void OnClickAddOil(){
		//Trace.trace ("// 增加石油" ,Trace.CHANNEL.UI);
		//		_addOilValue.text = "00000";
	}
	
	// 增加稀土 
	public void OnClickAddRare(){
		//Trace.trace ("// 增加粮食" ,Trace.CHANNEL.UI);
		//		_addFoodValue.text = "00000";
	}
	// 增加 铁 
	public void OnClickAddMeta(){
		//Trace.trace ("// 增加石油" ,Trace.CHANNEL.UI);
		//		_addOilValue.text = "00000";
	}
	
	// 增加体力 
	public void OnClickAddStreng(){
		//Trace.trace ("// 增加粮食" ,Trace.CHANNEL.UI);
		//		_addFoodValue.text = "00000";
	}
	// 增加  钻石 
	public void OnClickAddCash(){
		//Trace.trace ("// 增加石油" ,Trace.CHANNEL.UI);
		//		_addOilValue.text = "00000";
	
	}
	// 战役
	public void OnClickCampaign()
	{
		int missionMagicId = InstancePlayer.instance.uiDataStatus.GetMissionMagicId ();

		UISceneManager.instance.SetActive (false);
		UIController.instance.CreatePanel (UICommon.UI_PANEL_CAMPAIGN, missionMagicId);
	}
	//pvp
	void OnPVP()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_PVP);
	}

	void OnDestroy()
	{
		NotificationCenter.instance.RemoveEventListener (Notification_Type.RefreshGold);
		NotificationCenter.instance.RemoveEventListener (Notification_Type.RefreshCash);
		NotificationCenter.instance.RemoveEventListener (Notification_Type.RefreshFood);
		NotificationCenter.instance.RemoveEventListener (Notification_Type.RefreshOil);
		NotificationCenter.instance.RemoveEventListener (Notification_Type.RefreshMetal);
		NotificationCenter.instance.RemoveEventListener (Notification_Type.RefreshRare);
		StopAllCoroutines();
	}
	void Awake(){
		_AddStrengthBtn = transform.Find("playerInfoContainer/AddStrengthBtn").GetComponent<UIButton>();
		_AddStrength_Label = transform.Find("playerInfoContainer/AddStrengthBtn/AddStrength_Label").GetComponent<UILabel>();
		
		_AddFoodBtn = transform.Find("playerInfoContainer/AddFoodBtn").GetComponent<UIButton>();
		_AddFood_Label = transform.Find("playerInfoContainer/AddFoodBtn/AddFood_Label").GetComponent<UILabel>();
		
		_AddOilBtn = transform.Find("playerInfoContainer/AddOilBtn").GetComponent<UIButton>();
		_AddOilBtn_Label = transform.Find("playerInfoContainer/AddOilBtn/AddOilBtn_Label").GetComponent<UILabel>();
		
		_AddMetalBtn = transform.Find("playerInfoContainer/AddMetalBtn").GetComponent<UIButton>();
		_AddMetalBtn_Label = transform.Find("playerInfoContainer/AddMetalBtn/AddMetalBtn_Label").GetComponent<UILabel>();
		
		_AddRareBtn = transform.Find("playerInfoContainer/AddRareBtn").GetComponent<UIButton>();
		_AddRare_Label = transform.Find("playerInfoContainer/AddRareBtn/AddRare_Label").GetComponent<UILabel>();
		
		_AddCashBtn = transform.Find("playerInfoContainer/AddCashBtn").GetComponent<UIButton>();
		_AddCash_Label = transform.Find("playerInfoContainer/AddCashBtn/AddCash_Label").GetComponent<UILabel>();
		

		EventDelegate evFood = new EventDelegate (OnClickAddFood);
		_AddFoodBtn.onClick.Add (evFood);
		
		EventDelegate evOil = new EventDelegate (OnClickAddOil);
		_AddOilBtn.onClick.Add (evOil);
		
		
		EventDelegate evStrength = new EventDelegate (OnClickAddStreng);
		_AddStrengthBtn.onClick.Add (evStrength);
		
		EventDelegate evMare = new EventDelegate (OnClickAddMeta);
		_AddMetalBtn.onClick.Add (evMare);
		
		EventDelegate evCash = new EventDelegate (OnClickAddCash);
		_AddCashBtn.onClick.Add (evCash);
		
		EventDelegate evRare = new EventDelegate (OnClickAddRare);
		_AddRareBtn.onClick.Add (evRare);


		UIButton btn_campaign = UIHelper.FindChildInObject (gameObject, "Btn_Battle").GetComponent<UIButton>();
		UIHelper.AddBtnClick (btn_campaign, OnClickCampaign);
		_PVPBtn = UIHelper.FindChildInObject (gameObject, "Btn_world").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_PVPBtn,OnPVP);
		_mian_Panel = transform.Find("playerInfoContainer");
		_PlayerName_Label = _mian_Panel.Find("PlayerName_bg/PlayerName_Label").GetComponent<UILabel>();         
		_Fighting_Label = _mian_Panel.Find("Fighting_bg/Fighting_Label").GetComponent<UILabel>();   
		_PlayerLevel_Label = _mian_Panel.Find("Level_Sprite/PlayerLevel_Label").GetComponent<UILabel>();   
		_PlayerVip_Label = _mian_Panel.Find("Vip_Sprite/PlayerVip_Label").GetComponent<UILabel>();   
		_head_Sprite = _mian_Panel.Find("head_bg/head_Sprite").GetComponent<UISprite>();

	}
}
