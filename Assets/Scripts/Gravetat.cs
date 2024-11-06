using UnityEngine;

public class Gravetat : MonoBehaviour
{
    private Vector3 originalGravity;

    private void Start()
    {
        // Guardar la gravedad original al iniciar la escena
        originalGravity = Physics.gravity;
        
        // Configurar la nueva gravedad específica de esta escena
        Physics.gravity = new Vector3(0, 0, -10f);  // Por ejemplo, menos gravedad en el eje Y
    }

    private void OnDestroy()
    {
        // Restaurar la gravedad original al salir de la escena
        Physics.gravity = originalGravity;
    }
}

