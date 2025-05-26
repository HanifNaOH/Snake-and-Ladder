using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoardVisualizer : MonoBehaviour
{
    public GameManager gameManager;
    public Color snakeColor = Color.red;
    public Color ladderColor = Color.green;
    
    private List<GameObject> visualElements = new List<GameObject>();

    void Start()
    {
        // Wait a moment to ensure all other components are initialized
        Invoke("Visualize", 0.5f);
    }

    public void Visualize()
    {
        // Clean up any existing visualizations
        CleanupVisualElements();
        
        // Find GameManager if not set
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in the scene!");
                return;
            }
        }
        
        // Make sure we have tiles
        if (gameManager.tiles == null || gameManager.tiles.Length == 0)
        {
            Debug.LogError("No tiles found in GameManager!");
            return;
        }
        
        // Create visualization for snakes
        foreach (var snake in gameManager.snakes)
        {
            if (snake.startTile < gameManager.tiles.Length && snake.endTile < gameManager.tiles.Length)
            {
                CreateLine(
                    gameManager.tiles[snake.startTile].position, 
                    gameManager.tiles[snake.endTile].position, 
                    snakeColor,
                    4f  // Make snakes thicker
                );
            }
        }
        
        // Create visualization for ladders
        foreach (var ladder in gameManager.ladders)
        {
            if (ladder.startTile < gameManager.tiles.Length && ladder.endTile < gameManager.tiles.Length)
            {
                CreateLine(
                    gameManager.tiles[ladder.startTile].position, 
                    gameManager.tiles[ladder.endTile].position, 
                    ladderColor,
                    2f
                );
            }
        }
    }
    
    void CreateLine(Vector3 start, Vector3 end, Color color, float width)
    {
        // Create a new GameObject for the line
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(transform);
        
        // Add line renderer
        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        
        // Set line appearance
        line.startWidth = width;
        line.endWidth = width;
        line.startColor = color;
        line.endColor = color;
        
        // Use a simple shader
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.color = color;
        
        // Add to our list for cleanup
        visualElements.Add(lineObj);
    }
    
    void CleanupVisualElements()
    {
        foreach (var element in visualElements)
        {
            if (element != null)
            {
                Destroy(element);
            }
        }
        visualElements.Clear();
    }
}
