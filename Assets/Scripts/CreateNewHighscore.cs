using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateNewHighscore : MonoBehaviour
{
    [SerializeField] Keyboard[] keyboards;

    private bool bothSelected;

    private void Start()
    {
        bothSelected = false;
    }


    private void Update()
    {
        if (keyboards[0].nameSelected == Keyboard.NameSelected.Selected && keyboards[1].nameSelected == Keyboard.NameSelected.Selected && !bothSelected)
        {
            bothSelected = true;
            string teamName = keyboards[0].GetComponent<Keyboard>().highscoreName + "/" + keyboards[1].GetComponent<Keyboard>().highscoreName;
            GameManager.Instance.AddNewHighscore(teamName);
            SceneManager.LoadScene("GameOver");
        }
    }
}
