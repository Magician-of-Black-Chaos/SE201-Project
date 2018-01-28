using UnityEngine;

public class ItemController : MonoBehaviour
{

    private string name;
    private float value;
    private Sprite sprite;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public Sprite ItemSprite
    {
        get { return sprite; }
        set { sprite = value; }
    }

    public float Value
    {
        get { return value; }
        set { this.value = value; }
    }
    /*
    void OnMouseEnter()
    {
        Debug.Log("Radi ItemController");
        transform.parent.parent.GetComponent<InventoryController>().selectedItem = this.transform;
    }*/
}
