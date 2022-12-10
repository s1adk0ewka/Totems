using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        //cycled for prototype, rework later
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            SceneManager.LoadScene(1); //firstLevel
    }

    public void CloseTutorialPopUp()
    {
        //transform.parent.gameObject.SetActive(false);
        
        StartCoroutine(waitCoroutine());
        
    }
    private IEnumerator waitCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Totem.AnyActionAllowed = true;
        Time.timeScale = 1.0f;
        GameInfo.Instanse.IsTutorialPopUpActive = false;
        transform.parent.gameObject.SetActive(false);
    }
}
