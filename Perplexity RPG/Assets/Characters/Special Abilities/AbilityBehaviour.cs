using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";
        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public abstract void use(GameObject useParams = null);

        public void setConfig(AbilityConfig configToSet) { config = configToSet; }

        protected void playAbilityAnimation()
        {
            var animatorOverrideController = GetComponent<Character>().getOverrideController();
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK_STATE] = config.getAbilityAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);
        }

        protected void playAblitySound()
        {            
            var abilitySound = config.getRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }

        protected void playParticleEffect()
        {
            var particlePrefab = config.getParticlePrefab();
            var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);

            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        /// <summary>
        /// Run coroutine to destroy particle from scene AFTER it runs it's effect.
        /// </summary>
        /// <param name="particlePrefab"></param>
        /// <returns></returns>
        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }
    }
}