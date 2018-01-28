using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    private int MAX_ITEM_NUMBER = 100;                                  // maksimalni broj itema
    private int MIN_ITEM_NUMBER = 4;                                    // minimalni broj itema
    private int MINIMALNA_UDALJENOST_ITEMA = 4;                         // minimalna udaljenost izmedju 2 itema

    public GameObject Coins;

    public GameObject inventory;
    private CanvasRenderer cr;

    public int NumberOfItems;                                           // broj itema koji se kreiraju
    private int spawnFromItemAt;
    public List<GameObject> items;                                      // lista itema koji mogu biti kreirani
    public static List<Item> listItem = new List<Item>();               // lista koja sadrzi kreirane iteme
    public List<Sprite> sprites;                                        // lista sprite itema

    private List<Vector3> listPosition = new List<Vector3>();           // lista vektora pozicija na kojima se nalaze itemi, sluzi za proveru da se ne bi neki item spawnovao preko drugog

    private Item tmpItem;                                               // privremeni item koji se dodaje u listu itema

    /// <summary>
    /// proverava da distanca izmedju novog itema nije manja od minimalne u odnosu na pozicije ostalih itema
    /// </summary>
    /// <returns>pozicija itema za kreiranje</returns>
    private Vector3 spawnPosition()
    {
        float x, z;
        bool repeat = false;
        Vector3 itemPos = Vector3.zero;

        do
        {
            x = Random.Range(-49, 49);
            z = Random.Range(-49, 49);
            repeat = false;

            if (listPosition.Count == 0)
            {
                itemPos = new Vector3(x, 0.5f, z);
                listPosition.Add(itemPos);
            }
            else
            {
                foreach (Vector3 v3 in listPosition)
                {
                    if (Vector3.Distance(v3, new Vector3(x, 0.5f, z)) < MINIMALNA_UDALJENOST_ITEMA)
                    {
                        repeat = true;
                        break;
                    }
                }
                if (repeat == false)
                {
                    itemPos = new Vector3(x, 0.5f, z);
                    listPosition.Add(itemPos);
                }
            }
        } while (repeat);

        return itemPos;
    }

    /// <summary> metoda koja kreira iteme na random pozicijama </summary>
    /// <param name="numberOfItems"> broj itema koji se kreiraju</param>
    public void spawn(int numberOfItems)
    {
        if (numberOfItems == 0)
        {
            listItem.Reverse();
            return;
        }

        Vector3 itemPos = new Vector3(transform.position.x + Random.Range(-49, 49), 0.5f, transform.position.z + Random.Range(-49, 49));                // kreira random poziciju itema 
        itemPos = spawnPosition();

        int randItem = Random.Range(0, items.Count - 1);
        tmpItem = new Item(items[randItem], itemPos, Random.Range(1, 1000));                                                                            // kreira se item 
        tmpItem.Num = numberOfItems - 1;
        tmpItem.ItemSprite = sprites[randItem];
        tmpItem.ItemObject.name = "coin" + (numberOfItems - 1);
        Instantiate(tmpItem.ItemObject, tmpItem.ItemPosition, Quaternion.identity);                                                                     // item se kreira i postavlja na mapu

        // dodeli svakom coinu roditelja
        GameObject[] coinP = GameObject.FindGameObjectsWithTag("coin");
        foreach (GameObject cc in coinP)
        {
            cc.transform.parent = Coins.transform;
        }

        listItem.Add(tmpItem);                                                                                                                          // dodajemo item na listu itema
        spawn(numberOfItems - 1);                                                                                                                       // metoda se rekurzivno poziva
    }

    /// <summary>
    /// spawn itema iz liste
    /// </summary>
    /// <param name="list">lista itema koji se spawnuju</param>
    void spawn(List<Item> list, int spawnFrom)
    {
        for (int i = spawnFrom; i < list.Count; i++)//each (item itm in list)
            Instantiate(list[i].ItemObject, list[i].ItemPosition, Quaternion.identity);                                                                     // item se kreira i postavlja na mapu

        // dodeli svakom coinu roditelja
        GameObject[] coinP = GameObject.FindGameObjectsWithTag("coin");

        // TODO - ovde mi izbacuje unassigned gresku
        foreach (GameObject cc in coinP)
        {
            cc.transform.parent = Coins.transform;
        }                                                                                                                      // metoda se rekurzivno poziva
    }

    // Use this for initialization
    void Start()
    {
        cr = inventory.GetComponent<CanvasRenderer>();
        cr.SetAlpha(0);

        //NumberOfItems = LoadSceneOnClick.numOfItemsFromScene2;
        NumberOfItems = DB.Database.numOfCoins;

        listItem = new List<Item>();
        if (NumberOfItems < MIN_ITEM_NUMBER)
            NumberOfItems = MIN_ITEM_NUMBER;
        else if (NumberOfItems > MAX_ITEM_NUMBER)
            NumberOfItems = MAX_ITEM_NUMBER;

        spawn(NumberOfItems);
        /*
        // ----------->>> TEST <<<------------------
        print("Provera spawnItem liste listItem");      // ---- test je prosao
        foreach (item it in listItem)
            print(it.toString());
        print("Kraj testa");
        // ----------->>> KRAJ TESTA <<<------------
        */
    }

    // Update is called once per frame
    void Update()
    {/*
        // ----------->>> TEST <<<------------------
        print("Provera spawnItem liste listItem- update");      // ---- nakon drugog update redosled je jako los nesto
        foreach (item it in listItem)
            print(it.toString());
        print("Kraj testa");
        // ----------->>> KRAJ TESTA <<<------------
        */
    }
}
