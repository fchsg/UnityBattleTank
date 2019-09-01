//using UnityEngine;
//using System.Collections;
//
//using UICommonNameSpace;
//
//public class ChatPanel : PanelBase {
//	public static readonly string PANEL_PATH = "profab/ui/main/ChatPanel";
//	public static readonly UI_PANEL_TYPE PANEL_NAME = UI_PANEL_TYPE.ChatPanel;
//
//	override  public void Create(Transform parent ){
//		base.Init (PANEL_PATH, PANEL_NAME, parent);
//		base.showStyle = PANEL_SHOW_STYLE.DownToSlide;
//		InitData ();
//
//	}
//	private void  InitData(){
//
//	}
//	override protected void OnClick(GameObject target){
//		base.OnClick (target);
//
//		ButtonClick (target);
//	}
//	void  ButtonClick(GameObject click){
//		if (click.name.Equals ("BtnClosed")) {
//			this.Hide();
//		}
//	}
//}
