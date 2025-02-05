  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using Dan.Models;
using UnityEngine.Events;
public class LeaderboardDataLoader : MonoBehaviour
{



  
    // Start is called before the first frame update


    public UnityEvent<Entry[]> OnLeaderBoardUpdated;

    Entry[] leaderboardData;


    

    public void GetLeaderBoardData(){
        Leaderboards.detotest.GetEntries((entries) => {
            try {
                leaderboardData = entries;

                foreach(Entry entry in leaderboardData){
                    Debug.Log(entry.Username + " " + entry.Score);
                }
            
                OnLeaderBoardUpdated?.Invoke(leaderboardData);
            }
            catch (System.Exception e) {
                Debug.LogError($"Error loading leaderboard: {e.Message}");
            }
        }, (error) => {
            leaderboardData = null;
            OnLeaderBoardUpdated?.Invoke(leaderboardData);
            Debug.LogError($"Failed to get leaderboard entries: {error}");
        });
    }


    public void PostScore(string username,int score){
       Leaderboards.detotest.UploadNewEntry(username, score, (entry) => {
           GetLeaderBoardData();
       });
    }


    private void Awake() {
        //wait for leaderboard to initlizae then get the data
        StartCoroutine(WaitForLeaderboardInit());
    }

    IEnumerator WaitForLeaderboardInit(){
        yield return new WaitForSeconds(4f);
        GetLeaderBoardData();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
