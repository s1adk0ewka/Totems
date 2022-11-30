using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElectroBall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject ElectroObj;
    private GameObject target;
    private float speed = 15f;
    private Dictionary<ElementalType, int> priorityDict = new Dictionary<ElementalType, int>()
    {
        { ElementalType.Fire, 1 },
        { ElementalType.Ice, 2 },
        { ElementalType.Earth, 3 },
        { ElementalType.Air, 4 },
        { ElementalType.Electro, 5 },
    };
    void Start()
    {
        target = Spawner.Instanse
            .GetCurrentSpirits()
            .Where(spirit => spirit.GetComponent<Spirit>().GetElementalType() != ElementalType.Electro)
            .OrderBy(spirit => priorityDict[spirit.GetComponent<Spirit>().GetElementalType()])
            .ToList().FirstOrDefault();
    }

    void Update()
    {
        if (target == null)
            Destroy(gameObject);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if (transform.position == target.transform.position)
            {
                var electro = Instantiate(ElectroObj, new Vector3(0, 0, 1), Quaternion.identity).GetComponent<Electro>();
                electro.target = target;
                Destroy(gameObject);
            }
        }
    }
}
