using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private float pointMultiplier;

    [HideInInspector] public int points;

    private float elapsedTime;
    private bool gameOver;

    private const int MaxScores = 5;

    private HighScoreEntry[] highScores = new HighScoreEntry[MaxScores];

    private const string EMPTYNAME = "ABC/ABC";
    private const int EMPTYSCORE = 0;

    [Header("Debug")]
    [SerializeField] private bool resetHighscores;


    [HideInInspector] public Transform homeStation;
    private void Awake()
    {
        //Make this an Instance
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        SetUpGame();


        if(resetHighscores) PlayerPrefs.DeleteAll();

    }

    private void Update()
    {
        if (!gameOver) elapsedTime += Time.deltaTime;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        SetUpGame();
    }

    public void SetGameOver(bool go = true)
    {
        gameOver = go;

        if (gameOver)
        {
            timeAlivePoints();
        }
    }

    public bool getGameOver()
    {
        return gameOver;
    }

    public void AddPoints(int amount)
    {
        points += amount;
        //Debug.Log(points);
    }

    private int newHighscoreSpot()
    {
        for (int i = 0; i < highScores.Length; i++) {


            Debug.Log($"{points}");
            if (points > highScores[i].points)
            {
                return i;
            }
        }

        return -1;
    }

    private void timeAlivePoints()
    {
        int amount = Mathf.CeilToInt(elapsedTime * pointMultiplier);
        points += amount;


        if (newHighscoreSpot() != -1) {
            InitializeIfNeeded();
            LoadHighScores();
            SceneManager.LoadScene("EnterHighscore");
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }

    }

    public void AddNewHighscore(string teamName)
    {
        for (int i = 0; i < MaxScores; i++)
        {
            if (points > highScores[i].points)
            {
                // Shift down
                for (int j = MaxScores - 1; j > i; j--)
                {
                    highScores[j] = highScores[j - 1];
                }

                highScores[i] = new HighScoreEntry(teamName, points);
                SaveHighScores();
                return;
            }
        }
    }

    private void InitializeIfNeeded()
    {
        if (!PlayerPrefs.HasKey("HighScore_Points_0"))
        {
            for (int i = 0; i < MaxScores; i++)
            {
                PlayerPrefs.SetString($"HighScore_Name_{i}", EMPTYNAME);
                PlayerPrefs.SetInt($"HighScore_Points_{i}", EMPTYSCORE);
            }

            PlayerPrefs.Save();
        }
    }

    private void LoadHighScores()
    {
        for (int i = 0; i < MaxScores; i++)
        {
            string nameKey = $"HighScore_Name_{i}";
            string scoreKey = $"HighScore_Points_{i}";

            string name = PlayerPrefs.GetString(nameKey, EMPTYNAME);
            int points = PlayerPrefs.GetInt(scoreKey, EMPTYSCORE);

            highScores[i] = new HighScoreEntry(name, points);
        }
    }

    private void SaveHighScores()
    {
        for (int i = 0; i < MaxScores; i++)
        {
            PlayerPrefs.SetString($"HighScore_Name_{i}", highScores[i].name);
            PlayerPrefs.SetInt($"HighScore_Points_{i}", highScores[i].points);
        }

        PlayerPrefs.Save();
    }

    private void SetUpGame()
    {
        elapsedTime = 0;
        points = 0;
        gameOver = false;
    }

    public HighScoreEntry GetHighScore(int index)
    {
        if (index < 0 || index >= MaxScores)
        {
            Debug.LogWarning($"HighScore index {index} out of range");
            return default;
        }

        return highScores[index];
    }
}
