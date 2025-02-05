using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
//followed tutorial (Credit Pogle) but changes was added for sounds and collsions
public class PlayerController : MonoBehaviour
{
    //Defines constant strings for animations
    const string IDLE = "Idle";
    const string WALK = "Walk";

    //trying to access unity componets 
    CustomAction input;
    NavMeshAgent agent;
    Animator ani;
    AudioSource audioSource;

    //allows us to put in the hierarchy for audio and ground check
    [SerializeField] ParticleSystem click;
    [SerializeField] LayerMask clicklayer;
    [SerializeField] AudioClip clickSound;

    //how fast the character turns to walk
    float lookrotationspeed = 8f;

    private void Awake()
    {
        //grabbing unity componets
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //auto generated custom action by unity
        input = new CustomAction();
        AssignInputs();
    }

    //this binds the click to move function
    void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
    }

    //
    void ClickToMove()
    {
        //this checks if there is a click sound and plays it
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        RaycastHit hit; // declaring variable for ray cast hit

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clicklayer)) //casting ray from the mouse position
        {
            //this is trying to ignore collison with doorways
            if (hit.collider.CompareTag("Doorway"))
            {
                Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), hit.collider, true);
            }

            agent.destination = hit.point; //this sets the characters destination

            //creates the click effect at the hit point
            if (click != null)
            {
                Instantiate(click, hit.point + new Vector3(0, 0.1f, 0), click.transform.rotation); 
            }

            //starts coroutine to allow collsion with doorway
            if (hit.collider.CompareTag("Doorway"))
            {
                StartCoroutine(ReenableCollision(hit.collider));
            }
        }
    }

    //waits to renable collisions
    IEnumerator ReenableCollision(Collider doorwayCollider)
    {
        yield return new WaitForSeconds(1f);
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), doorwayCollider, false);
    }

    //enables the input when the player is activated
    private void OnEnable()
    {
        input.Enable();
    }

    //disables the input when the player is not active
    private void OnDisable()
    {
        input.Disable();
    }

    //this updates where the target faces and animation
    private void Update()
    {
        FaceTarget();
        SetAnimations();
    }

    //this function uses vectors to rotate to face the target
    void FaceTarget()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * lookrotationspeed);
    }

    //plays animations
    void SetAnimations()
    {
        if (agent.velocity == Vector3.zero)
        {
            ani.Play(IDLE);
        }
        else
        {
            ani.Play(WALK);
        }
    }
}
