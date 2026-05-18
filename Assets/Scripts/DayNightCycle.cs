using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public float dayLength = 120f; // 2 min turetu but

    [Range(0, 24)]
    public float timeOfDay = 8f;

    public EnemySpawner spawner;

    [Header("Skyboxes(turetu but in order)")]
    public Material[] skyboxes = new Material[8];

    private int currentIndex = -1;
    private bool isNight = false;

    void Update()
    {
        timeOfDay += Time.deltaTime * (24f / dayLength);

        if (timeOfDay >= 24f)
            timeOfDay = 0f;

        float sunRotation = (timeOfDay / 24f) * 360f;
        sun.transform.rotation = Quaternion.Euler(sunRotation - 90f, 170f, 0);

        UpdateSkybox();
    }
    void UpdateSkybox()
    {
        int index = GetSkyboxIndex(timeOfDay);

        if (index != currentIndex)
        {
            currentIndex = index;
            RenderSettings.skybox = skyboxes[index];
            NightLogic(index);
            Lighting(index);
            DynamicGI.UpdateEnvironment();
        }
    }

    int GetSkyboxIndex(float time)
    {
        if (time < 3) return 0;
        if (time < 6) return 1;
        if (time < 9) return 2;
        if (time < 12) return 3;
        if (time < 15) return 4;
        if (time < 18) return 5;
        if (time < 21) return 6;
        return 7;
    }

    void NightLogic(int index)
    {
        bool nowNight = (index == 0 || index == 1 || index == 7);

        if (nowNight && !isNight)
        {
            isNight = true;
            spawner.SpawnEnemies();
        }
        else if (!nowNight && isNight)
        {
            isNight = false;
            spawner.RemoveEnemies();
        }
    }

    void Lighting(int index)
    {
        bool isDay = (index >= 3 && index <= 5);

        if (isDay)
        {
            RenderSettings.ambientLight = Color.white * 0.8f;
        }
        else
        {
            RenderSettings.ambientLight = Color.blue * 0.2f;
        }
    }
}
