using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public int index;
    public string name;
    public List<Player> myPlayers;
    public int score;
}

public class GameManager : MonoBehaviour
{

    [System.Serializable]
    public class Game 
    {
        public GameMode myGameMode;
        public List<Team> teams;
        public float MaxTimeMin;
        public float TimerMin;
        public float TimerSec;
    }

    public GameObject CheckMyUnit(GameObject Checking)
    {
        if (Checking.GetComponent<Unit>() != null)
        {
            return Checking.gameObject;
        }
        else if (Checking.transform.parent != null)
        {
            return CheckMyUnit(CheckMyUnit(Checking.transform.parent.gameObject));
        }
        return null;
    }

    [System.Serializable]
    public enum GameMode 
    {
        Defense,
        TeamDeathMatch
    }

    [SerializeField]
    Game DefaultGame;

    [SerializeField]
    public Game CurrentGame;
    public static GameManager instance;
    int Seed;

    public List<Player>Players;
    public float WorldRadius = 3000;
    public float WorldHeight = 20;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else 
        {
            instance = this;
        }
    }

    void Initialize(Game ThisGame) 
    {
        Seed = GenerateSeed();
        CurrentGame = ThisGame;
        for (int i = 0; i < CurrentGame.teams.Count; i++)
        {
            Team thisTeam = CurrentGame.teams[i];
            thisTeam.index = i;
            foreach (Player thisPlayer in thisTeam.myPlayers) 
            {
                thisPlayer.myTeam = thisTeam;
                thisPlayer.tag = thisTeam.name;
            }
        }
    }

    void Start()
    {
        Initialize(DefaultGame);

        //The following game initialization is temporary;
        
    }

    void Update()
    {
        
    }

    private int GenerateSeed() 
    {
        int Seed = Random.Range(10000,99999);
        return Seed;
    }

    public float SeededRandomRange(float min, float max, float x, float y) 
    {
        return Mathf.Lerp(min,max, Mathf.PerlinNoise(x * Seed, y * Seed));
    }

    public void UnitDestroyed(Player Destroyer, Unit Destroyed) 
    {
        Debug.Log(Destroyer + " destroyed " + Destroyed!);
        if (Destroyer.myTeam == CurrentGame.teams[0])
        {
            Debug.Log(Destroyed + " got destroyed by neutral unit!");
        }
        else 
        {
            Scoring(1, Destroyer);
        }
    }

    public void Scoring(int Score, Player ScoringPlayer) 
    {
        foreach (Team thisTeam in CurrentGame.teams) 
        {
            if (thisTeam.myPlayers.Contains(ScoringPlayer)) 
            {
                thisTeam.score += Score;
            }
        }
    }
}
