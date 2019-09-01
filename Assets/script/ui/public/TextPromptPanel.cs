using UnityEngine;
using System.Collections;

public class TextPromptPanel : MonoBehaviour {

	private TweenAlpha _alpha;
	private UILabel _label;
 
	void Awake()
	{
		_alpha = transform.Find("PromptContainer").GetComponent<TweenAlpha>();
		_label = transform.Find("PromptContainer/Label").GetComponent<UILabel>();
	}
	void Start()
	{
		EventDelegate.Add(_alpha.onFinished,OnClose);
	}

	public void InitText(string text)
	{
		_label.text = text;
	}
	void OnClose()
	{
		NGUITools.Destroy(this.gameObject);
	}
	void Update () {
	
	}
}
