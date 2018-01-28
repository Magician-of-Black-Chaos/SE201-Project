using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DB : MonoBehaviour
{
    private static DB database;
    private Slider mainSlider;          // slider sa kog uzimamo broj novcica za kreiranje
    private Slider masterVolumeSlider;  // slide za podesavanje zvuka
    private Slider musicVolume;
    private Slider sfxVolume;
    public int numOfCoins;              // broj novcica koje kreiramo
    public int volume;                  // jacina zvuka
    public float speed = 0.15f;         // brzina
    public Vector2 inventorySize = new Vector2(10, 2);
    public Vector2 windowsSizePix = new Vector2(800, 400);

    // mixer za zvuk
    public AudioMixer audioMixer;

    /// <summary>
    /// pronalazi objekat tipa DB
    /// </summary>
    public static DB Database
    {
        get
        {
            if (database == null)
                database = GameObject.FindObjectOfType<DB>();
            return database;
        }
    }

    void Awake()
    {

    }

    public void Start()
    {

        if (database == null)
        {
            database = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (database != this)
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
    }

    /// <summary>
    /// na svaku promenu slajda postavi drugi broj novcica za spawn u igri
    /// </summary>
    public void SetNumOfCoins(float num)
    {
        numOfCoins = (int)num;

        inventorySize = new Vector2(10, Mathf.Ceil(numOfCoins / 10.0f));
        windowsSizePix = new Vector2(800, 100 * Mathf.Ceil(numOfCoins / 10.0f));
    }


    public void SetVolume(float val)
    {
        audioMixer.SetFloat("Master", val);
    }

    public void SetMusicVolume(float val)
    {
        audioMixer.SetFloat("Music", val);
    }

    public void SetSFXVolume(float val)
    {
        audioMixer.SetFloat("Sfx", val);
    }

    /// <summary>
    /// unisti ovaj objekat
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
