using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ListHelper {

	public static List<T> Clone<T>(List<T> list) where T : ICloneable
	{
		Assert.assert (list.Count > 0, "ListHelper Clone error");
		
		List<T> cloneList = new List<T> ();

		foreach (T element in list) 
		{
			cloneList.Add((T)element.Clone());
		}
		return cloneList;
	}

	public static List<T> Concat<T>(List<T> a, List<T> b)
	{
		Assert.assert (a.Count > 0 && b.Count > 0, "ListHelper Concat error");

		List<T> result = new List<T>();

		foreach (T element in a) 
		{
			result.Add(element);
		}

		foreach (T element in b) 
		{
			result.Add(element);
		}

		return result;
	}

	public static void Push<T>(List<T> destination, List<T> source)
	{
		foreach (T element in source) 
		{
			destination.Add(element);
		}

	}

	public static void Push<T>(List<T> destination, T[] source)
	{
		foreach (T element in source) 
		{
			destination.Add(element);
		}

	}

	public static void Push<T>(List<T> destination, List<T> source, int pos, int n)
	{
		for (int i = 0; i < n; ++i) {
			destination.Add (source [pos++]);
		}

	}


}
