using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    public Text diceText;
    private int diceValue;

    public void RollDice()
    {
        diceValue = Random.Range(1, 7);
        diceText.text = diceValue.ToString();
        MovePlayer(diceValue);
    }

    void MovePlayer(int steps)
    {

    }
}
