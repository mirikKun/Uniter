using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class FpController : MonoBehaviour,IDamageable
{
    public float maxHeals =100;
    private float currentHeals ;
    private CharacterController _controller;
    public Transform groundCheck,roofCheck;
    public Transform mainCamera;
    public float gravity = 20f;
    public LayerMask groundMasc;
    public float groundDistance = 0.5f;
    public float speed = 12f;
    public float jumpHeight = 10;
    public Image healthBar;
    
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
    private bool _isUnderRoof;
    private float _gravitySwitchDistance=10;
    private PhotonView PV;
    private Rigidbody rb;
    private Vector3 move;
    private bool isGrapMod;
    private PlayerManager _playerManager;
    public GameObject ui;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        _controller = GetComponent<CharacterController>();
        _playerManager = PhotonView.Find((int) PV.InstantiationData[0]).GetComponent<PlayerManager>();


    }

    void Start()
    {
        currentHeals = maxHeals;
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(ui);
            Destroy(rb);
            Destroy(_controller);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;
        GetInput();
        CameraRotate();
        if (isGrapMod)
            return;
        Move();
        Gravity();
        Jump();
        PlaceToBlink();
        
    }

    private void FixedUpdate()
    {
        if(!isGrapMod)
            return;
        rb.AddForce(gravity*0.5f*_gravityDirection, ForceMode.Acceleration);
        if(Vector3.Magnitude(rb.velocity)<20)
            rb.AddForce(speed*0.5f*move,ForceMode.Impulse);
    }

    public void GrapModeOn()
    {
        isGrapMod = true;
        _controller.enabled = false;
        rb.isKinematic = false;
        rb.velocity = _controller.velocity;
    }
    public void GrapModeOff()
    {
        isGrapMod = false;
        Vector3 vel = rb.velocity;
        rb.isKinematic = true;
        _controller.enabled = true;
        gravityVelocity = Vector3.zero;
        StartCoroutine(RushDeceleration(vel));
    }

    private IEnumerator RushDeceleration(Vector3 impulse)
    {
        while (impulse.magnitude>1||_isGrounded)
        {
            impulse *=0.97f;
            _controller.Move(Time.deltaTime*1.6f*impulse  );
            yield return null; 
        }
        
    }
    void Move()
    {
        _controller.Move(speed * Time.deltaTime * move);
    }

    void GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        move = transform.right * x + transform.forward * z;
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
        return velocity.x * dir.x + velocity.y * dir.y + velocity.z * dir.z > 0;
    }

    void Gravity()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMasc);
        if (_isGrounded && checkGravity(gravityVelocity, _gravityDirection))
        {
            gravityVelocity = _gravityDirection * 3;
        }

        _isUnderRoof = Physics.CheckSphere(roofCheck.position, groundDistance, groundMasc);
        if (_isUnderRoof&&!checkGravity(gravityVelocity, _gravityDirection))
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
        yRotation = 30;
        Vector3 point;
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, _gravitySwitchDistance,groundMasc))
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
            Debug.Log(direct+" direc");
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
            else if(direct.z == -1)
            {
                direct = new Vector3(-CalculateAngle(point.y - pos.y, point.x - pos.x), 90, -90);
            }
            
            Debug.Log(direct+" transform");
            transform.eulerAngles = direct;
        }
        mainCamera.GetComponentInParent<ShakeCamera>().StartShake(0.45f, 0.45f);
    }

    void PlaceToBlink()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift) || !(Time.time >= _fireTime)) return;
        _fireTime = Time.time + cooldown;
        StartCoroutine(sc.StartCooldown(2, cooldown));

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, rangeBlink,groundMasc))
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


    public void TakeDamage(float damage)
    {
        PV.RPC("RPCTakeDamage",RpcTarget.All,damage);
    }

    [PunRPC]
    private void RPCTakeDamage(float damage)
    {
        if(!PV.IsMine)
            return;
        currentHeals -= damage;
        healthBar.fillAmount = currentHeals / maxHeals;
        if (currentHeals <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        _playerManager.Die();
    }
}