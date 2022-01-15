using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector")]
    public float lifeTime = 5f;

    private float easing = 0.1f;
    private float minOffsetY = 2f;
    private float maxOffsetY = 2.7f;

    private Vector3[] points;
    private float birthTime;

    private void Start()
    {
        points = new Vector3[3];
        points[0] = pos;

        float minX = boundCheck.camWidth - boundCheck.radius;
        float maxX = -boundCheck.camWidth + boundCheck.radius;
        float minY = -boundCheck.camHeight * Random.Range(minOffsetY, maxOffsetY);
        float maxY = boundCheck.camHeight * Random.Range(minOffsetY, maxOffsetY);

        Vector3 point = Vector3.zero;
        point.x = Random.Range(minX, maxX);
        point.y = minY;
        points[1] = point;

        point = Vector3.zero;
        point.x = Random.Range(minX, maxX);
        point.y = maxY;
        points[2] = point;

        birthTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime)/lifeTime;
        if (u >= 1)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 p0, p1;
        u -= easing * Mathf.Sin(Mathf.PI * 2 * u);
        p0 = (1 - u) * points[0] + u * points[1];
        p1 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p0 + u * p1;
    }
}
