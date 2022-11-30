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
    //private Dictionary<ElementalType, int> priorityDict = new Dictionary<ElementalType, int>()
    //{
    //    { ElementalType.Electro, 2 },
    //    { ElementalType.Fire, 1 },
    //    { ElementalType.Earth, 3},
    //    { ElementalType.Air, 4 },
    //    { ElementalType.Ice, 5 },
    //};

    //public void SetTarget(GameObject target)
    //{
    //    this.target = target;
    //}
    void Start()
    {
        //target = Spawner.Instanse.GetCurrentSpirits().FirstOrDefault(spirit => spirit.GetComponent<Spirit>().GetElementalType() != ElementalType.Ice);
        //var spirits = Spawner.Instanse
        //    .GetCurrentSpirits()
        //    .Where(spirit => spirit.GetComponent<Spirit>().GetElementalType() != ElementalType.Ice)
        //    .OrderBy(spirit => priorityDict[spirit.GetComponent<Spirit>().GetElementalType()])
        //    .ToList();
        //target = spirits.FirstOrDefault();
        if (!isActionAllowed) return;
        if (target!=null)
        {
            switch (target.GetComponent<Spirit>().GetElementalType())
            {
                case ElementalType.Electro:
                    SlowAllSpirits();
                    break;
                case ElementalType.Fire:
                    Destroy(target);
                    break;
                case ElementalType.Earth:
                    target.GetComponent<Spirit>().Slow(IceTotemSlowTimeSeconds, IceTotemSlowCoefficient);
                    break;
                case ElementalType.Air:
                    //spirit will not change the color
                    target.GetComponent<Spirit>().Slow(IceTotemSlowTimeSeconds, IceTotemSlowCoefficient);
                    target.GetComponent<Spirit>().ChangeTypeFromAir(ElementalType.Ice);
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

