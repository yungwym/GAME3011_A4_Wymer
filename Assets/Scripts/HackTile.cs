using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HackTile : MonoBehaviour
{
    public int indexNumber;
    public TextMeshPro numberText;
    public int randomWaitTime;

    private bool repeating = false;
    private float timeRemaining;

    public Color greenColour;
    public Color redColour;

    public SpriteRenderer spriteRenderer;

    private void Update()
    {
       if (repeating)
        {
            if (timeRemaining > 0 )
            {
                timeRemaining -= Time.deltaTime;
                spriteRenderer.color = Color.Lerp(redColour, greenColour, timeRemaining);

            }
            else if (timeRemaining < 0)
            {
                GenerateRandomNumRepeating();
            }
        }
    }

    public void GenerateRandomNumber()
    {
        int randomNumber = Random.Range(1, 10);

        indexNumber = randomNumber;
        numberText.text = indexNumber.ToString();
        numberText.color = Color.black;
    }

    /*
    public IEnumerator GenerateRandomNumberRepeated()
    {
        repeating = true;

        int randomNumber = Random.Range(1, 10);

        indexNumber = randomNumber;
        numberText.text = indexNumber.ToString();
        yield return new WaitForSeconds(GetRandomWaitTime());  
        StartCoroutine(GenerateRandomNumberRepeated());
    }
    */

    public void GenerateRandomNumRepeating()
    {
        repeating = true;

        int randomNumber = Random.Range(1, 10);
        timeRemaining = GetRandomWaitTime();

        indexNumber = randomNumber;
        numberText.text = indexNumber.ToString();
    }


    public int GetRandomWaitTime()
    {
        return Random.Range(1, 4);
    }


    


}
