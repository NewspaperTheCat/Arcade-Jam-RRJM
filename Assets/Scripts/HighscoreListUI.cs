using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreListUI : MonoBehaviour
{
    [SerializeField] private Transform listParent;
    [SerializeField] private HighscoreRow rowPrefab;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            HighScoreEntry entry = GameManager.Instance.GetHighScore(i);
            HighscoreRow row = Instantiate(rowPrefab, listParent);

            RectTransform rt = row.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0f, -i * 100);
            row.SetRow(i + 1, entry);
        }
    }
}
