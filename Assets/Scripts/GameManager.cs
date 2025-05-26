using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class SnakeOrLadder
{
    public int startTile;
    public int endTile;
}

public class GameManager : MonoBehaviour
{
    public Player[] players;
    private int currentPlayer = 0;

    public List<SnakeOrLadder> snakes;
    public List<SnakeOrLadder> ladders;    public Transform[] tiles; // Array of tile positions
    
    // Events
    public event Action<int> OnDiceRolled;
    public event Action<int> OnGameWon;
    
    // Game state
    private bool gameActive = true;    public void EndTurn()
    {
        currentPlayer = (currentPlayer + 1) % players.Length;
        Debug.Log("Player " + (currentPlayer + 1) + "'s Turn");
    }
    
    public void ResetGame()
    {
        // Reset game state
        gameActive = true;
        currentPlayer = 0;
        
        // Reset all players to the start position
        foreach (var player in players)
        {
            player.ResetPosition();
            if (tiles.Length > 0)
            {
                player.transform.position = tiles[0].position;
            }
        }
        
        Debug.Log("Game Reset. Player 1's Turn.");
    }public void RollDice()
    {
        int diceRoll = UnityEngine.Random.Range(1, 7); // Roll a dice (1-6)
        Debug.Log($"Player {currentPlayer + 1} rolled a {diceRoll}");
        
        // Invoke the event
        OnDiceRolled?.Invoke(diceRoll);

        MovePlayer(diceRoll);
    }    private void MovePlayer(int steps)
    {
        if (!gameActive) return;
        
        Player player = players[currentPlayer];

        int targetTile = player.currentTile + steps;

        // Win condition - Player reached the last tile or beyond
        if (targetTile >= tiles.Length - 1)
        {
            Debug.Log($"Player {currentPlayer + 1} wins!");
            
            // Move to the last tile
            player.currentTile = tiles.Length - 1;
            player.transform.position = tiles[tiles.Length - 1].position;
            
            // Trigger win event
            OnGameWon?.Invoke(currentPlayer);
            
            // Set game to inactive
            gameActive = false;
            
            return;
        }

        player.currentTile = targetTile;
        player.transform.position = tiles[targetTile].position;

        CheckForSnakesOrLadders(player);

        EndTurn();
    }

    private void CheckForSnakesOrLadders(Player player)
    {
        foreach (var snake in snakes)
        {
            if (player.currentTile == snake.startTile)
            {
                Debug.Log($"Player {currentPlayer + 1} hit a snake! Moving to tile {snake.endTile}");
                player.currentTile = snake.endTile;
                player.transform.position = tiles[snake.endTile].position;
                return;
            }
        }

        foreach (var ladder in ladders)
        {
            if (player.currentTile == ladder.startTile)
            {
                Debug.Log($"Player {currentPlayer + 1} climbed a ladder! Moving to tile {ladder.endTile}");
                player.currentTile = ladder.endTile;
                player.transform.position = tiles[ladder.endTile].position;
                return;
            }
        }
    }
}
