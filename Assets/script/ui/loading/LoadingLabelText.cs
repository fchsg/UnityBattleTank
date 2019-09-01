using UnityEngine;
using System.Collections;

public class LoadingLabelText : MonoBehaviour
{
	private string[] stringArr = { "", ".", "..", "..." };
	private UILabel mLabel;

	void Start()
	{
		mLabel = gameObject.GetComponent<UILabel>();
		StartCoroutine(PlayText(0));
	}
	
	IEnumerator PlayText(int index)
	{
		mLabel.text = "正在加载" + stringArr[index];
		yield return new WaitForSeconds(0.5f);
		index++;
		if (index >= stringArr.Length) index = 0;
		StartCoroutine(PlayText(index));
	}
	
}
