using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackSequence : MonoBehaviour
{

    //Grid Variables
    public Transform startPosition;
    public Transform endPosition;

    public HackTile hackPrefab;

    public HackTile[,] grid;
    private int GridRowSize;
    private int GridColumnSize;

    private float nodeSize = 1.25f;

    //Select Number 
    public HackTile[] selectTiles;

    //Indicator Variables 
    public IndicatorController indicator;
    bool moveRight = true;
    float moveSpeed = 0.75f;

    //Current Target Number Variables 
    public GameObject currentTileIndicator;
    private int currentTargetNumber;
    private int targetIndex = 0;

    public NodeManager nodeManager;

    

    // Update is called once per frame
    void Update()
    {
        MoveIndicator();
    }

    public void SetupHackSequence()
    {
        targetIndex = 0;

        endPosition.position = new Vector2(-startPosition.position.x, startPosition.position.y);

        foreach (HackTile tile in selectTiles)
        {
            tile.GenerateRandomNumber();
        }

        SetupCurrentTargetInfo();
    }

    public void SetupCurrentTargetInfo()
    {
        currentTargetNumber = selectTiles[targetIndex].indexNumber;
        currentTileIndicator.transform.position = new Vector2(selectTiles[targetIndex].transform.position.x, currentTileIndicator.transform.position.y);
    }

    public void GenerateHackSet(int rows, int columns)
    {
        GridRowSize = rows;
        GridColumnSize = columns;

        grid = new HackTile[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float posY = (startPosition.position.y) - row * nodeSize;
                float posX = (startPosition.position.x) + col * nodeSize;

                grid[row, col] = SpawnHackTile(row, col, posX, posY);
            }
        }

        foreach (HackTile tile in grid)
        {
            StartCoroutine(tile.GenerateRandomNumberRepeated());
        }

        //float xPos = grid[rows, columns].transform.position.x + hackPrefab.transform.localScale.x / 2;
       // endPosition.position = new Vector2(xPos, startPosition.position.y);
    }

    public HackTile SpawnHackTile(int row, int col, float x, float y)
    {
        HackTile newHackTile = Instantiate(hackPrefab, new Vector2(x, y), Quaternion.identity);
        newHackTile.transform.parent = gameObject.transform;
        return newHackTile;
    }


    private void MoveIndicator()
    {
        indicator.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, Mathf.PingPong(Time.time * moveSpeed, 1.0f));
    }


    public void CheckHackTile(HackTile hackTile)
    {

        Debug.Log(hackTile.indexNumber);

        if (hackTile.indexNumber == currentTargetNumber)
        {
            Debug.Log("Successful Pick");
            targetIndex += 1;

            if (targetIndex >= selectTiles.Length)
            {
                Clear();

                Debug.Log("Hack Sequence Success");
                nodeManager.gameObject.SetActive(true);
                nodeManager.SuccessfulPinHack();
                gameObject.SetActive(false);
            }
            else
            {
                SetupCurrentTargetInfo();
            }
        }
        else
        {
            Debug.Log("Unsuccessful Pick");
            nodeManager.gameObject.SetActive(true);
            nodeManager.UnsuccessfulPinHack();
            gameObject.SetActive(false);
        }
    }

    private void Clear()
    {
        foreach (HackTile tile in grid)
        {
            Destroy(tile.gameObject);
        }
    }
       
}