using UnityEngine;

public class RotateCoins : MonoBehaviour
{
    public static bool pause;
    // Use this for initialization
    void Start()
    {
        pause = false;
        transform.position += new Vector3(0, 0.5f, 0);
        transform.Rotate(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
            transform.Rotate(0, 0, /*4*/241*Time.deltaTime);
    }
}
