using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FpController : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform groundCheck, roofCheck, mainCamera;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private LayerMask groundMasc;
    [SerializeField] private float groundDistance = 0.5f;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float blinkNear = 4;
    [SerializeField] private float rangeBlink = 30;
    [SerializeField] private float cooldownBlink = 2;

    [SerializeField] private float maxHeals = 100;
    [SerializeField] private SkillController sc;
    [SerializeField] private GameObject ui;
    [SerializeField] private Image healthBar;

    [SerializeField] private float mouseSensivity = 100f;
    [SerializeField] private int ivertion = -1;

    private Vector3 _gravityDirection = new Vector3(0, -1, 0);
    private Vector3 _gravityVelocity;
    private bool _isGrounded;
    private bool _isUnderRoof;
    private float _gravitySwitchDistance = 10;
    private CharacterController _controller;
    private Rigidbody _rb;
    private float _currentHeals;
    private float _yRotation = 0f;

    private Vector3 _move;
    private bool _isGrapMod;
    private float _fireTime = 0f;

    private PhotonView PV;
    private PlayerManager _playerManager;

    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        _controller = GetComponent<CharacterController>();
        _playerManager = PhotonView.Find((int) PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        _currentHeals = maxHeals;
        if (PV.IsMine)
            Destroy(trail);
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(ui);
            Destroy(_rb);
            Destroy(_controller);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!PV.IsMine)
            return;
        GetInput();
        CameraRotate();
        if (_isGrapMod)
            return;
        Move();
        Gravity();
        Jump();
        PlaceToBlink();
    }

    private void FixedUpdate()
    {
        if (!_isGrapMod)
            return;
        _rb.AddForce(gravity * 0.5f * _gravityDirection, ForceMode.Acceleration);
        if (Vector3.Magnitude(_rb.velocity) < 20)
            _rb.AddForce(speed * 0.5f * _move, ForceMode.Impulse);
    }

    public void GrapModeOn()
    {
        _isGrapMod = true;
        _controller.enabled = false;
        _rb.isKinematic = false;
        _rb.velocity = _controller.velocity;
    }

    public void GrapModeOff()
    {
        _isGrapMod = false;
        Vector3 vel = _rb.velocity;
        _rb.isKinematic = true;
        _controller.enabled = true;
        _gravityVelocity = Vector3.zero;
        StartCoroutine(RushDeceleration(vel));
    }

    private IEnumerator RushDeceleration(Vector3 impulse)
    {
        while (impulse.magnitude > 1 || _isGrounded)
        {
            impulse *= 0.97f;
            _controller.Move(Time.deltaTime * 1.6f * impulse);
            yield return null;
        }
    }

    private void Move()
    {
        _controller.Move(speed * Time.deltaTime * _move);
    }

    private void GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        _move = transform.right * x + transform.forward * z;
    }

    private void CameraRotate()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        _yRotation += ivertion * mouseY;
        _yRotation = Mathf.Clamp(_yRotation, -90, 90f);

        mainCamera.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    private bool checkGravity(Vector3 velocity, Vector3 dir)
    {
        return velocity.x * dir.x + velocity.y * dir.y + velocity.z * dir.z > 0;
    }

    private void Gravity()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMasc);
        if (_isGrounded && checkGravity(_gravityVelocity, _gravityDirection))
        {
            _gravityVelocity = _gravityDirection * 3;
        }

        _isUnderRoof = Physics.CheckSphere(roofCheck.position, groundDistance, groundMasc);
        if (_isUnderRoof && !checkGravity(_gravityVelocity, _gravityDirection))
        {
            _gravityVelocity = _gravityDirection * 3;
        }

        _gravityVelocity += gravity * Time.deltaTime * _gravityDirection;
        _controller.Move(_gravityVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _gravityVelocity = -_gravityDirection * Mathf.Sqrt(jumpHeight * 2 * gravity);
        }
    }


    private float CalculateAngle(float xTpPoint, float yTpPoint)
    {
        Vector2 toPoint = new Vector2(xTpPoint, yTpPoint);
        Vector2 axis = new Vector2(0, 1);
        if (xTpPoint > 0)
        {
            return Vector2.Angle(toPoint, axis);
        }
        else
        {
            return -Vector2.Angle(toPoint, axis);
        }
    }

    public void SwitchGravity(Vector3 direct)
    {
        _yRotation = 30;
        Vector3 point;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, _gravitySwitchDistance, groundMasc))
        {
            point = hit.point;
        }
        else
        {
            point = mainCamera.position + _gravitySwitchDistance * mainCamera.forward;
        }

        Vector3 pos = transform.position;
        if (direct.sqrMagnitude == 1)
        {
            Debug.Log(direct + " direc");
            _gravityDirection = -direct;

            if (direct.x == 1)
            {
                direct = new Vector3(-CalculateAngle(point.y - pos.y, point.z - pos.z), 0, -90);
            }
            else if (direct.x == -1)
            {
                direct = new Vector3(-CalculateAngle(point.y - pos.y, point.z - pos.z), 0, 90);
            }

            else if (direct.y == 1)
            {
                direct = new Vector3(0, CalculateAngle(point.x - pos.x, point.z - pos.z), 0);
            }

            else if (direct.y == -1)
            {
                direct = new Vector3(0, CalculateAngle(point.x - pos.x, point.z - pos.z), 180);
            }
            else if (direct.z == 1)
            {
                direct = new Vector3(-CalculateAngle(point.y - pos.y, point.x - pos.x), 90, 90);
            }
            else if (direct.z == -1)
            {
                direct = new Vector3(-CalculateAngle(point.y - pos.y, point.x - pos.x), 90, -90);
            }

            Debug.Log(direct + " transform");
            transform.eulerAngles = direct;
        }

        mainCamera.GetComponentInParent<ShakeCamera>().StartShake(0.25f, 0.35f);
    }

    private void PlaceToBlink()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift) || !(Time.time >= _fireTime)) return;
        _fireTime = Time.time + cooldownBlink;
        StartCoroutine(sc.StartCooldown(2, cooldownBlink));

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, rangeBlink, groundMasc))
        {
            Blink(hit.point - mainCamera.forward * blinkNear);
        }
        else
        {
            Blink(mainCamera.position + rangeBlink * mainCamera.forward);
        }
    }

    private void Blink(Vector3 pos)
    {
        _controller.enabled = false;
        transform.position = pos;
        _gravityVelocity = Vector3.zero;
        _controller.enabled = true;
    }


    public void TakeDamage(float damage)
    {
        PV.RPC("RPCTakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    private void RPCTakeDamage(float damage)
    {
        if (!PV.IsMine)
            return;
        _currentHeals -= damage;
        healthBar.fillAmount = _currentHeals / maxHeals;
        if (_currentHeals <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _playerManager.Die();
    }
}