using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HackTile : MonoBehaviour
{
    public int indexNumber;
    public TextMeshPro numberText;
    public int randomWaitTime;



    public void GenerateRandomNumber()
    {
        int randomNumber = Random.Range(1, 10);

        indexNumber = randomNumber;
        numberText.text = indexNumber.ToString();
    }

    public IEnumerator GenerateRandomNumberRepeated()
    {
        int randomNumber = Random.Range(1, 10);

        indexNumber = randomNumber;
        numberText.text = indexNumber.ToString();
        yield return new WaitForSeconds(GetRandomWaitTime());  
        StartCoroutine(GenerateRandomNumberRepeated());
    }


    public int GetRandomWaitTime()
    {
        return Random.Range(1, 4);
    }


    


}
