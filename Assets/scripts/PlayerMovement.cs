using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    private Animator animator;

    public float speed = 0;
    private int i = 0;                  // za proveru komsija
    List<Item> target = new List<Item>();
    float dist = 0.8f;                  // udaljenost od itema do koje player ide
    Item tmpTarget;
    bool goBack = false, popAgain = true, eog = false;          // goBack odredjuje da li se vraca na poziciju prethodnog itema, popAgain izvlaci poziciju prethodnog itema, eog je kraj igre
    Stack<Item> visitedTarget = new Stack<Item>();
    InventoryController ic;

    private GameObject playerSpotlight;         // osvetljenje playera
    private GameObject targetSpotlight;         // osvetljenje itema
    
    private AudioSource[] sounds;                // ovde je da bi mogao da aktiviram soundEffect kada se pokupi novcic

    public static bool inventoryPause, pause;    // playPause za inventory pauzira, pause pauzira za pause menu

    // Use this for initialization
    IEnumerator Start()
    {
        // ako ne radi obrisi
        speed = 12;

        playerSpotlight = GameObject.Find("PlayerSpotlight");
        targetSpotlight = GameObject.Find("DestinationSpotlight");

        eog = false;
        // pronadji listu zvukova
        sounds = GameObject.Find("MusicContainer").GetComponents<AudioSource>();

        pause = false;
        inventoryPause = false;

        animator = GetComponent<Animator>();
        //animator.SetBool("Run", true);

        GameObject inventory = GameObject.Find("Inventory");
        ic = inventory.GetComponent<InventoryController>();

        // inicijalizacija koja je potrebna zbog menua
        goBack = false;
        popAgain = true;
        visitedTarget = new Stack<Item>();

        // kraj inicijalizacije prouzrokovane menuom

        target = SpawnPlayer.getVisitedList();
        /*print("target.Count-> " + target.Count);
        for (int kk = 0; kk < target.Count; kk++)
            print("target[" + kk + "]->" + target[kk].Num);*/

        //yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => target != null);
        target = SpawnPlayer.getVisitedList();
        animator.SetBool("Run", true);              // run animacija krece tek kada player ima cilj ka kome ide
        //print("target.Count-> " + target.Count);
        /* for (int kk = 0; kk < target.Count; kk++)
         {
             print("target[" + kk + "]->" + target[kk].Num);
             print("target[" + kk + "] tip->" + target[kk].ItemObject.GetType() + " name->" + target[kk].ItemObject.name);
         }*/
    }

    #region kolizija sa itemom
    /// <summary>
    /// funkcija koja se aktivira prilikom kolizije
    /// </summary>
    /// <param name="other">predmet sa kojim je player dosao u koliziju</param>
    private void OnTriggerEnter(Collider other)
    {
        if (target.Count > i)
            if (other.transform.position - new Vector3(0, 0.5f, 0) == target[i].ItemPosition && !goBack)    // uslov da dolazi do kolizije samo sa elementom ka kome smo posli i da nismo u stanju go back da ne bi u povratku slucajno pokupio element
            {
                Destroy(other.gameObject);
                // kada pokupis item pokreni soundEffect
                sounds[1].Play();
            }
    }
    #endregion

    #region kretanje ka poziciji itema
    /// <summary>
    /// funkcija za kretanje ka itemu
    /// </summary>
    /// <param name="target">pozicija itema do kog player ide</param>
    public void MyMove(Vector3 target)
    {
        target -= new Vector3(0, target.y, 0);           // posto itemi nemaju y na 0 ovo ga postavlja na 0 da bi player ostao na podlozi
        Vector3 lookAt = new Vector3(target.x - transform.position.x, transform.position.y, target.z - transform.position.z);       // umesto funkcije lookAt, ima lepsu smooth rotaciju
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), 8 * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, target, speed*Time.deltaTime/*speed na player prefab je bilo 0.17*/);
    }
    #endregion

    #region put do itema
    /// <summary>
    /// funkcija koja odredjuje ka kom itemu se ide i odredjuje putanju do njega
    /// </summary>
    private void MoveToItems()
    {
        if (i < 2)
        {
            MyMove(target[i].ItemPosition);
        }
        else
        {
            if (visitedTarget.Peek().getNeighbour().Contains(target[i]) && !goBack)
            {
                MyMove(target[i].ItemPosition);
            }
            else
            {
                goBack = true;

                if (popAgain)
                {
                    visitedTarget.Pop();
                    tmpTarget = visitedTarget.Peek();
                }
                MyMove(tmpTarget.ItemPosition);
                popAgain = false;
                if (Vector3.Distance(transform.position, tmpTarget.ItemPosition) < dist)
                {
                    popAgain = true;
                    goBack = false;
                }
            }
        }

        // postavi spotlight na poziciju itema
        targetSpotlight.transform.position = new Vector3(target[i].ItemPosition.x, 0.3f, target[i].ItemPosition.z);

        if (transform.position.x == target[i].ItemPosition.x && transform.position.z == target[i].ItemPosition.z)      // uslov sa koje udaljenosti kupimo item
        {
            visitedTarget.Push(target[i]);
            ic.AddItem(target[i]);
            i++;
        }
    }
    #endregion


    // Update is called once per frame
    void Update()
    {
        if (!inventoryPause && !pause && !eog)     // update se ne izvrsava kada pregledamo inventory
        {
            if (i < target.Count)       // dok je i manje od ukupnog broja itema idi na sledeci item
            {
                MoveToItems();
            }
            else if (i == target.Count && i != 0)
            {
                print("EOG PlayerMovement");
                eog = true;

                PauseMenuController.finish = true;
            }
            playerSpotlight.transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
        }
    }
}
