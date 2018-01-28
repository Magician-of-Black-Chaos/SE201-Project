using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    public Transform selectedItem, selectedSlot, originalSlot;

    public GameObject slotPrefab, itemPrefab;
    public Vector2 inventorySize = new Vector2(10, 1);      //  koliko ima redova i kolona slotova
    public Vector2 inventorySizePix;                        // velicina inventory panela u kome se slotovi prikazuju
    public Vector2 windowSize;
    private List<Item> listItem, listItemChangeable;
    private int checkSize = 0;


    // Use this for initialization
    void Start()
    {

        inventorySize = DB.Database.inventorySize;
        inventorySizePix = DB.Database.windowsSizePix;
        // yield return new WaitForSeconds(1);
        transform.GetComponent<RectTransform>().sizeDelta = inventorySizePix;
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(inventorySizePix.x / 2, -inventorySizePix.y / 2);

        listItem = new List<Item>();
        listItemChangeable = new List<Item>();
    }

    /// <summary>
    /// iscrtavanje slotova i dodavanje itema na njih
    /// </summary>
    /// <param name="list">lista itema koje dodajemo na slotove</param>
    private void AddToInventory(List<Item> list)
    {
        for (int x = 0; x < inventorySize.x; x++)
        {
            for (int y = 0; y < inventorySize.y; y++)
            {
                GameObject slot = Instantiate(slotPrefab) as GameObject;
                slot.transform.SetParent(this.transform, false);
                slot.name = "slot_" + x + "_" + y;
                // postavljanje anchora slota
                slot.GetComponent<RectTransform>().anchorMax = new Vector2(0.05f, 0.95f - (0.1f - inventorySize.y / 100));
                slot.GetComponent<RectTransform>().anchorMin = new Vector2(0.05f, 0.95f - (0.1f - inventorySize.y / 100));

                slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(windowSize.x / (inventorySize.x) * x, windowSize.y / 10 * -y);


                // uslov da je broj itema u listi itema koji se dodaju u inventory manji od 
                if ((x + 1 + y * inventorySize.x) <= list.Count)
                {
                    GameObject item1 = Instantiate(itemPrefab) as GameObject;
                    item1.transform.SetParent(slot.transform, false);
                    item1.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    ItemController i = item1.GetComponent<ItemController>();


                    i.Value = list[x + y * ((int)inventorySize.x)].Value; // listaItemaKojeDodajemUInventory[x+y*4].Value;
                    i.Name = list[x + y * ((int)inventorySize.x)].ItemObject.name; // kaoGore[x+y*4].ItemObject.name;
                    i.ItemSprite = list[x + y * ((int)inventorySize.x)].ItemSprite; // istoKaoGore[x+y*4].ItemSprite;

                    item1.name = i.Name + "(" + (x + y * (inventorySize.x - 1)) + ")";
                    item1.transform.GetChild(0).GetComponent<Text>().text = i.Value + "";
                    item1.GetComponent<Image>().sprite = i.ItemSprite;
                }
            }
        }
    }

    /// <summary>
    /// obrisi sve slotove
    /// </summary>
    private void DeleteSlots()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("slot");
        foreach (GameObject s in slots)
            Destroy(s);
    }

    /// <summary>
    /// dodaj novi item u regularnu listu i u listu koja se sortira
    /// </summary>
    /// <param name="it">item za dodavanje</param>
    public void AddItem(Item it)
    {
        DeleteSlots();
        listItem.Add(it);
        listItemChangeable.Add(it);

        AddToInventory(listItemChangeable);
    }

    /// <summary>
    /// sortiraj od najveceg do najmanjeg
    /// </summary>
    public void SortToLowest()
    {
        if (listItemChangeable.Count != 0 && listItemChangeable != null)
        {
            DeleteSlots();

            #region sortiranje itema za upis u inventory
            Item[] sortedItems = new Item[listItemChangeable.Count];
            for (int i = 0; i < listItemChangeable.Count; i++)
            {
                sortedItems[i] = listItemChangeable[i];
            }
            CountingSort countSort = new CountingSort();
            sortedItems = countSort.SortToLow(sortedItems);

            for (int i = 0; i < sortedItems.Length; i++)
            {
                listItemChangeable[i] = sortedItems[i];
            }
            //listItemChangeable.Reverse();
            #endregion

            AddToInventory(listItemChangeable);
        }
    }

    /// <summary>
    /// sortiraj od najmanjeg do najveceg
    /// </summary>
    public void SortToHighest()
    {
        if (listItemChangeable.Count != 0 && listItemChangeable != null)
        {
            DeleteSlots();

            #region sortiranje itema za upis u inventory
            Item[] sortedItems = new Item[listItemChangeable.Count];
            for (int i = 0; i < listItemChangeable.Count; i++)
            {
                sortedItems[i] = listItemChangeable[i];
            }
            CountingSort countSort = new CountingSort();
            sortedItems = countSort.Sort(sortedItems);

            for (int i = 0; i < sortedItems.Length; i++)
            {
                listItemChangeable[i] = sortedItems[i];
            }
            #endregion

            AddToInventory(listItemChangeable);
        }
    }

    /// <summary>
    /// ispisi kao sto su redom unoseni elementi
    /// </summary>
    public void ReturnToRegular()
    {
        if (listItem.Count != 0 && listItem != null)
        {
            DeleteSlots();

            AddToInventory(listItem);
        }
    }
}
