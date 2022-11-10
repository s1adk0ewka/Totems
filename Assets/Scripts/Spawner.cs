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
    [SerializeField]
    private int totemSpawnLimit = 10;
    [SerializeField]
    private int spiritSpawnLimit = 3;
    private Vector3 totemSpawnPoint;
    private Vector3 spiritSpawnPoint;
    private System.Random rnd = new ();

    [SerializeField]
    private GameObject currentTotem;
    [SerializeField]
    private GameObject currentSpirit;



    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else if (Instanse == this)
            Destroy(gameObject);
        totemSpawnPoint = Lanes.TopPoints[1];
        spiritSpawnPoint = Camera.main.transform.position;
        SpawnTotem();
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(currentTotem is null||currentTotem.gameObject.IsUnityNull())
            SpawnTotem();
        if (currentSpirit is null || currentSpirit.gameObject.IsUnityNull())
            SpawnSpirit();
    }

    public void SpawnTotem()
    {
        if (totemSpawnLimit > 0 && Totems.Count>0)
        {
            var index = rnd.Next(0, Totems.Count);
            currentTotem = Instantiate(Totems[index], totemSpawnPoint, Quaternion.identity);
            Totems.RemoveAt(index);
            totemSpawnLimit--;
        }
    }

    public void SpawnSpirit()
    {
        if (spiritSpawnLimit > 0)
        {
            currentSpirit = Instantiate(Spirits[rnd.Next(0, Spirits.Count)], spiritSpawnPoint, Quaternion.identity);
            spiritSpawnLimit--;
        }
    }

    public GameObject GetCurrentTotem()
    {
        return currentTotem;
    }

    public GameObject GetCurrentSpirit()
    {
        return currentSpirit;
    }
}
