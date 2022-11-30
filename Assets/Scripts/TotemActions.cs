using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TotemActions : MonoBehaviour
{
    [SerializeField]
    private GameObject fireballObj;
    [SerializeField]
    private GameObject snowballObj;
    [SerializeField]
    private GameObject electroballObj;
    [SerializeField]
    private GameObject ElectroObj;
    //[SerializeField]
    //private GameObject IceObj;
    [SerializeField]
    private GameObject EarthObj;

    public Dictionary<ElementalType, Action> dict;

    private void Awake()
    {
        dict = new Dictionary<ElementalType, Action>
        {
            { ElementalType.Fire, FireAction },
            { ElementalType.Earth, EarthAction },
            { ElementalType.Electro, ElectroAction },
            { ElementalType.Ice, IceAction },
            { ElementalType.Air, AirAction }
        };
    }

    private void AirAction()
    {
    }

    public void FireAction()
    {
        Instantiate(fireballObj, transform.position + new Vector3(0, 0, -1), Quaternion.identity);
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
        //var spirit = Spawner.Instanse.GetCurrentSpirits().FirstOrDefault(spirit => spirit.GetComponent<Spirit>().type != ElementalType.Electro);
        //spirit.GetComponent<Spirit>().Stun(ElectroTotemStunTimeSeconds, new Color(175f / 255f, 0, 1, 1));
        Instantiate(ElectroObj, new Vector3(0, 0, 1), Quaternion.identity);
    }

    private void IceAction()
    {
       
        //var spirit = Spawner.Instanse.GetCurrentSpirits().FirstOrDefault(spirit=>spirit.GetComponent<Spirit>().type!=ElementalType.Ice);
        ////might be desync with effect, chek it later
        //spirit.GetComponent<Spirit>().Slow(IceTotemSlowTimeSeconds, IceTotemSlowCoefficient);
        Instantiate(snowballObj, transform.position+new Vector3(0, 0, -1), Quaternion.identity);
    }
}
