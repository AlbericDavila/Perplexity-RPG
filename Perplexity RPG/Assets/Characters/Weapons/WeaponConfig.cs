using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName = ("RPG/WeaponConfig"))]
	public class WeaponConfig : ScriptableObject {

		public Transform gripTransform;
		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip attackAnimation;
		[SerializeField] float timeBetweeenAnimationCycles = 0.5f;
		[SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = 0.5f;

        public float getTimeBetweenAnimationCycles() { return timeBetweeenAnimationCycles; }

		public float getMaxAttackRange() { return maxAttackRange; }

        public float getAdditionalDamage() { return additionalDamage; }

        public float getDamageDelay() { return damageDelay; }

        public GameObject getWeaponPrefab() { return weaponPrefab; }

		public AnimationClip getAttackAnimClip() 
		{
			// Remove animation events so that asset packs cannot cause crashes
			attackAnimation.events = new AnimationEvent[0];

			return attackAnimation; 
		}
	}
}