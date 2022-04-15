using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorController : MonoBehaviour
{

    //Memeber Variables 
    private Vector2 positionInGrid;
    private SpriteRenderer spriteRenderer;

    public void SetPositionInGrid(Vector2 _position)
    {
        positionInGrid = _position;
    }

    public Vector2 GetPositionInGrid()
    {
        return positionInGrid;
    }


}
