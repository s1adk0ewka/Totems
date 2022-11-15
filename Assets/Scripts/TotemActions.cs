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
    [SerializeField]
    private GameObject ElectroObj;
    [SerializeField]
    private GameObject IceObj;
    [SerializeField]
    private GameObject EarthObj;
    [SerializeField]
    [Range(0f, 10f)]
    private float IceTotemSlowTimeSeconds=10f;
    [SerializeField]
    [Range(0f, 1f)]
    private float IceTotemSlowCoefficient = 0.5f;
    [SerializeField]
    [Range(0f, 10f)]
    private float ElectroTotemStunTimeSeconds = 5f;

    public Dictionary<Totem.TotemType, Action> dict;

    private void Awake()
    {
        dict = new Dictionary<Totem.TotemType, Action>
        {
            { Totem.TotemType.Fire, FireAction },
            { Totem.TotemType.Earth, EarthAction },
            { Totem.TotemType.Electro, ElectroAction },
            { Totem.TotemType.Ice, IceAction },
            { Totem.TotemType.Air, AirAction }
        };
    }

    private void AirAction()
    {
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
        var totem = Spawner.Instanse.GetCurrentTotem();
        totem.GetComponent<Totem>().ProtectedByEarthTotem = true;
        var shield = Instantiate(EarthObj, totem.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        shield.transform.parent = totem.transform;
    }

    private void ElectroAction()
    {
        if (!Spawner.Instanse.GetCurrentSpirit().gameObject.IsUnityNull())
        {
            //StartCoroutine(SlowDownCoroutine(0f, ElectroTotemStunTimeSeconds));
            var spirit = Spawner.Instanse.GetCurrentSpirit();
            spirit.GetComponent<FollowRoute>().Stun(ElectroTotemStunTimeSeconds, new Color(175f / 255f, 0, 1, 1));
            Instantiate(ElectroObj, spirit.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
        }
    }

    private void IceAction()
    {
        if (!Spawner.Instanse.GetCurrentSpirit().gameObject.IsUnityNull())
        {
            //StartCoroutine(SlowDownCoroutine(IceTotemSlowCoefficient, IceTotemSlowTimeSeconds));
            var spirit = Spawner.Instanse.GetCurrentSpirit();
            Spawner.Instanse.GetCurrentSpirit().GetComponent<FollowRoute>().Slow(IceTotemSlowTimeSeconds, IceTotemSlowCoefficient);
            Instantiate(IceObj, spirit.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
        }
    }
}
