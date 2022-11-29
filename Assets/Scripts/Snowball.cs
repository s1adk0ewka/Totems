using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject target;
    private Dictionary<ElementalType, int> priorityDict = new Dictionary<ElementalType, int>()
    {
        { ElementalType.Electro, 2 },
        { ElementalType.Fire, 1 },
        { ElementalType.Earth, 3},
        { ElementalType.Air, 4 },
        { ElementalType.Ice, 5 },
    };
    void Start()
    {
        target = Spawner.Instanse
            .GetCurrentSpirits()
            .Where(spirit => spirit.GetComponent<Spirit>().GetElementalType() != ElementalType.Ice)
            .OrderBy(spirit => priorityDict[spirit.GetComponent<Spirit>().GetElementalType()])
            .ToList().FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
