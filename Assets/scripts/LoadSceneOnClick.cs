using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public static int numOfItemsFromScene2 = 20;
    public static bool active = false;

    /// <summary>
    /// ucitaj scenu na indexu
    /// </summary>
    /// <param name="index">indeks scene koja se ucitava</param>
    public void LoadByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    
    /// <summary>
    /// Unisti bazu
    /// </summary>
    public void DestroyDB()
    {
        DB.Database.Destroy();
    }
}
