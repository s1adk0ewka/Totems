using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instanse { get; private set; } = null;
    [SerializeField]
    private List<GameObject> Totems;
    [SerializeField]
    private List<GameObject> Spirits;

    private Vector3 totemSpawnPoint;
    private System.Random rnd = new ();


    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else if (Instanse == this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        totemSpawnPoint = Lanes.TopPoints[1];
        SpawnTotem();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnTotem()
    {
        
        Instantiate(Totems[rnd.Next(0,Totems.Count-1)], totemSpawnPoint, Quaternion.identity);
    }
}
