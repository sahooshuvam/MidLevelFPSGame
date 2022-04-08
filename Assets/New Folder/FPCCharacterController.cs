using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCCharacterController : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    public float rotationSpeed;

    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    Camera cam;
    Quaternion playerRotation;
    Quaternion camRotation;
    float minX = -90f;
    float maxX = 90f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        transform.position +=  new Vector3(xInput * playerSpeed, 0f, zInput * playerSpeed)  * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        float xMouse = Input.GetAxis("Mouse X") * rotationSpeed;
        float yMouse = Input.GetAxis("Mouse Y") * rotationSpeed;

        print(xMouse);
        print(yMouse);

        playerRotation = Quaternion.Euler(0f, yMouse, 0f) * playerRotation;
        camRotation = ClamRotationPlayer(Quaternion.Euler(-xMouse, 0f, 0f)*camRotation);

        print("PlayerRotation: " + playerRotation);
        print("CamRotation: " + camRotation);

        this.transform.localRotation = playerRotation;
        cam.transform.localRotation = camRotation;

    }
    public bool IsGrounded()
    {
        RaycastHit raycastHit;
        if (Physics.SphereCast(transform.position, capsuleCollider.radius, Vector3.down, out raycastHit, (capsuleCollider.height / 2) - (capsuleCollider.radius) + 0.1f))
              return true;
        else
            return false;
    }

    Quaternion ClamRotationPlayer(Quaternion quaternion)
    {
        quaternion.w = 1f;
        quaternion.x /= quaternion.w;
        quaternion.y /= quaternion.w;
        quaternion.z /= quaternion.w;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quaternion.x);
        angleX = Mathf.Clamp(angleX, minX, maxX);
        quaternion.x = Mathf.Tan(Mathf.Deg2Rad * 0.5f * angleX);
        return quaternion;


    }



}
