using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed = 1f;
    [SerializeField]
    public bool isFalling { get; private set; } = false;
    [SerializeField]
    public bool onBottom { get; private set; } = false;
    [SerializeField]
    public bool onTop { get; private set; } = true;
    [SerializeField]
    private int height = 0;
    [SerializeField]
    private TotemType type;

    [SerializeField]
    //public TotemType? neighborType { get; private set; } = null;

    private Action totemAction;

    public bool ProtectedByEarthTotem { get; set; } = false;
    public enum TotemType
    {
        Fire,
        Earth,
        Electro,
        Ice,
        Air
    }
    void Start()
    {
        transform.localScale = Constants.totemSize;
        totemAction = GetComponent<TotemActions>().dict[type];
    }
    private void FixedUpdate()
    {
        //can be optimized
        if (isFalling)
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        //If make this in FixedUpdate, there will be some strange bugs.
        //TODO
        //Refactor this later
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("Pressed left click.");
            if (!isFalling && !onBottom)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                10));
                //transform.position = Vector3.Lerp(transform.position, new Vector3(mousePos.x, transform.position.y, 0), fallSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, transform.position.y, 0), fallSpeed * Time.deltaTime);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isFalling && !onBottom)
            {
                transform.position = Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, transform.position)).First();
                isFalling = true;
            }
        }
    }


    //private void OnMouseDrag()
    //{
    //    //if (isFalling)
    //    //{
    //    //    isDragging = true;
    //    //    var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(
    //    //    Input.mousePosition.x,
    //    //    Input.mousePosition.y,
    //    //    10));
    //    //    //transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(
    //    //    //Input.mousePosition.x,
    //    //    //Input.mousePosition.y,
    //    //    //10)) * Time.deltaTime);
    //    //    transform.position = Vector3.Lerp(transform.position, mousePos, fallSpeed * Time.deltaTime);
    //    //}
    //    if (!isFalling&&!onBottom)
    //    {
    //        var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(
    //        Input.mousePosition.x,
    //        Input.mousePosition.y,
    //        10));
    //        transform.position = Vector3.Lerp(transform.position, new Vector3(mousePos.x, transform.position.y, 0), fallSpeed * Time.deltaTime);
    //    }
    //}

    //private void OnMouseUp()
    //{
    //    if (!isFalling&&!onBottom)
    //    {
    //        transform.position = Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, transform.position)).First();
    //        isFalling = true;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spirit")
        {
            if (!collision.gameObject.GetComponent<FollowRoute>().CanHurt) return;
            if (ProtectedByEarthTotem)
            {
                Destroy(collision.gameObject);
                ProtectedByEarthTotem = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator Wait(float seconds)
    {
        //yield return new WaitForSeconds(seconds);
        yield return new WaitUntil(() =>
        {
            var spirit = Spawner.Instanse.GetCurrentSpirit();
            return spirit != null || !spirit.IsUnityNull() || !spirit.IsDestroyed();
        });

        totemAction();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "bottom")
        {
            isFalling = false;
            onBottom = true;
            Spawner.Instanse.SpawnTotem();
            StartCoroutine(Wait(0.2f));
            //totemAction();
            height = 1;
        }
        else if (collision.gameObject.tag == "Totem")
        {
            if (onBottom) return;
            var totem = collision.gameObject.GetComponent<Totem>();
            if (!totem.onBottom || totem.height == Constants.MaxTotemHeightLimit)
                Destroy(gameObject);
            else
            {
                isFalling = false;
                onBottom = true;
                height = totem.height + 1;
                if (type == TotemType.Air)
                {
                    type = totem.type;
                    totemAction = GetComponent<TotemActions>().dict[type];
                }
                //neighborType= totem.type;
                Spawner.Instanse.SpawnTotem();
                StartCoroutine(Wait(0.2f));
                //totemAction();
            }

        }
    }

    public TotemType GetTotemType()
    {
        return type;
    }
}
