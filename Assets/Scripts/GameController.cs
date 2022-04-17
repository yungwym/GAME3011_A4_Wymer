using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public NodeManager nodeManager;

    //Canvases
    public GameObject difficultyCanvas;
    public GameObject startCanvas;
    public GameObject gameUiCanvas;
    public GameObject gameWinCanvas;
    public GameObject gameLoseCanvas;

    private bool b_GameStarted;

    private int difficultyRow;
    private int difficultyColumn;

    // Start is called before the first frame update
    void Start()
    {
        b_GameStarted = false;

        ShowDifficultyUi();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !b_GameStarted)
        {
            StartGame(); 
        }
    }

    private void StartGame()
    {
        ShowGameUi();
        b_GameStarted = true;
        nodeManager.GenerateGrid(difficultyRow, difficultyColumn);
    }

    public void ShowDifficultyUi()
    {
        difficultyCanvas.SetActive(true);
        startCanvas.SetActive(false);
        gameUiCanvas.SetActive(false);
        gameWinCanvas.SetActive(false);
        gameLoseCanvas.SetActive(false);
    }

    public void ShowStartUi()
    {
        difficultyCanvas.SetActive(false);
        startCanvas.SetActive(true);
        gameUiCanvas.SetActive(false);
        gameWinCanvas.SetActive(false);
        gameLoseCanvas.SetActive(false);
    }

    public void ShowGameUi()
    {
        difficultyCanvas.SetActive(false);
        startCanvas.SetActive(false);
        gameUiCanvas.SetActive(true);
        gameWinCanvas.SetActive(false);
        gameLoseCanvas.SetActive(false);
    }

    public void ShowGameWinUi()
    {
        difficultyCanvas.SetActive(false);
        startCanvas.SetActive(false);
        gameUiCanvas.SetActive(false);
        gameWinCanvas.SetActive(true);
        gameLoseCanvas.SetActive(false);
        nodeManager.gameObject.SetActive(false);
    }

    public void ShowGameLoseUi()
    {
        difficultyCanvas.SetActive(false);
        startCanvas.SetActive(false);
        gameUiCanvas.SetActive(false);
        gameWinCanvas.SetActive(false);
        gameLoseCanvas.SetActive(true);
        nodeManager.gameObject.SetActive(false);
    }



    //Difficulty Selection Functions 
    public void OnEasyClicked()
    {
        ShowStartUi();
        difficultyRow = 3;
        difficultyColumn = 5;
        nodeManager.SetGridSpawnPoint(new Vector2(-3.0f, 1.5f));
    }

    public void OnMediumClicked()
    {
        ShowStartUi();
        difficultyRow = 4;
        difficultyColumn = 6;
        nodeManager.SetGridSpawnPoint(new Vector2(-3.75f, 1.5f));
    }

    public void OnHardClicked()
    {
        ShowStartUi();
        difficultyRow = 5;
        difficultyColumn = 8;
        nodeManager.SetGridSpawnPoint(new Vector2(-5.25f, 2.0f));
    }

}
