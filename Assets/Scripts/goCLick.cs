using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class goCLick : MonoBehaviour
{
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
    
       if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
       /* if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        */
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;
         if (Physics.Raycast(ray, out hit))
            {
                // Verificar que sea el terreno
                if (hit.collider is TerrainCollider)
                {
                    Debug.Log("Hiciste clic en el terreno.");
                    agent.SetDestination(hit.point);
                }
                else
                {
                    Debug.Log("Hiciste clic en otro objeto: " + hit.collider.name);
                }
            }
        
    }
}
