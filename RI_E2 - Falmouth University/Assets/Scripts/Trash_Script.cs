using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash_Script : MonoBehaviour
{
    private Rigidbody myRb;
    public bool collected, pickedUp;
    public float lifeTime = 30f;
    private bool shrink;
    private float xScale, yScale, zScale;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
        myRb.AddForce(Vector3.up * 8, ForceMode.Impulse);
        myRb.AddForce(-Vector3.forward * 3, ForceMode.Impulse);

        var ranNo = Random.Range(-1f, 2f);
        myRb.AddForce(new Vector3(ranNo, 0, 0) * 2, ForceMode.Impulse);

        collected = false;

        xScale = 0.5f;
        yScale = 0.5f;
        zScale = 0.5f;
    }

    private void Update()
    {
        if (!pickedUp)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime < 0)
            {
                StartCoroutine(ShrinkDeath());
            }
        }

        if (shrink)
        {
            xScale -= Time.deltaTime;
            yScale -= Time.deltaTime;
            zScale -= Time.deltaTime;
            transform.localScale = new Vector3(xScale, yScale, zScale);

            if (yScale <= 0f)
            {
                xScale = 0f;
                yScale = 0f;
                zScale = 0f;
                shrink = false;
            }
        }
    }

    private IEnumerator ShrinkDeath()
    {
        shrink = true;
        gameObject.layer = default;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
