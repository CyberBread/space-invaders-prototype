using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static private Dictionary<WeaponType, WeaponDefenition> WEAPON_DICT;

    [Header("Set in Inspector")]
    public GameObject[] enemyPrefabs;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public GameObject powerUpPrefab;
    public WeaponDefenition[] weaponDefenitions;
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };


    private BoundsCheck boundCheck;

    private void Awake()
    {
        S = this;
        Invoke("SpawnEnemy", 2f);

        WEAPON_DICT = new Dictionary<WeaponType, WeaponDefenition>();
        foreach (WeaponDefenition wD in weaponDefenitions)
        {
            WEAPON_DICT[wD.type] = wD;
        }
    }

    public void SnipDestroyed(Enemy e)
    {
        if(Random.value <= e.powerUpDropChance)
        {
            int index = Random.Range(0, powerUpFrequency.Length);
            GameObject pu = Instantiate(powerUpPrefab);
            pu.GetComponent<PowerUp>().SetType(powerUpFrequency[index]);
            pu.transform.position = e.transform.position;
        }
    }

    private void SpawnEnemy()
    {
        int randEnemy = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate<GameObject>(enemyPrefabs[randEnemy]);

        boundCheck = GetComponent<BoundsCheck>();
        if (boundCheck != null)
        {
            Vector3 pos = Vector3.zero;
            float minX = -boundCheck.camWidth + boundCheck.radius;
            float maxX = boundCheck.camWidth - boundCheck.radius;
            float posX = Random.Range(minX, maxX);

            float posY = boundCheck.camHeight + boundCheck.radius;

            pos.x = posX;
            pos.y = posY;
            enemy.transform.position = pos;
        }

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    /// <summary>
    ///Статическая функция, возвращающая WeaponDefinition из статического
    ///защищенного поля WEAP_DICT класса Main.
    ///</summary>
    ///<returns> Экземпляр WeaponDefinition или, если нет такого определения
    /// для указанного WeaponType, возвращает новый экземпляр WeaponDefinition
    /// с типом none.</returns>
    /// <param name = "wt" > Tиn оружия WeaponType, для которого требуется получить
    // WeaponDefinition</param>

    static public WeaponDefenition GetWeaponDefenition(WeaponType wT)
    {
        if (WEAPON_DICT.ContainsKey(wT))
        {
            return WEAPON_DICT[wT];
        }
        else
            return new WeaponDefenition();
    }
}
