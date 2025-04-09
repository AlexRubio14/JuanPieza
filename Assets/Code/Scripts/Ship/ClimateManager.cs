using System.Collections.Generic;
using UnityEngine;

public class ClimateManager : MonoBehaviour
{
    //Climate General
    [SerializeField] private List<Material> skyboxs;

    //Storm
    [SerializeField] private GameObject rain;
    [SerializeField] private float sunIntensity;

    private void Start()
    {
        rain.SetActive(false);

        switch (NodeManager.instance.questData.questClimete)
        {
            case QuestData.QuestClimete.CLEAR:
                RenderSettings.skybox = skyboxs[0];
                break;
            case QuestData.QuestClimete.SNOW:
                RenderSettings.skybox = skyboxs[1];
                break;
            case QuestData.QuestClimete.STORM:
                RenderSettings.skybox = skyboxs[2];
                PreparedStorm();
                break;
        }
    }

    private void PreparedStorm()
    {
        rain.SetActive(true);
        RenderSettings.sun.intensity = sunIntensity;
        RenderSettings.sun.color = Color.blue;
    }
}
