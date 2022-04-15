using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{

    public HackTile currentOverlappingTile;

    private HackSequence hackSequence;

    private void Start()
    {
        hackSequence = FindObjectOfType<HackSequence>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentOverlappingTile != null)
        {
            hackSequence.CheckHackTile(currentOverlappingTile);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision with HackTile");

        if (collision.gameObject.CompareTag("HackTile"))
        {
            currentOverlappingTile = collision.gameObject.GetComponent<HackTile>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentOverlappingTile = null;
    }
}
