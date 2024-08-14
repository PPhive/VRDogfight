using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    TextMeshPro myText;

    void Update()
    {
        if (GameManager.instance.CurrentGame.teams[1].score >= 10)
        {
            myText.fontSize = 40;
            myText.text = "VICTORY\n";
            myText.text = "Blue Won";
        }
        else if (GameManager.instance.CurrentGame.teams[2].score >= 10) 
        {
            myText.fontSize = 40;
            myText.text = "DEFEAT\n";
            myText.text = "Red Won";
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
