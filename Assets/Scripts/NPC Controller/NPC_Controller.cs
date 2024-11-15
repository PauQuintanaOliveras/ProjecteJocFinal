using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class NPC_Controller : MonoBehaviour
{
    public Animator animator;
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

    // Nueva función para realizar el ataque (golpe)
    private void PerformPunch(GameObject target)
    {
        if (Time.time - lastPunchTime < punchCooldown) return;

        lastPunchTime = Time.time;
        animator.SetBool("Atacar", true);
        Debug.Log("Cop de Puny");

        FightingCharacterController eTarget = target.GetComponent<FightingCharacterController>();

        

        if (eTarget != null)
        {
            eTarget.GetPunched();
        }

        Rigidbody pTarget = target.GetComponent<Rigidbody>();
        if (pTarget != null)
        {
            Vector3 attackDirection = (target.transform.position - transform.position).normalized;
            pTarget.AddForce(attackDirection * 10f, ForceMode.Impulse);
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

    // Modificación del método de colisión para atacar automáticamente
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Ataque automático al colisionar con un objeto con el tag "Player" 
        if (collision.gameObject.CompareTag("Player"))
        {
            PerformPunch(collision.gameObject);
        }
    }

    private void CorrectUpright()
    {
        Quaternion uprightRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, Time.deltaTime * uprightCorrectionSpeed);
    }
}
