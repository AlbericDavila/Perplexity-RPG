using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {

        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickupSFX;

        AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void DestroyChildren()
        {
            // Iterate through transform and destroy the children
            foreach (Transform child in transform)
                DestroyImmediate(child.gameObject);
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.getWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        private void OnTriggerEnter()
        {
            FindObjectOfType<PlayerControl>().GetComponent<WeaponSystem>().putWeaponInHand(weaponConfig);
            audioSource.PlayOneShot(pickupSFX);            
        }
    }
}