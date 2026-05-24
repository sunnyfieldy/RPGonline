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

    [Header("Ambience")]
    public AudioSource ambientSource;
    public AudioClip nightAmbient;
    public AudioClip dayAmbient;

    void Start()
    {
        int index = GetSkyboxIndex(timeOfDay);

        currentIndex = index;

        Lighting(index);

        bool nowNight = (index == 0 || index == 1 || index == 7);

        if (nowNight)
        {
            ambientSource.clip = nightAmbient;
        }
        else
        {
            ambientSource.clip = dayAmbient;
        }

        ambientSource.Play();
    }
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

            ambientSource.clip = nightAmbient;
            ambientSource.Play();
        }
        else if (!nowNight && isNight)
        {
            isNight = false;
            spawner.RemoveEnemies();

            ambientSource.clip = dayAmbient;
            ambientSource.Play();
        }
    }

    void Lighting(int index)
    {
        switch (index)
        {
            // deep night
            case 0:
                RenderSettings.ambientLight = new Color(0.05f, 0.05f, 0.1f);

                RenderSettings.fogColor = new Color(0.02f, 0.02f, 0.05f);

                RenderSettings.fogDensity = 0.04f;
                break;

            // early morning
            case 1:
                RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.3f);

                RenderSettings.fogColor = new Color(0.3f, 0.3f, 0.4f);

                RenderSettings.fogDensity = 0.025f;
                break;

            // sunrise
            case 2:
                RenderSettings.ambientLight = new Color(0.5f, 0.4f, 0.3f);

                RenderSettings.fogColor = new Color(0.7f, 0.5f, 0.4f);

                RenderSettings.fogDensity = 0.015f;
                break;

            // day
            case 3:
            case 4:
            case 5:
                RenderSettings.ambientLight = Color.white * 0.8f;

                RenderSettings.fogColor = new Color(0.8f, 0.7f, 0.6f);

                RenderSettings.fogDensity = 0.008f;
                break;

            // sunset
            case 6:
                RenderSettings.ambientLight = new Color(0.4f, 0.3f, 0.3f);

                RenderSettings.fogColor = new Color(0.5f, 0.3f, 0.3f);

                RenderSettings.fogDensity = 0.02f;
                break;

            // night
            case 7:
                RenderSettings.ambientLight = new Color(0.1f, 0.1f, 0.2f);

                RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.1f);

                RenderSettings.fogDensity = 0.03f;
                break;
        }
    }
}
