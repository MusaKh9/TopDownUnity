using UnityEngine;

public class CookingMiniGame : MonoBehaviour
{
    public Camera mainCamera;
    public Camera miniGameCamera;

    public GameObject[] foodItems; // Assign food prefabs/objects
    public Transform pot; // Assign the pot's transform
    public float potDropDistance = 1f;

    public Bars bars;

    private GameObject selectedFood;
    private bool isMiniGameActive;

    void Update()
    {
        if (!isMiniGameActive) return;

        // Food dragging logic
        HandleFoodDrag();
    }

    void HandleFoodDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TrySelectFood();
        }

        if (selectedFood != null)
        {
            // Move food with mouse
            Ray ray = miniGameCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                selectedFood.transform.position = hit.point;
            }

            // Release food
            if (Input.GetMouseButtonUp(1))
            {
                if (Vector3.Distance(selectedFood.transform.position, pot.position) <= potDropDistance)
                {
                    bars.IncreaseHunger(20f);
                    Destroy(selectedFood);
                }
                selectedFood = null;
            }
        }
    }

    void TrySelectFood()
    {
        Ray ray = miniGameCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            foreach (var food in foodItems)
            {
                if (hit.collider.gameObject == food)
                {
                    selectedFood = food;
                    break;
                }
            }
        }
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;
        mainCamera.enabled = false;
        miniGameCamera.enabled = true;
    }

    public void EndMiniGame()
    {
        isMiniGameActive = false;
        mainCamera.enabled = true;
        miniGameCamera.enabled = false;
    }

    public bool IsMiniGameActive() => isMiniGameActive;
}