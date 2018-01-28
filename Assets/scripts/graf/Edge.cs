using System;
using UnityEngine;

public class Edge : IComparable<Edge>
{

    private readonly Item _v;
    private readonly Item _w;
    private readonly float _weight;

    /// <summary> kreira se kostruktor ivice sa tezinom </summary>
    /// <param name="_v"> polazni cvor </param>
    /// <param name="_w"> ciljani cvor </param>
    /// <param name="_weight"> tezina puta - udaljenost </param>
    public Edge(Item v, Item w, float weight)
    {
        this._v = v;
        this._w = w;
        this._weight = weight;
    }

    public void DrawLine()
    {
        Debug.DrawLine(_v.ItemPosition, _w.ItemPosition, Color.black, 10);
    }

    /// <returns> tezina puta </returns>
    public float Weight
    {
        get { return _weight; }
    }

    public Item StartNode
    {
        get { return _v; }
    }

    public Item EndNode
    {
        get { return _w; }
    }

    /// <summary> vrsi se komparacija izmedju dve putanje </summary>
    /// <param name="that"> duzina puta koju poredimo </param>
    /// <returns> da li je putanja veca od putanje that </returns>
    public int CompareTo(Edge that)
    {
        if (Weight < that.Weight)
            return -1;
        else if (Weight > that.Weight)
            return 1;
        return 0;
    }

    /// <returns> item.toString + item.toString + udaljenost izmedju ta dva itema </returns>
    public string toString()
    {
        return _v.toString() + "\n" + _w.toString() + "\n udaljenost-> " + _weight;
    }
}
