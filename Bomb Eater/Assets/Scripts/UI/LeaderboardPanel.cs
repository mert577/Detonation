using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine.Events;
public class LeaderboardPanel : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerCard playerCardPrefab;

    public RectTransform playerCardContainer;

    public TMP_InputField usernameInputField;


    public UnityEvent<string, int> OnSubmitLeaderBoardEntry;


    public TextMeshProUGUI scoreText;


    public GameObject errorPanel;




     private void OnEnable() {
       scoreText.text = "Score: " + GameManager.instance.score.ToString();
    }

    public void UpdateLeaderBoardVisuals(Entry[] leaderboardData){


        if(leaderboardData == null){


            errorPanel.SetActive(true);
            return;
        }
        
        foreach (Transform child in playerCardContainer)
        {
            Destroy(child.gameObject);
        }

        //sort the leaderboard data by score descending
        System.Array.Sort(leaderboardData, (x, y) => y.Score.CompareTo(x.Score));

        for(int i=0;i<leaderboardData.Length;i++){
            PlayerCard playerCard = Instantiate(playerCardPrefab, playerCardContainer);
            playerCard.playerNameText.text = leaderboardData[i].RankSuffix() +" " + leaderboardData[i].Username;
            playerCard.playerScoreText.text = leaderboardData[i].Score.ToString();
        }

    }


    public void SubmitLeaderBoardEntry(){
        string username = usernameInputField.text;
        int score = GameManager.instance.score;
        OnSubmitLeaderBoardEntry.Invoke(username, score);


    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
