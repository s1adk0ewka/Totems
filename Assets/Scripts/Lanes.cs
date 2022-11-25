using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanes : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private static List<Vector3> topPoints;
    private static List<Vector3> bottomPoints;
    public static List<Vector3> TopPoints { get => topPoints;}
    public static List<Vector3> BottomPoints { get => bottomPoints;}

    void Awake()
    {
        var width = Camera.main.pixelWidth;
        var height = Camera.main.pixelHeight;
        var firstLaneCenter= getVector3(width/6,0);
        var topY = getVector3(0, height).y - 2*Constants.totemSize.y / 2;
        var botY = getVector3(0, 0).y + Constants.totemSize.y / 2;
        var secondLaneCenter = getVector3(width/2, 0);
        var third = getVector3(width*5/ 6, 0);
        topPoints = new List<Vector3>(new[] {
            new Vector3(firstLaneCenter.x,topY,0),
            new Vector3(secondLaneCenter.x,topY,0),
            new Vector3(third.x,topY,0)
        });
        bottomPoints = new List<Vector3>(new[] {
            new Vector3(firstLaneCenter.x,botY,0),
            new Vector3(secondLaneCenter.x,botY,0),
            new Vector3(third.x,botY,0) 
        });
        //foreach(var p in topPoints)
        //{
        //    Debug.Log(p);
        //}
        //var firstLane = new GameObject("Second lane");
        //firstLane.transform.parent = transform;
        //firstLane.transform.position = new Vector3(secondLaneCenter.x, Camera.main.transform.position.y, 0);
    }

    private Vector3 getVector3(float x,float y)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(x,y, 10));
    }
}
