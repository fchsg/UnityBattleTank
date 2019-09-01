using UnityEngine;
using System.Collections;
using System;

public class ArrayHelper {


	public static T[] Fill<T>(T[] source, T value, int count)
	{
		T[] dest = new T[source.Length + count];

		Fill<T> (source, ref dest, 0, source.Length);

		for (int i = 0; i < count; ++i) {
			dest [source.Length + i] = value;
		}

		return dest;
	}

	public static void Fill<T>(T[] source, ref T[] destination, int at, int n)
	{
		for (int i = 0; i < n; ++i) {
			destination [i + at] = source [i];
		}
	}


	public static T[] Clone<T>(T[] array) where T : ICloneable
	{
		Assert.assert (array.Length > 0, "ArrayHelper Clone error");

		T[] cloneArray = new T[array.Length];
		for (int i = 0; i < array.Length; ++i)
		{
			cloneArray[i] = (T)array[i].Clone();
		}
		return cloneArray;
	}

	public static T[][] Clone<T>(T[][] array) where T : ICloneable
	{	
		Assert.assert ((array.Length > 0), "ArrayHelper Clone error");

		int row = array.Length;
		T[][] cloneArray = new T[row][];
		for (int i = 0; i < row; ++i)
		{
			int column = array[i].Length;
			cloneArray[i] = new T[column];
			for (int j = 0; j < column; ++j)
			{
				cloneArray[i][j] = (T)array[i][j].Clone();
			}
		}
		return cloneArray;
	}

	public static T[] Concat<T>(T[] a, T[] b)
	{
		Assert.assert ((a.Length > 0 && b.Length > 0), "ArrayHelper Concat error");

		int aLen = a.Length;
//		int aLen8 = (int)(aLen >> 3) << 3;

		int bLen = b.Length;
//		int bLen8 = (int)(bLen >> 3) << 3;

		T[] result = new T[aLen + bLen];
		a.CopyTo (result, 0);
		b.CopyTo (result, aLen);

		/*
		int i;
		int pos = 0;

		i = 0;
		while(i < aLen8)
		{
			result[pos++] = a[i++];
			result[pos++] = a[i++];
			result[pos++] = a[i++];
			result[pos++] = a[i++];
			result[pos++] = a[i++];
			result[pos++] = a[i++];
			result[pos++] = a[i++];
			result[pos++] = a[i++];
		}
		while(i < aLen)
		{
			result[pos++] = a[i++];
		}

		i = 0;
		while (i < bLen8) {
			result[pos++] = b[i++];
			result[pos++] = b[i++];
			result[pos++] = b[i++];
			result[pos++] = b[i++];
			result[pos++] = b[i++];
			result[pos++] = b[i++];
			result[pos++] = b[i++];
			result[pos++] = b[i++];
		}
		while (i < bLen) {
			result[pos++] = b[i++];
		}
		*/

		return result;
	}


}
