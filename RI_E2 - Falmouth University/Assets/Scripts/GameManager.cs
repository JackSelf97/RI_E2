using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float time = 120f;
    public CinemachineVirtualCamera vCam1, vCam2;
    public int playerScore = 0;
    public Text countdownTxt, scoreTxt, totalScoreTxt;
    public GameObject canvasMenu, canvasPlayer, canvasPause, canvasVictory;
    public MeshRenderer playerMesh;
    public GameObject player;
    private PlayerController_Script playerCont;
    public bool startGame;
    private bool isPaused;

    #region Singleton & Awake
    public static GameManager gMan = null; // should always initilize

    private void Awake()
    {
        if (gMan == null)
        {
            DontDestroyOnLoad(gameObject);
            gMan = this;
        }
        else if (gMan != null)
        {
            Destroy(gameObject); // if its already there destroy it
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        playerCont = player.GetComponent<PlayerController_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            // Coundown timer
            time -= Time.deltaTime;
            countdownTxt.text = "Time: " + time.ToString("00");
            if (time <= 0)
            {
                time = 0;
                VictoryState();
            }

            // Scoring
            scoreTxt.text = "Score: " + playerScore.ToString();
        }

        if (playerCont.isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
    }

    #region Buttons & UI
    
    public void PlayGame()
    {
        StartCoroutine(DelayGameState());
    }
    
    public void QuitGame()
    {
        Debug.Log("Quiting...");
        Application.Quit();
    }

    public void VictoryState()
    {
        canvasPlayer.SetActive(false);
        canvasVictory.SetActive(true);
        vCam2.enabled = true;
        vCam1.enabled = false;
        player.GetComponent<PlayerController_Script>().isActive = false;
        Cursor.lockState = CursorLockMode.None;

        // Scoring
        totalScoreTxt.text = "Score: " + playerScore.ToString();
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            canvasPause.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            canvasPause.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void RestartGame() // not finished yet
    {
        playerScore = 0;
        time = 120;
    }

    #endregion

    IEnumerator DelayGameState()
    {
        yield return new WaitForSeconds(2);
        canvasMenu.SetActive(false);
        vCam1.enabled = true;
        vCam2.enabled = false;
        canvasPlayer.SetActive(true);
        playerMesh.enabled = true; // does not need a mesh
        player.GetComponent<PlayerController_Script>().isActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        startGame = true;
    }
}
