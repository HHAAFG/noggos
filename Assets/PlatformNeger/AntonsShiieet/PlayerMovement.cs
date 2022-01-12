using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rb;
    Animator anim;
    public float jumpHeight;
    public int maxJumps;
    int currentJumps;
    public int currentHealth;
    public int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
            anim.SetFloat("speed", 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(0, -90, 0);
            anim.SetFloat("speed", 1);
        }
        else
        {
            anim.SetFloat("speed", 0);
        }
        Jump();
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&  currentJumps < maxJumps)
        {
            currentJumps++;
            rb.velocity = new Vector3(rb.position.x, jumpHeight, 0);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            ResetJumps();
        }
        if(other.tag == "Spikes")
        {
            ResetJumps();
            currentHealth--;
        }
    }

    void ResetJumps()
    {
        currentJumps = 0;
    }
}
