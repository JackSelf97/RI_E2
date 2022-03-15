using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumpster_Script : MonoBehaviour
{
    private const int trashLayer = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == trashLayer)
        {
            Trash_Script trashScript = other.GetComponent<Trash_Script>();
            if (!trashScript.collected)
            {
                GameManager.gMan.playerScore++;
                trashScript.collected = true;
            }
        }
    }
}
