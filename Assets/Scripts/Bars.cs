using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
//Written By Musa Khokhar
public class Bars : MonoBehaviour
{
    //creates string for scenes
    public string mainMenuScene = "MainMenu";
    public string miniGameSceneName = "AsteroidMiniGame";

    //creates float values each bar
    public float hunger = 100f;
    public float hyenine = 100f;
    public float boredom = 100f;
    public float tiredness = 100f;

    //all values for how fast the bars go down 
    public float hungerDecayRate = 1f;
    public float hyenineDecayRate = 0.5f;
    public float boredomDecayRate = 0.2f;
    public float tirednessDecayRate = 0.1f;

    //refrences the slider for each need
    public Slider hungerSlider;
    public Slider hyenineSlider;
    public Slider boredomSlider;
    public Slider tirednessSlider;

    //object positions for refilling needs
    public Transform kitchenCounter;
    public Transform bed;
    public Transform shower;
    public Transform pc;

    
    public float interactionDistance = 3f; // how far needed for interaction with object

    public cookingMiniGame cookingMiniGame; //reference to cooking mini script

    public Image fadeImage; //reference to unitys canvas image
    public float fadeSpeed = 1f; //float for how fast screen fades

    private bool isGameOver; //true or false for is game over

    AudioSource audioSource; //reference to unitys audio source
    [SerializeField] AudioClip clickSound;//allows to put audio clip into hierarchy

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //gets unity audio source
        CheckMiniGameResult(); //uses the check mini game function at beginning of scene
    }

    void Update()
    {
        if (isGameOver) return; //exiting the method if game over

        UpdateNeeds(); //uses the update needs function
        CheckNeedsFailure(); //uses if the needs failed funtion 

        // Interactions if key is down it starts mini games or fades the screen
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsNear(kitchenCounter))
            {
                PlayClickSound();
                cookingMiniGame.StartMiniGame();
            }
            else if (IsNear(bed))
            {
                PlayClickSound();
                StartCoroutine(FadeAndResetTiredness());
            }
            else if (IsNear(shower))
            {
                PlayClickSound();
                StartCoroutine(FadeAndResetHygiene());
            }
            else if (IsNear(pc))
            {
                PlayClickSound();
                StartCoroutine(StartBoredomMiniGame());
            }
        }
        //if esc key is clicked it ends mini game
        if (Input.GetKeyDown(KeyCode.Escape) && cookingMiniGame.IsMiniGameActive())
        {
            cookingMiniGame.EndMiniGame();
        }
    }

    //fucntion for if click sound needs to be played
    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    //checks if the bars has hit 0 and if it has it set game over to true and returns to main menu 
    void CheckNeedsFailure()
    {
        if (hunger <= 0f || hyenine <= 0f || boredom <= 0f || tiredness <= 0f)
        {
            isGameOver = true;
            StartCoroutine(ReturnToMainMenu());
        }
    }

    //function fade the screen and return to main menu
    IEnumerator ReturnToMainMenu()
    {
        yield return StartCoroutine(FadeScreen(0, 1));
        SceneManager.LoadScene(mainMenuScene);
    }

    //this saves the boredom then fades the screen and loads up the mini game scene
    IEnumerator StartBoredomMiniGame()
    {
        PlayerPrefs.SetFloat("CurrentBoredom", boredom);
        yield return StartCoroutine(FadeScreen(0, 1));
        SceneManager.LoadScene(miniGameSceneName);
    }

    //checks the results of mini game and if sucessful it brings you back to scene with full bars
    void CheckMiniGameResult()
    {
        if (PlayerPrefs.GetInt("MiniGameSuccess") == 1)
        {
            boredom = 100f;
            boredomSlider.value = 100f;
            PlayerPrefs.DeleteKey("MiniGameSuccess");
        }
    }

    //function to fade the screen it starts with an alpha of 0 and ends with 100 to create the black screen
    IEnumerator FadeScreen(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed);
            fadeImage.color = color;
            yield return null;
        }
    }

    //to fade the screen and reset the bar for tiredness 
    IEnumerator FadeAndResetTiredness()
    {
        yield return StartCoroutine(FadeScreen(0, 1));
        tiredness = 100f;
        tirednessSlider.value = 100f;
        yield return StartCoroutine(FadeScreen(1, 0));
    }

    //to fade the screen and reset the bar for Hygiene
    IEnumerator FadeAndResetHygiene()
    {
        yield return StartCoroutine(FadeScreen(0, 1));
        hyenine = 100f;
        hyenineSlider.value = 100f;
        yield return StartCoroutine(FadeScreen(1, 0));
    }

    //this decreases the bars over time and updates sliders
    void UpdateNeeds()
    {
        hunger = Mathf.Clamp(hunger - hungerDecayRate * Time.deltaTime, 0f, 100f);
        hyenine = Mathf.Clamp(hyenine - hyenineDecayRate * Time.deltaTime, 0f, 100f);
        boredom = Mathf.Clamp(boredom - boredomDecayRate * Time.deltaTime, 0f, 100f);
        tiredness = Mathf.Clamp(tiredness - tirednessDecayRate * Time.deltaTime, 0f, 100f);

        hungerSlider.value = hunger;
        hyenineSlider.value = hyenine;
        boredomSlider.value = boredom;
        tirednessSlider.value = tiredness;
    }

    //checks if the character is close to the target
    bool IsNear(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= interactionDistance;
    }

    //increases hunger if its in between 0 and 100
    public void IncreaseHunger(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0f, 100f);
    }
}
