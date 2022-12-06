using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 10f)]
    private float IceTotemSlowTimeSeconds = 10f;
    [SerializeField]
    [Range(0f, 1f)]
    private float IceTotemSlowCoefficient = 0.5f;
    public GameObject target;

    private bool isActionAllowed = true;
    void Start()
    {
        if (!isActionAllowed) return;
        if (target!=null)
        {
            switch (target.GetComponent<Spirit>().GetElementalType())
            {
                case ElementalType.Electro: case ElementalType.Fire:
                    Destroy(target);
                    break;
                case ElementalType.Earth:
                    SlowAllSpirits();
                    Destroy(target);
                    Destroy(gameObject);
                    break;
                case ElementalType.Air:
                    target.GetComponent<Spirit>().ChangeType(ElementalType.Ice);
                    break;
                case ElementalType.None:
                    target.GetComponent<Spirit>().Slow(IceTotemSlowTimeSeconds, IceTotemSlowCoefficient);
                    break;
                case ElementalType.Ice:
                    Debug.Log("Ice totem shouldn't target ice spirit");
                    break;
                default:
                    Debug.Log($"Unknown target elemental type {target.GetComponent<Spirit>().GetElementalType()}");
                    break;
            }
            transform.position = target.transform.position + new Vector3(0, 0, -10);
        }
    }

    private void SlowAllSpirits()
    {
        foreach (var spirit in Spawner.Instanse.GetCurrentSpirits())
        {
            spirit.GetComponent<Spirit>().Slow(IceTotemSlowTimeSeconds, IceTotemSlowCoefficient);
            var ice = Instantiate(gameObject, spirit.gameObject.transform.position + new Vector3(0, 0, -10), Quaternion.identity);
            ice.GetComponent<Ice>().target = spirit;
            ice.GetComponent<Ice>().isActionAllowed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //This is bad. Better way is through events
        if (target == null || target.IsUnityNull() || target.IsDestroyed())
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = target.transform.position + new Vector3(0,0,1);
            if (target.GetComponent<Spirit>().currentStatus != Status.Slowed)
            {
                Destroy(gameObject);
            }
        }
    }
}

