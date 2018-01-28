using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    public GameObject panel, pauseMenuPanel;    
    private GameObject player;
    private Animator anim;
    private GameObject inventory;
    private CanvasRenderer cr;
    private GameObject minimap;
    public static bool finish;
    public static bool pauseMenuOpened;           // provera da li je pause menu aktiviran, sluzi da drugim klasama zabrani neke funkcionalnosti

    [SerializeField]
    private GameObject eog;

    [SerializeField]
    private GameObject resumeButton;

    [SerializeField]
    private GameObject pauseMenuText;

    IEnumerator Start()
    {
        pauseMenuOpened = false;
        finish = false;

        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player"));
        // mora ovako da se pronadje player kao i njegova animacija jer kada postavim prefab izbacuje gresku
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (finish)
        {
            eog.active = true;
            resumeButton.GetComponent<Button>().enabled = false;
            pauseMenuText.active = false;
            anim.SetBool("Run", false);
        }
    }

    public void Pause()
    {
        // da player ne bi mogao da pomera kameru u pause menuu
        pauseMenuOpened = true;

        // iskljuci minimap dok si u menuu
        minimap = GameObject.Find("MinimapBorder");
        if (minimap != null)
            minimap.active = false;

        RotateCoins.pause = true;       // zaustavi update novcica
        PlayerMovement.pause = true;    // zaustavi update playerMovement
        anim.SetBool("Run", false);
        panel.active = false;
        pauseMenuPanel.active = true;
    }

    public void Resume()
    {
        // dozvoli pomeranje kamere
        pauseMenuOpened = false;

        inventory = panel.transform.GetChild(2).gameObject;
        cr = inventory.GetComponent<CanvasRenderer>();

        // ponovo aktiviraj minimap
        if (minimap != null)
            minimap.active = true;


        RotateCoins.pause = false;      // nastavi update novcica
        PlayerMovement.pause = false;   // nastavi update playerMovement
        pauseMenuPanel.active = false;
        panel.active = true;

        // ako je inventory bio otvoren, animacija se ne aktivira
        if (PlayerMovement.inventoryPause)
        {
            anim.SetBool("Run", false);
        }
        else
        {
            cr.SetAlpha(0);
            anim.SetBool("Run", true);
        }
    }
}
