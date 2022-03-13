using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container_Script : MonoBehaviour
{
    public int trashCount;
    private PlayerController_Script playerCont;
    public bool thrown = false;
    public List<GameObject> collectedTrash = new List<GameObject>();

    private void Start()
    {
        playerCont = GameObject.Find("Player").GetComponent<PlayerController_Script>();
    }

    private void Update()
    {
        playerCont.capTxt.text = "Capacity: " + trashCount + "/10";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) // if trash
        {
            if (trashCount < 10)
            {
                // Update UI
                trashCount++;

                //Destory trash
                collectedTrash.Add(collision.gameObject);
                collision.gameObject.SetActive(false);
                collision.gameObject.layer = default;
            }
            else
            {
                Debug.Log("I'm full!");
            }
        }

        if (thrown)
        {
            if (collision.gameObject.layer == 8) // if ground
            {
                trashCount = 0; // reset

                for (int i = 0; i < collectedTrash.Count; i++)
                {
                    collectedTrash[i].SetActive(true);
                    collectedTrash[i].transform.position = gameObject.transform.position;
                }

                collectedTrash.Clear();
            }
        }
    }
}
