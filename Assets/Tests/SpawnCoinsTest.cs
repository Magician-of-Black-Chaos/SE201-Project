using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SpawnCoinsTest : MonoBehaviour
{

    [UnityTest]
    public IEnumerator SpawningCoinsOnRandomPos()
    {
        setupScene();

        GameObject coin = Resources.Load<GameObject>("Test/CoinTest");        // novcic

        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return new WaitForSeconds(2);


        for (int i = 0; i < 50; i++)
        {
            Vector3 coinPos = new Vector3(coin.transform.position.x + Random.Range(-49, 49), 0.5f, coin.transform.position.z + Random.Range(-49, 49));
            Instantiate(coin, coinPos, Quaternion.identity);
        }

        if (coin == null)
            Assert.Fail();
        CleanUp();
    }

    [UnityTest]
    public IEnumerator CoinRotation()
    {
        setupScene();

        GameObject coin = Resources.Load<GameObject>("Test/CoinTest");        // novcic

        Instantiate(coin);

        Quaternion coinRot = coin.transform.rotation;
        yield return new WaitForEndOfFrame();

        GameObject gCoin = GameObject.FindGameObjectWithTag("coin1");
        if (coinRot == gCoin.transform.rotation)
            Assert.Fail();
        CleanUp();
    }

    [UnityTest]
    public IEnumerator PlayerMove()
    {
        setupScene();

        int speed = 12;

        GameObject player = Resources.Load<GameObject>("Test/PlayerTest");
        player.transform.position = new Vector3(49, 0, 49);
        GameObject coin = Resources.Load<GameObject>("Test/CoinTest");

        Instantiate(player);
        Instantiate(coin);
        yield return new WaitForSeconds(1);
        Vector3 target = coin.transform.position;

        target -= new Vector3(0, target.y, 0);
        Vector3 lookAt = new Vector3(target.x - player.transform.position.x, player.transform.position.y, target.z - player.transform.position.z);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(lookAt), 8 * Time.deltaTime);

        while (player.transform.position != target)
            player.transform.position = Vector3.MoveTowards(player.transform.position, target, speed * Time.deltaTime);
        
        CleanUp();
    }

    private void setupScene()
    {
        Instantiate(Resources.Load<GameObject>("Test/PlaneTest"));   // postolje
        Instantiate(Resources.Load<GameObject>("Test/CameraTest"));  // kamera
    }

    private void CleanUp()
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject gObj in gameObjects)
        {
            Component masterScript = gObj.GetComponent("UnityEngine.TestTools.TestRunner.PlaymodeTestController");
            if (masterScript != null)
            {
                continue;
            }
            DestroyImmediate(gObj);
        }
    }
}
