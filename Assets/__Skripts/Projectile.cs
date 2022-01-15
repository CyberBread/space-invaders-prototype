using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rigid;

    private BoundsCheck boundCheck;
    private Renderer rend;

    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    private void Awake()
    {
        boundCheck = GetComponent<BoundsCheck>();
        rigid = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (boundCheck.offUp)
        {
            Destroy(gameObject);
        }       
    }

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefenition def = Main.GetWeaponDefenition(_type);
        rend.material.color = def.projectileColor;
    }
}
