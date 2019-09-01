using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_Tasks {

	private List<SlgPB.Task> _activeTasks = new List<SlgPB.Task>();
	public List<SlgPB.Task> activeTasks
	{
		get { return _activeTasks; }
	}

	public void ResetTasks(List<SlgPB.Task> tasks)
	{
//		_activeTasks.Clear ();
//		ListHelper.Push (_activeTasks, tasks);


		foreach (SlgPB.Task pbTask in tasks) 
		{
			int taskId = pbTask.taskId;
			if (Contain (taskId)) 
			{
				Remove (taskId);
			}
			_activeTasks.Add (pbTask);
		}

	}
		
	public SlgPB.Task GetTask(int index)
	{
		return _activeTasks [index];
	}

	public SlgPB.Task GetTaskWithId(int id)
	{
		foreach (SlgPB.Task task in _activeTasks) {
			if (task.taskId == id) {
				return task;
			}
		}
		return null;
	}

	public List<SlgPB.Task> CollectCompleteTasks()
	{
		List<SlgPB.Task> tasks = new List<SlgPB.Task> ();

		foreach (SlgPB.Task task in _activeTasks) {
			if (IsTaskComplete (task)) {
				tasks.Add (task);
			}
		}

		return tasks;
	}

	public bool IsTaskComplete(SlgPB.Task task)
	{
		DataTask dataTask = DataManager.instance.dataTaskGroup.GetTask (task.taskId);


		if (!dataTask.command.Equals ("ArenaRankings")) 
		{
			if (task.progress >= dataTask.totalProgress) 
			{
				return true;
			} 
			else
			{
				return false;
			}
		}
		else
		{
			if (task.progress <= dataTask.totalProgress) 
			{
				return true;
			} 
			else
			{
				return false;
			}
		}
	}

	public bool IsTaskComplete(int taskId)
	{
		SlgPB.Task task = GetTaskWithId (taskId);
		return IsTaskComplete (task);
	}


	public void Remove(int taskId)
	{
		SlgPB.Task task = GetTaskWithId (taskId);
		_activeTasks.Remove (task);
	}
		
	public bool Contain(int taskId)
	{
		foreach (SlgPB.Task task in _activeTasks) 
		{
			if (task.taskId == taskId) 
			{
				return true;
			}
		}
		return false;
	}

}
