using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{

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
   
    private void Awake()
    {
        hackController.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
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

    private void SelectNode()
    {
        Vector2 selectedPos = selector.GetPositionInGrid();

        Debug.Log("Selected Node at Row: " + selectedPos.x + " Column: " + selectedPos.y);

        gameObject.SetActive(false);
        selector.gameObject.SetActive(false);
        hackController.SetActive(true);
        hackController.GetComponent<HackSequence>().GenerateHackSet(1, 7);
    }

    public void SuccessfulPinHack()
    {
        Debug.Log("Success from Node Manager");
    }

    private void UnsuccessfulPinHack()
    {

    }


}
