using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;

    [Header("Set Dinamically")]
    public int levelShown = 0;

    private Material matShield;

    private void Start()
    {
        matShield = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        int currentLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        if(levelShown != currentLevel)
        {
            levelShown = currentLevel;
            matShield.mainTextureOffset = new Vector2(levelShown * 0.2f, 0);
        }

        float rZ = -(rotationPerSecond * Time.time * 360) % 360;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
