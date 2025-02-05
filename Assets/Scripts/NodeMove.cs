using UnityEngine;
//Written By Musa Khokhar
public class NodeMove : MonoBehaviour
{
    public float moveSpeed = 5f; // float for how fast the player moves 
    private Rigidbody rb; //reference to unitys rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //grabs the rigidbody componment 
    }

    void Update()
    {
        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move player
        Vector3 move = new Vector3(x, 0, z) * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }
}
