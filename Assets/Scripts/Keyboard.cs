using System;
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


    private char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', ',', '?', '!'};
    private string[] words = { "SPA", "RUB", "END" };

    private RectTransform container;

    [SerializeField] private RectTransform prefab;

    private List<RectTransform> keys = new();

    private Vector2Int cursorPosition;
    private RectTransform[][] grid;

    private string name = String.Empty;

    public TMP_Text displayText;

    [SerializeField] private float moveCooldown;

    private float currentCooldown;

    enum NameSelected
    {
        Missing,
        Selected
    }

    enum CharacterControl
    {
        None,
        Robot,
        Zeus
    }

    private NameSelected nameSelected = NameSelected.Missing;
    [SerializeField] private CharacterControl characterControl;

    private void Start()
    {
        container = GetComponent<RectTransform>();
        CreateGrid();

        cursorPosition = new Vector2Int(0, 0);
    }

    private void Update()
    {
        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }

        if(characterControl == CharacterControl.Robot)
        {
            float rawX = InputManager.inst.GetRobotMovement().x;
            float rawY = InputManager.inst.GetRobotMovement().y;

            int x = 0;
            int y = 0;

            if (Mathf.Abs(rawX) > 0.1f)
            {
                x = rawX > 0 ? 1 : -1;
            }

            if (Mathf.Abs(rawY) > 0.1f)
            {
                y = rawY > 0 ? 1 : -1;
            }

            MoveCursor(x, -y);

            currentCooldown = moveCooldown;

        }

        if(characterControl == CharacterControl.Zeus)
        {
            Vector2 noramlizedMovement = InputManager.inst.GetZeusMovement().normalized;

            float rawX = noramlizedMovement.x;
            float rawY = noramlizedMovement.y;

            int x = 0;
            int y = 0;

            if (Mathf.Abs(rawX) > 0.5f)
            {
                x = rawX > 0 ? 1 : -1;
            }

            if (Mathf.Abs(rawY) > 0.5f)
            {
                y = rawY > 0 ? 1 : -1;
            }

            MoveCursor(x, -y);

            currentCooldown = moveCooldown;
        }
    }

    private void CreateGrid()
    {
        float totalWidth = container.rect.width;
        float totalHeight = container.rect.height;

        int i = 0;

        grid = new RectTransform[rows][];

        for (int r = 0; r < rows; r++)
        {

            int thisRowCols = (r == rows - 1) ? lastRowCols : cols;

            grid[r] = new RectTransform[thisRowCols];

            float cellWidth = totalWidth / thisRowCols;
            float cellHeight = totalHeight / rows;

            for (int c = 0; c < thisRowCols; c++)
            {

                RectTransform key = Instantiate(prefab, container);
                keys.Add(key);

                grid[r][c] = key;

                float x = (c * cellWidth + cellWidth * 0.5f) - (container.rect.width / 2);
                float y = -(r * cellHeight + cellHeight * 0.5f) + (container.rect.height / 2);


                if(thisRowCols < rows)
                {
                    key.sizeDelta = new Vector2(cellWidth - 10, cellHeight - 10);
                }
                else
                {
                    key.sizeDelta = new Vector2(cellWidth - 10, cellWidth - 10);
                }
                if (i < letters.Length)
                {
                    key.GetChild(0).GetComponent<TMP_Text>().text = letters[i].ToString();
                }
                else {
                    key.GetChild(0).GetComponent<TMP_Text>().text = words[i - letters.Length];

                }

                key.anchoredPosition = new Vector2(x, y);

                i++;

            }
        }
    }

    private void MoveCursor(int col, int row)
    {

        int newX;
        int newY;


        if (cursorPosition.y == rows - 1)
        {
            Debug.Log("Hello");
            if (cursorPosition.x + col > 2)
            {
                newX = 0;
            }
            else if (cursorPosition.x + col < 0)
            {
                newX = 2;
            }
            else
            {

                newX = cursorPosition.x + col;
            }

        }
        else
        {
            if (cursorPosition.x + col > 5)
            {
                newX = 0;
            }
            else if (cursorPosition.x + col < 0)
            {
                newX = 5;
            }
            else
            {

                newX = cursorPosition.x + col;
            }
        }

        if (cursorPosition.y + row > 5)
        {
            newY = 0;
        }
        else if (cursorPosition.y + row < 0)
        {
            newY = 5;
        }
        else
        {
            newY = cursorPosition.y + row;
        }

        if(cursorPosition.y == rows - 1 && newY == 0)
        {
            newX = Mathf.FloorToInt(newX * 2); 
        }

        if (cursorPosition.y == 0 && newY == 5)
        {
            newX = Mathf.CeilToInt(newX / 2);
        }

        if (cursorPosition.y == rows - 2 && newY == rows - 1)
        {
            newX = Mathf.CeilToInt(newX / 2);
        }

        if (cursorPosition.y == rows - 1 && newY == rows - 2)
        {
            newX = Mathf.FloorToInt(newX * 2);
        }


        grid[cursorPosition.y][cursorPosition.x].GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        cursorPosition = new Vector2Int(newX, newY);
        grid[cursorPosition.y][cursorPosition.x].GetChild(0).GetComponent<TMP_Text>().color = Color.red;

        //Debug.Log(cursorPosition);
    }

    private void InputKey()
    {
        string text = grid[cursorPosition.y][cursorPosition.x].GetChild(0).GetComponent<TMP_Text>().text;

        if (text == words[1] && text.Length > 0)
        {
            if(nameSelected == NameSelected.Selected)
            {
                DeSelect();
            }

            name = name.Substring(0, name.Length - 1);
        }

        if (nameSelected == NameSelected.Selected)
            return;

        if (name.Length == 3)
        {
            if (text == words[2])
            {
                SelectName();
            }
        }

        if (name.Length < 3)
        {
            if (text.Length > 1)
            {
                if (text == words[0])
                {
                    name += " ";
                }
            }
            else
            {
                name += text[0];
            }
        }
        displayText.text = name;

        Debug.Log(name);
        //Debug.Log(grid[cursorPosition.y, cursorPosition.x].GetChild(0).GetComponent<TMP_Text>().text[0]);
    }

    private void SelectName()
    {
        displayText.color = Color.blue;
        nameSelected = NameSelected.Selected;
    }

    private void DeSelect()
    {
        displayText.color = Color.black;
        nameSelected = NameSelected.Missing;
    }
}
