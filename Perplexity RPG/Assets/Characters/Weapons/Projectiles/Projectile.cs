using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
	public class Projectile : MonoBehaviour 
	{

		[SerializeField] float projectileSpeed;
		[SerializeField] GameObject shooter; 	// Can be inspected when paused

		const float DESTROY_DELAY = 0.01f;
		float damageInflicted;

		public void setDamageInflicted(float dmg) { damageInflicted = dmg; }

		public float getDefaultLaunchSpeed() { return projectileSpeed; }

		public void setShooter(GameObject shooter)
		{
			this.shooter = shooter;
		}

		void OnCollisionEnter(Collision collision)
		{
			var layerCollidedWith = collision.gameObject.layer;
			if (shooter && layerCollidedWith != shooter.layer) 
			{
				//dealDamage (collision);	
			}
		}


        /*
		void dealDamage (Collision collision)
		{
			Component damageableComponent = collision.gameObject.GetComponent (typeof(IDamageable));
			if (damageableComponent) {
				// We have to differentiate from Monobehaviour and IDamageable
				// This is calling the method from the interface
				(damageableComponent as IDamageable).takeDamage (damageInflicted);
			}
			Destroy (gameObject, DESTROY_DELAY);
		}*/
	}
}