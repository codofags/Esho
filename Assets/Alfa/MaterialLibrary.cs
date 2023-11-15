using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLibrary : MonoBehaviour
{
    public static MaterialLibrary ins;

    public Material wallMaterial;
    public Material roofMaterial;
    public Material regionMaterial;
    public Material waterMaterial;
    public Material segmentMaterial;
    public Material intersectionMaterial;

    public void Setup()
    {
        ins = this;
    }
}
