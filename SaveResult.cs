using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveResult : MonoBehaviour
{
    public GameObject pauseMenuUI, newRecord;
    public TMP_Text scoreText, recordeText;

    public void Lost(int _Score){
        pauseMenuUI.SetActive(true);

        if(PlayerPrefs.GetInt("PlayerScore", 0) < _Score){
            newRecord.SetActive(true);
            PlayerPrefs.SetInt("PlayerScore", _Score);
            PlayerPrefs.Save();
        }

        scoreText.text = "Your score: " + _Score;
        recordeText.text = "Your recorde: " + PlayerPrefs.GetInt("PlayerScore");

        GamePause.isPaused = true;
    }
}
