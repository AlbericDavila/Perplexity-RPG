using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AnimationClip abilityAnimation;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToAttachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
            behaviourComponent.setConfig(this);
            behaviour = behaviourComponent;
        }

        public void use(GameObject target) { behaviour.use(target); }

        public float getEnergyCost() { return energyCost; }

        public GameObject getParticlePrefab() { return particlePrefab; }

        public AnimationClip getAbilityAnimation() { return abilityAnimation; }

        public AudioClip getRandomAbilitySound() { return audioClips[Random.Range(0, audioClips.Length)]; }
    }

}