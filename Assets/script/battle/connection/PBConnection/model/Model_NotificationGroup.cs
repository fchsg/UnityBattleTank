using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_NotificationGroup {

	public Dictionary<int, SlgPB.Notify> notificationsMap = new Dictionary<int, SlgPB.Notify> ();

	public void Push(SlgPB.GetNotifyResponse response)
	{
		foreach (SlgPB.Notify n in response.notifies) {
			Add (n);
		}

		Save ();
	}

	public void Save()
	{
		DataNotificationStorage.SaveNotification (GetAllNotifications ());
	}

	public void Load()
	{
		List<SlgPB.Notify> notifications = DataNotificationStorage.LoadNotification ();
		foreach (SlgPB.Notify n in notifications) {
			Add (n);
		}
	}

	public void Add(SlgPB.Notify n)
	{
		notificationsMap [n.notifyId] = n;
	}

	public void Delete(int id)
	{
		if (CanBeDel (id)) {
			notificationsMap.Remove (id);
			Save ();
		}
	}

	public void Delete(int[] ids)
	{
		foreach (int id in ids) {
			if (CanBeDel (id)) {
				notificationsMap.Remove (id);
			}
		}
		Save ();
	}

	public bool IsRead(int id)
	{
		return notificationsMap [id].isRead > 0;
	}

	public void MarkRead(int id)
	{
		notificationsMap [id].isRead = 1;
		Save ();
	}

	public void MarkRead(int[] ids)
	{
		foreach (int id in ids) {
			notificationsMap [id].isRead = 1;
		}
		Save ();
	}

	public bool IsDisposed(int id)
	{
		return notificationsMap [id].disposed > 0;
	}

	public void MarkDisposed(int id)
	{
		notificationsMap [id].disposed = 1;
		Save ();
	}

	public void MarkDisposed(int[] ids)
	{
		foreach (int id in ids) {
			notificationsMap [id].disposed = 1;
		}
		Save ();
	}

	public bool HasBonus(int id)
	{
		SlgPB.Notify n = notificationsMap [id];
		if (n.prizeItems.Count > 0) {
			return !IsDisposed (id);
		}
		return false;
	}

	public bool CanBeDel(int id)
	{
		return IsRead (id) && !HasBonus (id);
	}

	public int[] CollectCanBeDelIds()
	{
		List<int> ids = new List<int> ();

		foreach (int id in notificationsMap.Keys) {
			if (CanBeDel (id)) {
				ids.Add (id);
			}
		}

		return ids.ToArray ();
	}

	public List<SlgPB.Notify> GetAllNotifications()
	{
		List<SlgPB.Notify> notifications = new List<SlgPB.Notify> ();
		foreach (SlgPB.Notify n in notificationsMap.Values) {
			notifications.Add (n);
		}
		return notifications;
	}

}
