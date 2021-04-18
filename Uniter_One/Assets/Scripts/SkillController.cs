using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public Image[] imageCooldowns;

    public void OuterStartCoroutine(int index, float cooldown)
    {
        StartCoroutine(StartCooldown(index, cooldown));
    }
    public IEnumerator StartCooldown(int index, float cooldown)
    {
        imageCooldowns[index].fillAmount = 0 ;
        while (imageCooldowns[index].fillAmount != 1)
        {
            imageCooldowns[index].fillAmount += 1 / cooldown * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
