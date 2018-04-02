using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area of Effect"))]
    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 20f;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<AreaEffectBehaviour>();
        }

        public float getDamageToEachTarget() { return damageToEachTarget; }

        public float getRadius() { return radius; }
    }
}