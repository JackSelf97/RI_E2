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
    public GameObject player, playerSpawnPos;
    private PlayerController_Script playerCont;
    public bool startGame;
    private bool isPaused, hasWon;
    

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

            // Play music
            FindObjectOfType<AudioManager>().StopSound("Track_1");
            
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
        hasWon = true;

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
        if (isPaused)
        {
            PauseGame();
        }

        if (hasWon) // reset if won
        {
            canvasVictory.SetActive(false);
            canvasPlayer.SetActive(true);
            vCam1.enabled = true;
            vCam2.enabled = false;
            player.GetComponent<PlayerController_Script>().isActive = true;
            Cursor.lockState = CursorLockMode.Locked;
            hasWon = false;
        }
        
        playerScore = 0;
        time = 120;

        Trash_Script[] trashItems = FindObjectsOfType<Trash_Script>();
        for (int i = 0; i < trashItems.Length; i++)
        {
            trashItems[i].StartCoroutine(trashItems[i].ShrinkDeath());
            //Destroy(trashItems[i].gameObject);
            Debug.Log("Clean up in progress...");
        }

        player.transform.position = playerSpawnPos.transform.position; // send the player back to the starting pos
    }

    #endregion

    IEnumerator DelayGameState()
    {
        yield return new WaitForSeconds(1);
        canvasMenu.SetActive(false);
        canvasPlayer.SetActive(true);
        player.GetComponent<PlayerController_Script>().isActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        startGame = true;
        FindObjectOfType<AudioManager>().PlaySound("Track_2");
        vCam1.enabled = true;
        vCam2.enabled = false;
    }
}
