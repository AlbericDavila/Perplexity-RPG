using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerControl player = null;

        private void Start()
        {
            player = GetComponent<PlayerControl>();
        }


        public override void use(GameObject target)
        {
            playAblitySound();
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.heal((config as SelfHealConfig).getExtraHealth());
            playParticleEffect();
            playAbilityAnimation();
        }
    }
}