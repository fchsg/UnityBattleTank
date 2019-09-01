using UnityEngine;
using System.Collections;

public class PanelBase : UIBase
{
	public enum AnimationType
	{
		NONE,
		SCALE,
		ALPHA,
	}

	private AnimationType _animationType = AnimationType.SCALE;
	public AnimationType animationType
	{
		set { _animationType = value; }
	}


	public delegate void OnFinishCallBack();

	private OnFinishCallBack _onFinishCallBack = null;
	public void SetOnFinishCallBack(OnFinishCallBack callBack)
	{
		_onFinishCallBack = callBack;
	}

	override public void Init()
	{
		base.Init ();
	}

	override public void Delete()
	{
		StartCoroutine (LateDelete());
	}

	override public void Closed()
	{
		StartCoroutine (LateClosed ());
	}

	override public void Open(params System.Object[] parameters)
	{
		AdjustPositionByUICamera ();
	 	StartCoroutine (LateOpen ());
		UIParticleManager.instance.DestoryParticle ();
	}

	IEnumerator  LateOpen()
	{
	    yield return new WaitForEndOfFrame ();
		UIPanelManager.instance.SetDepth (this, true);

		PlayAnimation (true);
	}
	
	IEnumerator LateClosed()
	{
		yield return new WaitForEndOfFrame ();
		if (_animationType == AnimationType.NONE) {
			ClosedCallBack ();
		} else {
			PlayAnimation (false, ClosedCallBack);
		}
	}

	private void ClosedCallBack()
	{
		UIPanelManager.instance.SetDepth (this, false);

		if (_onFinishCallBack != null) {
			_onFinishCallBack();
		}
	}

	IEnumerator LateDelete()
	{
		yield return new WaitForEndOfFrame ();
		if (_animationType == AnimationType.NONE) {
			DeleteCallBack ();
		} else {
			PlayAnimation (false, DeleteCallBack);
		}
	}

	private void DeleteCallBack()
	{
		UIPanelManager.instance.DestoryPanel (this);
		if (_onFinishCallBack != null) {
			_onFinishCallBack();
		}
		base.Delete ();
	}

	private void PlayAnimation(bool openOrClosed, EventDelegate.Callback callBack = null)
	{
		switch (_animationType) 
		{
		case AnimationType.NONE:
			break;
		case AnimationType.SCALE:
			UIHelper.PlayPanelScale (this, openOrClosed, callBack);
			break;
		case AnimationType.ALPHA:
			UIHelper.PlayPanelAlpha (this, openOrClosed, callBack);
			break;
		}
	}

	private void AdjustPositionByUICamera()
	{
		GameObject uicamera = GameObject.FindGameObjectWithTag (AppConfig.TAB_UI_CAMERA);
		if (uicamera != null) 
		{
			rootTransform.localPosition = uicamera.transform.localPosition;
		}
	}

	public void DeleteImmediately()
	{
		UIPanelManager.instance.DestoryPanel (this);
		base.Delete ();
	}
	
	override public void Reset() {}
}
