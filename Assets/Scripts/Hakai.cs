using UnityEngine;
using System.Collections;  // Asegúrate de incluir esta línea

public class Hakai : MonoBehaviour
{
    public float shrinkDuration = 1f;  // Duración del encogimiento y rotación
    public float rotationSpeed = 360f; // Velocidad de rotación en grados por segundo

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        StartCoroutine(ShrinkAndDestroy());  // Inicia la corutina cuando hay colisión
    }

    private IEnumerator ShrinkAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;  // Tamaño original del objeto

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;

            // Reducir el tamaño del objeto gradualmente
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsedTime / shrinkDuration);

            // Rotar el objeto continuamente
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

            yield return null;  // Esperar al siguiente frame
        }

        // Destruir el objeto después de completar la animación
        Destroy(gameObject);
        
    }
}
