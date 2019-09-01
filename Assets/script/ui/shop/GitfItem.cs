using UnityEngine;
using System.Collections;

public class GitfItem : ItemBase {
	private GameObject itmefb;
	public static int count = 0;
	public UILabel lable ;
	public override void SetData(object data)
	{
		base.SetData (data);
		print ("!!!!!!!!!!" + data.ToString ());

	}

	void CreateItem(){
		print ("************* creta");
		itmefb = (GameObject)Resources.Load ("profab/ui/Shop/giftItem");
		lable = itmefb.transform.Find ("name_Label").GetComponent<UILabel> ();
		print(" lable " + lable.name);
		lable.text = "label_";

	}
	void Awake(){
		CreateItem ();
	}

	void Start()
	{
		string name = itmefb.gameObject.name;
		print (" name  " + name);
		InitData ("jun");

	}
	public override Transform GetTransform ()
	{
		return base.GetTransform ();
	}
	public override GameObject GetGameObject ()
	{
		return itmefb;
	}
	public void  InitData(string name)
	{print("111133333331");
		Transform[] allChildren = itmefb.gameObject.GetComponents<Transform> ();
		print (allChildren.Length);
		foreach (Transform child in allChildren)
		{
			print("11111");
			if(child.gameObject.name.Equals("name_Label"))
			{
				print("22222");
				print("star 44");
				UILabel itemName_ = child.gameObject.GetComponent<UILabel>();
				count++;
				itemName_.text = "礼包 " + name;
			}else
			{
				print("test");
			}
			print("333333");
		}

	}
}
