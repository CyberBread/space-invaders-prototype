using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector")]
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10f;

    [Header("Set Dinamically")]
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    void Start()
    {
        p0 = Vector3.zero;
        p0.x = -boundCheck.camWidth - boundCheck.radius;
        p0.y = Random.Range(-boundCheck.camHeight, boundCheck.camHeight);

        p1 = Vector3.zero;
        p1.x = boundCheck.camWidth + boundCheck.radius;
        p1.y = Random.Range(boundCheck.camWidth, boundCheck.camHeight); 

        if(Random.value > 0.5)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        if(u >= 1)
        {
            Destroy(gameObject);
            return;
        }
        u += sinEccentricity * Mathf.Sin(u * 2 * Mathf.PI);
        pos = (1 - u) * p0 + u * p1; 
    }
}
