using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.velocity;
        
        // After a collision, accelerate by 1%
        velocity += velocity.normalized * 0.01f;
        
        // If the ball is going totally vertically, add a little horizontal force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        // Maximum velocity
        if (velocity.magnitude > 3)
        {
            velocity = velocity.normalized * 3;
        }

        m_Rigidbody.velocity = velocity;
    }
}
