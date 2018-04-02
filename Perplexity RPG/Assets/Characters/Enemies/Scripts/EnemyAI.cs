using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
	public class EnemyAI : MonoBehaviour
	{
		[SerializeField] float chaseRadius = 6f;
        [SerializeField] float WaitSecondsBetweenWaypoints = 0.5f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2f;

        //float currentHealthPoints;
        PlayerControl player = null;
        Character character;
		bool isAttacking = false;
        int nextWaypointIndex;
		float currentWeaponRange;
        float distanceToPlayer;

        enum State { idle, patrolling, attacking, chasing }
        State state = State.idle;

		void Start()
		{
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
		}

		void Update()
		{
			// Get position between enemy and player
			distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.getCurrentWeapon().getMaxAttackRange();

            bool inWeaponCircle = distanceToPlayer <= currentWeaponRange;
            bool inChaseRing = distanceToPlayer > currentWeaponRange
                                && distanceToPlayer <= chaseRadius;
            bool outsideChaseRing = distanceToPlayer > chaseRadius;

            // Player out of range, go back to patrolling
            if (outsideChaseRing)
            {
                StopAllCoroutines();
                weaponSystem.stopAttacking();
                StartCoroutine(patrol());
            }

            // Player in range, chase
            if (inChaseRing)
            {
                StopAllCoroutines();
                weaponSystem.stopAttacking();
                StartCoroutine(chasePlayer());
            }

            // Player in attack range, attack
            if (inWeaponCircle)
            {
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.attackTarget(player.gameObject);
            }
        }

        IEnumerator patrol()
        {
            state = State.patrolling;

            while (patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.setDestination(nextWaypointPos);
                cycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(WaitSecondsBetweenWaypoints);
            }
        }

        IEnumerator chasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.setDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        private void cycleWaypointWhenClose(Vector3 nextWaypointPosition)
        {
            if (Vector3.Distance(transform.position, nextWaypointPosition) <= waypointTolerance)
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
        }

        void OnDrawGizmos()
		{
			// Draw chase range gizmos
			Gizmos.color = new Color(0, 0, 255f, 0.5f);
			Gizmos.DrawWireSphere (transform.position, chaseRadius);

			// Draw attack range gizmos
			Gizmos.color = new Color(255f, 0f, 0, 0.5f);
			Gizmos.DrawWireSphere (transform.position, currentWeaponRange);
		}
    }
}