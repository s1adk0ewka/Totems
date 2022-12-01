using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private GameObject Explosion;
    [SerializeField]
    private static float speed = 20f;
    private GameObject target;
    void Start()
    {
        target = Spawner.Instanse.GetCurrentSpirits()
            .FirstOrDefault(spirit => spirit.GetComponent<Spirit>().GetElementalType() != ElementalType.Fire);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(target.transform.position * speed * Time.deltaTime);
        if (target == null) Destroy(gameObject);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if (transform.position == target.transform.position)
            {
                Instantiate(Explosion, target.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
                Destroy(target);
                Destroy(gameObject);
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Spirit")
    //    {
    //        Destroy(collision.gameObject);
    //        Destroy(gameObject);
    //    }
    //}
}
