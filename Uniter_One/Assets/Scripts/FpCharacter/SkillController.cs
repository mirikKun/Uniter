using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public Image[] imageCooldowns;
    public Image[] imageCountdowns;

    public void OuterStartCoroutineCooldown(int index, float cooldown)
    {
        StartCoroutine(StartCooldown(index, cooldown));
    }
    public void OuterStartCoroutineCountdown(int index, float countdown)
    {
        StartCoroutine(StartCountdown(index, countdown));
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
    public IEnumerator StartCountdown(int index, float cooldown)
    {
        imageCountdowns[index].fillAmount = 1 ;
        while (imageCountdowns[index].fillAmount != 0)
        {
            imageCountdowns[index].fillAmount -= 1 / cooldown * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
