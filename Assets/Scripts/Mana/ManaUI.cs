using UnityEngine;
using UnityEngine.UI;

namespace Mana
{
    public class ManaUI : MonoBehaviour
    {
        [SerializeField] private Image manaFill; // Image type Filled

        private void OnEnable()
        {
            ManaManager.OnManaChanged += UpdateUI;
            UpdateUI(ManaManager.Instance.currentMana);
        }

        private void OnDisable()
        {
            ManaManager.OnManaChanged -= UpdateUI;
        }

        private void UpdateUI(float currentMana)
        {
            if (manaFill != null && ManaManager.Instance != null)
                manaFill.fillAmount = currentMana / ManaManager.Instance.maxMana;
        }
    }

}