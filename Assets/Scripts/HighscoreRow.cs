using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreRow : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetRow(int rank, HighScoreEntry entry)
    {
        rankText.text = rank.ToString();
        nameText.text = entry.name;
        scoreText.text = entry.points.ToString();
        
    }
}