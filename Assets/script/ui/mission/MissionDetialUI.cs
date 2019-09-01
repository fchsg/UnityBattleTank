using UnityEngine;
using System.Collections;

public class MissionDetialUI : MonoBehaviour {

	public UILabel Mission_Name_Label;

	public UILabel Left_Count;
	public UILabel Recommend_Power;
	public UILabel Need_Energy;
	public UILabel Current_Power;

	public UILabel Honor_Label;
	public UILabel Exp_Label;

	public UILabel Star_Label;
	public UILabel Energy_Label;

	private GameObject _ItemTipsPanel;

	public Model_Mission _model_Mission;
	public int left_fight_num = 0;
	public int total_fight_num = 0;

	public UISprite[] stars_Sprite = new UISprite[3];

	void Awake()
	{
		Mission_Name_Label = UIHelper.FindChildInObject(gameObject, "Mission_Name_Label").GetComponent<UILabel>();

		Left_Count = UIHelper.FindChildInObject(gameObject, "Left_Count").GetComponent<UILabel>();
		Recommend_Power = UIHelper.FindChildInObject(gameObject, "Recommend_Power").GetComponent<UILabel>();
		Need_Energy = UIHelper.FindChildInObject(gameObject, "Need_Energy").GetComponent<UILabel>();
		Current_Power = UIHelper.FindChildInObject(gameObject, "Current_Power").GetComponent<UILabel>();

		Honor_Label = UIHelper.FindChildInObject(gameObject, "Honor_Label").GetComponent<UILabel>();
		Exp_Label = UIHelper.FindChildInObject(gameObject, "Exp_Label").GetComponent<UILabel>();

		Star_Label = UIHelper.FindChildInObject(gameObject, "Top_Sprite/Top_Sprite_L/Label").GetComponent<UILabel>();
		Energy_Label = UIHelper.FindChildInObject(gameObject,"Top_Sprite/Top_Sprite_R/Label").GetComponent<UILabel>();

		_ItemTipsPanel = UIHelper.FindChildInObject (gameObject, "ToolTipPanel");

		for (int i = 0; i < 3; ++i) 
		{
			string path = "Star_Container/Star_" + i;
			stars_Sprite [i] = UIHelper.FindChildInObject (this.gameObject, path).GetComponent<UISprite> ();
		}

	}
		
	public void UpdateUI(Model_Mission model_Mission)
	{
		_model_Mission = model_Mission;

		int magicId = model_Mission.magicId;
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (magicId);
		int power = InstancePlayer.instance.model_User.model_Formation.CalcPower ();

		if (dataMission != null) 
		{
			Mission_Name_Label.text = dataMission.name;

			int fightNum = 0;
			if (model_Mission.difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
				fightNum = Model_Mission.FIGHT_NUM_NORMAL;
			} else {
				fightNum = Model_Mission.FIGHT_NUM_ELITE;
			}

			total_fight_num = fightNum;

			left_fight_num = model_Mission.remainFightNum;

			Left_Count.text = model_Mission.remainFightNum + "/" + fightNum;

			Recommend_Power.text = dataMission.evaluate + "";
			Need_Energy.text = dataMission.EnergyCost + "";
			Current_Power.text = power + "";

			Honor_Label.text = dataMission.honor + "";
			Exp_Label.text = dataMission.exp + "";

			// 更新可掉落Item
			UpdateItem(dataMission.itemsId);
		}


		int stageId = DataMission.GetStageId (magicId);
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (magicId);
		int allStarCount= InstancePlayer.instance.model_User.model_level.GetAllMissionsStar (difficulty, stageId);
		Star_Label.text = allStarCount + "";
		Energy_Label.text = power + "";


		// 更新星级
		UpdateStar(_model_Mission.starCount);
	}

	public void UpdateItem(int[] itemsId)
	{
		GameObject Item_Container = UIHelper.FindChildInObject (gameObject, "Item_Container");
		UITexture[] textures = Item_Container.GetComponentsInChildren<UITexture> ();
		int itemCount = itemsId.Length;
		for (int i = 0; i < textures.Length; ++i) 
		{
			textures [i].gameObject.SetActive (false);
			if (i < itemCount) 
			{

				int itemId = itemsId [i];
				textures [i].gameObject.SetActive (true);
				textures [i].SetItemTexture (itemId);

				DataItem dataItem = DataManager.instance.dataItemGroup.GetItem (itemId);
				MissionTipsItem item = textures [i].gameObject.GetComponent<MissionTipsItem> ();
				item.InitData (_ItemTipsPanel, dataItem);
			}
		}
	}

	void Update()
	{
		// 更新体力
		int energy = InstancePlayer.instance.model_User.model_Energy.energy;
		if (Energy_Label != null)
		{
			Energy_Label.text = energy + "";
		}

		// 更新剩余战斗次数
		if(Left_Count != null)
		{
			Left_Count.text = _model_Mission.remainFightNum + "/" + total_fight_num;
		}
	}

	public void UpdateStar(int starCount)
	{
		for (int i = 0; i < 3; ++i) 
		{
			if (i < starCount) {
				stars_Sprite [i].SetColorGrey (false);
			} else {
				stars_Sprite [i].SetColorGrey (true);
			}
		}
	}


}
