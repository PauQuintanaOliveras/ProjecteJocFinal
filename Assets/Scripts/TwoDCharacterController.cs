using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TwoDCharacterController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    public float punchRange = 2.0f;
    public float punchCooldown = 1.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isStunned = false;
    private float stunDuration = 0.5f;
    private float lastPunchTime = -1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = 0f;

        float moveX = Input.GetAxis("Horizontal");
        Vector3 move = transform.right * moveX;
        controller.Move(move * speed * Time.deltaTime);

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) && isGrounded)
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
            if (Physics.Raycast(transform.position, transform.right, out hit, punchRange))
            {
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

