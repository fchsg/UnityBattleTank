using UnityEngine;
using System.Collections;

public class StateMachine {

	private IState _currentState = null;
	public IState currentState
	{
		get { return _currentState; }
	}

	public void Change(IState state)
	{
		if(_currentState != null)
		{
			_currentState.Exit();
			_currentState = null;
		}

		_currentState = state;
		if(_currentState != null)
		{
			_currentState.Enter();
		}

	}

	public int Tick()
	{
		if (_currentState != null) {
			return _currentState.Tick();
		}
		return -1;
	}

}
