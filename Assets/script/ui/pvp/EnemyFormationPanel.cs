using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class EnemyFormationPanel : PanelBase {

	private const string UNIT_ITEM_PATH = "profab/ui/pvp/EnemyTankItem";
	private const string HERO_ITEM_PATH = "profab/ui/BattleFormation/formation_hero_item";

	private Transform _Container;
	private UIButton _close_Btn;
	private UILabel _namePanel;
	private UILabel _EnergyLabel;

	//data
	private SlgPB.PVPUser _pvpUser;

	private List<GameObject> tank_slots = new List<GameObject> ();
	private List<GameObject> hero_slots = new List<GameObject> ();

	private List<GameObject> spine_tanks = new List<GameObject> ();

	private GameObject _UIDragDropRoot; // 层级 RenderQueue 3100

	public override void Init ()
	{
		base.Init ();

		_Container = transform.Find("EnemyContainer");
		_close_Btn = _Container.Find("bg/CloseBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_close_Btn,OnClose);
		_EnergyLabel =  _Container.Find("bg/EnergyLabel/Label").GetComponent<UILabel>();

		GameObject team_Formation = UIHelper.FindChildInObject (this.gameObject, "Formation_Team_0");

		_UIDragDropRoot = UIHelper.FindChildInObject (this.gameObject, "UIDragDropRoot");

		// unit slot
		for (int i = 0; i < 6; ++i) 
		{
			string path = "tank_slot_" + i;
			GameObject slot = team_Formation.transform.Find (path).gameObject;
			tank_slots.Add (slot);
		}

		// hero slot
		for (int i = 0; i < 6; ++i) 
		{
			string path = "hero_slot_" + i;
			GameObject slot = team_Formation.transform.Find (path).gameObject;
			hero_slots.Add (slot);
		}
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null && parameters.Length > 0)
		{
			_pvpUser = parameters[0] as SlgPB.PVPUser;
			if (_pvpUser != null) 
			{
				CreateItem ();
				_EnergyLabel.text = _pvpUser.fightPower.ToString();
			}
		}
	}

	public void CreateItem()
	{
		// Unit
		List<SlgPB.UnitGroup>  unitGroups = _pvpUser.unitGroups;
		foreach (SlgPB.UnitGroup unitGroup in unitGroups) 
		{
			int unitId = unitGroup.unitId;
			int num = unitGroup.num;
			int posId = unitGroup.posId;
			int heroId = unitGroup.heroId;

			if (unitId > 0 && num > 0) 
			{
				DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
				if (dataUnit != null)
				{
					GameObject prefab = Resources.Load (UNIT_ITEM_PATH) as GameObject;
					GameObject slot = tank_slots [posId - 1];
					GameObject go = NGUITools.AddChild (slot, prefab);

					// 层级调整
					go.transform.parent = _UIDragDropRoot.transform;

					// number
					UILabel num_Label	= go.transform.Find ("Num_Label").GetComponent<UILabel> ();
					num_Label.text = num.ToString();

					// name
					UILabel name_Label	= go.transform.Find ("Name_Label").GetComponent<UILabel> ();
					name_Label.text = dataUnit.name;

					// spine Tank
					Transform Tank_Container	= go.transform.Find ("Tank_Container");
					CreateSpineTank (Tank_Container, unitId);
				}
			}

			//  Hero
			if (heroId > 0) 
			{
				SlgPB.Hero  hero = GetHero(heroId);
				DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (heroId, hero.exp, hero.stage);

				if (hero != null)
				{
					GameObject prefab = Resources.Load (HERO_ITEM_PATH) as GameObject;
					GameObject slot = hero_slots [posId - 1];
					GameObject go = NGUITools.AddChild (slot, prefab);

					// 层级调整
					go.transform.parent = _UIDragDropRoot.transform;

					go.transform.Find ("Hero_Head_Sprite").gameObject.SetActive (true);
					go.transform.Find ("Hero_Body_Sprite").gameObject.SetActive (false);

					UILabel hero_name = go.transform.Find ("Name_Label").GetComponent<UILabel> ();
					hero_name.text = dataHero.name;
				}
			}
		}
	}
		
	private void CreateSpineTank(Transform transform, int unitId)
	{
		DataUnit unit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
		string primitive = "tankIconPrimitiveUI";

		GameObject spineTankIcon = TankIconSpineAttach.Create (unit, Vector3.zero, 7f, 90, 90, primitive);
		spineTankIcon.name = "SpineTankIconUI";

		spineTankIcon.transform.parent = transform;
	    spineTankIcon.transform.localPosition = Vector3.zero;
		spineTankIcon.transform.localScale = new Vector3 (-1, 1, 1);
		RenderHelper.ChangeTreeLayer (spineTankIcon, gameObject.layer);

		spine_tanks.Add (spineTankIcon);
	}

	private void FreeSpineTank()
	{
		for (int i = spine_tanks.Count - 1; i > 0; --i) {
			GameObject spineTankIcon = spine_tanks [i];
			if (spineTankIcon != null)
			{
				MonoBehaviour.DestroyImmediate (spineTankIcon);
				spineTankIcon = null;
			}
		}
	}

	private SlgPB.Hero GetHero(int heroId)
	{
		List<SlgPB.Hero> heros = _pvpUser.heroes;
		foreach (SlgPB.Hero hero in heros) 
		{
			if (heroId == hero.heroId) {
				return hero;
			}
		}
		return null;
	}
		
	void OnClose()
	{
		FreeSpineTank ();

		this.Delete();
	}

}
