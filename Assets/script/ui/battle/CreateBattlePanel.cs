using UnityEngine;
using System.Collections;

public class CreateBattlePanel : MonoBehaviour {

	void Start ()
	{
		UIController.instance.CreatePanel (UICommon.UI_PANEL_BATTLE);

	}

	void OnDestroy()
	{
		UIController.instance.DeleteImmediatelyPanel (UICommon.UI_PANEL_BATTLE);
	}

}
