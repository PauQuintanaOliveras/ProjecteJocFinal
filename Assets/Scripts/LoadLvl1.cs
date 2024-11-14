using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI; // Necesario si se usa con un botón UI

public class LoadLvl1 : MonoBehaviour
{
  
    // Método que se llamará al hacer clic en el botón
    public void LoadScene(string nom)
    {
        // Cargar la escena especificada
        SceneManager.LoadScene(nom);
    }
}
