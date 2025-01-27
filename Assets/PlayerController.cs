using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    const string IDLE = "Idle";
    const string WALK = "Walk";
       

    CustomAction input;
    NavMeshAgent agent;
    Animator ani;

    [Header("Movement")]
    [SerializeField] ParticleSystem click;
    [SerializeField] LayerMask clicklayer;

    float lookrotationspeed = 8f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        input = new CustomAction();
        AssginInputs();
    }

    void AssginInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
    }

    void ClickToMove()
    {
        RaycastHit hit;
       if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clicklayer))
        {
            agent.destination = hit.point;
            if(click != null)
            {
                Instantiate(click, hit.point += new Vector3(0, 0.1f, 0), click.transform.rotation);
            }
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable ()
    {
        input.Disable();
    }

    private void Update()
    {
        FaceTarget();
        SetAnimations();
    }

    void FaceTarget()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * lookrotationspeed);
    }

    void SetAnimations()
    {
        if(agent.velocity == Vector3.zero)
        {
            ani.Play(IDLE);
        }
        else
        {
            ani.Play(WALK);
        }
    }
    
}
