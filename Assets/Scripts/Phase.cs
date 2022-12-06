using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Phase 1",menuName = "ScriptableObjects/Waves", order = 1)]
public class Phase : ScriptableObject
{
    [field: SerializeField]
    public List<ElementalType> SpiritsInPhase { get; private set; }
    [field: SerializeField]
    public List<GameObject> Routes { get; private set; }

    //public List<GameObject> SpiritsGameObjects { get; set; } = new List<GameObject>();
}
