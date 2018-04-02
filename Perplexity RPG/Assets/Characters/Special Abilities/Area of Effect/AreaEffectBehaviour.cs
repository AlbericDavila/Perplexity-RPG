using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaEffectBehaviour : AbilityBehaviour
{
    public override void use(GameObject target)
    {
        playAblitySound();
        dealRadialDamage();
        playParticleEffect();
        playAbilityAnimation();
    }

    private void dealRadialDamage()
    {
        // Static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            (config as AreaEffectConfig).getRadius(),
            Vector3.up,
            (config as AreaEffectConfig).getRadius()
        );

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerControl>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = (config as AreaEffectConfig).getDamageToEachTarget();
                damageable.takeDamage(damageToDeal);
            }
        }
    }
}
