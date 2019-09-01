using UnityEngine;
using System.Collections;

public class Model_Energy {

	public const int ENERGY_MAX = 200;
	public const int ENERGY_RECOVER_INTERVAL_SEC = 180;
	public const int ENERGY_RECOVER_INTERVAL_MILLSEC = ENERGY_RECOVER_INTERVAL_SEC * 1000;
	
	public int energy;//当前体力
	public long nextEnergyRecoverTimestamp;//下一次获得体力需要的秒数

	public Model_Energy()
	{
		InitTimer ();
	}

	public void PauseRecoverEnergyTimer()
	{
		if (recoverEnergyTimer != null) {
			recoverEnergyTimer.Pause();
		}
	}
	
	public void ResumeRecoverEnergyTimer()
	{
		if (recoverEnergyTimer != null) {
			recoverEnergyTimer.Resume();
		}
	}
	
	private TimerEx recoverEnergyTimer;
	private void OnRecoverEnergy(System.Object parameter)
	{
		long cTime = TimeHelper.GetCurrentRealTimestamp ();
		if (nextEnergyRecoverTimestamp > 0 && cTime >= nextEnergyRecoverTimestamp) {
			long dTime = cTime - nextEnergyRecoverTimestamp;
			int n = (int)(dTime / ENERGY_RECOVER_INTERVAL_MILLSEC) + 1;
			energy = Mathf.Min(energy + n, ENERGY_MAX);
			nextEnergyRecoverTimestamp += n * ENERGY_RECOVER_INTERVAL_MILLSEC;
		}	
	}
	
	private void InitTimer()
	{
		recoverEnergyTimer = TimerEx.Init ("RecoverEnergy", 1.0f, OnRecoverEnergy, true);
		recoverEnergyTimer.Pause ();
	}

	public void SubEnergy(int n)
	{
		energy -= n;
		Assert.assert (energy >= 0);
	}

}
