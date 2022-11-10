using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private bool coroutineAllowed { get; set; } = true;

    public bool CanHurt { get; private set; } = true;

    public void SetSpeed(float speed)
    {
        speed = speed < 0 ? 0 : speed;
        speedModifier= speed;
    }

    private IEnumerator WaitToHurt(int times)
    {
        SetSpeed(0);
        CanHurt = false;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var defaultColor = spriteRenderer.color;
        var transparentColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);

        for(var i = 0; i < times*2; i++)
        {
            
            spriteRenderer.color = transparentColor;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = defaultColor;
            yield return new WaitForSeconds(0.25f);
        }
        //yield return new WaitForSeconds(time);
        SetSpeed(Constants.DefaultSpiritSpeedModifier);
        CanHurt = true;
        //GetComponent<SpriteRenderer>().color = Color.white;
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
        StartCoroutine(WaitToHurt(2));
        StartCoroutine(WaitTrail(0.2f));
        
    }

    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;
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
        coroutineAllowed = true;
    }
}
