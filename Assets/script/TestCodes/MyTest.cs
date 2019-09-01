using UnityEngine;
using System.Collections;
using System.IO;


public class MyTest {

	public MyTest()
	{
		Test ();

//		CreateTestData();
	}

	private void Test()
	{
		new ProtobufNet ();
		
//		TestSendPB ();
		
		//		TestSendHttp ();
		
	}
	
	/*
	private void TestSendPB()
	{
		Test.Test testData = new global::Test.Test ();
		testData.id = 1234;
		testData.result = 4321;
		testData.name = "ding.ning";
		testData.names.Add ("ABC");
		testData.names.Add ("CBA");
		testData.user = new TestUser.TestUser ();
		testData.user.coin = 789;
		testData.user.cash = 987;
		testData.user.uid = 100;
		testData.user.name = "user";
		testData.user.energy = 1000;
		
		
		if (true) {
			(new PCConnect_test ()).Send (testData, OnTestPBConnectComplete);
			
		} else {
			
			MemoryStream stream = new MemoryStream ();
			ProtoBuf.Serializer.Serialize<Test.Test>(stream, testData);
			
			stream.Position = 0;
			byte[] data = new byte[stream.Length];
			stream.Read (data, 0, (int)stream.Length);
			
			HttpClientHelper.Send("http://alpha.tank.api.zjfygames.com/test.php", data, null, false);
		}
		
	}

	private void OnTestPBConnectComplete(bool success, System.Object content)
	{
		Trace.trace ("PB connect success, connect = " + content);
	}
	

	*/
	
	private void TestSendHttp()
	{
		for (int i = 0; i < 1; ++i) {
			HttpClientHelper.Send("http://hq.sinajs.cn/list=sz000001", new byte[]{0}, OnTestHttpComplete, false);
		}
	}
	
	private int cbSuccessTimes = 0;
	private int cbFailTimes = 0;
	void OnTestHttpComplete(bool success, byte[] data)
	{
		if (success) {
			++cbSuccessTimes;
		} else {
			cbFailTimes++;
		}
		
		Trace.trace ("http client test, success = " + cbSuccessTimes + ",\tfail = " + cbFailTimes, Trace.CHANNEL.HTTP);
	}




	/*
	void CreateTestData()
	{
		//add player test data
		InstancePlayer.instance.playerArmy = new InstancePlayerArmy ();
		InstancePlayer.instance.playerArmy.memberCount = 6;  // test player member conunt
		int memberCount = InstancePlayer.instance.playerArmy.memberCount;
		
		InstancePlayer.instance.playerArmy.unitId = new int[memberCount];
		InstancePlayer.instance.playerArmy.unitCount = new int[memberCount];
		
		for (int i = 0; i < memberCount; i++) 
		{
			InstancePlayer.instance.playerArmy.unitId [i] = (int)RandomHelper.Range(1, 3);
			InstancePlayer.instance.playerArmy.unitCount [i] = 1;
		}
		
		
		//
		DataManager.instance.InitData ();
		
	}
	*/

}
