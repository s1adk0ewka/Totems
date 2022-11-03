using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBorders : MonoBehaviour
{
    [SerializeField]
    void Awake()
    {
        CreateBorders(GetBorderPoints());
    }

    private Vector2[] GetBorderPoints()
    {
        var topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 10));
        var bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));
        var bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 10));
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 10));
        return new[] { topLeft, bottomLeft, bottomRight, topRight }.Select(vector => (Vector2)vector).ToArray();
    }

    private void CreateBorders(params Vector2[] points)
    {
        //Bad way to do it
        if (points.Count() > 4) Debug.Log("Wrong parametrs for CreateBorders method");
        var leftBorder = new GameObject("leftBorder", typeof(EdgeCollider2D));
        leftBorder.transform.parent = transform;
        leftBorder.GetComponent<EdgeCollider2D>().points = new[] { points[0], points[1] };
        leftBorder.tag = "Border";
        var rightBorder = new GameObject("rightBorder", typeof(EdgeCollider2D));
        rightBorder.transform.parent = transform;
        rightBorder.GetComponent<EdgeCollider2D>().points = new[] { points[2], points[3] };
        rightBorder.tag = "Border";
        var bottom = new GameObject("bottom", typeof(EdgeCollider2D));
        bottom.transform.parent = transform;
        bottom.GetComponent<EdgeCollider2D>().points = new[] { points[1], points[2] };
        //bottom.tag = "Bottom";
    }

    // Update is called once per frame

}
