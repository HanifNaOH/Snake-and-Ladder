using UnityEngine;
using System.Collections.Generic;

public class TileSetup : MonoBehaviour
{
    private GameManager gameManager;
    
    void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }
        
        SetupTiles();
    }
    
    void SetupTiles()
    {
        // Find all tiles under the Map object
        Transform mapTransform = transform;
        List<Transform> tilesList = new List<Transform>();
        
        // Collect all child transforms (tiles)
        for (int i = 0; i < mapTransform.childCount; i++)
        {
            Transform child = mapTransform.GetChild(i);
            if (child.name.StartsWith("Tile"))
            {
                tilesList.Add(child);
            }
        }
        
        // Sort tiles by name (assuming they are named Tile1, Tile2, etc.)
        tilesList.Sort((a, b) => 
        {
            string aNumber = a.name.Replace("Tile", "");
            string bNumber = b.name.Replace("Tile", "");
            
            int aVal, bVal;
            if (int.TryParse(aNumber, out aVal) && int.TryParse(bNumber, out bVal))
            {
                return aVal.CompareTo(bVal);
            }
            
            return a.name.CompareTo(b.name);
        });
        
        // Assign sorted tiles to GameManager
        if (gameManager != null)
        {
            gameManager.tiles = tilesList.ToArray();
            Debug.Log($"Set up {tilesList.Count} tiles for the game board");
            
            // After tiles are set up, place the player at the starting position (Tile1)
            if (gameManager.players != null && gameManager.players.Length > 0 && gameManager.tiles.Length > 0)
            {
                Player player = gameManager.players[0];
                player.transform.position = gameManager.tiles[0].position;
                player.currentTile = 0;
                
                // Make the player visible and correctly sized
                RectTransform rectTransform = player.GetComponent<RectTransform>();
                if (rectTransform)
                {
                    rectTransform.sizeDelta = new Vector2(30, 30); // Set an appropriate size
                }
            }
        }
        else
        {
            Debug.LogError("GameManager reference not set in TileSetup");
        }
    }
}