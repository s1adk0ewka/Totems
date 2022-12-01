using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    private int height = 1;
    [SerializeField]
    private ElementalType type;

    public bool isLast=false;

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
        if (!AnyActionAllowed)
            return;
        if (Input.GetMouseButton(0))
        {
            if (!isFalling && !onBottom)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                10));
                var closestToMousePoint= Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, mousePos)).First();
                if (closestToMousePoint != transform.position)
                {
                    transform.position = Vector3.Lerp(transform.position, closestToMousePoint, fallSpeed * Time.deltaTime);
                }
                //transform.position = Vector3.Lerp(transform.position, new Vector3(mousePos.x, transform.position.y, 0), fallSpeed * Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, transform.position.y, 0), fallSpeed * Time.deltaTime);
                //transform.position = Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, mousePos)).First();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isFalling && !onBottom)
            {
                transform.position = Lanes.TopPoints.OrderBy(p => Vector3.Distance(p, transform.position)).First() + new Vector3(0, 0, -height);
                isFalling = true;
                onTop= false;
            }
        }
    }
    private void OnEnable()
    {
        AnyActionAllowed= true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spirit")
        {
            if (ProtectedByEarthTotem)
            {
                Destroy(collision.gameObject);
                ProtectedByEarthTotem = false;
            }
            else if (!collision.gameObject.GetComponent<Spirit>().CanHurt)
            {
                return;
            }
            else
            { 
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator WaitForSpiritsSpawn()
    {

        yield return new WaitUntil(() =>
        {
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
                transform.position += new Vector3(0, 0, -height);
                if (type == ElementalType.Air)
                {
                    type = totem.type;
                    totemAction = GetComponent<TotemActions>().dict[type];
                }
                Spawner.Instanse.SpawnTotem();
                GetComponent<Rigidbody2D>().gravityScale = 1;
                StartCoroutine(WaitForSpiritsSpawn());
            }
        }
    }

    public ElementalType GetTotemType()
    {
        return type;
    }
}
