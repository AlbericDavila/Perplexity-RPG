using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

        GameObject target;
        GameObject weaponObject;
        Animator animator;
        Character character;
        float lastHitTime;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();         

            setAttackAnimation();
            putWeaponInHand(currentWeaponConfig);
        }

        // Update is called once per frame
        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;

            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                // Check if target is dead
                var targetHealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                // Test if target is out of range
                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.getMaxAttackRange();
            }

            float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = characterHealth <= Mathf.Epsilon;

            if (characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
        }

        public void stopAttacking()
        {
            animator.StopPlayback();
            StopAllCoroutines();
        }

        public void putWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.getWeaponPrefab();
            GameObject dominantHand = requestDominantHand();
            Destroy(weaponObject); // empty hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        public void attackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            StartCoroutine(attackTargetRepeatedly());
        }

        IEnumerator attackTargetRepeatedly()
        {
            // Check if attacker and target are alive
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                var animationClip = currentWeaponConfig.getAttackAnimClip();
                float animationClipTime = animationClip.length / character.getAnimSpeedMultiplier();
                float timeToWait = animationClipTime + currentWeaponConfig.getTimeBetweenAnimationCycles();

                //float weaponHitPeriod = currentWeaponConfig.getTimeBetweenAnimationCycles();
                //float timeToWait = weaponHitPeriod * character.getAnimSpeedMultiplier();

                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;
                if (isTimeToHitAgain)
                {
                    attackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void attackTargetOnce()
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = currentWeaponConfig.getDamageDelay();
            setAttackAnimation();
            StartCoroutine(damageAfterDelay(damageDelay));
        }

        IEnumerator damageAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            target.GetComponent<HealthSystem>().takeDamage(calculateDamage());
        }

        public WeaponConfig getCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        private void setAttackAnimation()
        {
            if (!character.getOverrideController())
            {
                Debug.Break();
                Debug.LogAssertion("Provide " + gameObject + " with an animator override controller.");
            }
            else
            {
                var animatorOverrideController = character.getOverrideController();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.getAttackAnimClip();
            }
        }

        private GameObject requestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on PlayerControl, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on PlayerControl, please remove one");
            return dominantHands[0].gameObject;
        }

        private void attackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.getTimeBetweenAnimationCycles())
            {
                setAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                //enemy.takeDamage(calculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float calculateDamage()
        {
            return baseDamage + currentWeaponConfig.getAdditionalDamage();
        }
    }
}