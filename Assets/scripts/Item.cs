using System.Collections.Generic;
using UnityEngine;
using System;

public class Item
{
    private GameObject gObject;
    private int num;                                                // redni broj za listu u grafu
    private Vector3 position;
    private int value;
    private List<Item> neighbour = new List<Item>();

    Sprite sprite;

    public Item(GameObject obj, Vector3 position, int value)
    {
        gObject = obj;
        this.position = position;
        this.value = value;
    }

    public List<Item> getNeighbour()
    {
        return neighbour;
    }

    public void setNeighbour(Item value)
    {
        neighbour.Add(value);
    }

    public int Num
    {
        get { return num; }
        set { num = value; }
    }

    public GameObject ItemObject
    {
        get { return gObject; }
        set { gObject = value; }
    }

    public Vector3 ItemPosition
    {
        get { return position; }
        set { position = value; }
    }

    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }

    public Sprite ItemSprite
    {
        get { return sprite; }
        set { this.sprite = value; }
    }

    public bool isSame(Item that)
    {
        if (ItemPosition == that.ItemPosition && gObject.name == that.gObject.name)
            return true;
        return false;
    }

    public string toString()
    {
        string s = "ime -> " + gObject.name + " pozicija -> " + position + " vrednost -> " + value + " redni broj -> " + num + " komsija -> ";
        foreach (Item koms in neighbour)
            s += koms.Num + "\n";

        return s;
    }
}