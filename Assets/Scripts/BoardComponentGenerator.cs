using System.Collections.Generic;
using UnityEngine;

public class BoardComponentGenerator : MonoBehaviour {
    public BoardGenerator boardGenerator;
    public int snakeCount = 3;
    public int ladderCount = 3;

    private Dictionary<int, int> snakes = new Dictionary<int, int>();
    private Dictionary<int, int> ladders = new Dictionary<int, int>();

    void Start() {
        GenerateSnakesAndLadders();
    }

    void GenerateSnakesAndLadders() {
        for (int i = 0; i < snakeCount; i++) {
            int start = Random.Range(15, 99); // Avoid very low tiles
            int end = Random.Range(1, start - 1); // Must go down
            if (!snakes.ContainsKey(start)) {
                snakes[start] = end;
                Debug.Log($"Snake from {start} to {end}");
            }
        }

        for (int i = 0; i < ladderCount; i++) {
            int start = Random.Range(1, 80); // Avoid high tiles
            int end = Random.Range(start + 1, 100); // Must go up
            if (!ladders.ContainsKey(start)) {
                ladders[start] = end;
                Debug.Log($"Ladder from {start} to {end}");
            }
        }
    }

    public int CheckSnakeOrLadder(int tileNumber) {
        if (snakes.ContainsKey(tileNumber)) return snakes[tileNumber];
        if (ladders.ContainsKey(tileNumber)) return ladders[tileNumber];
        return tileNumber;
    }
}
