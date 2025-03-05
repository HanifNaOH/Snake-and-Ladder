using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Transform[] boardPositions; // Assign board tile positions
    private int currentPosition = 0;

    public void Move(int steps)
    {
        currentPosition += steps;
        transform.position = boardPositions[currentPosition].position;
        CheckSnakeOrLadder();
    }

    void CheckSnakeOrLadder()
    {
        // Define snake and ladder positions
        Dictionary<int, int> snakes = new Dictionary<int, int> {
            { 17, 7 }, { 54, 34 }, { 62, 19 } // Example
        };
        Dictionary<int, int> ladders = new Dictionary<int, int> {
            { 3, 22 }, { 15, 44 }, { 27, 46 } // Example
        };

        if (snakes.ContainsKey(currentPosition))
        {
            currentPosition = snakes[currentPosition];
        }
        else if (ladders.ContainsKey(currentPosition))
        {
            currentPosition = ladders[currentPosition];
        }

        transform.position = boardPositions[currentPosition].position;
    }
}
