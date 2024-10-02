using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogJump : AnimalMove
{
    private float jumpDistance = 3f; 
    private float jumpCooldown;  
    private float nextJumpTime;  

    public FrogJump(float jumpDistance, float jumpCooldown)
    {
        this.jumpDistance = jumpDistance;
        this.jumpCooldown = jumpCooldown;
        this.nextJumpTime = 0f;
    }

    public void Move(Animals animal)
    {
        if (Time.time >= nextJumpTime && !animal.IsDead())
        {
            Vector3 jumpDirection = Random.insideUnitSphere;
            jumpDirection.y = 0; 
            jumpDirection.Normalize(); 
            nextJumpTime = Time.time + jumpCooldown;
        }
    }    
}
