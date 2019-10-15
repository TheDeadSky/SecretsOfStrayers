using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuActions : MonoBehaviour
{

    public GameObject gameMenuPanel;

    public GameObject exitButton;
    public NetworkController networkManager;

    private bool isCursorShowed = true;
    private bool gameMenuShowed = false;

	void Start ()
    {
        HideCursor();
        if(gameMenuPanel != null)
        {
            gameMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchGameMenu();
        }
    }

    public void ResumeGame()
    {
        SwitchGameMenu();
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SwitchGameMenu()
    {
        switch(gameMenuShowed)
        {
            case true:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gameMenuPanel.SetActive(false);
                gameMenuShowed = false;
                break;
            case false:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gameMenuPanel.SetActive(true);
                gameMenuShowed = true;
                break;
        }
    }
}
