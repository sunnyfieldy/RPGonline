using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Image damageImage;

    public void ShowDamage()
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        Color color = damageImage.color;

        color.a = 0.4f;
        damageImage.color = color;

        yield return new WaitForSeconds(0.15f);

        color.a = 0f;
        damageImage.color = color;
    }
}
