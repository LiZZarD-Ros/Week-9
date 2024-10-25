using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static reference

    public GameObject backgroundPanel; // grey background 
    public GameObject victoryPanel;
    public GameObject losePanel;

    public int goal; // the amount of points you need to get to to win.
    public int moves; // the number of turns you can take
    public int points; // the current points you have earned.

    public bool isGameEnded;

    public TMP_Text pointsTxt;
    public TMP_Text movesTxt;
    public TMP_Text goalTxt;
    public TMP_Text warningTxt;

    public int coinScore;

    public int numberOfScene = 5;


    private void Awake()
    {
        Instance = this;
        
    }


    public void Initialize(int _moves, int _goal)
    {
        moves = _moves;
        goal = _goal;
    }

    // Update is called once per frame
    void Update()
    {
        pointsTxt.text = "Points: " + points.ToString();
        movesTxt.text = "Moves: " + moves.ToString();
        goalTxt.text = "Goal: " + goal.ToString();
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {
        points += _pointsToGain;
        if (_subtractMoves)
            moves--;

        if (points >= goal)
        {
            //you've won the game
            isGameEnded = true;
            //Display a victory screen
            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            HoldPotionBoard.Instance.potionParent.SetActive(false);
            SaveCoinsToPlayerPrefs();
            return;
        }
        if (moves == 0)
        {
            //lose the game
            isGameEnded = true;
            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            HoldPotionBoard.Instance.potionParent.SetActive(false);
            SaveCoinsToPlayerPrefs();
            return;
        }
    }

    //attached to a button to change scene when winning
    public void WinGame()
    {
        NextLevel();
    }

    //attached to a button to change scene when losing
    public void LoseGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method to buy additional moves
    public void BuyMoves()
    {
        if (HoldPotionBoard.Instance.coins >= 10)
        {
            HoldPotionBoard.Instance.coins -= 10;
            moves += 5;
        }
        else
        {
            StartCoroutine(ShowWarning());
        }
    }

    void SaveCoinsToPlayerPrefs()
    {
        PlayerPrefs.SetInt("SavedCoins", HoldPotionBoard.Instance.coins);
        PlayerPrefs.Save();
        Debug.Log("Coins saved: " + HoldPotionBoard.Instance.coins);
    }

    public void Clearcache()
    {
        PlayerPrefs.DeleteKey("SavedCoins");
        PlayerPrefs.Save();
        Debug.Log("SavedCoins key cleared from PlayerPrefs.");
    }

    private IEnumerator ShowWarning()
    {
        warningTxt.gameObject.SetActive(true);  // Activate the text
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds
        warningTxt.gameObject.SetActive(false); // Deactivate the text
    }

    private void NextLevel()

    {
        if (SceneManager.GetActiveScene().buildIndex == numberOfScene)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }


    }
}
