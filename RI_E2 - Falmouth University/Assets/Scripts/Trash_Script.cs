using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash_Script : MonoBehaviour
{
    private Rigidbody myRb;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
        myRb.AddForce(Vector3.up * 8, ForceMode.Impulse);
        myRb.AddForce(-Vector3.forward * 3, ForceMode.Impulse);

        var ranNo = Random.Range(-1f, 2f);
        myRb.AddForce(new Vector3(ranNo, 0, 0) * 2, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
