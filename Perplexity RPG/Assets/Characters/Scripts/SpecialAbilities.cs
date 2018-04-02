using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;
using System;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regemPointsPerSecond = 1f;
        [SerializeField] AudioClip outOfMana;

        float currentEnergyPoints;
        //CameraRaycaster cameraRaycaster;
        AudioSource audioSource;

        float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;
            attachInitialAbilities();
            updateEnergyBar();
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                addEnergyPoints();
                updateEnergyBar();
            }
        }

        public int getNumberOfAbilities() { return abilities.Length; }

        private void attachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public void attemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            var energyCost = abilities[abilityIndex].getEnergyCost();

            if (energyCost <= currentEnergyPoints)
            {
                energyComponent.consumeEnergy(energyCost);
                abilities[abilityIndex].use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfMana);               
            }
        }

        private void addEnergyPoints()
        {
            var pointsToAdd = regemPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        public void consumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            updateEnergyBar();
        }

        void updateEnergyBar()
        {
            if(energyBar)
            {
                energyBar.fillAmount = energyAsPercent;
            }
        }        
    }
}
