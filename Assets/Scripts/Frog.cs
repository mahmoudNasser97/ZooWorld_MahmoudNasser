using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Animals
{
    public float jumpForce = 5f;
    public float jumpDistance = 3f;
    public float jumpInterval = 2f;

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        SetMovementStrategy(new FrogJump(jumpDistance, jumpInterval));
        StartCoroutine(JumpAutomatically());
    }

    private IEnumerator JumpAutomatically()
    {
        while (!isDead)
        {
            Jump();
            yield return new WaitForSeconds(jumpInterval);
        }
    }

    private void Jump()
    {
        Vector3 jumpDirection = Vector3.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode.Impulse);
        StartCoroutine(MoveForward(jumpDistance));
    }

    private IEnumerator MoveForward(float distance)
    {
        float movedDistance = 0f;

        while (movedDistance < distance)
        {
            float step = jumpDistance * Time.deltaTime;
            rb.MovePosition(rb.position + rb.transform.forward * step);
            movedDistance += step;
            yield return null;
        }
    }

    protected override void HandleCollision(Animals otherAnimal)
    {
        if (otherAnimal is Frog)
        {
            Vector3 direction = (transform.position - otherAnimal.transform.position).normalized;
            rb.AddForce(direction * 5f, ForceMode.Impulse);
        }
        else if (otherAnimal is Snake)
        {
            Die();
        }
    }
}
