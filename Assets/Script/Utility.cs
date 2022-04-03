using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static T[] ShuffletileArray<T>(T[] array, int seed) {
        System.Random pring = new System.Random (seed);
        for(int i = 0; i < array.Length -1; i++) {
            int randomIndex = pring.Next (i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }
}
