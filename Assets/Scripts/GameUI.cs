using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public Button resetButton;
    public GameManager gameManager;
    
    void Start()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
        
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(false);
            resetButton.onClick.AddListener(ResetGame);
        }
        
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        
        // Subscribe to win events
        gameManager.OnGameWon += ShowWinScreen;
    }
    
    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (gameManager != null)
        {
            gameManager.OnGameWon -= ShowWinScreen;
        }
        
        if (resetButton != null)
        {
            resetButton.onClick.RemoveListener(ResetGame);
        }
    }
    
    public void ShowWinScreen(int playerIndex)
    {
        if (winText != null)
        {
            winText.text = $"Player {playerIndex + 1} Wins!";
            winText.gameObject.SetActive(true);
        }
        
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(true);
        }
    }
    
    public void ResetGame()
    {
        // Hide win UI
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
        
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(false);
        }
        
        // Tell the GameManager to reset the game
        if (gameManager != null)
        {
            gameManager.ResetGame();
        }
    }
}
