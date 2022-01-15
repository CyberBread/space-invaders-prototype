using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector")]
    public float waveFrequency = 2f;
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0;
    private float birthTime;

    // Start is called before the first frame update
    void Start()
    {
        x0 = pos.x;

        birthTime = Time.time;
    }

    public override void Move()
    {
        Vector3 tempPos = pos;
        float age = Time.time - birthTime;
        float theta = Mathf.PI * age * 2 / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        Vector3 rot = new Vector3(0, waveRotY * sin, 0);
        transform.rotation = Quaternion.Euler(rot);

        base.Move();
    }
}
