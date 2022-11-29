using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System;

public class Spawner : MonoBehaviour
{
    public static Spawner Instanse { get; private set; } = null;
    [SerializeField]
    private List<GameObject> Totems;
    [SerializeField]
    private Phase[] Phases;
    [SerializeField]
    private List<GameObject> SpiritsGameObjects = new List<GameObject>();
    [SerializeField]
    public Phase currentPhase { get; private set; }
    [SerializeField]
    private int currentPhaseNumber;
    private int totemSpawnLimit;
    private Vector3 totemSpawnPoint;
    private Vector3 spiritSpawnPoint;
    private System.Random rnd = new ();

    //[SerializeField]
    private GameObject currentTotem;
    //[SerializeField]
    private GameObject currentSpirit;

    [Header("Spirits objects")]
    [SerializeField]
    private GameObject FireSpirit;
    [SerializeField]
    private GameObject IceSpirit;
    [SerializeField]
    private GameObject ElectroSpirit;
    [SerializeField]
    private GameObject EarthSpirit;
    [SerializeField]
    private GameObject AirSpirit;

    private Dictionary<ElementalType, GameObject> spiritTypeDict;



    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else if (Instanse == this)
            Destroy(gameObject);
        spiritTypeDict= new Dictionary<ElementalType, GameObject>() 
        {
            {ElementalType.Fire, FireSpirit },
            {ElementalType.Ice, IceSpirit },
            {ElementalType.Electro, ElectroSpirit },
            {ElementalType.Earth, EarthSpirit },
            {ElementalType.Air, AirSpirit },
        };
        totemSpawnLimit =Totems.Count;
        
        spiritSpawnPoint = Camera.main.transform.position;
        //SpawnTotem();
        
      
    }

    // Start is called before the first frame update
    private void Start()
    {
        var totemsWithoutAir = Totems.Where(x => x.GetComponent<Totem>().GetTotemType() != ElementalType.Air).ToList();
        var index = rnd.Next(0, totemsWithoutAir.Count);
        totemSpawnPoint = Lanes.TopPoints[1];
        currentTotem = Instantiate(totemsWithoutAir[index], totemSpawnPoint, Quaternion.identity);
        //Totems.RemoveAt(index);
        Totems.Remove(totemsWithoutAir[index]);
        totemSpawnLimit--;
        //SpawnPhase();
        var defTotemLimit= Totems.Count+1;
        var NumberOFPhases = Phases.Count();
        //� ����� � ����� ������ ���� ����� ������ ������ � ���� ������� xxDDDDDDDDDDDDDDDDDDDDDDDDD ���� ��������� ��� ���������������
        DisplayText.Instanse.OnSpiritsCountChanged.AddListener(amount =>
        {
            DisplayText.Instanse.SpiritsCount.text = $"���� {amount}/{NumberOFPhases}";
        }
        );
        DisplayText.Instanse.OnTotemsCountChanged.AddListener(amount =>
        {
            DisplayText.Instanse.TotemsCount.text = $"������: {amount}/{defTotemLimit}";
        }
        );
        DisplayText.Instanse.ChangeTotemsCount(totemSpawnLimit);
        DisplayText.Instanse.ChangePhaseCount(currentPhaseNumber-1);
        SpawnPhase();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Current phase - {currentPhaseNumber}\nSpirits count{SpiritsGameObjects.Count}");
        if(currentTotem is null||currentTotem.gameObject.IsUnityNull())
            SpawnTotem();

        if (currentPhaseNumber < Phases.Count() &&
            SpiritsGameObjects.Count == 0
             && (currentTotem.GetComponent<Totem>().onTop ||
                    (currentTotem.GetComponent<Totem>().isLast && currentTotem.GetComponent<Totem>().onBottom)))
        {
            SpawnPhase();
        }
        if (currentPhaseNumber == Phases.Count() && SpiritsGameObjects.Count == 0)
        {
            GameInfo.Instanse.ShowGameWon();
            Totem.AnyActionAllowed = false;
        }
        //if ((currentSpirit is null || currentSpirit.gameObject.IsUnityNull())||!currentTotem.GetComponent<Totem>().isFalling)
        //    SpawnSpirit();
        //Debug.Log(currentSpirit);
    }

    private void SpawnPhase()
    {
        SpiritsGameObjects.Clear();
        currentPhase = Phases[currentPhaseNumber++];
        var counter = 0;
        foreach(var spiritType in currentPhase.SpiritsInPhase)
        {
            var spirit = Instantiate(spiritTypeDict[spiritType], spiritSpawnPoint, Quaternion.identity);
            if(currentPhase.Routes.Count()>0)
                spirit.GetComponent<Spirit>().SetRoute(currentPhase.Routes[counter++]);
            SpiritsGameObjects.Add(spirit);
        }
        DisplayText.Instanse.ChangePhaseCount(currentPhaseNumber);
    }

    public void SpawnTotem()
    {
        if (totemSpawnLimit > 0 && Totems.Count>0)
        {
            var index = rnd.Next(0, Totems.Count);
            currentTotem = Instantiate(Totems[index], totemSpawnPoint, Quaternion.identity);
            Totems.RemoveAt(index);
            totemSpawnLimit--;
            if (totemSpawnLimit == 0) currentTotem.GetComponent<Totem>().isLast = true;
            DisplayText.Instanse.ChangeTotemsCount(totemSpawnLimit);
        }
    }

    //public void SpawnSpirit()
    //{
    //    if (spiritSpawnLimit > 0
    //        && (currentSpirit == null || currentSpirit.IsUnityNull() || currentSpirit.IsDestroyed()))
    //    {
    //        //currentSpirit = Instantiate(Spirits[rnd.Next(0, Spirits.Count)], spiritSpawnPoint, Quaternion.identity);
    //        spiritSpawnLimit--;
    //        DisplayText.Instanse.ChangeSpiritsCount(spiritSpawnLimit);
    //    }
    //}

    public GameObject GetCurrentTotem()
    {
        return currentTotem;
    }


    //TODO rework referenced stuff
    public List<GameObject> GetCurrentSpirits()
    {
        return SpiritsGameObjects;
    }
}
