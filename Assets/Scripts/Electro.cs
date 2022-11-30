using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Electro : MonoBehaviour
{
    //[SerializeField]
    public GameObject target;
    [SerializeField]
    [Range(0f, 10f)]
    private float ElectroTotemStunTimeSeconds = 5f;

    public bool isActionAllowed = true;

    //private Dictionary<ElementalType, int> priorityDict = new Dictionary<ElementalType, int>()
    //{
    //    { ElementalType.Fire, 1 },
    //    { ElementalType.Ice, 2 },
    //    { ElementalType.Earth, 3 },
    //    { ElementalType.Air, 4 },
    //    { ElementalType.Electro, 5 },
    //};
    void Start()
    {
        //var spirits = Spawner.Instanse
        //    .GetCurrentSpirits()
        //    .Where(spirit => spirit.GetComponent<Spirit>().GetElementalType() != ElementalType.Electro)
        //    .OrderBy(spirit => priorityDict[spirit.GetComponent<Spirit>().GetElementalType()])
        //    .ToList();

        //target=spirits.FirstOrDefault();
        if (!isActionAllowed) return;
        if (target != null)
        {
            switch (target.GetComponent<Spirit>().GetElementalType())
            {
                case ElementalType.Fire:
                    Explode();
                    break;
                case ElementalType.Ice:
                    StunAllSpirits();
                    break;
                case ElementalType.Earth:
                    target.GetComponent<Spirit>().Stun(ElectroTotemStunTimeSeconds, new Color(175f / 255f, 0, 1, 1));
                    break;
                case ElementalType.Air:
                    //spirit will not change the color
                    target.GetComponent<Spirit>().Stun(ElectroTotemStunTimeSeconds, new Color(175f / 255f, 0, 1, 1));
                    target.GetComponent<Spirit>().ChangeTypeFromAir(ElementalType.Electro);
                    break;
                default:
                    Debug.Log($"Unknown target elemental type {target.GetComponent<Spirit>().GetElementalType()}");
                    break;


            }
            transform.position = target.transform.position + new Vector3(0, 0, -10);
        }
            
    }

    private void StunAllSpirits()
    {
        foreach (var spirit in Spawner.Instanse.GetCurrentSpirits())
        {
            spirit.GetComponent<Spirit>().Stun(ElectroTotemStunTimeSeconds, new Color(175f / 255f, 0, 1, 1));
            var electro =  Instantiate(gameObject, spirit.gameObject.transform.position+ new Vector3(0,0,-10),Quaternion.identity);
            electro.GetComponent<Electro>().isActionAllowed= false;
        }
    }

    private void Explode()
    {
        foreach(var spirit in Spawner.Instanse.GetCurrentSpirits())
        {
            Destroy(spirit);
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
            if (target.GetComponent<Spirit>().currentStatus != Status.Stuned)
            {
                Destroy(gameObject);
            }
        }
    }
}
