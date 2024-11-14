using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINavigation : MonoBehaviour
{
 public UnityEngine.AI.NavMeshAgent agent; // Agente del NavMesh
    public Transform player;   // Referencia al jugador
    public float followRange = 10f; // Distancia máxima para empezar a seguir al jugador

    private void Start()
    {
        // Si no asignaste el NavMeshAgent en el inspector, se intenta obtenerlo automáticamente
        if (agent == null)
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        // Busca al jugador automáticamente si no se asignó en el inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Si el jugador está dentro del rango de seguimiento
            if (distanceToPlayer <= followRange)
            {
                // El enemigo se mueve hacia el jugador
                agent.SetDestination(player.position);
            }
            else
            {
                // Si el jugador está fuera del rango, el enemigo se detiene
                agent.ResetPath();
            }
        }
    }

    // Método para visualizar el rango de persecución en la escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
    }
}
