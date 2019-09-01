using UnityEngine;
using System.Collections;

public interface IState {

	void Enter();
	void Exit();
	int Tick();

}
