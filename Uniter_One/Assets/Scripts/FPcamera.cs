using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPcamera : MonoBehaviour
{
    public float mouseSensivity = 100f;
    public Transform player;
    float yRotation = 0f;
    public int ivertion = -1;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")*mouseSensivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        yRotation += ivertion * mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

    }
    }
