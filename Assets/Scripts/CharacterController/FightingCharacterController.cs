using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class FightingCharacterController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float punchRange = 2.0f;
    public float punchCooldown = 1.0f;
    public float stunDuration = 0.5f;
    public float uprightCorrectionSpeed = 5f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isStunned = false;
    private float lastPunchTime = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isStunned) return;

        HandleMovement();
        HandleJump();
        HandlePunch();
        CorrectUpright();
    }
    
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * speed;
        rb.MovePosition(transform.position + move * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
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
                FightingCharacterController eTarget = hit.collider.GetComponent<FightingCharacterController>();
                if (eTarget != null)
                {
                    eTarget.GetPunched();
                }

                Rigidbody pTarget = hit.collider.GetComponent<Rigidbody>();
                if (pTarget != null)
                {
                    pTarget.AddForce(-hit.normal * 10f, ForceMode.Impulse);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void CorrectUpright()
    {
        Quaternion uprightRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, Time.deltaTime * uprightCorrectionSpeed);
    }
}