using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataNotificationStorage {

	private static string EXPECT_VERSION = "_VERSION_" + 5;

	private static int _gotNotificationId = 0;
	public static int gotNotificationId
	{
		get { return _gotNotificationId; }
	}

	private static int _userId = -1;

	private static string GetLocalDynamicUrl()
	{
		Assert.assert (_userId >= 0);

		string localUrl = DynamicFileControl.GetDynamicDataFolder() + "notification_storage" + _userId + ".bin";
		return localUrl;
	}

	private static XmlSerializer GetXmlSerializer()
	{
		//创建XML序列化器，需要指定对象的类型
		XmlSerializer xmlFormat = new XmlSerializer (
			                          typeof(List<SlgPB.Notify>),
			                          new System.Type[] { typeof(SlgPB.Notify), typeof(SlgPB.PrizeItem), typeof(List<SlgPB.PrizeItem>) }
		                          );
		return xmlFormat;
	}

	private static BinaryFormatter GetBinaryFormatter()
	{
		BinaryFormatter formatter = new BinaryFormatter ();
		return formatter;
	}

	public static bool SaveNotification(List<SlgPB.Notify> notifications)
	{
//		XmlSerializer xmlFormat = GetXmlSerializer();
		BinaryFormatter binFormatter = GetBinaryFormatter ();

		try
		{
			string localUrl = GetLocalDynamicUrl ();
			FileStream stream = new FileStream(localUrl, FileMode.OpenOrCreate);

			if(notifications.Count > 0)
			{
				SlgPB.Notify lastNotification = notifications[notifications.Count - 1];
				_gotNotificationId = Mathf.Max(_gotNotificationId, lastNotification.notifyId);
			}

//			xmlFormat.Serialize(stream, notifications);
			binFormatter.Serialize(stream, notifications);

			StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.WriteLine(EXPECT_VERSION);
			streamWriter.WriteLine(_gotNotificationId);
			streamWriter.Flush();


//			streamWriter.Close();
			stream.Close();

			Trace.trace (
				"save local notifications, count = " + notifications.Count + ", gotNotificationId = " + _gotNotificationId,
				Trace.CHANNEL.IO);

			return true;
		}
		catch(System.Exception e)
		{
			Trace.trace(e.ToString(), Trace.CHANNEL.IO);
			return false;
		}

		/*

		Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);

		XmlSerializer xmlFormat = new XmlSerializer(

			typeof(List<Programmer>),

			new Type[] { typeof(Programmer),typeof(Person) }

		);//创建XML序列化器，需要指定对象的类型

		xmlFormat.Serialize(fStream, list);



		System.IO.MemoryStream stream = new System.IO.MemoryStream ();
		xmlFormat.Serialize(stream, notifications);
		string content = stream.ToString ();
		PlayerPrefs.SetString ("notificationDatabase", content);
		*/

	}

	public static void Init(int userId)
	{
		Assert.assert (userId >= 0);
		Assert.assert (_userId < 0);
		_userId = userId;
	}

	public static List<SlgPB.Notify> LoadNotification()
	{

		List<SlgPB.Notify> notifications = new List<SlgPB.Notify>();

		try
		{
			string localUrl = GetLocalDynamicUrl ();
			FileStream stream = new FileStream (localUrl, FileMode.Open);
			stream.Position = 0;

//			XmlSerializer xmlFormat = GetXmlSerializer();
//			notifications = xmlFormat.Deserialize(stream) as List<SlgPB.Notify>;

			BinaryFormatter binFormatter = GetBinaryFormatter ();
			List<SlgPB.Notify> _notifications = binFormatter.Deserialize(stream) as List<SlgPB.Notify>;

			StreamReader streamReader = new StreamReader(stream);
			string _version = streamReader.ReadLine();
			int _notificationId = int.Parse(streamReader.ReadLine());
			if(_version == EXPECT_VERSION)
			{
				notifications = _notifications;
				_gotNotificationId = _notificationId;
			}

//			streamReader.Close();
			stream.Close();

			Trace.trace (
				"load local notifications, count = " + notifications.Count + ", gotNotificationId = " + _gotNotificationId,
				Trace.CHANNEL.IO);
		}
		catch(System.Exception e)
		{
			Trace.trace(e.ToString(), Trace.CHANNEL.IO);
		}

		return notifications;

	}


}
