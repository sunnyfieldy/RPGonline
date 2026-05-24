using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    public Light torchLight;

    [Header("torch settings")]
    public float minIntensity = 7.5f;
    public float maxIntensity = 8.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            torchLight.enabled = !torchLight.enabled;
        }

        if (torchLight.enabled)
        {
            torchLight.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
