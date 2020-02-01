using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI happyCountText;

    [SerializeField] int numberOfHappyToWin;

    [ShowInInspector]
    private int currentNumberOfHappy = 0;

    [ReadOnly]
    public List<Personne> personnes = new List<Personne>();

    public UnityEvent onWin;

    [ReadOnly]
    public bool gameEnded = false;

    public void UpdateWinCondition()
    {
        currentNumberOfHappy = 0;

        if (personnes.Count <= 0) return;

        foreach(Personne personne in personnes)
        {
            if (personne.isHappy) currentNumberOfHappy++;
        }

        StringBuilder builder = new StringBuilder();

        happyCountText.text = builder.Append(currentNumberOfHappy).Append("/").Append(happyCountText).ToString();

        if (currentNumberOfHappy >= numberOfHappyToWin) Win();
    }

    private void Win()
    {
        onWin?.Invoke();
        gameEnded = true;
    }
}
