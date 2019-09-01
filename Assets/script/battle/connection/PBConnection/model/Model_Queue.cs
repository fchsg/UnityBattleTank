using UnityEngine;
using System.Collections;

using SlgPB;

public class Model_Queue {

	private int _buildingQueueMaxNum;	   //建筑建造队列数上限
	private int _buildingQueueUsedNum;	//建筑建造队使用数量

	private int _unitProduceQueueMaxNum;  //战斗单位生产队列数上限
	private int _unitProduceQueueUsedNum; //战斗单位生产队列使用数量

	private int _unitRepairQueueMaxNum;   //战斗单位维修队列数上限
	private int _unitRepairQueueUsedNum;  //战斗单位维修队列使用数量
	
	public Model_Queue()
	{
		_buildingQueueUsedNum = 0;
		_unitProduceQueueUsedNum = 0;
		_unitRepairQueueUsedNum = 0; 
	}

	public int buildingQueueMaxNum
	{
		get{ return _buildingQueueMaxNum; }
	}
	public int buildingQueueUsedNum
	{
		get{ return _buildingQueueUsedNum; }
	}
	
	public int unitProduceQueueMaxNum
	{
		get{ return _unitProduceQueueMaxNum; }
	}
	public int unitProduceQueueUsedNum
	{
		get{ return _unitProduceQueueUsedNum; }
	}
	
	public int unitRepairQueueMaxNum
	{
		get{ return _unitRepairQueueMaxNum; }
	}
	public int unitRepairQueueUsedNum
	{
		get{ return _unitRepairQueueUsedNum; }
	}

	public bool IsHadBuildingQueue()
	{
		return _buildingQueueUsedNum < _buildingQueueMaxNum ? true : false;
	}
	public void AddBuildingQueue()
	{
		++_buildingQueueUsedNum;
	}
	public void RemoveBuildingQueue()
	{
		--_buildingQueueUsedNum;
		_buildingQueueUsedNum = Mathf.Max (0, _buildingQueueUsedNum);
	}

	public bool IsHadUnitProduceQueue()
	{
		return _unitProduceQueueUsedNum < _unitProduceQueueMaxNum ? true : false;
	}
	public void AddUnitProduceQueue()
	{
		++_unitProduceQueueUsedNum;
	}
	public void RemoveUnitProduceQueue()
	{
		--_unitProduceQueueUsedNum;
		_unitProduceQueueUsedNum = Mathf.Max (0, _unitProduceQueueUsedNum);
	}

	public bool IsHadUnitRepairQueue()
	{
		return _unitRepairQueueUsedNum < _unitRepairQueueMaxNum ? true : false;
	}
	public void AddUnitRepairQueue()
	{
		++_unitRepairQueueUsedNum;
	}
	public void RemoveUnitRepairQueue()
	{
		--_unitRepairQueueUsedNum;
		_unitRepairQueueUsedNum = Mathf.Max (0, _unitRepairQueueUsedNum);
	}

	// 更新队列上限
	public void UpdateQueue(User user)
	{
		_buildingQueueMaxNum = user.buildingQueueNum;	   
		_unitProduceQueueMaxNum = user.unitProduceQueueNum;
		// 战斗维修队列最大为1
		_unitRepairQueueMaxNum = 1; 
	}
	
	/* 获取建筑临时队列价格
	 *  公式:
		临时队列价格=ceil((当前建筑所需升级时间*金钱换算比*比例系数)+((当前临时队列个数-1)*区间钻石系数))
		比例系数：0.1
		区间钻石系数：150
	 */
	public int GetTempBuildingQueueCash(int buildingId, int buildingLevel)
	{
		if (_buildingQueueUsedNum < _buildingQueueMaxNum) 
		{
			return -1;  // 没有用临时队列
		}

		DataBuilding building = DataManager.instance.dataBuildingGroup.GetBuilding(buildingId, buildingLevel);
		float buildingCashFactor = DataManager.instance.dataInitialConfigGroup.GetDataInitialConfig ().buildingCashFactor;
		int tempBuildingCount = _buildingQueueUsedNum - _buildingQueueMaxNum + 1;

		return Mathf.CeilToInt ((building.cost.costTime * buildingCashFactor * 0.1f) + ((tempBuildingCount - 1) * 150.0f));
	}
}
