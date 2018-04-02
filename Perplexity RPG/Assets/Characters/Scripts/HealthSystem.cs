using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float DeathVanishSeconds = 2f;
        float currentHealthPoints = 0;
        Animator animator;
        AudioSource audioSource;
        Character characterMovement;

        const string DEATH_TRIGGER = "Death";

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();
            currentHealthPoints = maxHealthPoints;
        }

        // Update is called once per frame
        void Update()
        {
            updateHealthBar();
        }

        private void updateHealthBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }

        public void heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }

        public void takeDamage(float damage)
        {            
            bool characterDies = (currentHealthPoints - damage <= 0);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);
            if (characterDies)
            {
                StartCoroutine(killCharacter());
            }
        }

        IEnumerator killCharacter()
        {
            characterMovement.kill();
            animator.SetTrigger(DEATH_TRIGGER);

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            var playerComponent = GetComponent<PlayerControl>();
            if (playerComponent && playerComponent.isActiveAndEnabled)
            {
               SceneManager.LoadScene(0);
            }
            else // Assume it's an npc
            {
                DestroyObject(gameObject, DeathVanishSeconds);
            }
        }
    }
}
