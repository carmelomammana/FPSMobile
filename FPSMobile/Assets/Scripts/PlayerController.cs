using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Transform bulletSpawn;
    public GameObject crosshair;
    public GameObject bullet;
    public Camera cam;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;

    private void Start()
    {
        Physics.IgnoreLayerCollision(9, 10);
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        MoveInput();
        RotationInput();
        FireInput();
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
            
            Ray ray = cam.ScreenPointToRay(crosshair.transform.position);

            GameObject _bullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            Vector3 direction = (ray.GetPoint(100000.0f) - bullet.transform.position).normalized;

            bulletRigidbody.AddForce(direction * 3f, ForceMode.Impulse);
            //Rigidbody _rb = Instantiate(bullet, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            //_rb.AddForce(new Vector3(0.5f,0,0.5f) * 3f, ForceMode.Impulse);
            //_rb.AddForce(transform.up * 3f, ForceMode.Impulse);
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
