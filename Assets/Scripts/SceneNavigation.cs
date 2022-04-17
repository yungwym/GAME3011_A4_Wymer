using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{

    public AudioManager audioManager;

    

    public void QuitGame()
    {
        audioManager.Play("Select");
        Application.Quit();
    }

    public void LoadMainMenu()
    {

        StartCoroutine(LoadSceneWithSound("StartScene"));
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneWithSound("GameScene"));
    }

    public void LoadInstructionScene()
    {
        StartCoroutine(LoadSceneWithSound("InstructionScene"));
    }

    public IEnumerator LoadSceneWithSound(string scene)
    {
        audioManager.Play("Select");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(scene);
    }
}
