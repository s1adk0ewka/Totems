using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private GameObject target;

    //public void SetTarget(GameObject target)
    //{
    //    this.target = target;
    //}
    void Start()
    {
        target = Spawner.Instanse.GetCurrentSpirit();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || target.IsUnityNull() || target.IsDestroyed())
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = target.transform.position;
            if (target.GetComponent<FollowRoute>().currentStatus != Status.Slowed)
            {
                Destroy(gameObject);
            }
        }
    }
}
