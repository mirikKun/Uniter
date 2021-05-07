using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGun : MonoBehaviour
{
    [SerializeField] private Transform weaponSwitcher;
    [SerializeField] private Transform fpCamera;
    [SerializeField] private Transform player;
    [SerializeField] private SkillController sc;
    [SerializeField] private float timeWithGun = 10;

    public void AddingGun(GameObject gun)
    {
        gun.transform.SetParent(weaponSwitcher);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localEulerAngles = Vector3.zero;
        gun.GetComponentInChildren<GravitySwitchGun>().SetPlayer(player, fpCamera);
        gun.SetActive(false);
        sc.OuterStartCoroutineCountdown(0, timeWithGun);
        StartCoroutine(gun.GetComponentInChildren<GravitySwitchGun>().Disappear(timeWithGun));
    }
}