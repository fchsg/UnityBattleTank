using UnityEngine;
using System.Collections;

public class SkillItem : MonoBehaviour 
{
	public UIButton _skillBtn;
	public UILabel _sillName;
	private UISprite _skillIcon;

	private UIToggle _toggle;

	private GameSkill _gameSkill;
	private int _currentIndex;
	private DataSkill _skillData;


	void Awake(){
		_skillBtn = transform.Find ("SkillBtn").GetComponent<UIButton> ();
		_sillName = transform.Find ("SkillNameLabel").GetComponent<UILabel>();
		_skillIcon = transform.Find ("SkillBtn/Background").GetComponent<UISprite> ();
		_toggle = _skillBtn.GetComponent<UIToggle> ();
	}

	void Start(){
		if (_skillBtn != null) {
			EventDelegate evskill = new EventDelegate(OnBtnSelected);
			_skillBtn.onClick.Add(evskill);
		}
	}
	
	/// <summary>
	/// 更新数据
	/// </summary>
	public void UpdateData(int index)
	{
		_currentIndex = index;
		_gameSkill = BattleGame.instance.gameSkill;
		_skillData = _gameSkill.GetData(_currentIndex);
		if(_skillData != null)
		{
//			NGUITools.SetActive(this.icon.gameObject, true);
			this._sillName.text = _skillData.name;
			this._skillIcon.spriteName = "SkillICONS_" + (int)_skillData.type;
		}else
		{
			this._sillName.text = "";
//			NGUITools.SetActive(this.icon.gameObject, false);
		}
	}
	void Update()
	{
		if(_gameSkill != null)
		{
			//Trace.trace("BattleGame.instance.gameSkill.GetCount(_currentIndex)  " + " " + _currentIndex  + "      "+ BattleGame.instance.gameSkill.GetCount(_currentIndex) ,Trace.CHANNEL.UI);
			if(BattleGame.instance.gameSkill.GetCount(_currentIndex) <= 0)
			{
				BattleGame.instance.gameSkill.Select(-1);
//				this.gameObject.SetActive(false);
				NGUITools.Destroy(this.gameObject);
			}
		}
	}

	public void OnBtnSelected(){
		if(BattleGame.instance.gameSkill.GetCount(_currentIndex) > 0)
		{
			BattleGame.instance.gameSkill.Select(_currentIndex);
//			BattleGame.instance.gameSkill.Use();
		}
		else
		{
			BattleGame.instance.gameSkill.Select(-1);
		}

	}

	public void DisableSkillBtn(){
//		this.skillData.skillConfig.isReleaseSkill = true;
		_toggle.Set(false);
		_skillBtn.isEnabled = false;
//		SkillManager.currentSkillID = "0";
	}


}
