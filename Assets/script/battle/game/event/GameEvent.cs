using UnityEngine;
using System.Collections;

public class GameEvent {

	public enum EVENT
	{
//		SHOW_TEAM_CAMERA,

		// base event
		BASE_BUILDING_LEVELUP,
		BASE_BUILDING_UNLOCK,
		BASE_UNIT_FINISHPRODUCE,
		BASE_UNIT_FINISHREPAIR

	}

	private long _timestamp;
	public long timestamp
	{
		get { return _timestamp; }
	}
	
	private EVENT _id;
	public EVENT id
	{
		get { return _id; }
	}

	private System.Object _parameter;
	public System.Object parameter
	{
		get { return _parameter; }
	}

	public GameEvent(EVENT id, System.Object parameter)
	{
		_timestamp = TimeHelper.GetCurrentRealTimestamp ();

		_id = id;
		_parameter = parameter;
	}

	public GameEvent Clone()
	{
		GameEvent e = new GameEvent (id, parameter);
		e._timestamp = _timestamp;
		return e;
	}

}
