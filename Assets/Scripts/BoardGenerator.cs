using UnityEngine;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour {
    public GameObject tilePrefab;
    public int rows = 10;
    public int cols = 10;
    public float spacing = 1.1f;

    private Dictionary<int, Vector3> boardPositions = new Dictionary<int, Vector3>();

    void Start() {
        GenerateBoard();
    }

    void GenerateBoard() {
        int tileNumber = 1;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                // Calculate tile position
                Vector3 tilePosition = new Vector3(j * spacing, i * spacing, 0);

                // Instantiate tile at position
                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.name = "Tile " + tileNumber;

                // Store tile position in dictionary
                boardPositions[tileNumber] = tilePosition;
                
                tileNumber++;
            }
        }
    }

    public Vector3 GetTilePosition(int tileNumber) {
        return boardPositions.ContainsKey(tileNumber) ? boardPositions[tileNumber] : Vector3.zero;
    }
}
