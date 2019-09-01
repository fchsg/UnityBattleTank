using UnityEngine;
using System.Collections;

public class UpLevelPanel : PanelBase {
	public UIButton _btnHide;



	override public void Init ()
	{
		base.Init ();

		EventDelegate ev = new EventDelegate (OnHide);
		_btnHide.onClick.Add (ev);
	}

	public void OnHide()
	{
		Closed ();
	}
 
}
