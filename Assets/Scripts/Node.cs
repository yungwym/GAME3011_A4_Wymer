using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    NONE,
    OPEN, 
    END,    
    IMPASS,
    HACKED,
    CLOSED
}

public class Node : MonoBehaviour
{
    //Member Variables 
    public SpriteRenderer spriteRenderer;
    private NodeType nodeType;

    //Sprite Icons 
    public Sprite openIcon;
    public Sprite endIcon;
    public Sprite impassIcon;
    public Sprite hackedIcon;
    public Sprite closedIcon;

    public Color greenColour;
    public Color redColour;


    public NodeType GetNodeType()
    {
        return nodeType;
    }

    public void SetNodeAsOpen()
    {
        nodeType = NodeType.OPEN;
        spriteRenderer.sprite = openIcon;
    }

    public void SetNodeAsEnd()
    {
        nodeType = NodeType.END;
        spriteRenderer.sprite = endIcon;
        spriteRenderer.color = greenColour;
    }

    public void SetNodeAsImpass()
    {
        nodeType = NodeType.IMPASS;
        spriteRenderer.sprite = impassIcon;
        spriteRenderer.color = redColour;
    }

    public void SetNodeAsHacked()
    {
        nodeType = NodeType.HACKED;
        spriteRenderer.sprite = hackedIcon;
        spriteRenderer.color = greenColour;
    }

    public void SetNodeAsClosed()
    {
        nodeType = NodeType.CLOSED;
        spriteRenderer.sprite = closedIcon;
        spriteRenderer.color = redColour;
    }
}
