using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : Singleton<SkyboxChange>
{
    public List<Material> skyMaterials;

    public void ChangeSkyBox(int n)
    {
            //int n = UnityEngine.Random.Range(0, skyMaterials.Count);
           RenderSettings.skybox = skyMaterials[n];
    }
    public void ChangeSkyBoxtoDefalt()
    {
        
    }
}
