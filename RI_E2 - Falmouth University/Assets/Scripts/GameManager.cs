using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float time = 120f;
    public Text countdownTxt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        //time = Mathf.Round(time * 100f) / 100f;
        countdownTxt.text = "Time: " + time.ToString("00");
    }
}
