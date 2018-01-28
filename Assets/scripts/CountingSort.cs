using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingSort
{

    public CountingSort()
    {
    }

    /// <summary>
    /// Counting sort -> (n+k) - od najmanjeg ka najvecem
    /// </summary>
    /// <param name="arr">niz integera koji sortiram</param>
    /// <returns>sortirani niz itema</returns>
    public Item[] Sort(Item[] arr)
    {
        Item[] sortedArray = new Item[arr.Length];

        // naci najmanju i najvecu vrednost
        int minVal = arr[0].Value, maxVal = arr[0].Value;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i].Value < minVal)
                minVal = arr[i].Value;
            else if (arr[i].Value > maxVal)
                maxVal = arr[i].Value;
        }

        // kreiram neki niz
        int[] counts = new int[maxVal - minVal + 1];

        // popuni niz
        for (int i = 0; i < arr.Length; i++)
        {
            counts[arr[i].Value - minVal]++;
        }

        counts[0]--;
        for (int i = 1; i < counts.Length; i++)
        {
            counts[i] = counts[i] + counts[i - 1];
        }

        // sortiraj niz
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            sortedArray[counts[arr[i].Value - minVal]--] = arr[i];
        }

        return sortedArray;
    }

    /// <summary>
    /// Counting sort -> (n+k) - od najveceg ka najmanjem
    /// </summary>
    /// <param name="arr">niz integera koji sortiram</param>
    /// <returns>sortirani niz itema</returns>
    public Item[] SortToLow(Item[] arr)
    {
        Item[] sortedArray = new Item[arr.Length];

        // naci najmanju i najvecu vrednost
        int minVal = arr[0].Value, maxVal = arr[0].Value;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i].Value < minVal)
                minVal = arr[i].Value;
            else if (arr[i].Value > maxVal)
                maxVal = arr[i].Value;
        }

        // kreiram neki niz
        int[] counts = new int[maxVal - minVal + 1];

        // popuni niz
        for (int i = 0; i < arr.Length; i++)
        {
            counts[maxVal - arr[i].Value]++;
        }

        counts[0]--;
        for (int i = 1; i < counts.Length; i++)
        {
            counts[i] = counts[i] + counts[i - 1];
        }

        // sortiraj niz
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            sortedArray[counts[maxVal - arr[i].Value]--] = arr[i];
        }

        return sortedArray;
    }
}
