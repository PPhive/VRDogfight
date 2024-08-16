using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    bool gameOver;
    [SerializeField]
    TextMeshPro myText;
    [SerializeField]
    float timerToTitle;
    Vector2 finalScore;

    void Start()
    {
        gameOver = false;
        timerToTitle = 10;
    }

    void Update()
    {
        if (!gameOver)
        {
            if (Mathf.Max(GameManager.instance.CurrentGame.teams[1].score, GameManager.instance.CurrentGame.teams[2].score) >= 15) 
            {
                finalScore = new Vector2(GameManager.instance.CurrentGame.teams[1].score, GameManager.instance.CurrentGame.teams[2].score);
                gameOver = true;
            }
            else
            {
                myText.fontSize = 36;
                myText.text =
                "Best of " + 15 + "\n" +
                "BlueTeam   VS   RedTeam\n" +
                GameManager.instance.CurrentGame.teams[1].score + "             " + GameManager.instance.CurrentGame.teams[2].score + "\n";
            }
        }
        else 
        {
            if (timerToTitle > 0)
            {
                if (GameManager.instance.CurrentGame.teams[1].score > GameManager.instance.CurrentGame.teams[2].score)
                {
                    myText.text = "<size=200%>\nVICTORY\n<size=100%>Blue Won\n";
                }
                else
                {
                    myText.text = "<size=200%>\nDEFEAT\n<size=100%>Red Won\n";
                }
                myText.text +=
                                "BlueTeam   VS   RedTeam\n" +
                                finalScore.x + "          " + finalScore.y + "\n" +
                                "Back to title in " + Mathf.RoundToInt(timerToTitle);
                timerToTitle -= Time.deltaTime;
            }
            else 
            {
                GameManager.instance.backToTitle();
            }
        }
    }
}
