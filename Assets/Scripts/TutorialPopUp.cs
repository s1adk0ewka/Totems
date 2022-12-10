using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameInfo.Instanse.IsTutorialPopUpActive= true;
        Time.timeScale = .0f;
        Totem.AnyActionAllowed = false;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
