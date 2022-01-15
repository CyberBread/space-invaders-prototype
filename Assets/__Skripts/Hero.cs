using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    public float        speed = 30;
    public float        rollMult = -45;
    public float        pitchMult = 30;
    public float        gameRestartDelay = 2f;
    public float        projectileSpeed = 10f;
    public Projectile   projectilePrefab;
    public Weapon[]     weapons;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    [Header("Set Dinamically")]
    private float        _shieldLevel = 1;

    private GameObject lastTriggerGo = null;

    public float shieldLevel
    {
        get { return _shieldLevel; }
        set
        {
            _shieldLevel = Mathf.Min(4, value);
            if(value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Second initialization Hero.S!");
        }

        ClearWeapons();
        weapons[0].type = WeaponType.blaster;
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 tPos = transform.position;
        tPos.x += xAxis * Time.deltaTime * speed;
        tPos.y += yAxis * Time.deltaTime * speed;
        transform.position = tPos;

        transform.rotation = Quaternion.Euler(-yAxis * rollMult, -xAxis * pitchMult, 0);

        if (Input.GetAxis("Jump") > 0)
        {
            fireDelegate?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.transform.root;
        GameObject rootGo = rootT.gameObject;

        if(lastTriggerGo == rootGo)
        {
            return;
        }

        lastTriggerGo = rootGo;
        
        if(lastTriggerGo.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(lastTriggerGo);
        }
        if(lastTriggerGo.tag == "PowerUp")
        {
            AbsorbPowerUp(lastTriggerGo);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            default:
                if(pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if(w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(gameObject);
    }

    public Weapon GetEmptyWeaponSlot()
    {
        for(int i =0; i < weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none)
            {
                return weapons[i];
            }
        }
        return (null);
    }

    public void ClearWeapons()
    {
        foreach(Weapon weapon in weapons)
        {
            weapon.SetType(WeaponType.none);
        }
    }

}