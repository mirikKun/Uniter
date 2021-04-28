using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController(); 
        }
    }

    void CreateController()
    {
        Debug.Log("InstantiatePlayer");
        //Instantiating
    }
}
