using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mana
{
    public class ManaUI : MonoBehaviour
    {
        [Header("Provider")]
        [Tooltip("Assign a MonoBehaviour that implements IManaReadOnly (usually a ManaReadOnlyFacade).")]
        [SerializeField] private MonoBehaviour manaProvider;

        [Header("UI Elements")]
        [SerializeField] private Image manaFill; // Image type Filled
        [SerializeField] private TMP_Text manaText;
        
        private IManaReadOnly _mana;

        private void Awake()
        {
            if (manaProvider == null)
                throw new InvalidOperationException(
                    $"[{nameof(ManaUI)}] 'manaProvider' reference not assigned in inspector."
                );
            
            if (manaFill == null)
                throw new InvalidOperationException(
                    $"[{nameof(ManaUI)}] 'manaFill' reference not assigned in inspector."
                );
            
            if (manaText ==null) 
                throw new InvalidOperationException(
                    $"[{nameof(ManaUI)}] 'manaText' reference not assigned in inspector"
                );
            
            _mana = manaProvider as IManaReadOnly;
            if (_mana == null)
                throw new InvalidOperationException(
                    $"[{nameof(ManaUI)}] Assigned manaProvider does not implement IManaReadOnly."
                );

        }
        
        private void Start()
        {
            _mana.OnManaChanged += UpdateUI;
            UpdateUI(_mana.CurrentMana);
        }
        private void OnDisable()
        {
            _mana.OnManaChanged -= UpdateUI;
        }

        private void UpdateUI(float currentMana)
        {
            manaFill.fillAmount = currentMana / _mana.MaxMana;
            manaText.text = $"{Mathf.FloorToInt(currentMana)}/{_mana.MaxMana}";
        }
    }

}