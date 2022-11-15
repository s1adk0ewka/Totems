using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject totem;
    void Start()
    {
        totem = Spawner.Instanse.GetCurrentTotem();
    }

    // Update is called once per frame
    void Update()
    {
        if (!totem.GetComponent<Totem>().ProtectedByEarthTotem)
        {
            Destroy(gameObject);
        }
    }
}
