using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BaseClass
{
    public class PlayerHandler : MonoBehaviour
    {
        [Header("Health and Money")]
        public int maxHealth = 3;
        public int currentHealth;
        public int money;

        [Header("Combo Management")]
        public int currentCombo;
        public float comboWindow = 5.0f;
        private Coroutine comboCoroutine;
        private float remainingComboTime; 

        [Header("UI Elements")]
        public HealthBar healthBar;
        public TextMeshProUGUI currentHealthText;
        public TextMeshProUGUI maxHealthText;
        public TextMeshProUGUI moneyText;
        public GameObject comboDisplay;
        public TextMeshProUGUI comboText;
        public ComboBar comboBar;

        private void Awake()
        {
            currentHealth = maxHealth;
            healthBar.setMaxHealth(maxHealth);
            currentHealthText.text = currentHealth.ToString();
            maxHealthText.text = maxHealth.ToString();
        }


        public void SetMaxHealth(int newMaxHealth) { 
            maxHealth = newMaxHealth;
            currentHealth = maxHealth;
            healthBar.setHealth(currentHealth);
            healthBar.setMaxHealth(maxHealth);
            currentHealthText.text = currentHealth.ToString();
            maxHealthText.text = maxHealth.ToString();
        }

        // Example method to apply damage to the player
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.setHealth(currentHealth); 
            currentHealthText.text = currentHealth.ToString();

            if (currentHealth <= 0) {
                SceneManager.LoadScene(0);
            }
        }

        // Example method to heal the player
        public void Heal(int amount)
        {
            if (maxHealth < amount) {
                currentHealth = maxHealth;

            } else {
                currentHealth += amount;
            }

            currentHealthText.text = currentHealth.ToString();
            maxHealthText.text = maxHealth.ToString();
 
        }

        // method to handle enemy kills and combo logic
        public void OnEnemyKilled()
        {
            comboDisplay.SetActive(true);
            currentCombo++;
            comboText.text = currentCombo.ToString();

            // restart the combo timer
            if (comboCoroutine != null)
            {
                StopCoroutine(comboCoroutine);
            }

            comboCoroutine = StartCoroutine(ComboCountdown());
        }

        // coroutine to handle the combo countdown
        private IEnumerator ComboCountdown()
        {
            remainingComboTime = comboWindow;
            while (remainingComboTime > 0)
            {
                remainingComboTime -= Time.deltaTime;
                comboBar.setTime(remainingComboTime);
                yield return null;
            }

            // Reset combo when timer runs out
            GiveMoney(currentCombo);
            currentCombo = 0;
            comboText.text = currentCombo.ToString();
            comboDisplay.SetActive(false);
        }

        public float GetRemainingComboTime()
        {
            return remainingComboTime;
        }

        public void GiveMoney(int comboSize)
        {
            int moneyToGive = CalculateMoney(comboSize);
            money += moneyToGive;
            moneyText.text = money.ToString();
        }

        private int CalculateMoney(int comboSize)
        {
            return 2 * comboSize - 1;
        }
    }
}
