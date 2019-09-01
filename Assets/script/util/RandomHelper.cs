using UnityEngine;
using System;
using System.Collections.Generic;


public class RandomHelper {

	private static System.Random random = new System.Random (DateTime.Now.Millisecond);

	public static void SetSeed(int seed)
	{
		random = new System.Random(seed);
	}

	public static float Range01()
	{
//		long tick = DateTime.Now.Ticks;
//		seed = (int)(tick & 0xffffffffL) | ((int)tick >> 32);
//		
//		System.Random random = new System.Random (seed);

		float rand = (float)random.NextDouble ();
		return rand;
	}
	
	public static float Range(float minInclude, float maxExclude)
	{
//		long tick = DateTime.Now.Ticks;
//		seed = (int)(tick & 0xffffffffL) | ((int)tick >> 32);
//		
//		System.Random random = new System.Random (seed);

		float v = (float)random.NextDouble ();
		float d = maxExclude - minInclude;
		float rand = minInclude + v * d;
		return rand;
	}

	public static void Shuffle(ref int[] numbers)
	{
		int count = numbers.Length;
		for (int i = 0; i < count; ++i) 
		{
			int rand = random.Next(count);
			int temp = numbers[i];
			numbers[i] = numbers[rand];
			numbers[rand] = temp;
		}
	}

	public static void Shuffle(ref List<int> numbers)
	{
		int count = numbers.Count;
		for (int i = 0; i < count; ++i) 
		{
			int rand = random.Next(count);
			int temp = numbers[i];
			numbers[i] = numbers[rand];
			numbers[rand] = temp;
		}
	}

	/*
	public static int Range(int minInclude, int maxExclude)
	{
//		long tick = DateTime.Now.Ticks;
//		seed = (int)(tick & 0xffffffffL) | ((int)tick >> 32);
//		
//		System.Random random = new System.Random (seed);
		int rand = random.Next (minInclude,maxInclude);
		return rand;
	}
	*/



}
