using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection_Script : MonoBehaviour
{
    private const int trashLayer = 10;
    public int pointYield;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == trashLayer)
        {
            Trash_Script trashScript = other.GetComponent<Trash_Script>();
            if (!trashScript.collected)
            {
                GameManager.gMan.playerScore += pointYield;
                trashScript.collected = true;
            }
        }
    }
}
