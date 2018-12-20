using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject howToPlayMenu;
    public GameObject selectionMenu;

    private GameObject currentMenu;

    // Use this for initialization
    void Start () {
        currentMenu = mainMenu;
	}

    public void LoadMainMenu()
    {
        LoadLevel(mainMenu);
    }

    public void LoadHowToPlayMenu()
    {
        LoadLevel(howToPlayMenu);
    }

    public void LoadSelectionMenu()
    {
        LoadLevel(selectionMenu);
    }

    public void LoadNextLevelScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene() {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void LoadLevel(GameObject loadedMenu)
    {
        currentMenu.SetActive(false);
        currentMenu = loadedMenu;
        currentMenu.SetActive(true);
    }
}
