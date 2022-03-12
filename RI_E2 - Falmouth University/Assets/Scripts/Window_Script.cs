using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Script : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] trash;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2)
        {

            var ranNo = Random.Range(0, 3);
            Debug.Log(ranNo);
            Instantiate(trash[ranNo], spawnPoint.transform.position, Quaternion.identity);
            Debug.Log("Here comes some trash!");
            timer = 0; // resets





        }

        
    }
}
