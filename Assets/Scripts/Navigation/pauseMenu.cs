using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitGame : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject shopMenu;
    bool isPaused;
    bool isShopping;

    void Start()
    {
        pauseMenu.SetActive(false); // Ensure the pause menu is initially inactive
        isPaused = false;

        shopMenu.SetActive(false); // Ensure the shop menu is initially inactive
        isShopping = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isShopping) // if the user is NOT in the shop menu...
            {
                if (isPaused) // if the user is in the pause menu...
                {
                    PauseMenu_Resume(); // resume
                }
                else // if the user is NOT in the pause menu...
                {
                    PauseMenu_Pause(); // pause
                }
            }
            else // if the user is in the shop menu...
            {
                ShopMenu_Exit(); // close the shop menu
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && !isPaused) // if the game isn't paused when the menu is opened...
        {
            if (isShopping) // if the user is in the shop menu...
            {
                ShopMenu_Exit(); // exit the shop
            }
            else // if the user is NOT in the shop menu...
            {
                ShopMenu_Show(); // enter the shop
            }
        }
    }

    // PAUSE MENU
    public void PauseMenu_Pause()
    {
        Debug.Log("Pausing...");
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true); // Activate the pause menu
        isPaused = true;
        Debug.Log("Paused.");
    }

    public void PauseMenu_Resume()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        pauseMenu.SetActive(false); // Deactivate the pause menu
        Debug.Log("Resumed.");
    }

    public void ExitLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StartMenu");
    }

    // SHOP MENU
    public void ShopMenu_Show()
    {
        shopMenu.SetActive(true);
        isShopping = true;
    }

    public void ShopMenu_Exit()
    {
        shopMenu.SetActive(false);
        isShopping = false;
    }
}
