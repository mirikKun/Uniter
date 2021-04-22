using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGun : MonoBehaviour
{
    
    public Transform weaponSwitcher;
    public Transform fpCamera;
    public Transform player;
    public SkillController sc;
    public float timeWithGun = 10;

    public void AddingGun(GameObject gun)
    {
        gun.transform.SetParent(weaponSwitcher);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localEulerAngles=Vector3.zero;
        gun.GetComponentInChildren<GravitySwitchGun>().SetPlayer(player,fpCamera);
        gun.SetActive(false);
        sc.OuterStartCoroutineCountdown(0,timeWithGun);
        
        StartCoroutine(gun.GetComponentInChildren<GravitySwitchGun>().Disappear(timeWithGun));
        
    }
}
