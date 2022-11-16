using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    public static DisplayText Instanse { get; private set; } = null;
    public TextMeshProUGUI TotemsCount;
    public TextMeshProUGUI SpiritsCount;
    public UnityEvent<int> OnSpiritsCountChanged= new UnityEvent<int>();
    public UnityEvent<int> OnTotemsCountChanged = new UnityEvent<int>();

    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else if (Instanse == this)
            Destroy(gameObject);
    }
    public void ChangeSpiritsCount(int amount)
    {
        OnSpiritsCountChanged.Invoke(amount);
    }

    public void ChangeTotemsCount(int amount)
    {
        OnTotemsCountChanged.Invoke(amount);
    }
}
