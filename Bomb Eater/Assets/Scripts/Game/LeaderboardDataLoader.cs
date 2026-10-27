using System.Collections;
using Dan.Main;
using Dan.Models;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fetches and posts leaderboard entries; the leaderboard panel listens to <see cref="OnLeaderBoardUpdated"/>.
/// </summary>
public class LeaderboardDataLoader : MonoBehaviour
{
    [Tooltip("Seconds to wait for the leaderboard service to initialise before the first fetch.")]
    [SerializeField] float initialFetchDelay = 4f;

    public UnityEvent<Entry[]> OnLeaderBoardUpdated;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(initialFetchDelay);
        GetLeaderBoardData();
    }

    public void GetLeaderBoardData()
    {
        Leaderboards.detotest.GetEntries(
            entries => OnLeaderBoardUpdated?.Invoke(entries),
            error =>
            {
                Debug.LogError($"Failed to get leaderboard entries: {error}");
                OnLeaderBoardUpdated?.Invoke(null);
            });
    }

    public void PostScore(string username, int score)
    {
        Leaderboards.detotest.UploadNewEntry(username, score, _ => GetLeaderBoardData());
    }
}
