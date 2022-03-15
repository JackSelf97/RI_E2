using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumpster_Script : MonoBehaviour
{
    private Rigidbody myRb;
    public float speed = 5;
    [SerializeField] private float timer = 0;
    public float timeLimit = 5;
    private bool directionSwitch;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
        myRb.velocity = new Vector2(speed, myRb.velocity.y);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeLimit)
        {
            SwitchDirection();
            if (directionSwitch)
            {
                myRb.velocity = new Vector2(-speed, myRb.velocity.y);
            }
            else if (!directionSwitch)
            {
                myRb.velocity = new Vector2(speed, myRb.velocity.y);
            }
            timer = 0;
        }
    }

    void SwitchDirection()
    {
        directionSwitch = !directionSwitch;
    }
}
