using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FightingCharacterController : MonoBehaviour
{
    //Multiplicador de la velocitat
    public float speed = 5.0f;
    //Quantitat de força aplicada al personatge al saltar
    public float jumpForce = 5.0f;
    //Distancia a la que el personatge arriba a pegar
    public float punchRange = 2.0f;
    //Temps despera per a poder tornar a pegar
    public float punchCooldown = 1.0f;
    //Segons de aturdiment que rebra al ser pegat
    public float stunDuration = 0.5f;
    //Velocitat a la que s'aixeca quan cau
    public float uprightCorrectionSpeed = 5f;
    // Rigidbody del model del persontage
    private Rigidbody rb;
    //Boolea que indica si el personatge a tocat el terra des de l'ultim cop que ha saltat.
    //NO INDICA SI ESTA ACTUALMENT AL TERRA, ES POSSIBLE SALTAR DES DE L'AIRE SI S'HA CAIGUT DE UN LLOC ELEVAT SENSE SALTAR
    //AIXO NO ES UN ERROR I ES EL COMPORTAMENT QUE ES BUSCAVA.
    private bool isGrounded = true;
    //Aquest boolea indica si el personatge esta aturdit.
    private bool isStunned = false;
    //Variable que guarda una marca de temps de l'ultim com que s'ha golpejat, calculem el temps aixi per que es mes eficient que obrir una corrutina amb un temporitzador.
    private float lastPunchTime = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent que el personatge es tombi sense voler congelant les fisiques de rotacio, tota rotacio del personatge ahura de ser programada
    }

    void Update()
    {
        if (isStunned) return; //Si el persontage esta aturdit no es podra moure

        HandleMovement();   //Gestiona el moviment del jugador
        HandleJump();       //Gestiona el salt del jugador
        HandlePunch();      //Gestiona els atacs del jugador
        CorrectUpright();   //Corregeix la inclinacio del personatge en cas de que fos incorrecte
    }
    
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // Agafa la delta de moviment del control horizontal
        float moveZ = Input.GetAxis("Vertical"); // Agafa la delta de moviment del control frontal 

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * speed; //Ajunta els deltes de moviment dels eixos X i Z els normalitza i els multiplica per la velocitat
        rb.MovePosition(transform.position + move * Time.deltaTime); // Mou el personatge a la seva posicio mes el delta de moviment multiplicat per el deltaTime
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)// detecta quan s'apreta la tecla de saltar i si pot saltar
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //impulsa el personatge amont per la força de salt
            isGrounded = false; // marca que el jugador a saltat i ja no es a terra.
        }
    }

    private void HandlePunch()
    {
        if (Time.time - lastPunchTime < punchCooldown) return; // detecta si es pot pegar, si encara no es pot, surt de la funcio

        if (Input.GetKeyDown(KeyCode.Q)) // detecta si s'apreta la tecla q 
        {
            lastPunchTime = Time.time; //guarda el temps en el que s'ha pegat

            RaycastHit hit; //Llança un raycast
            if (Physics.Raycast(transform.position, transform.forward, out hit, punchRange))//detecta si el raig colisiona amb alguna cosa dins el rang del personage
            {
                FightingCharacterController eTarget = hit.collider.GetComponent<FightingCharacterController>(); //agafa el controllador del objectiu
                if (etarget != null)//si no es null
                {
                    eTarget.GetPunched(); // activa el metoda de ser pegat del objectiu.
                }
                Rigidbody pTarget = hit.collider.GetComponent<rigidbody>();
                if(pTarget != null){
                    pTarget.AddForce(Vector3)
                }
            }
        }
    }
    //gestiona ser colpejat
    public void GetPunched()
    {
        if (!isStunned) //si no estas atordit
            StartCoroutine(StunCoroutine()); //engega la corrutina de aturdiment
    }
    //gestiona ser atordit, espera un numero prestablert de segons
    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
    //quan colisiones amb un objecte terra et torna habilitar el salt
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void CorrectUpright()
    {
        // Rotacio objectiu conservant la y per no canvia la direccio a la que mira
        Quaternion uprightRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        
        // Rotar gradualment cap a la orientacio objectiu
        transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, Time.deltaTime * uprightCorrectionSpeed);
    }
}
