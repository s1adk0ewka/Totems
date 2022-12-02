using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Electro : MonoBehaviour
{
    public GameObject target;
    [SerializeField]
    [Range(0f, 10f)]
    private float ElectroTotemStunTimeSeconds = 5f;

    public bool isActionAllowed = true;

    void Start()
    {
        if (!isActionAllowed) return;
        if (target != null)
        {
            switch (target.GetComponent<Spirit>().GetElementalType())
            {
                case ElementalType.Fire: case ElementalType.Earth:
                    Destroy(target);
                    break;
                case ElementalType.Ice:
                    StunAllSpirits();
                    break;
                case ElementalType.None:
                    target.GetComponent<Spirit>().Stun(ElectroTotemStunTimeSeconds);
                    break;
                case ElementalType.Air:
                    //spirit will not change the color
                    target.GetComponent<Spirit>().ChangeType(ElementalType.Electro);
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
            spirit.GetComponent<Spirit>().Stun(ElectroTotemStunTimeSeconds);
            var electro =  Instantiate(gameObject, spirit.gameObject.transform.position+ new Vector3(0,0,-10),Quaternion.identity);
            electro.GetComponent<Electro>().target= spirit;
            electro.GetComponent<Electro>().isActionAllowed= false;
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
