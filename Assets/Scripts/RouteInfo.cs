using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteInfo : MonoBehaviour
{
    [field: SerializeField]

    public bool isReversed { get; set; } 
    
    [field:SerializeField]
    public Vector2 offset { get; set; }

    [field: SerializeField]
    public int startRoute { get; set; }
}
