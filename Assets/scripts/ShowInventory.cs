using UnityEngine;

public class ShowInventory : MonoBehaviour
{

    public GameObject inventory;
    private GameObject player;
    private Animator anim;
    private CanvasRenderer cr;
    private GameObject minimap;     // minimapa koja treba da se deakrivira dok se pregleda inventory

    public static bool inventoryOpened;                           // sluzi da nekim klasam zabrani radnje dok je inventory otvoren

    [SerializeField]
    private GameObject toLow, toHigh, revert;

    private void Start()
    {
        minimap = GameObject.Find("MinimapBorder");
    }

    public void ShowInventoryOnClick()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
        cr = inventory.GetComponent<CanvasRenderer>();

        if (cr.GetAlpha() != 0)
        {
            // inventory je zatvoren, dozvoli akcije
            inventoryOpened = false;

            anim.SetBool("Run", true);
            PlayerMovement.inventoryPause = false;

            // sakrij dugmice za sortiranje
            toHigh.active = toLow.active = revert.active = false;

            if (CameraMovement.FindObjectOfType<CameraMovement>().FollowPlayer)
                minimap.active = true;                  // ukljuci minimap kada inventory nije aktivan
            cr.SetAlpha(0);
        }
        else
        {
            // inventory je otvoren, setuj na true i zabrani akcije
            inventoryOpened = true;

            anim.SetBool("Run", false);
            PlayerMovement.inventoryPause = true;

            // prikazi dugmice za sortiranje
            toHigh.active = toLow.active = revert.active = true;

            if (CameraMovement.FindObjectOfType<CameraMovement>().FollowPlayer)
                minimap.active = false;                 // iskljuci minimap dok je inventory aktivan
            cr.SetAlpha(1.75f);
        }
    }
}
