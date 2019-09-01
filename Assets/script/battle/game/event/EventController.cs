using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController {

	public delegate void Handler(GameEvent e);

	private Dictionary<GameEvent.EVENT, List<Handler>> _map = new Dictionary<GameEvent.EVENT, List<Handler>>();

	public void AddHandler(GameEvent.EVENT id, Handler handler)
	{
		if (!_map.ContainsKey (id)) {
			_map [id] = new List<Handler>();
		}

		List<Handler> handlers = _map [id];
		Assert.assert (!handlers.Contains (handler));
		handlers.Add (handler);

	}

	public void RemoveHandler(GameEvent.EVENT id, Handler handler)
	{
		if (!_map.ContainsKey (id)) {
			_map [id] = new List<Handler>();
		}
		
		List<Handler> handlers = _map [id];
		handlers.Remove (handler);

	}
	
	public void Dispatch(GameEvent e)
	{
		if (_map.ContainsKey (e.id)) {
			List<Handler> handlers = _map[e.id];

			foreach(Handler handler in handlers)
			{
				handler(e);
			}
		}
	}

	public void RemoveAllHandleBelongTo(System.Object target)
	{
		foreach(KeyValuePair<GameEvent.EVENT, List<Handler>> keyValue in _map)
		{
			GameEvent.EVENT id = keyValue.Key;
			foreach(Handler handler in keyValue.Value)
			{
				if(handler.Target == target)
				{
					RemoveHandler(id, handler);
				}
			}
		}
	}

}
