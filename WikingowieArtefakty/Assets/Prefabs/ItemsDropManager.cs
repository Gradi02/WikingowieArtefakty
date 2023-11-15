using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDropManager : MonoBehaviour
{
    public MapGenerator generator;
    public bool[,] isItem;


    private void Awake()
    {
        int size = generator.size;
        isItem = new bool[size,size];

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                isItem[i, j] = false;
    }
    public void SetItem(bool s, int x, int y)
    {
        isItem[x,y] = s;
    }

    public bool IsPlaceEmpty(int x, int y)
    {
        return isItem[x,y];
    }
}
