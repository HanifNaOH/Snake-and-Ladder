using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player[] players;
    private int currentPlayer = 0;

    public void EndTurn()
    {
        currentPlayer = (currentPlayer + 1) % players.Length;
        Debug.Log("Player " + (currentPlayer + 1) + "'s Turn");
    }
}
