using UnityEngine;
using System.Collections;

public class MainCityEffectPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnPlay(Model_Building model_building,Vector3 v)
	{
		
		if(model_building != null)
		{
			string iconName = "";
			int resNum = 0;
			switch (model_building.buildingType) 
			{
			case Model_Building.Building_Type.FoodFactory:
				iconName = "Food";
				break;
			case Model_Building.Building_Type.OilFactory:
				iconName = "Accel";
				break;
			case Model_Building.Building_Type.MetalFactory:
				iconName = "Metal";
				break;
			case Model_Building.Building_Type.RareFactory:
				iconName = "Rare";
				break;
			}

			if(model_building.model_Production.num != null && model_building.model_Production.num != 0)
			{
				resNum = model_building.model_Production.num;
				if(resNum > 50 && resNum <= 100)
				{
					iconName = iconName + "01"; 
				}
				else if(resNum > 100 && resNum <= 1000)
				{
					iconName = iconName + "02"; 
				}
				else if(resNum > 1000 && resNum <= 100000)
				{
					iconName = iconName + "03";
				}
				else
				{
					return;

				}
				model_building.model_Production.ResetProductionCount();
				StartCoroutine(PlayEffect(iconName,v));
			}
		}
	}

	// 播放粒子效果 
	IEnumerator PlayEffect(string name,Vector3 v)
	{ 
		string DUST_PARTICLE_PATH = "profab/ui/effect/" + name;

		GameObject parent = this.gameObject;
		GameObject instanceObj = ResourceHelper.Load (DUST_PARTICLE_PATH);

		Vector3 ui_world_pos = transform.TransformPoint (v);
		instanceObj.transform.localPosition = ui_world_pos;
		instanceObj.transform.parent = transform;
	
		// 添加粒子对象到UI管理容器
		UIParticleManager.instance.AddParticle (instanceObj);

		float leftTime = ParticleHelper.GetlifeCycleSec (instanceObj.transform);
		yield return new WaitForSeconds (leftTime);

		if (instanceObj != null) 
		{
			DestroyImmediate (instanceObj);
		} 
	}
}
