using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part
{
    public string name;
    public float health;
    public string[] protectBy;

    protected internal GameObject go;
    protected internal Material mat;
}

public class Enemy_4 : Enemy
{
    [Header("Set in Inspector")]
    public Part[] parts;

    private Vector3 p0, p1;
    private float startTime;
    private float duration = 4f;

    private void Start()
    {
        p0 = p1 = pos;
        InitMovment();

        Transform t;
        foreach(Part part in parts)
        {
            t = transform.Find(part.name);
            if(t != null)
            {
                part.go = t.gameObject;
                part.mat = t.GetComponent<Renderer>().material;
            }
        }
    }

    private void InitMovment()
    {
        p0 = p1;
        float widthMinRad = boundCheck.camWidth - boundCheck.radius;
        float heightMinRad = boundCheck.camHeight - boundCheck.radius;
        p1.x = Random.Range(-widthMinRad, widthMinRad);
        p1.y = Random.Range(-heightMinRad, heightMinRad);

        startTime = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - startTime) / duration;

        if(u > 1)
        {
            InitMovment();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    private Part FindPart(string n)
    {
        foreach(Part part in parts)
            if (part.name == n)
                return part;
        return null;
    }
    private Part FindPart(GameObject go)
    {
        foreach (Part part in parts)
            if (part.go == go)
                return part;
        return null;
    }

    private bool Destroyed(string n)
    {
        return FindPart(n) != null ? false : true;
    }
    private bool Destroyed(GameObject go)
    {
        return FindPart(go) != null ? false : true;
    }
    private bool Destroyed(Part part)
    {
        if (part == null)
            return (true);
        return (part.health <= 0);
    }

    private void ShowLocalizeDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if(other.tag == "ProjectileHero")
        {
            if (!boundCheck.isOnScreen)
            {
                Destroy(other);
                return;
            }

            GameObject goHit = collision.contacts[0].thisCollider.gameObject;
            Part partHit = FindPart(goHit);
            if(partHit == null)
            {
                goHit = collision.contacts[0].otherCollider.gameObject;
                partHit = FindPart(goHit);
            }

            if(partHit.protectBy != null)
            {
                foreach(string s in partHit.protectBy)
                {
                    if (!Destroyed(s))
                    {
                        Destroy(other);
                        return;
                    }
                }
            }

            partHit.health  -= Main.GetWeaponDefenition(other.GetComponent<Projectile>().type).damageOnHit;
            ShowLocalizeDamage(partHit.mat);
            if (partHit.health <= 0)
                partHit.go.SetActive(false);

            bool allDestroyed = true;
            foreach(Part part in parts)
            {
                if (!Destroyed(part))
                {
                    allDestroyed = false;
                    break;
                }
            }

            if (allDestroyed)
            {
                Main.S.SnipDestroyed(this);
                Destroy(gameObject);
            }
            Destroy(other);
        }
    }
}
