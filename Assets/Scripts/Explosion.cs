using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    private void Awake()
    {
        StartCoroutine(DestroyAfterTimeInSeconds(2f));
    }
    void Start()
    {
        if (target != null)
        {
            switch (target.GetComponent<Spirit>().GetElementalType())
            {
                case ElementalType.Ice:
                case ElementalType.Earth:
                case ElementalType.None:
                    Destroy(target);
                    break;
                case ElementalType.Electro:
                    ExplodeAll();
                    break;
                case ElementalType.Air:
                    target.GetComponent<Spirit>().ChangeType(ElementalType.Fire);
                    break;
                case ElementalType.Fire:
                    Debug.Log("Fire totem shouldn't target fire spirit");
                    break;
                default:
                    Debug.Log($"Unknown target elemental type {target.GetComponent<Spirit>().GetElementalType()}");
                    break;
            }
            transform.position = target.transform.position + new Vector3(0, 0, -10);
        }
    }

    private void ExplodeAll()
    {
        foreach(var spirit in Spawner.Instanse.GetCurrentSpirits())
        {
            var explosion = Instantiate(gameObject, spirit.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
            explosion.GetComponent<Explosion>().target = spirit;
            Destroy(spirit);
        }
    }

    private IEnumerator DestroyAfterTimeInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
