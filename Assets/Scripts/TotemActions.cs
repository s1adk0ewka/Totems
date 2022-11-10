using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TotemActions : MonoBehaviour
{
    [SerializeField]
    private GameObject fireballObj;

    public Dictionary<Totem.TotemType, Action> dict;

    private void Awake()
    {
        dict = new Dictionary<Totem.TotemType, Action>
        {
            { Totem.TotemType.Fire, FireAction },
            { Totem.TotemType.Earth, EarthAction },
            { Totem.TotemType.Electro, ElectroAction },
            { Totem.TotemType.Ice, IceAction }
        };
    }

    public void FireAction()
    {

        if (!Spawner.Instanse.GetCurrentSpirit().gameObject.IsUnityNull())
        {
            Instantiate(fireballObj, transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        }
            
    }

    private void EarthAction()
    {
        Spawner.Instanse.GetCurrentTotem().GetComponent<Totem>().ProtectedByEarthTotem = true;
    }

    private void ElectroAction()
    {

    }

    private void IceAction()
    {

    }
}
