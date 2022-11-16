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
    private int totemSpawnLimit;
    [SerializeField]
    private int spiritSpawnLimit;
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
        totemSpawnLimit =Totems.Count;
        totemSpawnPoint = Lanes.TopPoints[1]-new Vector3(0,0.5f,0);
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
    private void Start()
    {
        var defTotemLimit= Totems.Count+1;
        var defSpiritLimit = spiritSpawnLimit;
        DisplayText.Instanse.OnSpiritsCountChanged.AddListener(amount =>
        {
            DisplayText.Instanse.SpiritsCount.text = $"Фаза {defSpiritLimit-amount}/{defSpiritLimit}";
        }
        );
        DisplayText.Instanse.OnTotemsCountChanged.AddListener(amount =>
        {
            DisplayText.Instanse.TotemsCount.text = $"Тотемы: {amount}/{defTotemLimit}";
        }
        );
        DisplayText.Instanse.ChangeTotemsCount(totemSpawnLimit);
    }

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
            DisplayText.Instanse.ChangeTotemsCount(totemSpawnLimit);
        }
    }

    public void SpawnSpirit()
    {
        if (spiritSpawnLimit > 0
            && (currentSpirit == null || currentSpirit.IsUnityNull() || currentSpirit.IsDestroyed()))
        {
            currentSpirit = Instantiate(Spirits[rnd.Next(0, Spirits.Count)], spiritSpawnPoint, Quaternion.identity);
            spiritSpawnLimit--;
            DisplayText.Instanse.ChangeSpiritsCount(spiritSpawnLimit);
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
