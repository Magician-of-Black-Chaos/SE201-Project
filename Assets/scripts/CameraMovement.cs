using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    public float smoothSpeed = 1f;                                                                      // brzina kojom se kamera krece od jednog do drugog vektora
    public Vector3 offset;                                                                                  // udaljenost od playera

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;

    [SerializeField]
    private GameObject minimap;

    private bool nextPos;   // pomocna bool vrednost za frontAboveBack pregled, dozvoljava kameri da promeni svoj polozaj

    private bool followPlayer, lookFromAbove, frontAboveBack;       // stanja u kojima kamera moze da se nadje

    IEnumerator Start()
    {
        followPlayer = true;
        lookFromAbove = false;
        frontAboveBack = false;
        nextPos = true;
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player"));
        target = GameObject.FindGameObjectWithTag("Player");
        minimap = GameObject.Find("MinimapBorder");
    }

    void LateUpdate()
    {
        if (!PauseMenuController.finish && !PauseMenuController.pauseMenuOpened && !ShowInventory.inventoryOpened)
        {
            if (Input.GetKeyDown("v"))
            {
                if (followPlayer)
                {
                    followPlayer = false;
                    lookFromAbove = true;
                    minimap.active = false;
                }
                else if (lookFromAbove)
                {
                    lookFromAbove = false;
                    frontAboveBack = true;
                }
                else if (frontAboveBack)
                {
                    followPlayer = true;
                    frontAboveBack = false;

                    minimap.active = true;
                    nextPos = true;            // postavi na default vrednost
                }
            }

            if (followPlayer)
                Refresh();
            else if (lookFromAbove)
            {
                transform.position = new Vector3(0, 20, 0);
                transform.LookAt(target.transform.position);
            }
            else if (frontAboveBack)
            {
                if (nextPos)
                {
                    // postavi rotaciju kao kod playera
                    transform.rotation = target.transform.rotation;

                    // postavi se na poziciju playera i podji napred i gore, mozda je 20 previse, moram da proverim
                    transform.position = target.transform.position + new Vector3(0, 5, 0) + transform.forward * 10 + transform.right * 2;

                    nextPos = false;
                }

                // zatim sa te pozicije gledaj u njega
                transform.LookAt(target.transform.position);

                // tek kada napravi odredjenu distancu moze da ponovi pomeranje pozicije kamere
                if (Vector3.Distance(transform.position - new Vector3(0, 10, 0), target.transform.position) > 13)
                    nextPos = true;
            }
        }
    }

    public void Refresh()
    {
        if (target == null)
        {
            Debug.LogWarning("Nema Playera!", this);

            return;
        }

        // pozicioniraj
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = target.transform.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = target.transform.position + offsetPosition;
        }

        // rotiraj
        if (lookAt)
        {
            transform.LookAt(target.transform.position + new Vector3(0f, 1.3f, 0f));
        }
        else
        {
            transform.rotation = target.transform.rotation;
        }
    }

    public bool FollowPlayer
    {
        get { return followPlayer; }
    }
}
