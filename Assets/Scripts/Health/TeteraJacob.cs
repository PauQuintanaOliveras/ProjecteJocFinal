using UnityEngine;
using UnityEngine.UI;

public class TeteraJacob : MonoBehaviour
{
    // Referencia pública al Slider que representa la vida
    public Slider healthSlider;

    // Método llamado al entrar en colisión con otro objeto
    private void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.CompareTag("Player"))
        {
        Debug.Log("Pre recuperacio de vida");
      
            healthSlider.value = 100;
            Debug.Log("recuperacio de vida");
        }
       
    }
}