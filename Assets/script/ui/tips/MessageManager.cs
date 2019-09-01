using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {

	protected static MessageManager _instance;
	public static MessageManager instance {
		get{
			if(_instance == null)
			{
				_instance = new MessageManager();
			}
			return _instance;
		}
	}
	private UILabel messageLabel;
	private TweenAlpha tween;
	private bool isSetActive = true;
	private GameObject messPF;

	void Awake(){

		CreateMessage ();
		_instance = this;
		messageLabel = messPF.transform.Find ("Label").GetComponent<UILabel> ();
		tween = messPF.GetComponent<TweenAlpha> ();

		EventDelegate ed = new EventDelegate (this,"OnTweenFinished");
		tween.onFinished.Add (ed);

		messPF.gameObject.SetActive (true);
	}

	public void ShowMessage(string message,float time = 1){
		gameObject.SetActive (true);
		StartCoroutine (Show(message,time));
	}

	IEnumerator Show(string message,float time){
		isSetActive = true;
		tween.PlayForward ();
		messageLabel.text = message;

		yield return new WaitForSeconds (time);
		isSetActive = false;
		tween.PlayReverse ();
	}

	public void OnTweenFinished(){
		if (isSetActive == false) {
			messPF.gameObject.SetActive(false);
		}
	}
	private void CreateMessage(){
		print ("******* CreateMessage");
		messPF = (GameObject)Resources.Load ("profab/ui/public/MessagePanel");
	}
}
