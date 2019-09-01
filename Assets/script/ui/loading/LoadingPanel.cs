using UnityEngine;
using System.Collections;

public class LoadingPanel : PanelBase {

	override public void Init()
	{
		base.Init ();

		RenderHelper.SetUIPanelSortingOrder (GetComponent<UIPanel> (), AppConfig.SORTINGORDER_UI_POPUP); 

		animationType = AnimationType.NONE;
	}
	
	override public void Open(params System.Object[] parameters)
	{
		base.Open ();
	}

	override public void Closed()
	{
		base.Closed ();
	}

}
