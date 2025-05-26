using UnityEngine;
using TMPro;

public class DiceDisplay : MonoBehaviour
{
    private TextMeshProUGUI diceText;
    private GameManager gameManager;
    
    void Start()
    {
        diceText = GetComponent<TextMeshProUGUI>();
        if (diceText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on DiceDisplay!");
            return;
        }
        
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }
        
        // Subscribe to dice roll events
        gameManager.OnDiceRolled += UpdateDiceDisplay;
        
        // Default text
        diceText.text = "Roll the dice!";
    }
    
    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (gameManager != null)
        {
            gameManager.OnDiceRolled -= UpdateDiceDisplay;
        }
    }
    
    public void UpdateDiceDisplay(int rollResult)
    {
        diceText.text = $"Dice Roll: {rollResult}";
    }
}
