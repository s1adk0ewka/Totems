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
    private float speedModifier { get; set; } = 0.5f;
    private bool coroutineAllowed { get; set; } = true;

    void Start()
    {
        var rnd = new System.Random();
        var randomRoute = Instantiate(combinedRoutes[rnd.Next(0,combinedRoutes.Count)], transform.position, Quaternion.identity);
        routes = randomRoute.GetComponentsInChildren<Route>().Select(x=>x.gameObject.transform).ToArray();
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
