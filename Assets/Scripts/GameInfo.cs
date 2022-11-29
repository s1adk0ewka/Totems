using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInfo : MonoBehaviour
{
    public static GameInfo Instanse { get; private set; } = null;
    [SerializeField]
    private GameObject GameWonCanvas;
    [SerializeField]
    private GameObject GameLostCanvas;

    public bool isGameOver { get; set; } = false;

    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else if (Instanse == this)
            Destroy(gameObject);
        GameWonCanvas.SetActive(false);
        GameLostCanvas.SetActive(false);
    }

    public void ShowGameLost()
    {
        isGameOver = true;
        GameLostCanvas.SetActive(true);
    }

    public void ShowGameWon()
    {
        isGameOver = true;
        GameLostCanvas.SetActive(false);
        GameWonCanvas.SetActive(true);
    }
}