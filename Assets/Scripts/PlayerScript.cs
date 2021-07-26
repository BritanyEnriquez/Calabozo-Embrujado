using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 4.0f;
    Rigidbody2D rb;

    private float health = 200;
    private float startHealth;

    public bool turnedLeft = false;
    public Image healthFill;
    private float healthWidth;

    public Text mainText;
    public Image redOverlay;
    public Text expText;

    private int experience = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthWidth = healthFill.sprite.rect.width;
        startHealth = health;
        mainText.gameObject.SetActive(true);
        redOverlay.gameObject.SetActive(true);
        Invoke("HideTitle", 2);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        turnedLeft = false;
        if (horizontal > 0)
        {
            GetComponent<Animator>().Play("Right");
        }
        else if (horizontal < 0)
        {
            GetComponent<Animator>().Play("Left");
            turnedLeft = true;
        }
        else if (vertical > 0)
        {
            GetComponent<Animator>().Play("Up");
        }
        else if (vertical < 0)
        {
            GetComponent<Animator>().Play("Down");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            health -= collision.gameObject.GetComponent<EnemyScript>().GetHitStrength();
            if (health < 1)
            {
                healthFill.enabled = false;
                mainText.gameObject.SetActive(true);
                mainText.text = "Game Over";
                redOverlay.gameObject.SetActive(true);
                StartCoroutine("ReiniciarJuegoTiempo");
            }
            Vector2 temp = new Vector2(healthWidth * (health / startHealth), healthFill.sprite.rect.height);
            healthFill.rectTransform.sizeDelta = temp;
            Invoke("HidePlayerBlood", 0.25f);
        }
        else if (collision.gameObject.CompareTag("Spawner"))
        {
            collision.gameObject.GetComponent<SpawnerScript>().GetGatewayWeapon();
        }
        else if (collision.gameObject.CompareTag("TPLaberinto"))
        {
            SceneManager.LoadScene("laberinto");
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            healthFill.enabled = false;
            mainText.gameObject.SetActive(true);
            mainText.text = "Game Over";
            redOverlay.gameObject.SetActive(true);
            StartCoroutine("ReiniciarJuegoTiempo");
            Destroy(this.GetComponent<SpriteRenderer>());
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            healthFill.enabled = false;
            mainText.gameObject.SetActive(true);
            mainText.text = "You win!";
            redOverlay.gameObject.SetActive(true);
        }
    }

    void HidePlayerBlood()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        expText.text = experience.ToString();
    }

    void HideTitle()
    {
        mainText.gameObject.SetActive(false);
        redOverlay.gameObject.SetActive(false);
    }
    void ReiniciarJuego()
    {
        SceneManager.LoadScene(3);
        mainText.gameObject.SetActive(false);
        redOverlay.gameObject.SetActive(false);

    }
    IEnumerator ReiniciarJuegoTiempo()
    {
        yield return new WaitForSeconds(3f);
        ReiniciarJuego();
    }
}
