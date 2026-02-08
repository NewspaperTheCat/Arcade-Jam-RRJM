using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private int lastRowCols;


    private char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', ',', '?', '!' };

    private RectTransform container;

    [SerializeField] private RectTransform prefab;

    private List<RectTransform> keys = new();

    private Vector2 cursorPosition;
    private RectTransform[,] grid;

    private void Start()
    {
        container = GetComponent<RectTransform>();
        CreateGrid();

        cursorPosition = new Vector2(0, 0);
    }

    private void CreateGrid()
    {
        float totalWidth = container.rect.width;
        float totalHeight = container.rect.height;

        int i = 0;

        for (int r = 0; r < rows; r++)
        {
            float cellWidth = totalWidth / cols;
            float cellHeight = totalHeight / rows;

            for (int c = 0; c < cols; c++)
            {
                RectTransform key = Instantiate(prefab, container);
                keys.Add(key);

                grid[r, c] = key;

                float x = (c * cellWidth + cellWidth * 0.5f) - (container.rect.width / 2);
                float y = -(r * cellHeight + cellHeight * 0.5f) + (container.rect.height / 2);

                key.sizeDelta = new Vector2(cellWidth - 10, cellWidth - 10);

                key.GetChild(0).GetComponent<TMP_Text>().text = letters[i].ToString();

                key.anchoredPosition = new Vector2(x, y);

                i++;

            }
        }
    }

    private RectTransform MoveCursor (int row, int col)
    {
        if (row < 0 || col < 0 || row >= rows || col >= cols)
        {
            Debug.LogWarning("Get Cell out of range!");
            return null;
        }

        return grid[row, col];
    }
}
