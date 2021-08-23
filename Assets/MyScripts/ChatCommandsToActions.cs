using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCommandsToActions : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpHeight = 7f;
    public bool isGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Testing
        if (Input.GetKeyDown(KeyCode.S))
        {
            Jump();
        }
    }

    public void SendCommand(string text){
        if(text.ToLower().Contains("jump")){
            Jump();
        }
    }

    void Jump()
    {
        Debug.Log("jumping...");
        if(isGrounded){
            rb.AddForce(Vector3.up * jumpHeight);
        }
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
             isGrounded = true;
        }
    }
 
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
             isGrounded = false;
        }
    }
}
