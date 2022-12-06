using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    public static DisplayText Instanse { get; private set; } = null;
    public TextMeshProUGUI TotemsCount;
    public TextMeshProUGUI SpiritsCount;
    public TextMeshProUGUI LevelId;
    //public UnityEvent<int> OnSpiritsCountChanged= new UnityEvent<int>();
    //public UnityEvent<int> OnTotemsCountChanged = new UnityEvent<int>();

    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else if (Instanse == this)
            Destroy(gameObject);
        if(LevelId!=null)
            LevelId.text = $"Уровень {SceneManager.GetActiveScene().buildIndex}";
    }
    public void ChangePhaseCount(int amount)
    {
        if(SpiritsCount!=null)
            SpiritsCount.text = $"Фаза: {amount}/{Spawner.Instanse.GetPhasesCount()}";
        //OnSpiritsCountChanged.Invoke(amount);
    }

    public void ChangeTotemsCount(int amount)
    {
        if(TotemsCount!=null)
            TotemsCount.text= $"Тотемы: {amount}/{Spawner.Instanse.GetTotemsLimit()}";
    }

}
