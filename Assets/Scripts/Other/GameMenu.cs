using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameMenu : Utils
{
    public bool gameIsPaused;

    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;

    public AudioSource music;
    public AudioSource menuBlip;

    public Slider musicSlider;
    public Slider sfxSlider;

    public void Start()
    {
        Time.timeScale = 1f;

        musicSlider.value = PlayerPrefs.GetFloat("musicVol", 0.7f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.7f);
        
        g.globalVolume = sfxSlider.value;
        menuBlip.volume = g.globalVolume/2f;
    }
    
    public void OnEscapePress(InputAction.CallbackContext inputContext)
    {
        if (inputContext.ReadValue<float>() == 0) TogglePause();
    }

    public void OnPlayerDeath()
    {
        Cursor.visible = true;
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        if (g.timerAndScore.currentScore > PlayerPrefs.GetFloat("highscore")) {
            deathMenuUI.transform.Find("Subtitle").GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + g.timerAndScore.currentScore + "\nNew highscore!";
            PlayerPrefs.SetFloat("highscore", g.timerAndScore.currentScore);
            PlayerPrefs.Save();
        }
        else {
            deathMenuUI.transform.Find("Subtitle").GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + g.timerAndScore.currentScore + "\nHighscore: " + PlayerPrefs.GetFloat("highscore");
        }
    }

    public void TogglePause()
    {
        if (deathMenuUI.activeSelf) return;

        menuBlip.Play();

        if (gameIsPaused) {
            Cursor.visible = false;
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }

        else {
            Cursor.visible = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
    }

    public void ChangeMusicVolume(Slider slider)
    {
        music.volume = slider.value;

        PlayerPrefs.SetFloat("musicVol", slider.value);
    }

    public void ChangeSoundEffectsVolume(Slider slider)
    {
        g.globalVolume = slider.value;
        PlayerPrefs.SetFloat("sfxVol", slider.value);

        menuBlip.volume = g.globalVolume/2f;
    }

    public void RetryGame()
    {
        menuBlip.Play();
        SceneManager.LoadScene("Game World");
    }

    public void LoadMenu()
    {
        // Had to scrap the main menu
    }

    public void QuitGame()
    {
        menuBlip.Play();
        Application.Quit();
    }
}
