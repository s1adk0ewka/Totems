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
        //if spirit just spawned, he might cleansse this effect
        //TODO
        if (!Spawner.Instanse.GetCurrentSpirit().gameObject.IsUnityNull())
        {
            StartCoroutine(SlowDownCoroutine(0f, ElectroTotemStunTimeSeconds));
        }
    }

    private void IceAction()
    {
        //if spirit just spawned, he might cleansse this effect
        //TODO
        if (!Spawner.Instanse.GetCurrentSpirit().gameObject.IsUnityNull())
        {
            StartCoroutine(SlowDownCoroutine(IceTotemSlowCoefficient, IceTotemSlowTimeSeconds));
        }
    }

    private IEnumerator SlowDownCoroutine(float slowCoefficient, float time)
    {
        var followRouteComp = Spawner.Instanse.GetCurrentSpirit().GetComponent<FollowRoute>();
        followRouteComp.SetSpeed(Constants.DefaultSpiritSpeedModifier*slowCoefficient);
        yield return new WaitForSeconds(time);
        followRouteComp.SetSpeed(Constants.DefaultSpiritSpeedModifier);
    }
}
