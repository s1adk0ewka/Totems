using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed = 1f;
    [SerializeField]
    private bool isFalling=false;
    [SerializeField]
    private bool onBottom=false;
    [SerializeField]
    private int height = 0;
    void Awake()
    {
        transform.localScale = Constants.totemSize;
    }
    private void FixedUpdate()
    {
        if (isFalling)
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseDrag()
    {
        //if (isFalling)
        //{
        //    isDragging = true;
        //    var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(
        //    Input.mousePosition.x,
        //    Input.mousePosition.y,
        //    10));
        //    //transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(
        //    //Input.mousePosition.x,
        //    //Input.mousePosition.y,
        //    //10)) * Time.deltaTime);
        //    transform.position = Vector3.Lerp(transform.position, mousePos, fallSpeed * Time.deltaTime);
        //}
        if (!isFalling&&!onBottom)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            10));
            transform.position = Vector3.Lerp(transform.position, new Vector3(mousePos.x, transform.position.y, 0), fallSpeed * Time.deltaTime);
        }
    }

    private void OnMouseUp()
    {
        if (!isFalling&&!onBottom)
        {
            transform.position = Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, transform.position)).First();
            isFalling = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        if (!onBottom&&(collision.gameObject.name == "bottom" || collision.gameObject.tag=="Totem"))
        {
            isFalling = false;
            onBottom = true;
            if(collision.gameObject.tag == "Totem")
            {
                var totem = collision.gameObject.GetComponent<Totem>();
                if (totem.height == Constants.MaxTotemHeightLimit)
                    Destroy(gameObject);
                else
                    height = totem.height + 1;
            }
            else
            {
                height += 1;
            }
            Spawner.Instanse.SpawnTotem();
        }
        else if (collision.gameObject.tag == "Spirit")
        {
            Destroy(gameObject);
            Spawner.Instanse.SpawnTotem();
        }
    }
}
