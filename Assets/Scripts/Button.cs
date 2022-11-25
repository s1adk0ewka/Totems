using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    [SerializeField]
    private string lvlName;
    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if (lvlName != null)
            SceneManager.LoadScene(lvlName);
        else
            Debug.Log($"Unknown level name {lvlName}");
    }
}
