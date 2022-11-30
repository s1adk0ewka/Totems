using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject IceObj;
    private GameObject target;
    private float speed = 10f;
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

    void Update()
    {
        if (target == null)
            Destroy(gameObject);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if (transform.position == target.transform.position)
            {
                var ice=Instantiate(IceObj, new Vector3(0, 0, 1), Quaternion.identity).GetComponent<Ice>();
                ice.target = target;
                Destroy(gameObject);
            }
        }
    }
}
