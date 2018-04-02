using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void use(GameObject target)
        {
            playAblitySound();
            dealDamage(target);
            playParticleEffect();
            playAbilityAnimation();
        }

        private void dealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).getExtraDamage();
            target.GetComponent<HealthSystem>().takeDamage(damageToDeal);
        }
    }
}