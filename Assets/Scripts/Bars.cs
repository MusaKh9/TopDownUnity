using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bars : MonoBehaviour
{
    // Existing stats and decay rates
    public float hunger = 100f;
    public float hyenine = 100f;  // Using existing hygiene stat
    public float boredom = 100f;
    public float tiredness = 100f;
    public float hungerDecayRate = 1f;
    public float hyenineDecayRate = 0.5f;
    public float boredomDecayRate = 0.2f;
    public float tirednessDecayRate = 0.1f;

    // UI Sliders
    public Slider hungerSlider;
    public Slider hyenineSlider;
    public Slider boredomSlider;
    public Slider tirednessSlider;

    // Interaction points
    public Transform kitchenCounter;
    public Transform bed;
    public Transform shower;
    public float interactionDistance = 3f;
    public CookingMiniGame cookingMiniGame;

    // Fade effect
    public Image fadeImage;
    public float fadeSpeed = 1f;

    void Update()
    {
        UpdateNeeds();

        if (IsNear(kitchenCounter) && Input.GetKeyDown(KeyCode.E))
        {
            cookingMiniGame.StartMiniGame();
        }

        if (IsNear(bed) && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeAndResetTiredness());
        }

        // Simple shower interaction added here
        if (IsNear(shower) && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeAndResetHygiene());
        }

        if (Input.GetKeyDown(KeyCode.Escape) && cookingMiniGame.IsMiniGameActive())
        {
            cookingMiniGame.EndMiniGame();
        }
    }

    IEnumerator FadeAndResetTiredness()
    {
        yield return StartCoroutine(FadeScreen());
        tiredness = 100f;
        tirednessSlider.value = 100f;
    }

    IEnumerator FadeAndResetHygiene()
    {
        yield return StartCoroutine(FadeScreen());
        hyenine = 100f;
        hyenineSlider.value = 100f;
    }

    IEnumerator FadeScreen()
    {
        while (fadeImage.color.a < 1)
        {
            fadeImage.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        while (fadeImage.color.a > 0)
        {
            fadeImage.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
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