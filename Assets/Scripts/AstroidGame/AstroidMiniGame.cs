using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidMiniGame : MonoBehaviour
{
    public static AsteroidMiniGame Instance;
    public int score;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        score += points;
    }

    public void ReturnToMainGame()
    {
        PlayerPrefs.SetInt("BoredomReset", 1);
        SceneManager.LoadScene("MainScene");
    }
}
