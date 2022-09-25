using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{

    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        int score = PlayerPrefs.GetInt("SCORE", 0); 
        scoreText.text=""+score;
    }


}
