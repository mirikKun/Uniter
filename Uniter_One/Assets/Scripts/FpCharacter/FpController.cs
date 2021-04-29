using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;

public class FpController : MonoBehaviour
{
    private CharacterController _controller;
    public Transform groundCheck;
    public Transform mainCamera;
    public float gravity = 20f;
    public LayerMask groundMasc;
    public float groundDistance = 0.5f;
    public float speed = 12f;
    public float jumpHeight = 10;
    
    public float mouseSensivity = 100f;
    private float yRotation = 0f;
    public int ivertion = -1;

    public float blinkNear = 4;
    public float rangeBlink = 30;
    public float cooldown = 2;
    private float _fireTime = 0f;

    public Vector3 gravityVelocity;
    public SkillController sc;
    private Vector3 _gravityDirection = new Vector3(0, -1, 0);
    private bool _isGrounded;
    private float _gravitySwitchDistance=10;
    private PhotonView _photonView;
    private SwitchGravity sg;

    // Start is called before the first frame update
    void Awake()
    {   
        _photonView = GetComponent<PhotonView>();
        _controller = GetComponent<CharacterController>();

    }

    void Start()
    {
        sg=GameObject.FindWithTag("Manager").GetComponent<SwitchGravity>();
        if (!_photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!_photonView.IsMine)
            return;
        Move();
        Gravity();
        Jump();
        PlaceToBlink();
        CameraRotate();
    }
    

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        var transform1 = transform;
        Vector3 move = transform1.right * x + transform1.forward * z;
        _controller.Move(speed * Time.deltaTime * move);
    }

    private void CameraRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        yRotation += ivertion * mouseY;
        yRotation = Mathf.Clamp(yRotation, -90, 90f);

        mainCamera.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    bool checkGravity(Vector3 velocity, Vector3 dir)
    {
        if (velocity.x * dir.x + velocity.y * dir.y + velocity.z * dir.z > 0)
            return true;
        return false;
    }

    void Gravity()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMasc);
        if (_isGrounded && checkGravity(gravityVelocity, _gravityDirection))
        {
            gravityVelocity = _gravityDirection * 3;
        }

        gravityVelocity += gravity * Time.deltaTime * _gravityDirection;
        _controller.Move(gravityVelocity * Time.deltaTime);
    }

    void Jump()
    {
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            gravityVelocity = -_gravityDirection * Mathf.Sqrt(jumpHeight * 2 * gravity);
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
        sg.GravitySwitch(direct,gravity);
        yRotation = 30;
        Vector3 point;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, _gravitySwitchDistance))
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
            else //(direct.z == -1)
            {
                direct = new Vector3(-CalculateAngle(point.y - pos.y, point.x - pos.x), 90, -90);
            }

            transform.eulerAngles = direct;
        }
    }

    void PlaceToBlink()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift) || !(Time.time >= _fireTime)) return;
        _fireTime = Time.time + cooldown;
        StartCoroutine(sc.StartCooldown(2, cooldown));

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, rangeBlink))
        {
            Blink(hit.point - mainCamera.forward * blinkNear);
        }
        else
        {
            Blink(mainCamera.position + rangeBlink * mainCamera.forward);
        }
    }

    void Blink(Vector3 pos)
    {
        _controller.enabled = false;
        transform.position = pos;
        gravityVelocity = Vector3.zero;
        _controller.enabled = true;
    }
}