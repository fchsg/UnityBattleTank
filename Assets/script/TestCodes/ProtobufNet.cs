using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using System;

// protobuf PB
using PersonPB;


public class ProtobufNet {
	
	
//	private const String PATH = "c://data.bin";
	
	
	public ProtobufNet () {
		//生成数据
		List<TestPB> testData = new List<TestPB>();
		for (int i = 0; i < 100; i++)
		{
			testData.Add(new TestPB() { Id = i, data = new List<string>(new string[]{"1","2","3"}) });
		}

		//将数据序列化后存入本地文件
		Stream file = new MemoryStream ();
//		using(Stream file = File.Create(PATH))
		{
			Serializer.Serialize<List<TestPB>>(file, testData);
//			file.Close();
		}

		//将数据从文件中读取出来，反序列化
		List<TestPB> fileData;
//		using (Stream file = File.OpenRead(PATH))
		{
			file.Position = 0;
			fileData = Serializer.Deserialize<List<TestPB>>(file);
			file.Close();
		}

		//打印数据
		foreach (TestPB data in fileData)
		{
			Trace.trace(data.ToString());
		}

// test PersonPB
//		Person t1 = new Person ();
//		t1.name = "fch123";
//
//		Stream file2 = new MemoryStream ();
//		Serializer.Serialize<Person>(file2, t1);
//
//		Person t2;
//		file2.Position = 0;
//		t2 = Serializer.Deserialize<Person>(file2);
//		file2.Close();
//
//		Debug.Log (t2.name);
	}

}