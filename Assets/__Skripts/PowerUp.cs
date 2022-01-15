using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header ("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;
    public WeaponType type;

    private GameObject cube;
    private TextMesh letter;
    private Vector3 rotPerSeconds;
    private float birthTime;

    private Rigidbody rigidbody;
    private BoundsCheck boundsCheck;
    private Renderer cubeRend;

    private void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rigidbody = GetComponent<Rigidbody>();
        boundsCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigidbody.velocity = vel;

        transform.rotation = Quaternion.identity;
        rotPerSeconds = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
    }

    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSeconds * Time.time);

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if(u>= 1)
        {
            Destroy(gameObject);
            return;
        }
        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - 0.5f * u;
            letter.color = c;
        }

        if (!boundsCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType wt)
    {
        WeaponDefenition def = Main.GetWeaponDefenition(wt);
        cubeRend.material.color = def.weaponColor;
        letter.text = def.letterOnBonusCube;
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(gameObject);
    }
}
