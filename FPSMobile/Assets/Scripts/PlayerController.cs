using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Transform bulletSpawn;
    public Transform laserEnd;
    public GameObject crosshair;
    public GameObject bullet;
    public Camera cam;
    public LineRenderer laser;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;

    private void Start()
    {
        Physics.IgnoreLayerCollision(9, 10);
        motor = GetComponent<PlayerMotor>();
        motor.Rotate(new Vector3(0,0,0), new Vector3(0,0,0));
        SetLaser();
    }

    private void Update()
    {
        MoveInput();
        RotationInput();
        FireInput();
    }

    private void LateUpdate()
    {
        SetLaser();
    }

    private void SetLaser()
    {
        laser.positionCount = 2;
        laser.SetPosition(0, bulletSpawn.position);
        laser.SetPosition(1, laserEnd.position);
    }

    private void MoveInput()
    {
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

        motor.Move(velocity);
    }

    private void RotationInput()
    {
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;

        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * lookSensitivity;

        motor.Rotate(rotation, cameraRotation);
    }

    private void FireInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Rigidbody _rb = Instantiate(bullet, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            Vector3 force = (laserEnd.position - bulletSpawn.position).normalized;
            _rb.AddForce(force * 30f, ForceMode.Impulse);
            //_rb.AddForce(transform.up * 1f, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            motor.StopOnCollision();
        }
    }
}
