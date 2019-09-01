using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_PvpUser {

	public int remainChallengeTimes = 0;

//	public int remainRefreshTimes = 0;
	public int refreshCost = 0;

	public SlgPB.PVPUser selfPvpUser = null;
	public List<SlgPB.PVPUser> pvpUsers = new List<SlgPB.PVPUser> ();

	public List<SlgPB.FightLog> pvpLogs = new List<SlgPB.FightLog>();


	public void Parser(SlgPB.GetPVPLadderResponse content)
	{
		remainChallengeTimes = content.remainFightNum;
		refreshCost = content.refreshPrice;

		selfPvpUser = content.PVPUser;
		SetPvpUsers (content.opponents);
	}

	public void Parser(SlgPB.RefreshPVPLadderResponse content)
	{
		refreshCost = content.refreshPrice;

		SetPvpUsers (content.opponents);
	}

	public void SetPvpUsers(List<SlgPB.PVPUser> pvpUsers)
	{
		this.pvpUsers = pvpUsers;
	}


	public void PushPvpUser(SlgPB.PVPUser user)
	{
		for (int i = 0; i < pvpUsers.Count; ++i) {
			SlgPB.PVPUser originUser = pvpUsers [i];
			if (originUser.rank == user.rank) {
				pvpUsers [i] = user;
				return;
			}
		}

		pvpUsers.Add (user);
	}

//	public void ConsumeRefreshTicket()
//	{
//		remainRefreshTimes--;
//		Assert.assert (remainRefreshTimes >= 0);
//	}

	public void SubChallengeTimes(int times)
	{
		remainChallengeTimes -= times;
		Assert.assert (remainChallengeTimes >= 0);
	}

	// ======================================================
	// rank

	public List<SlgPB.PVPUser> pvpRankUsers = new List<SlgPB.PVPUser> ();

	public void SetPvpRankUsers(List<SlgPB.PVPUser> pvpUsers)
	{
		this.pvpRankUsers = pvpUsers;
	}



}


