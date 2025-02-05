using UnityEngine;
using UnityEngine.SceneManagement;
//Written By Musa Khokhar
public class NodeGame : MonoBehaviour
{
    public string targetScene = "SampleScene"; // string for 
    public float winDistance = 1f; // How close player needs to be to win

    public Transform player; // The cylinder
    public Transform target; // The target location

    void Update()
    {
        CheckForWin();//constantly uses function to check for win
    }

    void CheckForWin()
    {
        // Check if player is close enough to target
        float distance = Vector3.Distance(player.position, target.position);

        //checks the distance if its small enough to win
        if (distance <= winDistance)
        {
            WinGame();
        }
    }

    //logs the win and loads back to the main scene
    void WinGame()
    {
        Debug.Log("Mini-game completed!");
        PlayerPrefs.SetInt("MiniGameSuccess", 1);
        SceneManager.LoadScene(targetScene);
    }
}