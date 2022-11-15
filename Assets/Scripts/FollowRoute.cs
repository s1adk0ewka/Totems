using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusAttribute : Attribute
{
    public StatusAttribute(Status statusEffect)
    {
        StatusEffect = statusEffect;
    }

    public Status StatusEffect { get; }

}

public enum Status
{
    OK,
    Stuned,
    Slowed
}

public class FollowRoute : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> combinedRoutes;
    private Transform[] routes;
    private int routeToGo { get; set; } = 0;
    private float tParam { get; set; } = 0f;
    private Vector2 objectPosition { get; set; }
    [SerializeField]
    private float speedModifier { get; set; } = 0.5f;
    private bool ActionCoroutineAllowed { get; set; } = true;

    public bool CanHurt { get; private set; } = true;

    private IEnumerator currentStatusCoroutine;


    public Status currentStatus { get; private set; }
    public void SetSpeed(float speed)
    {
        speed = speed < 0 ? 0 : speed;
        speedModifier= speed;
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
        currentStatus = Status.Stuned;
        if (stunColor == default(Color))
        {
            stunColor = Color.gray;
        }
        SetSpeed(0);
        CanHurt= false;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = stunColor;
        yield return new WaitForSeconds(time);
        currentStatus = Status.OK;
        SetSpeed(Constants.DefaultSpiritSpeedModifier);
        CanHurt= true;
        spriteRenderer.color = Constants.DefaultSpiritColor;
    }

    public IEnumerator SlowCoroutine(float time,float slowCoef, Color slowColor = default(Color))
    {
        currentStatus = Status.Slowed;
        CanHurt = true;
        //to prevent CS1736
        if (slowColor == default(Color))
        {
            slowColor = Color.blue;
        }
        SetSpeed(Constants.DefaultSpiritSpeedModifier*slowCoef);
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = slowColor;
        yield return new WaitForSeconds(time);
        currentStatus = Status.OK;
        SetSpeed(Constants.DefaultSpiritSpeedModifier);
        spriteRenderer.color = Constants.DefaultSpiritColor;
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
        var randomRoute = Instantiate(combinedRoutes[rnd.Next(0,combinedRoutes.Count)], transform.position, Quaternion.identity);
        routes = randomRoute.GetComponentsInChildren<Route>().Select(x=>x.gameObject.transform).ToArray();
        Stun(2f,Color.gray);
        StartCoroutine(WaitTrail(0.2f));
        
    }

    void Update()
    {
        if (ActionCoroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        ActionCoroutineAllowed = false;
        Vector2 p0 = routes[routeNum].GetChild(0).position;
        Vector2 p1 = routes[routeNum].GetChild(1).position;
        Vector2 p2 = routes[routeNum].GetChild(2).position;
        Vector2 p3 = routes[routeNum].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }
        tParam = 0f;
        routeToGo += 1;
        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }
        ActionCoroutineAllowed = true;
    }
}
