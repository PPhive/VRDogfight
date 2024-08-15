using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    bool gameOver;
    [SerializeField]
    TextMeshPro myText;

    void Start()
    {
        gameOver = false;
    }

    void Update()
    {
        if (!gameOver) 
        {
            if (GameManager.instance.CurrentGame.teams[1].score >= 10)
            {
                myText.fontSize = 40;
                myText.text = "VICTORY\n";
                myText.text = "Blue Won";
                gameOver = true;
            }
            else if (GameManager.instance.CurrentGame.teams[2].score >= 10)
            {
                myText.fontSize = 40;
                myText.text = "DEFEAT\n";
                myText.text = "Red Won";
                gameOver = true;
            }
            else
            {
                myText.fontSize = 36;
                myText.text =
                "Best of 10\n" +
                "BlueTeam   VS   RedTeam\n" +
                GameManager.instance.CurrentGame.teams[1].score + "          " + GameManager.instance.CurrentGame.teams[2].score + "\n";
            }
        }
    }
}
