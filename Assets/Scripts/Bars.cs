using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bars : MonoBehaviour
{
    // Add these new variables
    public Transform pc;  // Reference to PC object position
    public string miniGameSceneName = "AsteroidMiniGame";

    // Existing variables remain the same
    public float hunger = 100f;
    public float hyenine = 100f;
    public float boredom = 100f;
    public float tiredness = 100f;

    public float hungerDecayRate = 1f;
    public float hyenineDecayRate = 0.5f;
    public float boredomDecayRate = 0.2f;
    public float tirednessDecayRate = 0.1f;

    public Slider hungerSlider;
    public Slider hyenineSlider;
    public Slider boredomSlider;
    public Slider tirednessSlider;

    public Transform kitchenCounter;
    public Transform bed;
    public Transform shower;
    public float interactionDistance = 3f;
    public CookingMiniGame cookingMiniGame;
    public Image fadeImage;
    public float fadeSpeed = 1f;

    void Start()
    {
        // Check for mini-game result when returning to main scene
        CheckMiniGameResult();
    }

    void Update()
    {
        UpdateNeeds();

        // Existing interactions
        if (IsNear(kitchenCounter) && Input.GetKeyDown(KeyCode.E))
        {
            cookingMiniGame.StartMiniGame();
        }

        if (IsNear(bed) && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeAndResetTiredness());
        }

        if (IsNear(shower) && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeAndResetHygiene());
        }

        // New PC interaction for boredom mini-game
        if (IsNear(pc) && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartBoredomMiniGame());
        }

        if (Input.GetKeyDown(KeyCode.Escape) && cookingMiniGame.IsMiniGameActive())
        {
            cookingMiniGame.EndMiniGame();
        }
    }

    IEnumerator StartBoredomMiniGame()
    {
        // Save current boredom value
        PlayerPrefs.SetFloat("CurrentBoredom", boredom);

        // Fade out
        yield return StartCoroutine(FadeScreen(0, 1));

        // Load mini-game scene
        SceneManager.LoadScene(miniGameSceneName);
    }

    void CheckMiniGameResult()
    {
        if (PlayerPrefs.GetInt("MiniGameSuccess", 0) == 1)
        {
            // Reset boredom if mini-game was won
            boredom = 100f;
            boredomSlider.value = 100f;

            // Reset the flag
            PlayerPrefs.DeleteKey("MiniGameSuccess");
        }
    }

    // Modified to handle fade in/out
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

    // Existing methods remain the same
    IEnumerator FadeAndResetTiredness()
    {
        yield return StartCoroutine(FadeScreen(0, 1));
        tiredness = 100f;
        tirednessSlider.value = 100f;
        yield return StartCoroutine(FadeScreen(1, 0));
    }

    IEnumerator FadeAndResetHygiene()
    {
        yield return StartCoroutine(FadeScreen(0, 1));
        hyenine = 100f;
        hyenineSlider.value = 100f;
        yield return StartCoroutine(FadeScreen(1, 0));
    }

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

    bool IsNear(Transform target)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= interactionDistance;
    }

    public void IncreaseHunger(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0f, 100f);
    }
}