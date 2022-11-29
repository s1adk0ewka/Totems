using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public partial class Totem : MonoBehaviour
{
    public static bool AnyActionAllowed = true;
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
    private ElementalType type;

    public bool isLast=false;

    [SerializeField]
    //public TotemType? neighborType { get; private set; } = null;

    private Action totemAction;


    public bool ProtectedByEarthTotem { get; set; } = false;
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
        //Debug.Log(AnyActionAllowed);
        if (!AnyActionAllowed)
            return;
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
                //transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, transform.position.y, 0), fallSpeed * Time.deltaTime);
                transform.position = Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, mousePos)).First();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isFalling && !onBottom)
            {
                transform.position = Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, transform.position)).First();
                isFalling = true;
                onTop= false;
            }
        }
    }
    private void OnEnable()
    {
        AnyActionAllowed= true;
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
            if (!collision.gameObject.GetComponent<Spirit>().CanHurt) return;
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

    public IEnumerator WaitForSpiritsSpawn()
    {
        //yield return new WaitForSeconds(seconds);
        yield return new WaitUntil(() =>
        {
            //var spirit = Spawner.Instanse.GetCurrentSpirits();
            //return spirit != null || !spirit.IsUnityNull() || !spirit.IsDestroyed();
            return Spawner.Instanse.GetCurrentSpirits().Count>0;
        });

        totemAction();
        if (isLast)
        {
            yield return new WaitForSecondsRealtime(1f);
            GameInfo.Instanse.ShowGameLost();
        }
    }

    public void OnDestroy()
    {
        if (isLast) GameInfo.Instanse.ShowGameLost();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "bottom")
        {
            isFalling = false;
            onBottom = true;
            Spawner.Instanse.SpawnTotem();
            GetComponent<Rigidbody2D>().gravityScale = 1;
            StartCoroutine(WaitForSpiritsSpawn());
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
                if (type == ElementalType.Air)
                {
                    type = totem.type;
                    totemAction = GetComponent<TotemActions>().dict[type];
                }
                //neighborType= totem.type;
                Spawner.Instanse.SpawnTotem();
                GetComponent<Rigidbody2D>().gravityScale = 1;
                StartCoroutine(WaitForSpiritsSpawn());
                //totemAction();
            }

        }
    }

    public ElementalType GetTotemType()
    {
        return type;
    }
}
