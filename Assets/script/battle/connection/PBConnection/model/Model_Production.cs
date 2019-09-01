using UnityEngine;
using System.Collections;

using SlgPB;

public class Model_Production {

	public enum Production_Type
	{
		Food = 7,
		Metal = 9,
		Oil = 8,
		Rare = 10,
		Cash = 11
	}

	public int resourceType;//资源类型
	private float _num;	    //当前持有的已生产的数量

	public float produceSpeed; 	//生产速度
	public float capacity;		//生产上限

	private long nextProductionTimeStamp; //下次更新生产时间

	public Model_Production()
	{
		InitTimer ();
	}

	public void Parse(Production production, int buildingLevel)
	{
		resourceType = production.resourceType;
		_num = (float)production.num;

		DataProduct dataProduct = new DataProduct();
		switch((Production_Type)resourceType)
		{
			case Production_Type.Food:
				dataProduct	= DataManager.instance.dataProductFoodGroup.GetProduct(buildingLevel);
				break;
			case Production_Type.Metal:
				dataProduct = DataManager.instance.dataProductMetalGroup.GetProduct(buildingLevel);
				break;
			case Production_Type.Oil:
				dataProduct	= DataManager.instance.dataProductOilGroup.GetProduct(buildingLevel);
				break;
			case Production_Type.Rare:
				dataProduct = DataManager.instance.dataProductRareGroup.GetProduct(buildingLevel);
				break;
			case Production_Type.Cash:
				dataProduct = DataManager.instance.dataProductCashGroup.GetProduct(buildingLevel);
				break;
		}
	
		if (dataProduct != null)
		{
			produceSpeed = dataProduct.produceSpeed;
			capacity = dataProduct.capacity;
		}

		nextProductionTimeStamp = TimeHelper.GetCurrentRealTimestamp ();

		ResumeTimer ();
	}
	
	private TimerEx productionTimer;
	public void InitTimer()
	{
		productionTimer = TimerEx.Init ("Production ", 1.0f, OnProductionTimer, true);
		if (productionTimer != null)
		{
			productionTimer.Pause ();
		}
	}
	
	public void StopTimer()
	{
		if (productionTimer != null) 
		{
			productionTimer.Stop ();
		}
	}

	public void PauseTimer()
	{
		if (productionTimer != null) 
		{
			productionTimer.Pause ();
		}
	}

	public void ResumeTimer()
	{
		if (productionTimer != null) 
		{
			productionTimer.Resume ();
		}
	}

	private void OnProductionTimer(System.Object parameter)
	{
		long cTimestamp = TimeHelper.GetCurrentRealTimestamp ();

		if (cTimestamp >= nextProductionTimeStamp)
		{
			float elapseTime = (float)(cTimestamp - nextProductionTimeStamp) / 1000.0f;
			float intervalSec = elapseTime + 1.0f;
			float produce = intervalSec * produceSpeed;
			_num = Mathf.Min(_num + produce, capacity);
			nextProductionTimeStamp = cTimestamp + 1000;

			//Trace.trace((Production_Type)resourceType + " num: " + _num, Trace.CHANNEL.INTEGRATION);
		}	
	}

	public int num
	{
		get { return Mathf.FloorToInt(_num); }
	}

	// 收取资源完成后 产出数量清零
	public void ResetProductionCount()
	{
		_num = 0;
	}

}
