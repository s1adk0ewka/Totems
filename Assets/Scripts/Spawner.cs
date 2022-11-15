using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour
{
    public static Spawner Instanse { get; private set; } = null;
    [SerializeField]
    private List<GameObject> Totems;
    [SerializeField]
    private List<GameObject> Spirits;
    [SerializeField]
    private int totemSpawnLimit;
    [SerializeField]
    private int spiritSpawnLimit = 5;
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
        totemSpawnLimit=Totems.Count;
        totemSpawnPoint = Lanes.TopPoints[1];
        spiritSpawnPoint = Camera.main.transform.position;
        //SpawnTotem();
        var totemsWithoutAir = Totems.Where(x => x.GetComponent<Totem>().GetTotemType() != Totem.TotemType.Air).ToList();
        var index = rnd.Next(0, totemsWithoutAir.Count);
        currentTotem = Instantiate(totemsWithoutAir[index], totemSpawnPoint, Quaternion.identity);
        //Totems.RemoveAt(index);
        Totems.Remove(totemsWithoutAir[index]);
        totemSpawnLimit--;
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(currentTotem is null||currentTotem.gameObject.IsUnityNull())
            SpawnTotem();
        if ((currentSpirit is null || currentSpirit.gameObject.IsUnityNull())||!currentTotem.GetComponent<Totem>().isFalling)
            SpawnSpirit();
        //Debug.Log(currentSpirit);
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
        if (spiritSpawnLimit > 0
            && (currentSpirit == null || currentSpirit.IsUnityNull() || currentSpirit.IsDestroyed()))
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
