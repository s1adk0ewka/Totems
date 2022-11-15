using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Electro : MonoBehaviour
{
    //[SerializeField]
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
        //This is bad. Better way is through events
        if (target == null || target.IsUnityNull() || target.IsDestroyed()) 
        {
            Destroy(gameObject);
        }
        else
        {
            if (target.GetComponent<FollowRoute>().currentStatus != Status.Stuned)
            {
                Destroy(gameObject);
            }
        }
    }
}
