using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeManager : MonoBehaviour
{

    //Game Controller
    public GameController gameController;

    public AudioManager audioManager;

    public TextMeshProUGUI nodesHackedText;

    //Grid Variables
    public Transform startPosition;
    public Node nodePrefab;

    public Node[,] grid;
    private int GridRowSize;
    private int GridColumnSize;

    private float nodeSize = 1.5f;

    //Selector
    public SelectorController selectorPrefab;
    private SelectorController selector;
    private Vector2 selectorPosition;

    public GameObject hackController;
    private HackSequence hackSequence;

    //Line
    public List<Transform> hackedTiles;
    public LineController lineController;

    private bool b_GameStarted = false;
   
    private void Awake()
    {
        hackController.SetActive(false);
        lineController.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (b_GameStarted)
        {
            HandleInput();
            UpdateUI();
        }
      
    }

    public void GenerateGrid(int rows, int columns)
    {
        GridRowSize = rows;
        GridColumnSize = columns;

        grid = new Node[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float posY = (startPosition.position.y) - row * nodeSize;
                float posX = (startPosition.position.x) + col * nodeSize;

                grid[row, col] = SpawnEmptyNode(row, col, posX, posY);
            }
        }
        SetInitialSelectorPosition();
        SetEndNodePosition();
        SpawnImpassNodes();

        b_GameStarted = true;
    }

    public Node SpawnEmptyNode(int row, int col, float x, float y)
    {
        Node newnode = Instantiate(nodePrefab, new Vector2(x, y), Quaternion.identity);
        newnode.transform.parent = gameObject.transform;
        return newnode;
    }

    private void SetInitialSelectorPosition()
    {
        selectorPosition = grid[0, 0].transform.position;
        selector = Instantiate(selectorPrefab, selectorPosition, Quaternion.identity);
        selector.transform.parent = gameObject.transform;
        selector.SetPositionInGrid(new Vector2(0.0f, 0.0f));
    }

    private void SetEndNodePosition()
    {
        int randomRow = Random.Range(0, GridRowSize - 1);
        int column = GridColumnSize - 1;

        Debug.Log("End Node: Row: " + randomRow + " Column: " + column);

        Node endNode = grid[randomRow, column];
        endNode.SetNodeAsEnd();
    }

    private void SpawnImpassNodes()
    {
        int minColumn = (int)selector.GetPositionInGrid().y;

        int randomColumn = Random.Range(minColumn, GridColumnSize - 1);
        int randomRow = Random.Range(0, GridRowSize - 1);

        Node impassNode = grid[randomRow, randomColumn];

        if (impassNode.GetNodeType() == NodeType.NONE)
        {
            impassNode.SetNodeAsImpass();
        }
        else
        {
            SpawnImpassNodes();
        }
    }

    private void SpawnImpassNodeAtSpecificLocation(int row, int col)
    {
        Node impassNode = grid[row, col];
        impassNode.SetNodeAsImpass();
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectNode();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelector(-1, 0);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelector(1, 0);
        }
    }

    private Node GetNodeAtRowAndColumn(int row, int col)
    {
        //Check if Row and Column are valid 
        if (row >= 0 && row < GridRowSize)
        {
            if (col >= 0 && col < GridColumnSize)
            {
                return grid[row, col];
            }
        }
        return null;
    } 


    private void MoveSelector(int row, int col)
    {
        int newRow = (int)selector.GetPositionInGrid().x + row;
        int newCol = (int)selector.GetPositionInGrid().y + col;

        if (GetNodeAtRowAndColumn(newRow, newCol))
        {
            selector.transform.position = GetNodeAtRowAndColumn(newRow, newCol).transform.position;
            selector.SetPositionInGrid(new Vector2(newRow, newCol));
        }
    }

    private void IncreaseSelectorColumn()
    {
        int row = (int)selector.GetPositionInGrid().x;
        int newCol = (int)selector.GetPositionInGrid().y + 1;

        selector.transform.position = GetNodeAtRowAndColumn(row, newCol).transform.position;
        selector.SetPositionInGrid(new Vector2(row, newCol));
    }

    private void SelectNode()
    {
        Vector2 selectedPos = selector.GetPositionInGrid();

        Debug.Log("Selected Node at Row: " + selectedPos.x + " Column: " + selectedPos.y);

        Node nodeToTest = grid[(int)selectedPos.x, (int)selectedPos.y];

        if (nodeToTest.GetNodeType() != NodeType.IMPASS)
        {
            gameObject.SetActive(false);
            selector.gameObject.SetActive(false);
            lineController.gameObject.SetActive(false);
            hackController.SetActive(true);

            hackSequence = hackController.GetComponent<HackSequence>();
            hackSequence.SetupHackSequence();
            hackSequence.GenerateHackSet(1, 7);
        }
        else 
        {
            Debug.Log("Invalid Selection");
            audioManager.Play("Error");
        }
       
    }

    public void SuccessfulPinHack()
    {
        selector.gameObject.SetActive(true);
        lineController.gameObject.SetActive(true);

        Debug.Log("Success from Node Manager");

        Vector2 successNode = selector.GetPositionInGrid();

        if (grid[(int)successNode.x, (int)successNode.y].GetNodeType() == NodeType.END)
        {
            hackedTiles.Add(grid[(int)successNode.x, (int)successNode.y].transform);
            lineController.SetUpLine(hackedTiles);
            GameOverWin();
        }
        else
        {
            hackedTiles.Add(grid[(int)successNode.x, (int)successNode.y].transform);
            lineController.SetUpLine(hackedTiles);
            grid[(int)successNode.x, (int)successNode.y].SetNodeAsHacked();
            IncreaseSelectorColumn();
        }
    }

    public void UnsuccessfulPinHack()
    {
        Debug.Log("Failure from Node Manager");
        selector.gameObject.SetActive(true);
        lineController.gameObject.SetActive(true);
        lineController.SetUpLine(hackedTiles);

        audioManager.Play("Error");

        //Check surrounding nodes for an end condition 
        Vector2 nodeXY = selector.GetPositionInGrid();
        
        if (CheckSurroundingNodes(nodeXY))
        {
            //Game Over
            Debug.Log("Game Over");
            SpawnImpassNodeAtSpecificLocation((int)nodeXY.x, (int)nodeXY.y);
            audioManager.Play("Error");
            StartCoroutine(GameOverLose());
        }
        else
        {
            SpawnImpassNodes();
        }
    }


    public void GameOverWin()
    {
        Debug.Log("Win");
        lineController.gameObject.SetActive(false);
        gameController.ShowGameWinUi();
    }

    IEnumerator GameOverLose()
    {
        Debug.Log("Lose");
        yield return new WaitForSeconds(0.75f);
        lineController.gameObject.SetActive(false);
        gameController.ShowGameLoseUi();
    }

    public void SetGridSpawnPoint(Vector2 spawnPoint)
    {
        startPosition.position = spawnPoint;
    }


    public bool CheckSurroundingNodes(Vector2 currentNodeCoord)
    {
        int row = (int)currentNodeCoord.x;
        int col = (int)currentNodeCoord.y;

        //Check Top
        if (CheckIfGridIndexIsValid(row, col - 1))
        {
            if (grid[row, col - 1].GetNodeType() == NodeType.IMPASS)
            {
                return true;
            }
        }

        //Check Bottom
        if (CheckIfGridIndexIsValid(row, col + 1))
        {
            if (grid[row, col + 1].GetNodeType() == NodeType.IMPASS)
            {
                return true;
            }
        }
        //Right
        if (CheckIfGridIndexIsValid(row + 1, col))
        {
            if (grid[row + 1, col].GetNodeType() == NodeType.IMPASS)
            {
                return true;
            }
        }
        //Left
        if (CheckIfGridIndexIsValid(row - 1, col))
        {
            if (grid[row - 1, col].GetNodeType() == NodeType.IMPASS)
            {
                return true;
            }
        }

        return false;
    }


    private Node CheckIfGridIndexIsValid(int row, int col)
    {
        //Check if Row and Column are valid 
        if (row >= 0 && row < GridRowSize)
        {
            if (col >= 0 && col < GridColumnSize)
            {
                return grid[row, col];
            }
        }
        return null;
    }

    public void UpdateUI()
    {
        nodesHackedText.text = "Nodes Hacked: " + selector.GetPositionInGrid().y + " / " + GridColumnSize;
    }

}
