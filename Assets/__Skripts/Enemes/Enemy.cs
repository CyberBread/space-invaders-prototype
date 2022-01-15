using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float    health = 10f;
    public float    speed = 10f;
    public float    fireRate = 0.1f;
    public float    showDamageDuration = 0.1f; 
    public int      score = 100;
    public float    powerUpDropChance = 0.1f;

    protected Color[] originalColors;
    protected Material[] materials;
    protected float damageDoneTime;
    protected bool showingDamage = false;
    protected bool notifiedOfDestruction = false;

    protected BoundsCheck boundCheck;
    

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void Awake()
    {
        boundCheck = GetComponent<BoundsCheck>();

        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    private void Update()
    {
        Move();

        if(showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        
        if(boundCheck != null && boundCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collObject = collision.gameObject;
        if(collObject.tag == "ProjectileHero")
        {
            Projectile projectile = collObject.GetComponent<Projectile>();
            if (!boundCheck.isOnScreen)
            {
                Destroy(collObject);
            }

            health -= Main.GetWeaponDefenition(projectile.type).damageOnHit;
            Destroy(collObject);
            ShowDamage();

            if(health <= 0)
            {
                if (!notifiedOfDestruction) 
                {
                    Main.S.SnipDestroyed(this);
                }
                notifiedOfDestruction = true;
                Destroy(gameObject);
            }
        }
    }

    public void ShowDamage()
    {
        showingDamage = true;
        foreach(Material mat in materials)
        {
            mat.color = Color.red;
        }
        damageDoneTime = Time.time + showDamageDuration;
    }

    public void UnShowDamage()
    {
        showingDamage = false;
        for(int i = 0; i < originalColors.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
