using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentTile = 0; // Start at the first tile (index 0)
    
    // Add avatar/character customization properties if needed
    public Color playerColor = Color.white;
    
    public void ResetPosition()
    {
        currentTile = 0;
    }
}
