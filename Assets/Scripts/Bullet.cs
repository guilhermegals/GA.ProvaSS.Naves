using UnityEngine;

/// <summary>
/// Classe do Tiro (Bullet)
/// </summary>
public class Bullet : MonoBehaviour{

    /// <summary>
    /// Dano do Tiro
    /// </summary>
    public int Damage;

    /// <summary>
    /// Velociddade do Tiro
    /// </summary>
    public float Speed;

    /// <summary>
    /// Tipo do Tiro.
    /// Vai ser sempre o mesmo tipo da Nave que o projetou
    /// </summary>
    [HideInInspector]
    public string Type;

    /// <summary>
    /// Animação de Impacto do Tiro
    /// </summary>
    public GameObject ImpactEffect;

    /// <summary>
    /// RigidiBody do Tiro
    /// </summary>
    private Rigidbody2D Rigidbody;

    private void Start(){
        // Captura o compomente Rigibody do Tiro
        this.Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        
    }

    private void FixedUpdate() {
        // Adiciona uma força relativa para a direção que o Tiro estiver apontando com a velocidade definida
        // Promove a aceleração do tiro
        this.Rigidbody.AddRelativeForce(Vector2.up * this.Speed);    
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        // Verifica se o Tiro não colidiu com a Nave que o projetou ou se colidiu com algum colisor com o Trigger ativado
        if (!collision.tag.Contains(this.Type) && !collision.isTrigger) {

            // Verifica se o Tiro colidiu com alguma Nave
            if (collision.tag.Contains("Spaceship")) { // Se tiver colidido com alguma Nave

                // Pega a classe da Nave e chama a função de dano passando como parametro o dano do Tiro
                collision.gameObject.GetComponent<Spaceship>().Damage(this.Damage);
            }

            // Projeta a animação de impácto do Tiro e depois destroi o objeto
            Instantiate(this.ImpactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }        
    }
}
