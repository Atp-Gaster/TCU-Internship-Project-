using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextController : MonoBehaviour
{
    public GameManagerScript GM;
    public GameObject ScorePanal;
    public Text WinnerText;
    public Text ScoreATextResult;
    public Text ScoreBTextResult;

    public Text ScoreAText;
    public Text ScoreBText;

    public Text TurnText;
    public void SetResult()
    {
        ScorePanal.SetActive(true);
        if (GM.PlayerAScore > GM.PlayerBScore) WinnerText.text = "Player 1";
        else if (GM.PlayerAScore < GM.PlayerBScore) WinnerText.text = "Player 2";
        else WinnerText.text = "Draw";

        ScoreATextResult.text = "Player 1 Score:   " + GM.PlayerAScore;
        ScoreBTextResult.text = "Player 2 Score:   " + GM.PlayerBScore;
    }   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GM.GameMode == 2 || GM.GameMode == 1)
        {           
            ScoreAText.text = "Player 1 Score:   " + GM.PlayerAScore;
            ScoreBText.text = "Player 2 Score:   " + GM.PlayerBScore;

            if (GM.GameMode == 2) TurnText.text = "Turn: " + GM.PlayerTurn;
        }        
    }
}
