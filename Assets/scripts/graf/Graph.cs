using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private int _v;                                               // broj cvorova
    private int _e;                                               // broj grana
    private List<Item> listItems;                                 // lista koju dobijam iz konstruktora, treba mi za dfs                                                              
    private List<Edge> listaEdges = new List<Edge>();             // lista povezanosti, probacu na ovaj nacin
    private List<Item> visitedItemsArray;                         // niz kojim su redom cvorovi poseceni

    /// <summary> konstruktor praznog grafa </summary>
    /// <param name="size"> broj cvorova u grafu </param>
    public Graph(List<Item> v)
    {
        if (v.Count < 0)
            throw new System.Exception("Velicina grafa ne moze biti negativan broj");
        else
        {
            createGraph(v);
            _v = v.Count;
            _e = 0;
            listItems = v;
            visitedItemsArray = new List<Item>(12);
        }
    }

    /// <summary> 
    /// kreiraju se graf i putevi izmedju itema
    /// </summary>
    /// <param name="vertex"> lista kreiranih itema </param>
    void createGraph(List<Item> vertex)
    {
        int rand = 0;
        for (int i = 0; i < vertex.Count; i++)
        {
            int numOfEdges = Random.Range(vertex.Count / 4, vertex.Count / 2);
            for (int k = 0; k < numOfEdges; k++)
            {
                do
                {
                    rand = Random.Range(0, vertex.Count);
                } while (i == rand);

                Edge e = new Edge(vertex[i], vertex[rand], Vector3.Distance(vertex[i].ItemPosition, vertex[rand].ItemPosition));

                AddEdge(e);
            }
        }
    }

    /// <returns> velicina grafa - broj cvorova </returns>
    public int V
    {
        get { return _v; }
    }

    public int E
    {
        get { return _e; }
    }

    /// <summary>
    /// za ovaj novi pokusaj ako ne radi da obrisem
    /// </summary>
    /// <returns>lista povezanih cvorova</returns>
    public List<Edge> getListaEdge()
    {
        return isort(listaEdges);                       // sortirana lista povezanih cvorova
    }

    /// <summary>
    /// dodaje se veza izmedju dva cvora
    /// </summary>
    /// <param name="e">veza izmedju dva cvora</param>
    public void AddEdge(Edge e)
    {
        listaEdges.Add(e);
        _e++;
    }

    public List<Item> getVisitedItems()
    {
        return visitedItemsArray;
    }

    public string toString()
    {
        string s = "Cvorovi grafa: " + _v + ", grane grafa: " + _e + "\n";
        return s;
    }

    /// <summary> sortiranje liste </summary>
    /// <param name="list">lista povezanosti koju sortiramo</param>
    /// <returns>sortirana lista po tezini cvorova</returns>
    List<Edge> isort(List<Edge> list)
    {
        int i, j, n = list.Count;
        Edge k;
        List<Edge> l = list;
        for (i = 1; i < n; i++)
            for (j = i; (j > 0) && (l[j].Weight < l[j - 1].Weight); j--)
            {
                k = l[j];
                l[j] = l[j - 1];
                l[j - 1] = k;
            }
        return l;
    }

    /// <summary>
    /// Kruskalov algoritam pretrage stabla
    /// </summary>
    /// <param name="vertex">lista cvorova</param>
    /// <param name="edges">lista veza izmedju tih cvorova</param>
    /// <returns>MST lista povezanosti</returns>
    public List<Edge> Kruskal(List<Item> vertex, List<Edge> lEdges)
    {
        // sortiranje edges liste
        List<Edge> edges = isort(lEdges);

        // lista roditelja
        List<Item> parent = vertex;

        // Minimum Spanning Tree list
        List<Edge> spanningTree = new List<Edge>();
        foreach (Edge edge in edges)
        {
            Item startNodeRoot = FindRoot(edge.StartNode, parent);
            Item endNodeRoot = FindRoot(edge.EndNode, parent);

            if (!startNodeRoot.isSame(endNodeRoot))
            {
                spanningTree.Add(edge);
                parent[endNodeRoot.Num] = startNodeRoot;
            }
        }
        createNeighbours(spanningTree);
        return spanningTree;
    }

    private void createNeighbours(List<Edge> spt)
    {
        foreach (Edge e in spt)
        {
            e.StartNode.setNeighbour(e.EndNode);
            e.EndNode.setNeighbour(e.StartNode);
        }
    }

    private static Item FindRoot(Item node, List<Item> parent)
    {
        Item root = node;
        while (!root.isSame(parent[root.Num]))
        {
            root = parent[root.Num];
        }

        return root;
    }


    // dfs metoda
    private void DFS(Item v, bool[] visited)
    {
        visited[v.Num] = true;
        // Debug.Log("Posecujem " + v.toString());

        visitedItemsArray.Add(v);

        List<Item> nbr = v.getNeighbour();
        for (int i = 0; i < nbr.Count; i++)
        {
            if (!visited[nbr[i].Num])
            {

                DFS(nbr[i], visited);
            }
        }
    }

    //dfs metoda
    public void DFS(Item start)
    {
        bool[] visited = new bool[listItems.Count];

        // Debug.Log("Krecemo od " + start.toString());
        DFS(start, visited);

        // test za listu - uspesno je proslo
        /* Debug.Log("Red kojim su itemi ispisani: ");
         for (int b = 0; b < visitedItemsArray.Count; b++)
         {
             Debug.Log(visitedItemsArray[b].toString());
         */
    }
}