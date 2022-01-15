using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();
        foreach(Renderer rend in renderers)
        {
            materials.Add(rend.material);
        }
        return materials.ToArray();
    }
}
