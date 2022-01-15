using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,       //Нет оружия
    blaster,    //Простой бластер
    spread,     //Веерная пушка, стреляет несколькими снарядами
    phaser,     //Волновой фазер
    missile,    //Самонаводящиеся ракеты
    laser,      //Лазер, медленно сносит HP
    shield      //Уровень щита
}

[System.Serializable]
public class WeaponDefenition
{
    public WeaponType       type = WeaponType.none;
    public string           letterOnBonusCube;                             
    public Color            weaponColor = Color.white;
    public GameObject       projectilePrefab;
    public Color            projectileColor = Color.red;
    public float            damageOnHit = 0f;
    public float            continiusDamage = 0f;

    public float            delayBetweenShots = 0f;
    public float            projectileVelociti = 20f;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dinamically")]
    public WeaponDefenition def;
    public GameObject collar;
    public float lastShotTime;
    public Renderer collarRend;

    public WeaponType _type = WeaponType.none;

    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type);

        if(PROJECTILE_ANCHOR == null)
        {
            GameObject anchorGO = new GameObject("ProjectileAnchor");
            PROJECTILE_ANCHOR = anchorGO.transform;
        }

        Hero rootGO = transform.root.gameObject.GetComponent<Hero>();
        if(rootGO != null)
        {
            rootGO.fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(WeaponType wType)
    {
        if (wType == WeaponType.none)
        {
            gameObject.SetActive(false);
            _type = WeaponType.none;
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefenition(wType);
        _type = def.type;

        collarRend.material.color = def.weaponColor;
        lastShotTime = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if(Time.time - lastShotTime <= def.delayBetweenShots)
        {
            return;
        }

        Projectile proj;
        Vector3 vel = Vector3.up * def.projectileVelociti;
        if(transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (_type)
        {
            case WeaponType.blaster:
                proj = MakeProjectile();
                proj.rigid.velocity = vel;
                break;
            case WeaponType.spread:
                proj = MakeProjectile();
                proj.rigid.velocity = vel;
                proj = MakeProjectile();
                proj.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                proj.rigid.velocity = proj.transform.rotation * vel;
                proj = MakeProjectile();
                proj.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                proj.rigid.velocity = proj.transform.rotation * vel;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if(transform.parent.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR,true);

        Projectile projectile = go.GetComponent<Projectile>();
        projectile.type = type;

        lastShotTime = Time.time;

        return projectile;
    }

}
