using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum Status
{
    OK,
    Stuned,
    Slowed
}

public class Spirit : MonoBehaviour
{
    [SerializeField]
    private GameObject Route;
    private RouteInfo routeInfo;
    private GameObject[] partsOfRoute;
    [SerializeField]
    private ElementalType type;
    private int routeToGo { get; set; } = 0;
    private float tParam { get; set; } = 0f;
    private Vector2 objectPosition { get; set; }
    [SerializeField]
    private float speedModifier { get; set; } = 0.5f;
    [SerializeField]
    private Color defaultColor; 
    private bool ActionCoroutineAllowed { get; set; } = true;

    public bool CanHurt { get; private set; } = true;

    private IEnumerator currentStatusCoroutine;


    public Status currentStatus { get; private set; }
    public void SetSpeed(float speed)
    {
        speed = speed < 0 ? 0 : speed;
        speedModifier= speed;
    }

    public void SetRoute(GameObject route)
    {
        Route= route;
    }

    public void ChangeTypeFromAir(ElementalType type)
    {
        if (this.type == ElementalType.Air)
        {
            this.type = type;
        }
    }

    public ElementalType GetElementalType()
    {
        return type;
    }

    public void Stun(float time,Color color)
    {
        //DRY, refactor later
        if(currentStatusCoroutine!=null)
            StopCoroutine(currentStatusCoroutine);
        currentStatusCoroutine= StunCoroutine(time,color);
        StartCoroutine(currentStatusCoroutine);
    }

    public void Slow(float time, float slowCoefficient)
    {
        //DRY, refactor later
        if (currentStatusCoroutine != null)
            StopCoroutine(currentStatusCoroutine);
        currentStatusCoroutine = SlowCoroutine(time,slowCoefficient);
        StartCoroutine(currentStatusCoroutine);
    }
    public IEnumerator StunCoroutine(float time,Color stunColor = default(Color))
    {
        //to prevent CS1736
        var spriteRenderer = GetComponent<SpriteRenderer>();
        //var defaultColor = spriteRenderer.color;
        currentStatus = Status.Stuned;
        if (stunColor == default(Color))
        {
            stunColor = Color.gray;
        }
        SetSpeed(0);
        CanHurt= false;
        //var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = stunColor;
        yield return new WaitForSeconds(time);
        currentStatus = Status.OK;
        SetSpeed(Constants.DefaultSpiritSpeedModifier);
        CanHurt= true;
        spriteRenderer.color = defaultColor;
    }

    public IEnumerator SlowCoroutine(float time,float slowCoef, Color slowColor = default(Color))
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        //var defaultColor = spriteRenderer.color;
        currentStatus = Status.Slowed;
        CanHurt = true;
        //to prevent CS1736
        if (slowColor == default(Color))
        {
            slowColor = Color.blue;
        }
        SetSpeed(Constants.DefaultSpiritSpeedModifier*slowCoef);
        //var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = slowColor;
        yield return new WaitForSeconds(time);
        currentStatus = Status.OK;
        SetSpeed(Constants.DefaultSpiritSpeedModifier);
        spriteRenderer.color = defaultColor;
    }


    private IEnumerator WaitTrail(float time)
    {
        var trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        yield return new WaitForSeconds(time);
        trail.enabled = true;
    }

    void Start()
    {
        var rnd = new System.Random();
        //var rnd = new System.Random();
        //var randomRoute = Instantiate(Route[rnd.Next(0,Route.Count)], transform.position, Quaternion.identity);
        partsOfRoute = Route.GetComponentsInChildren<Route>().Select(x=>x.gameObject).ToArray();
        //Stun(2f,Color.gray);
        //routeToGo = rnd.Next(partsOfRoute.Length);
        //THIS IS BAD WAY TO REVERSE ROUTES, BUT I DONT CARE ANYMORE
        routeInfo = Route.GetComponent<RouteInfo>();
        routeToGo = routeInfo?.startRoute ?? 0;
        //routeToGo= routeInfo?.isReversed??false? partsOfRoute.Length - 1 : 0;
        //followMode= routeInfo?.isReversed ?? false ? GoByTheReverseRoute:GoByTheRoute;
        StartCoroutine(WaitTrail(0.2f));
        
    }

    void Update()
    {
        if (ActionCoroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private void OnDestroy()
    {
        Spawner.Instanse.GetCurrentSpirits().Remove(gameObject);
        //Destroy(partsOfRoute[0].transform.parent.gameObject);
    }


    //REFACTOR THIS ABOMINATION LATER
    private IEnumerator GoByTheRoute(int routeNum)
    {
        routeNum = routeNum > partsOfRoute.Length - 1 ? partsOfRoute.Length - 1 : routeNum;
        var offset = routeInfo?.offset ?? new Vector2(0, 0);
        ActionCoroutineAllowed = false;
        
        Vector2 p0 = partsOfRoute[routeNum].transform.GetChild(0).position;
        Vector2 p1 = partsOfRoute[routeNum].transform.GetChild(1).position;
        Vector2 p2 = partsOfRoute[routeNum].transform.GetChild(2).position;
        Vector2 p3 = partsOfRoute[routeNum].transform.GetChild(3).position;
        var points = new[] { p0, p1, p2, p3 }.Select(point=>point+offset).ToList();
        //points = routeInfo?.isReversed ?? false ? points.Reverse() : points;
        if(routeInfo?.isReversed ?? false)
        {
            points.Reverse();
        }


        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            objectPosition = Mathf.Pow(1 - tParam, 3) * points[0] + 3 * Mathf.Pow(1 - tParam, 2) * tParam * points[1] + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * points[2] + Mathf.Pow(tParam, 3) * points[3];
            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }
        tParam = 0f;
        routeToGo += 1;
        if (routeToGo > partsOfRoute.Length - 1)
        {
            routeToGo = 0;
        }
        ActionCoroutineAllowed = true;
    }

}
