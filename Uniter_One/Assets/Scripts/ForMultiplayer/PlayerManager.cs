using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    private Spawner spawner;
    private GameObject player;
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

    
    private void CreateController()
    {
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        Vector3 playerPosition = spawner.GetPlayerPosition();
        
        player=PhotonNetwork.Instantiate("FPCharacter", playerPosition, Quaternion.identity,0,new object[]{PV.ViewID});
    }

    public void Die()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(player);
            CreateController();
        }
    }
}
 