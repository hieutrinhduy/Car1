using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CarController : MonoBehaviour
{

    public static CarController Ins { get; private set; }
    public int GoldInGame = 0;

    [SerializeField] private Transform raycastPoint;
    //Wheel
    [SerializeField] private Transform _transformFL;
    [SerializeField] private Transform _transformFR;
    [SerializeField] private Transform _transformBL;
    [SerializeField] private Transform _transformBR;
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;

    private bool isBreak;
    private Rigidbody carRB;
    public bool IsGameOver;

    private float horizontal;
    private float vertical;
    private Vector3 dir;

    //groundLayerMask
    public LayerMask GroundLayer;
    public LayerMask IncreaseSpeedPlatformLayer;
    public LayerMask WallLayer;

    //carForce
    public float _force;
    public float _maxAngle;
    public float _accelerationMultiplier = 1f;
    public int _maxBackSpeed;
    public int _brake = 500000;
    public float forceForward;

    //checkGround
    [SerializeField]
    private bool isGround;
    public bool IsSpeedUpGround;
    public float DoDAI;

    //Nitro
    public float NitroLast = 10f;
    public float NitroTimer = 0f;
    public bool isNitroActive = false;

    //Center of mass
    public GameObject CenterOfMass;
    private void Start()
    {
        IsGameOver = false;
        CarController.Ins = this;
        carRB = GetComponent<Rigidbody>();
        carRB.centerOfMass = CenterOfMass.transform.localPosition;
        GameController.Ins.Load();
    }

    private void Update()
    {
        GetInput();
        isGround = CheckGround();
        Nitro();
        CarBoostSpeed();
        IsSpeedUpGround = CheckIncreaseSpeedPlatform();
    }

    private void CarBoostSpeed()
    {
        if (isNitroActive)
        {
            _accelerationMultiplier = 10f;
        }
        else
        {
            _accelerationMultiplier = 1f;
        }
    }
    public void TurnOffGravity()
    {
        carRB.useGravity = false;
        carRB.velocity = Vector3.zero;
        carRB.angularVelocity = Vector3.zero;
    }


    private void GetInput() {
        horizontal = SimpleInput.GetAxis("Horizontal");
        vertical = SimpleInput.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        MoveBody();
        WheelMove();
        Brake();
        WheelSteering();
        CheckBrake();
    }
    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || isBreak)
        {
            _colliderBL.brakeTorque = _brake;
            _colliderBR.brakeTorque = _brake;
            _colliderFR.brakeTorque = _brake;
            _colliderFR.brakeTorque = _brake;
        }
        else
        {
            _colliderBL.brakeTorque = 0;
            _colliderBR.brakeTorque = 0;
            _colliderFR.brakeTorque = 0;
            _colliderFR.brakeTorque = 0;
        }
    }
    private void CheckBrake()
    {
        if (isBreak)
        {
            _colliderBL.brakeTorque = _brake;
            _colliderBR.brakeTorque = _brake;
            _colliderFR.brakeTorque = _brake;
            _colliderFR.brakeTorque = _brake;
        }
        else
        {
            _colliderBL.brakeTorque = 0;
            _colliderBR.brakeTorque = 0;
            _colliderFR.brakeTorque = 0;
            _colliderFR.brakeTorque = 0;
        }
    }
    private void MoveBody()
    {
        if (!isGround)
        {
           
                dir = new Vector3(horizontal, 0, 0);
                dir.Normalize();
                Quaternion dirTarget = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, dirTarget, 0.5f * Time.fixedDeltaTime);
            
            
            //carRB.velocity = dir * _force * Time.fixedDeltaTime + Vector3.up * carRB.velocity.y;
            //if (carRB.velocity.magnitude < 15f)
            //{
            //    carRB.AddForce(Vector3.forward * forceForward);
            //} 
        }
        //else if(SimpleInput.GetAxis("Horizontal") == 0)
        //{
        //    dir = new Vector3(0, 0, 0);
        //    Quaternion dirTarget = Quaternion.LookRotation(dir);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, dirTarget,2*Time.fixedDeltaTime);
        //}
    }
    //WheelMoving
    private void WheelMove()
    {
        _colliderBL.motorTorque = (_force * _accelerationMultiplier *4) *SimpleInput.GetAxis("Vertical");
        _colliderBR.motorTorque = (_force * _accelerationMultiplier *4) *SimpleInput.GetAxis("Vertical");
        _colliderBL.motorTorque = Mathf.Clamp(_colliderBL.motorTorque, -1800f, 1800f);
        _colliderBR.motorTorque = Mathf.Clamp(_colliderBR.motorTorque, -1800f, 1800f);
        RotateWheel(_colliderBL, _transformBL);
        RotateWheel(_colliderBR, _transformBR);
    }
    private void WheelSteering()
    {
        _colliderFL.steerAngle = _maxAngle * SimpleInput.GetAxis("Horizontal");
        _colliderFR.steerAngle = _maxAngle * SimpleInput.GetAxis("Horizontal");
        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFR, _transformFR);
    }
    private void RotateWheel(WheelCollider coll, Transform transform) {
        Vector3 position;
        Quaternion rotation;

        coll.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }
    //WheelMoving

    //WheelBraking
    public void OnBrake()
    {
        isBreak = true;
    }
    public void OnUnBrake()
    {
        isBreak = false;
    }
    //WheelBraking

    //CheckGround
    private bool CheckGround()
    {
        if (Physics.Raycast(raycastPoint.position, Vector3.down, DoDAI, GroundLayer) || CheckIncreaseSpeedPlatform())
        {
            return true;
        }
        return false;
    }
    private bool CheckIncreaseSpeedPlatform()
    {
        if (Physics.Raycast(raycastPoint.position, Vector3.down, DoDAI, IncreaseSpeedPlatformLayer))
        {
            carRB.AddForce(Vector3.forward * 2, ForceMode.Impulse);
            return true;
        }
        return false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = raycastPoint.TransformDirection(Vector3.down) * DoDAI;
        Gizmos.DrawRay(raycastPoint.position, direction);
    }
    //CheckGround

    //Nitro

    private void Nitro()
    {
        if (NitroTimer > 0 && isNitroActive)
        {
            NitroTimer -= Time.deltaTime;
            carRB.AddForce(Vector3.forward * 15, ForceMode.Impulse);
            _accelerationMultiplier = 10f;

        }
        else
        {
            isNitroActive = false;
            _accelerationMultiplier = 1f;
        }
    }
    public bool CanActiveNitro()
    {
        if (NitroTimer > 0)
        {
            return true;
        }
        return false;
    }
    //Nitro

    //ColliderEnter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nitro"))
        {
            if (NitroTimer < NitroLast) {
                Debug.Log("+1 nitro");            
                NitroTimer += (NitroLast / 2);
                if (NitroTimer > NitroLast)
                    NitroTimer = 6f;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Gold")) {
            //OnGoldCollecting?.Invoke(this, EventArgs.Empty);
            GoldInGame += 1;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Goal"))
        {
            Debug.Log("Stage Finish");
            UIManager.Ins.Finish();
            GameController.Ins.FinishMap();
            RandomReward.Ins.SetArrowXPosition(-280f);
            RandomReward.Ins.ActiveClaimBTN();
            GameController.Ins.TotalGold += 1000;
            RewardManager.Ins.RewardPileOfCoin(null,6);
            GameController.Ins.Save();
        }
        else if (other.CompareTag("DeathBorder"))
        {
            GameController.Ins.Save();
            IsGameOver = true;
            UIManager.Ins.Lose();
            Debug.Log("Lose");
        }
        else if (other.CompareTag("escalator"))
        {
            CameraFollow.Ins.ChangeAspect();
            Destroy(other.gameObject);
            this.OnBrake();
        }
        else if (other.CompareTag("ResetBrake"))
        {
            this.OnUnBrake();
        }
    }
    public void ResetItemCount()
    {
        this.NitroTimer = 0;
        this.GoldInGame = 0;
    }

}
