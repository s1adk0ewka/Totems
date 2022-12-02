using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject totemObj;
    private Totem totem;
    private float invulTime = .3f;
    void Start()
    {
        totemObj = Spawner.Instanse.GetCurrentTotem();
        totem = totemObj.GetComponent<Totem>();
        totem.ProtectedByEarthTotem = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (totemObj == null || !totem.ProtectedByEarthTotem)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spirit")
        {
            var spirit = collision.gameObject.GetComponent<Spirit>();
            switch (spirit.GetElementalType())
            {
                case ElementalType.Ice: case ElementalType.Electro: case ElementalType.None:
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    break;
                case ElementalType.Earth:
                    StartCoroutine(DestroyShieldAfterSpiritCantHurtForSeconds(spirit, invulTime));
                    break;
                case ElementalType.Air:
                    spirit.ChangeType(ElementalType.Earth);
                    StartCoroutine(DestroyShieldAfterSpiritCantHurtForSeconds(spirit, invulTime));
                    break;
                case ElementalType.Fire:
                    SetAllSpiritsToDefaultElementalType();
                    StartCoroutine(DestroyShieldAfterSpiritCantHurtForSeconds(spirit, invulTime));
                    break;
            }
        }
    }

    private IEnumerator DestroyShieldAfterSpiritCantHurtForSeconds(Spirit spirit,float time)
    {
        Debug.Log("OK");
        Destroy(gameObject.GetComponent<CircleCollider2D>());
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        spirit.CanHurt = false;
        totem.ProtectedByEarthTotem = false;
        yield return new WaitForSeconds(time);
        spirit.CanHurt = true;
        Destroy(gameObject);
    }

    private void SetAllSpiritsToDefaultElementalType()
    {
        var spirits = Spawner.Instanse
            .GetCurrentSpirits()
            .Select(spiritObj => spiritObj.GetComponent<Spirit>())
            .ToList();
        foreach(var spirit in spirits)
        {
            spirit.ChangeType(ElementalType.None);
        }
    }

    private void OnDestroy()
    {
        totem.ProtectedByEarthTotem = false;
    }
}
