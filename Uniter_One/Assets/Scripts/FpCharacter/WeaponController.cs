using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WeaponController : MonoBehaviour
{
    public int selectedWeapon = 0;

    public GrapGun graplinGun;
    private Gun currentGun;

    private int prevSelected;
    private PhotonView _photonView;
    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponentInParent<PhotonView>();
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    { 
        if (!_photonView.IsMine)
            return;
        WeaponShooting();
        WeaponSwitching();
    }

    private void WeaponShooting()
    {
        
        if (Input.GetButtonDown("Fire2"))
        {
            graplinGun.Shoot();
        }
        else if (Input.GetButton("Fire2")&&(Input.GetKey(KeyCode.LeftShift)))
        {
            Debug.Log("rofl");
            graplinGun.Climbing();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            graplinGun.StopGrap();
        }
        if (!currentGun) return;
        if (Input.GetButton("Fire1"))
        {
            currentGun.Shoot();
        }
    }

    private void WeaponSwitching()
    {
        prevSelected = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (prevSelected != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            if (i == selectedWeapon)
                currentGun = weapon.GetComponent<Gun>();
            i++;
        }
    }
}