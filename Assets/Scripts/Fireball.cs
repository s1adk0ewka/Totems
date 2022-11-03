using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private static float speed = 20f;
    private GameObject target;
    void Start()
    {
        target = Spawner.Instanse.GetCurrentSpirit();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(target.transform.position * speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
        if (transform.position == target.transform.position)
        {
            Destroy(target);
            Destroy(gameObject);
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
