using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Всі панелі (сторінки)")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject levelSelectPanel;
    public GameObject gameHudPanel;
    public GameObject pauseMenuPanel;
    public GameObject resultPanel;

    
    private Stack<GameObject> _historyStack = new Stack<GameObject>();
    private GameObject _currentScreen;

    private void Start()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        gameHudPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        resultPanel.SetActive(false);
        
        OpenRootScreen(mainMenuPanel);
    }

    // router
    public void OpenScreen(GameObject screenToOpen)
    {
        if (_currentScreen != null)
        {
            _historyStack.Push(_currentScreen);
            _currentScreen.SetActive(false);
        }

        screenToOpen.SetActive(true);
        _currentScreen = screenToOpen;
    }
    
    public void OpenRootScreen(GameObject screenToOpen)
    {
        _historyStack.Clear(); 

        if (_currentScreen != null)
        {
            _currentScreen.SetActive(false);
        }

        screenToOpen.SetActive(true);
        _currentScreen = screenToOpen;
    }
    
    public void GoBack()
    {
        if (_historyStack.Count > 0)
        {
            _currentScreen.SetActive(false);
            _currentScreen = _historyStack.Pop();
            _currentScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Історія порожня! Далі відступати нікуди.");
        }
    }

    // Відкриває панель ПОВЕРХ усього, не ховаючи HUD і не ламаючи історію
    public void ShowPopup(GameObject popup)
    {
        popup.SetActive(true);
    }

    // Просто ховає панель
    public void HidePopup(GameObject popup)
    {
        popup.SetActive(false);
    }
}
