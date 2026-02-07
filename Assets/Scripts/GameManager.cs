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

        elapsedTime = 0;
        gameOver = false;

    }

    private void Update()
    {
        if (gameOver)
        {
            timeAlivePoints();
            SceneManager.LoadScene("GameOver");
        }

        elapsedTime += Time.deltaTime;
    }

    public void SetGameOver(bool go = true)
    {
        gameOver = go;
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

    private void timeAlivePoints()
    {
        int amount = Mathf.CeilToInt(points * pointMultiplier);
        points += amount;
        Debug.Log(points);
    }
}
