using UnityEngine;
using System.Collections;

public class AnimateTexture : MonoBehaviour
{
	
	private string _SequenceName_Gewei = "1000";
	private string _SequenceName_Shiwei = "100";
	private string _SequenceName_Baiwei = "10";
	private int _currentFrame;
	private UITexture _TextureUI;
	float timeElipsed = 0.0f;
	
	private float fps = 24;
	//public List<Texture2D> ani;
	
	private int SequenceNum = 22;
	
	
	void Awake()
	{
		_TextureUI = transform.Find("Texture").GetComponent<UITexture>();
		
	}
	public void Init(string SequenceName,int SequenceNum,string SequencePath)
	{

	}

	void Update () 
	{
		
		AutoPlayTexture();
		
		
	}
	
	void AutoPlayTexture()
	{
		timeElipsed += Time.deltaTime;
		if (timeElipsed >= 1.0 / fps)
		{
			timeElipsed = 0;
			
			
			if (_currentFrame < SequenceNum)
			{
				_currentFrame++;
			}
			else
			{
//				_currentFrame = 0;
				RemoveTexture();

			}
			DynamicLoadUnload(_currentFrame);
		}
		
	}


	void RemoveTexture()
	{
		Destroy(this.gameObject);
	}
	

	void DynamicLoadUnload(int curframe )
	{

		if (curframe < 10)
		{
			_TextureUI.mainTexture = (Texture2D)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "frameAnimation/"+_SequenceName_Gewei + curframe.ToString(), typeof(Texture2D));
		}
		else if (curframe >= 10 && curframe < 100)
		{
			if (curframe % 50 == 0)
				Resources.UnloadUnusedAssets();
			
			_TextureUI.mainTexture = (Texture2D)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "frameAnimation/"+ _SequenceName_Shiwei + curframe.ToString(), typeof(Texture2D));
		}
		else
		{
			if (curframe % 50 == 0)
				Resources.UnloadUnusedAssets();
			
			_TextureUI.mainTexture = (Texture2D)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "frameAnimation/" + _SequenceName_Baiwei + curframe.ToString(), typeof(Texture2D));
		}
	} 
	
}