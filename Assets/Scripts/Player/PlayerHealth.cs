﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    
    Animator anim;
    AudioSource playerAudio;
    [SerializeField] private PlayerMovement playerMovement; 
    PlayerShooting playerShooting;

    bool isDead;                                                
    bool damaged;                                               

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();

        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }


    void Update()
    {
        if (damaged) damageImage.color = flashColour;
        else damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        
        damaged = false;
    }

    // fungsi untuk mendapatkan damage
    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        
        playerAudio.Play();
        
        if (currentHealth <= 0 && !isDead) Death();
    }

    //fungsi membuat player mati
    void Death()
    {
        isDead = true;
        playerShooting.DisableEffects();
        
        anim.SetTrigger("Die");
        
        playerAudio.clip = deathClip;
        playerAudio.Play();
        
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            PowerUp power = collision.gameObject.GetComponent<PowerUp>();
            SetEffect(power.powerType, power.amount);
            Destroy(collision.gameObject);
        }
    }
    //fungsi tambahan untuk health and speed
    void SetEffect(PowerUpType type, float amount)
    {
        if(type == PowerUpType.heal)
        {
            int health = Mathf.RoundToInt(amount);

            currentHealth += health;
            healthSlider.value = currentHealth;
        }
        else if(type == PowerUpType.boost)
        {
            playerMovement.speed += amount;
        }
    }
}
