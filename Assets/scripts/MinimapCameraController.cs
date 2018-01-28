using System.Collections;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    [SerializeField]
    private GameObject player;       // player objekat

    [SerializeField]
    private Vector3 pos;             // pozicija na koju se kamera setuje

    [SerializeField]
    private GameObject minimap;

    // Use this for initialization
    IEnumerator Start () {

        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player"));
        player = GameObject.FindGameObjectWithTag("Player");
        minimap = GameObject.Find("Minimap");
	}

	// Update is called once per frame
	void LateUpdate () {
        // u slucaju da minimapa nije velicine 300 x 300 prati playera po njoj
        if (minimap!=null && minimap.GetComponent<RectTransform>().sizeDelta != new Vector2(300, 300))
        {
            pos = player.transform.position;
            pos.y = 20;
            transform.position = pos;
        }
	}
}
