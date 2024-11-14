using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Full3DCharacterController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100.0f;
    public float punchRange = 2.0f;
    public float punchCooldown = 1.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private bool isStunned = false;
    private float stunDuration = 0.5f;
    private float lastPunchTime = -1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isStunned)
            return;

        HandleMovement();
        HandlePunch();
    }

    private void HandleMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = 0f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandlePunch()
    {
        if (Time.time - lastPunchTime < punchCooldown) return;
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lastPunchTime = Time.time;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, punchRange))
            {
                //Full3DCharacterController target = hit.collider.GetComponent<Full3DCharacterController>();
                TwoDCharacterController target = hit.collider.GetComponent<TwoDCharacterController>();
                if (target != null)
                {
                    target.GetPunched();
                }
            }
        }
    }

    public void GetPunched()
    {
        if (!isStunned)
            StartCoroutine(StunCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
}
