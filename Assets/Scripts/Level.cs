using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe do Level (Level)
/// </summary>
public class Level : MonoBehaviour{

    /// <summary>
    /// Nome da Cena
    /// </summary>
    public string SceneName;

    void Start(){
        
    }

    void Update(){

        // Verifica se o botão R foi pressionado
        if (Input.GetKeyDown(KeyCode.R)) { // Se o botão R for pressionado

            // Reinicia a Cena
            SceneManager.LoadScene(this.SceneName);
        }
    }
}
