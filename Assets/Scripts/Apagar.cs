using UnityEngine;

public class Apagar : MonoBehaviour
{
    // Metode per Apagar el Joc
    public void ExitGame()
    {
        // Envia un missatge x Consola al Apagar el Joc (per comprovar que el Script funciona sense fer un Built)
        Debug.Log("Apagant..");

        // Apaga el Joc quan s'executa en una Build
        Application.Quit();
    }
}
