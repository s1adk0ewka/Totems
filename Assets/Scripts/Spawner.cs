using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System;
using System.Reflection;

public class Spawner : MonoBehaviour
{
    public static Spawner Instanse { get; private set; } = null;
    [SerializeField]
    private List<GameObject> Totems;
    [SerializeField]
    private bool AreTotemsSpawnRandomly;
    [SerializeField]
    private Phase[] Phases;
    [SerializeField]
    private List<GameObject> SpiritsGameObjects = new List<GameObject>();
    [SerializeField]
    public Phase currentPhase { get; private set; }
    [SerializeField]
    private int currentPhaseNumber;
    private int totemSpawnLimit;
    private int totemSpawnLimitNumber;
    [SerializeField]
    private bool isNoTotemSpawnLimit;
    private Vector3 totemSpawnPoint;
    private Vector3 spiritSpawnPoint;
    private System.Random rnd = new ();

    //[SerializeField]
    private GameObject currentTotem;
    //[SerializeField]
    private GameObject currentSpirit;

    [Header("Spirits objects")]
    [SerializeField]
    private GameObject DefaultSpirit;
    //[SerializeField]
    private Color DefaultColor = Color.white;
    [SerializeField]
    private GameObject FireSpirit;
    [SerializeField]
    private Color FireColor;
    [SerializeField]
    private GameObject IceSpirit;
    [SerializeField]
    private Color IceColor;
    [SerializeField]
    private GameObject ElectroSpirit;
    [SerializeField]
    private Color ElectroColor;
    [SerializeField]
    private GameObject EarthSpirit;
    [SerializeField]
    private Color EarthColor;
    [SerializeField]
    private GameObject AirSpirit;
    [SerializeField]
    private Color AirColor;

    private Dictionary<ElementalType, GameObject> spiritTypeDict;
    public Dictionary<ElementalType, Color> typeColorsDict { get; private set; }



    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else if (Instanse == this)
            Destroy(gameObject);
        spiritTypeDict = new Dictionary<ElementalType, GameObject>()
        {
            {ElementalType.None,DefaultSpirit },
            {ElementalType.Fire, FireSpirit },
            {ElementalType.Ice, IceSpirit },
            {ElementalType.Electro, ElectroSpirit },
            {ElementalType.Earth, EarthSpirit },
            {ElementalType.Air, AirSpirit },
        };
        typeColorsDict = new Dictionary<ElementalType, Color>()
        {
            {ElementalType.None,DefaultColor },
            {ElementalType.Fire, FireColor },
            {ElementalType.Ice, IceColor },
            {ElementalType.Electro, ElectroColor },
            {ElementalType.Earth, EarthColor },
            {ElementalType.Air, AirColor },
        };
        totemSpawnLimit =Totems.Count;
        totemSpawnLimitNumber = totemSpawnLimit;
        
        spiritSpawnPoint = Camera.main.transform.position;
        //SpawnTotem();
        
      
    }

    // Start is called before the first frame update
    private void Start()
    {
        //var totemsWithoutAir = Totems.Where(x => x.GetComponent<Totem>().GetTotemType() != ElementalType.Air).ToList();
        //var index = rnd.Next(0, totemsWithoutAir.Count);
        //totemSpawnPoint = Lanes.TopPoints[1];
        //currentTotem = Instantiate(totemsWithoutAir[index], totemSpawnPoint, Quaternion.identity);
        //Totems.Remove(totemsWithoutAir[index]);
        //totemSpawnLimit--;
        var defTotemLimit = Totems.Count;// +1;
        var NumberOFPhases = Phases.Count();
        //À ÍÀÕÓß ß ÄÅËÀË ÈÂÅÍÒÛ ÅÑËÈ ÌÎÆÍÎ ÏÐÎÑÒÎ ÌÅÒÎÄÛ Ó ÍÅÃÎ ÂÛÇÂÀÒÜ xxDDDDDDDDDDDDDDDDDDDDDDDDD ÁÎÆÅ ÇÀÏÐÅÒÈÒÅ ÌÍÅ ÏÐÎÃÐÀÌÌÈÐÎÂÀÒü
        //DisplayText.Instanse.OnSpiritsCountChanged.AddListener(amount =>
        //{
        //    DisplayText.Instanse.SpiritsCount.text = $"Ôàçà {amount}/{NumberOFPhases}";
        //}
        //);
        //DisplayText.Instanse.OnTotemsCountChanged.AddListener(amount =>
        //{
        //    DisplayText.Instanse.TotemsCount.text = $"Òîòåìû: {amount}/{defTotemLimit}";
        //}
        //);
        DisplayText.Instanse.ChangeTotemsCount(totemSpawnLimit);
        DisplayText.Instanse.ChangePhaseCount(currentPhaseNumber-1);
        SpawnPhase();
        SpawnFirstTotem();
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

    public int GetPhasesCount()
    {
        return Phases.Count();
    }

    public int GetTotemsLimit()
    {
        return totemSpawnLimitNumber;
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
        //TODO refactor this later
        if (isNoTotemSpawnLimit)
        {
            currentTotem=Instantiate(Totems[rnd.Next(0, Totems.Count)], totemSpawnPoint, Quaternion.identity);
        }
        else if (isNoTotemSpawnLimit||(totemSpawnLimit > 0 && Totems.Count>0))//<-- this is bad 
        {
            var index = AreTotemsSpawnRandomly ? rnd.Next(0, Totems.Count) : 0;//Totems.Count-1;
            currentTotem = Instantiate(Totems[index], totemSpawnPoint, Quaternion.identity);
            if(!isNoTotemSpawnLimit) Totems.RemoveAt(index);
            totemSpawnLimit--;
            if (totemSpawnLimit == 0 && !isNoTotemSpawnLimit )
                currentTotem.GetComponent<Totem>().isLast = true;
            DisplayText.Instanse.ChangeTotemsCount(totemSpawnLimit);
        }
    }

    private void SpawnFirstTotem()
    {
        //if (isNoTotemSpawnLimit)
        //{
        //    Instantiate(Totems[rnd.Next(0, Totems.Count)], totemSpawnPoint, Quaternion.identity);
        //}
        if(isNoTotemSpawnLimit||(totemSpawnLimit > 0 && Totems.Count > 0))
        {
            var spiritsType = GetCurrentSpirits()
                .Select(spirit => spirit.GetComponent<Spirit>().GetElementalType())
                .ToList();
            var totemsWithoutAir = Totems
                .Where(x =>
                x.GetComponent<Totem>().GetTotemType() != ElementalType.Air &&
                !spiritsType.Contains(x.GetComponent<Totem>().GetTotemType()))
                .ToList();
            var index = AreTotemsSpawnRandomly ? rnd.Next(0, totemsWithoutAir.Count) : 0;//totemsWithoutAir.Count-1;
            totemSpawnPoint = Lanes.TopPoints[1];
            currentTotem = Instantiate(totemsWithoutAir[index], totemSpawnPoint, Quaternion.identity);
            if (!isNoTotemSpawnLimit) Totems.Remove(totemsWithoutAir[index]);
            totemSpawnLimit--;
            if (totemSpawnLimit == 0 && !isNoTotemSpawnLimit)
                currentTotem.GetComponent<Totem>().isLast = true;
            DisplayText.Instanse.ChangeTotemsCount(totemSpawnLimit);
        }
    }


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
