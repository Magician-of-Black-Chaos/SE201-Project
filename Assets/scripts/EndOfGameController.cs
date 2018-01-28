using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGameController : MonoBehaviour {

    [SerializeField]
    private GameObject toHighButton;

    [SerializeField]
    private GameObject toLowButton;

    [SerializeField]
    private GameObject revertButton;

    [SerializeField]
    private GameObject playerSpotlight;

    [SerializeField]
    private GameObject targetSpotlight;

    // Use this for initialization
    void Start () {
        // aktiviraj dugmice za sortiranje
        toHighButton.active = true;
        toLowButton.active = true;
        revertButton.active = true;

        // ugasi svetala ispod playera i itema
        playerSpotlight.active = false;
        targetSpotlight.active = false;
        
        // prikazi inventory
        GameObject.Find("InventoryScroll").GetComponent<CanvasRenderer>().SetAlpha(1.75f);

        // iskljuci minimapu
        if (GameObject.Find("MinimapBorder") != null)
            GameObject.Find("MinimapBorder").active = false;
    }
}
