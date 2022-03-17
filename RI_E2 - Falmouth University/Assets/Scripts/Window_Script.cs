using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Script : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] trash;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gMan.startGame)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                int ranNo = Random.Range(0, 3);
                Debug.Log(ranNo);
                Instantiate(trash[ranNo], spawnPoint.transform.position, Quaternion.identity);
                Debug.Log("Here comes some trash!");
                timer = 0; // resets
            }
        }
    }
}
