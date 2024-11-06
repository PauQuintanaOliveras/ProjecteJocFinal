using UnityEngine;

public class ReduirObjecte : MonoBehaviour
{
    // Velocidad de reducción del tamaño
    public float shrinkSpeed = 5.0f;

    // Tamaño al cual desaparecer (debe ser muy pequeño, cercano a cero)
    private float minSize = 0.01f;

    void Update()
    {
        // Si el tamaño actual es mayor que el mínimo
        if (transform.localScale.x > minSize && transform.localScale.y > minSize && transform.localScale.z > minSize)
        {
            // Calcula el nuevo tamaño basado en la velocidad y el tiempo transcurrido
            Vector3 newScale = transform.localScale - Vector3.one * shrinkSpeed * Time.deltaTime;

            // Asegura que el tamaño no baje de cero
           newScale = new Vector3(
               Mathf.Max(newScale.x, 0),
               Mathf.Max(newScale.y, 0),
               Mathf.Max(newScale.z, 0)
           );

            // Aplica el nuevo tamaño al objeto
            transform.localScale = newScale;
        }
        else
        {
            // Si el objeto es suficientemente pequeño, destrúyelo
            Destroy(gameObject);
        }
    }
}

