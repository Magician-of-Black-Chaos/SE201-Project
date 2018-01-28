using UnityEngine;

public class MinimapController : MonoBehaviour
{

    private int lower = 0;      // broj koliko puta moze da se smanji

    [SerializeField]
    private GameObject minimapCamera;

    // Use this for initialization
    void Start()
    {
        lower = 0;
        minimapCamera = GameObject.Find("MinimapCamera");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // kada se pritisne srednje dugme na misu smanji mapu ili je vrati na pocetnu vrednost
        if (Input.GetMouseButtonDown(2))
        {
            if (lower == 0)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
                minimapCamera.transform.position = new Vector3(0, minimapCamera.transform.position.y, 0);  // postavi poziciju kamere na centar sa njenom trenutnom visinom
                lower++;
            }
            else if (lower == 1)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 800);
                lower = 0;
            }
        }
    }
}
