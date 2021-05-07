using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    private Spawner _spawner;
    private GameObject _player;
    private PhotonView _pv;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (_pv.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        _spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        Vector3 playerPosition = _spawner.GetPlayerPosition();
        _player = PhotonNetwork.Instantiate("FPCharacter", playerPosition, Quaternion.identity, 0,
            new object[] {_pv.ViewID});
    }

    public void Die()
    {
        if (_pv.IsMine)
        {
            PhotonNetwork.Destroy(_player);
            CreateController();
        }
    }
}