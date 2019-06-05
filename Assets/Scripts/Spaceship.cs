using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe da Nave (Spaceship)
/// </summary>
public class Spaceship : MonoBehaviour {

    /// <summary>
    /// Velocidade da Nave
    /// </summary>
    public float Speed;

    /// <summary>
    /// Velocidade de Rotação da Nave
    /// </summary>
    public float Rotation;

    /// <summary>
    /// Vida Máxima da Nave
    /// </summary>
    public float MaxLife;

    /// <summary>
    /// Vida atual da Nave
    /// </summary>
    [HideInInspector]
    public float Life;

    /// <summary>
    /// Tempo de cada tiro da Nave
    /// </summary>
    public float FireRate;

    /// <summary>
    /// Tipo da Nave
    /// </summary>
    public string Type;

    /// <summary>
    /// Inputs Verticais da Nave
    /// </summary>
    public string Axis_Vertical;

    /// <summary>
    /// Inputs Horizontais da Nave
    /// </summary>
    public string Axis_Horizontal;

    /// <summary>
    /// Input de Tiro da Nave
    /// </summary>
    public string Axis_Fire;

    /// <summary>
    /// Tipo do Tiro da Nave
    /// </summary>
    public GameObject Bullet;

    /// <summary>
    /// Efeito de Explosão da Nave
    /// </summary>
    public GameObject ExplosionEffect;

    /// <summary>
    /// Local onde o Tiro vai ser criado
    /// </summary>
    public Transform SpawnBullet;

    /// <summary>
    /// Imagem da Barra de Vida
    /// </summary>
    public Image LifeBar;


    /// <summary>
    /// Velocidade resultante com o Input
    /// </summary>
    private float SpeedInput;

    /// <summary>
    /// Rotação resultante com o Input
    /// </summary>
    private float RotationInput;

    /// <summary>
    /// Tempo para o próximo Tiro
    /// </summary>
    private float NextFire;


    /// <summary>
    /// RigidiBody da Nave
    /// </summary>
    private Rigidbody2D Rigidbody;

    /// <summary>
    /// Sprite da Nave
    /// </summary>
    private SpriteRenderer Sprite;


    private void Start() {

        // Captura o compomente Rigibody da Nave
        this.Rigidbody = GetComponent<Rigidbody2D>();

        // Captura o compomente SpriteRenderer da Nave
        this.Sprite = GetComponent<SpriteRenderer>();

        // Inicializa a Vida Atual com a Vida Máxima
        this.Life = this.MaxLife;
    }

    private void Update() {

        // Captura o valor referente ao botão vertical pressionado e multiplica com a velocidade para criar uma aceleração 
        this.SpeedInput = Input.GetAxis(Axis_Vertical) * this.Speed;

        // Captura o valor referente ao botão horizontal pressionado e multiplica com a velocidade para criar uma aceleração de rotação
        this.RotationInput = Input.GetAxis(Axis_Horizontal) * this.Rotation;

        // Verifica se botão de Tiro foi disparado e se já passou o tempo necessário desde o último Tiro
        if (Input.GetButtonDown(this.Axis_Fire) && Time.time > this.NextFire) { // Se o botão de tiro for precionado e já ter passado o tempo desde o último tiro

            // Define quando vai ser possível disparar um novo Tiro
            this.NextFire = Time.time + this.FireRate;

            // Cria o objeto do Tiro
            GameObject bullet = Instantiate(this.Bullet, this.SpawnBullet.position, this.SpawnBullet.rotation);

            // Define o tipo do Tiro para o mesmo da Nave
            bullet.gameObject.GetComponent<Bullet>().Type = this.Type;
        }
    }

    private void FixedUpdate() {

        // Adiciona uma força relativa a Nave
        // Ao adicionar essa força a Nave recebe uma velocidade que vai aumentando conforme a aceleração do Axis
        if (this.SpeedInput > 0)
            this.Rigidbody.AddRelativeForce(Vector2.up * this.SpeedInput);
        else
            this.Rigidbody.AddRelativeForce(Vector2.up * this.SpeedInput / 3);

        // Adiciona o novo ángulo a Nave
        this.Rigidbody.AddTorque(-this.RotationInput);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        // Verifica se a Nave colidiu com outra Nave
        if (collision.tag.Contains("Spaceship")) { // Se a Nave colidir com outra Nave

            // Aplica um Dano letal a Nave
            this.Damage(this.Life);
        }
    }

    /// <summary>
    /// Atualiza Barra de Vida da Nave
    /// </summary>
    private void UpdateLifeBar() {

        // Calcula a nova porcentagem de vida
        float amout = this.Life / this.MaxLife;

        // Atualiza a quantidade de barra preenchida com a nova porcentagem de vida
        this.LifeBar.fillAmount = amout;
    }

    /// <summary>
    /// Faz a Nave piscar 2 vezes quando tomar um Dano
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageEffect() {

        for (float i = 0f; i < 1f; i += 0.5f) {
            this.Sprite.enabled = false; // Desativa a Sprite
            yield return new WaitForSeconds(0.1f); // Espera 1 décimo de segundo
            this.Sprite.enabled = true; // Ativa a Sprite
            yield return new WaitForSeconds(0.1f); // Espera 1 décimo de segundo
        }
    }

    /// <summary>
    /// Aplica uma quantidade de dano na Nave
    /// </summary>
    /// <param name="amount">Quantidade de Dano</param>
    public void Damage(float amount) {

        // Diminui a Vida da Nave
        this.Life -= amount;

        // Assim que a Vida for reduzida, a Barra é reduzida também
        this.UpdateLifeBar();

        // Verifica se a Vida está menor ou igual a 0
        if (this.Life <= 0) { // Se a vida estiver menor ou igual a 0

            // Projeta a animação de explosão da Nave e depois destroi o objeto
            Instantiate(this.ExplosionEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        } else {

            // Inicializa uma Corrotina para ativar a animação de dano
            StartCoroutine(this.DamageEffect());
        }
    }
}
