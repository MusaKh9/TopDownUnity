using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
//Written By Musa Khokhar
public class MainMenu : MonoBehaviour
{
    public string gameScene = "SampleScene"; //changable scene name string
    public float fadeSpeed = 1f; //flaot for how fast the scene changes
    public Image fadeImage; //reference to unitys image

    void Start()
    {
        // Initialize fade image
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    public void PlayGame()
    {
        StartCoroutine(FadeAndLoad()); //uses function to fade and load up the scene
    }

    //this fades out the screen then while faded it loads the game scene
    IEnumerator FadeAndLoad()
    {
        // Fade out
        float elapsed = 0f;
        Color color = fadeImage.color; 

        while (elapsed < 1f) //keeps screen faded
        {
            elapsed += Time.deltaTime * fadeSpeed;
            color.a = Mathf.Lerp(0, 1, elapsed);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(gameScene); //loading scene
    }
}