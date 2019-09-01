using UnityEngine;
using System.Collections;

public class InitObject : MonoBehaviour 
{
	public UISprite proxyIcon;
//	public EquipPanel equipPanel;
//	public BackpackPanel backpackPanel;
//	public SkillPanel skillPanel;
//	public ShortcutPanel shortcutPanel;

	void Awake()
	{
		UDragDropManager.Init (proxyIcon);
//		// 初始化配置文件
//		ItemConfigManager.Init ();
//		// 初始化技能配置文件
//		SkillConfigManager.Init ();
//		// 初始化装备数据
//		EquipManager.Init ();
//		// 初始化物品数据
//		ItemManager.Init ();
//		// 初始化技能数据
//		SkillManager.Init ();
//		// 初始化快捷菜单数据
//		ShortcutManager.Init ();
//
//		this.equipPanel.ChangeData ();
//		this.backpackPanel.ChangeData ();
//		this.skillPanel.ChangeData ();
//		this.shortcutPanel.ChangeData ();
	}
}
