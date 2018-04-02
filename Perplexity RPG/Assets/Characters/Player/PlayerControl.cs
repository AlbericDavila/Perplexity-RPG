using UnityEngine;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        EnemyAI enemy;
        Character character;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            RegisterForMouseEvents();          
        }

        private void RegisterForMouseEvents()
        {
            CameraRaycaster cameraRaycaster;
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void Update()
        {
            scanForAbilityKeyDown();
        }

        // Search which key was pressed and trigger ability mapped to it
        private void scanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.getNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.attemptSpecialAbility(keyIndex);
                }
            }
        }

        // Move player to clicked destination
        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                weaponSystem.stopAttacking();
                character.setDestination(destination);
            }
        }

        private bool isTargetInRange(GameObject target)
         {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.getCurrentWeapon().getMaxAttackRange();
         }

        // Move player to enemy and attack
        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && isTargetInRange(enemy.gameObject))
            {
                weaponSystem.attackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(0) && !isTargetInRange(enemy.gameObject))
            {
                StartCoroutine(moveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && isTargetInRange(enemy.gameObject))
            {
                abilities.attemptSpecialAbility(0, enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1) && !isTargetInRange(enemy.gameObject))
            {
                StartCoroutine(moveAndPowerAttack(enemy));
            }

        }

        IEnumerator moveToTarget(GameObject target)
        {
            character.setDestination(target.transform.position);
            while (!isTargetInRange(target))            
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator moveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(moveToTarget(enemy.gameObject));
            weaponSystem.attackTarget(enemy.gameObject);
        }

        IEnumerator moveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(moveToTarget(enemy.gameObject));
            abilities.attemptSpecialAbility(0, enemy.gameObject);
        }
    }
}