using UnityEngine;
using Photon.Pun;

public class GrapGun : Gun
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject particle;
    [SerializeField] private Transform hookTip, player;

    [SerializeField] private float maxDist = 100f;
    [SerializeField] private float spring = 5f;
    [SerializeField] private float damper = 5f;
    [SerializeField] private float massScale = 5f;
    [SerializeField] private float cooldown = 1.5f;
    [SerializeField] private float climbSpeed = 3;

    private PhotonView PV;
    private float _fireTime = 0f;
    private SkillController _sc;
    private SpringJoint _joint;
    private Vector3 _grapPoint;
    private FpController _fpm;
    private LineRenderer _lr;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        _fpm = player.GetComponent<FpController>();
        _lr = GetComponent<LineRenderer>();
        _sc = player.GetComponent<SkillController>();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    public override void Shoot()
    {
        if (!(Time.time >= _fireTime)) return;
        RaycastHit hit;
        if (Physics.Raycast(fpCamera.position, fpCamera.forward, out hit, maxDist, groundLayer))
        {
            _fpm.GrapModeOn();

            _grapPoint = hit.point;
            _joint = player.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _grapPoint;

            float distance = Vector3.Distance(player.position, _grapPoint);

            _joint.maxDistance = distance;
            _joint.minDistance = distance * 0.01f;

            _joint.spring = spring;
            _joint.damper = damper;
            _joint.massScale = massScale;

            _lr.positionCount = 2;
            PV.RPC("ParticleOn", RpcTarget.All, true);
        }
    }

    public void Climbing()
    {
        _joint.maxDistance -= Time.deltaTime * climbSpeed;
    }

    void DrawRope()
    {
        if (!_joint)
            return;
        _lr.SetPosition(0, hookTip.position);
        _lr.SetPosition(1, _grapPoint);
    }

    [PunRPC]
    void ParticleOn(bool enable)
    {
        particle.SetActive(enable);
    }

    public void StopGrap()
    {
        if (!_joint)
            return;
        _fireTime = Time.time + cooldown;
        StartCoroutine(_sc.StartCooldown(1, cooldown));
        _lr.positionCount = 0;
        Destroy(_joint);
        _fpm.GrapModeOff();
        PV.RPC("ParticleOn", RpcTarget.All, false);
    }
}