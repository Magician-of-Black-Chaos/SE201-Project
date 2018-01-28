using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Road;

    public GameObject player;                                                       // player objekat
    private List<Item> itemsV = SpawnItems.listItem;                                // lista vertex itema
    private static List<Item> listVisited = new List<Item>();
    public GameObject road;
    bool allowOneUpdate = true;

    Vector3 playerPos;                                                              // player pozicija

    /// <summary> 
    /// funkcija koja player objektu daje random poziciju na mapi 
    /// </summary> 
    public void playerSpawn()
    {
        playerPos = new Vector3(transform.position.x + Random.Range(-49f, 49f), 0f, transform.position.z + Random.Range(-49f, 49f));
        Instantiate(player, playerPos, Quaternion.identity);                        // postavljanje igraca na vec odredjenu poziciju
        player.transform.position = playerPos;
    }

    public void playerSpawn(Vector3 playerPos)
    {
        Instantiate(player, playerPos, Quaternion.identity);                        // postavljanje igraca na vec odredjenu poziciju
        player.transform.position = playerPos;
    }


    /// <summary>
    /// funkcija za nalazenje itema najblizeg playeru
    /// </summary>
    /// <param name="v">lista svih itema</param>
    Item findClosestToPlayer()
    {
        Item closest = itemsV[0];

        foreach (Item it in itemsV)
        {
            if (Vector3.Distance(closest.ItemPosition, playerPos) > Vector3.Distance(it.ItemPosition, playerPos))
            {
                closest = it;
            }
        }
        //print("najblizi item je: " + closest.toString());

        return closest;
    }

    /// <summary>
    /// funkcija koja sluzi za kreiranje zemljanog puta izmedju cvorova
    /// </summary>
    void spawnRoad()
    {
        for (int i = 0; i < listVisited.Count - 1; i++)                                                                     // prolazimo kroz sve elemente
        {
            for (int j = 0; j < listVisited[i].getNeighbour().Count; j++)                                                   // prolazimo kroz sve komsije elementa i
            {
                // TODO - da postavim uslov da se ne ponavlja  prm -> 1-8 | 8-1
                if (true)
                {
                    Vector3 line = listVisited[i].getNeighbour()[j].ItemPosition - listVisited[i].ItemPosition;             // ne znam po kojoj logici ali ovo izvlaci liniju

                    for (int dist = 0; dist < line.magnitude; dist += 2)
                    {                                              // inicijalizuje se put na toj liniji na udaljenosti od 2, -vektor je dodat da bi put spustio na visinu platforme
                        Instantiate(road, listVisited[i].ItemPosition + line.normalized * dist, Quaternion.identity);
                    }
                }
            }
            // ovo dodajem da poslednji element ima platformu ispod sebe
            if (i == listVisited.Count - 1)
                Instantiate(road, listVisited[i].ItemPosition, Quaternion.identity);
        }

        // napravi put od playera do prvog itema
        Item start = findClosestToPlayer();
        Vector3 lineP = player.transform.position - start.ItemPosition;
        //print(player.transform.position + " pozicija na kojoj je player, pozicija na kojoj je item-> " + start.ItemPosition + " a ovo je lineP-> " + lineP);
        for (int dist = 0; dist < lineP.magnitude; dist += 2)
            Instantiate(road, start.ItemPosition + lineP.normalized * dist, Quaternion.identity);

        // postavi sve puteve na istu visinu
        GameObject[] go = GameObject.FindGameObjectsWithTag("road");
        foreach (GameObject gl in go)
        {
            gl.transform.position = new Vector3(gl.transform.position.x, 0.01f, gl.transform.position.z);
            gl.transform.eulerAngles = new Vector3(0, gl.transform.rotation.y, 0);
            gl.transform.SetParent(Road.transform);
        }
    }

    // Use this for initialization
    IEnumerator Start()
    {
        // inicijalizacija za svaki slucaj zbog menua
        listVisited = new List<Item>();
        itemsV = SpawnItems.listItem;

        // kraj menu inicijalizacije

        playerSpawn();
        yield return new WaitForEndOfFrame();                       // da se ostatak pozove na kraju frejma


        // ispisujemo listu itema, provera da li sve radi kako treba
        /*foreach (item it in itemsV)                               
        {
            print(it.toString());
        }*/

        Graph g = new Graph(itemsV);

        List<Edge> mst = new List<Edge>();                                   // minimum spenning tree

        // print("broj edge ->" + g.getListaEdge().Count + ", broj cvorova ->" + itemsV.Count);

        mst = g.Kruskal(itemsV, g.getListaEdge());                           // kreira se MST lista edge

        //  print("mst stablo edges:\n\n\n"+mst.Count);

        for (int i = 0; i < mst.Count; i++)                                  // stampa edges mst stabla i crta linije izmedju cvorova
        {
            //print(mst[i].toString());
            mst[i].DrawLine();
        }

        //print(mst.Count + " velicina MST stabla nakon kruskal algoritma");
        g.DFS(findClosestToPlayer());                                       // pretraga grafa krece od itema kojii je najblizi playeru

        listVisited = g.getVisitedItems();                       // lista kojim redom su cvorovi grafa posecivani

        spawnRoad();
        /*
        //yield return new WaitForSeconds(5);
        //------------->>> TEST <<<----------------------
        print("Testiram spawnPlayer listVisited listu");       //------> lista koju dobijam je skroz losa
        foreach (item ite in listVisited)
        {
            print(ite.toString());
        }
        print("Kraj testa");
        //------------->>> KRAJ TESTA <<<----------------
        */
    }

    public static List<Item> getVisitedList()
    {
        return listVisited;
    }


    // Update is called once per frame
    void Update()
    {
        if (allowOneUpdate)
        {
            itemsV = SpawnItems.listItem;
            allowOneUpdate = false;
        }
        /*
        //------------->>> TEST <<<----------------------
        print("Testiram spawnPlayer itemsV listu - update");       //------> lista koju dobijam je skroz losa
        foreach (item ite in itemsV)
        {
            print(ite.toString());
        }
        print("Kraj testa");
        //------------->>> KRAJ TESTA <<<----------------
        */
    }
}
