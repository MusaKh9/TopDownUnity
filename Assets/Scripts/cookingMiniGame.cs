using UnityEngine;
//Written By Musa Khokhar
public class cookingMiniGame : MonoBehaviour
{
    public Camera mainCamera; //reference to kitchens camera
    public Camera miniGameCamera;//reference to the camera for the mini game

    public GameObject[] foodItems; // array for food objects
    public Transform pot; //the pot's transform
    public float potDropDistance = 1f; //distance from how far needed to drop food into the pot

    public Bars bars; //reference to bars script

    private GameObject selectedFood; //reference to the food object 
    private bool isMiniGameActive;//true or false if mini game is active 

    AudioSource audioSource; //reference to unitys audio source
    [SerializeField] AudioClip clickSound; //allows you put clip into inspector

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //grabs audio source componet 
    }

    //plays sound if it can
    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    void Update()
    {
        //exits method if mini game is not active
        if (!isMiniGameActive) return;

        // Food dragging logic
        HandleFoodDrag();
    }

    void HandleFoodDrag()
    {
        //triggers food grab when right click 
        if (Input.GetMouseButtonDown(1))
        {
            TrySelectFood();
        }

        //moves the food and releases if it is close enough to pot
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
                    PlayClickSound();
                    bars.IncreaseHunger(20f);
                    Destroy(selectedFood);
                }
                selectedFood = null;
            }
        }
    }

    //function to select food when clicked on with mouse
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

    //activates the mini game and switches the mini game camera
    public void StartMiniGame()
    {
        isMiniGameActive = true;
        mainCamera.enabled = false;
        miniGameCamera.enabled = true;
    }

    //deactivates mini game and switches back to main cam 
    public void EndMiniGame()
    {
        isMiniGameActive = false;
        mainCamera.enabled = true;
        miniGameCamera.enabled = false;
    }

    //checks if mini game is active
    public bool IsMiniGameActive() => isMiniGameActive;
}