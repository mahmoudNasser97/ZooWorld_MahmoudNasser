using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Snake : Animals
{
    public GameObject tastyLabelPrefab;
    public Transform labelPosition;
    private bool isRotating = false;
    private Vector3 lastPosition;
    private float stuckTimeThreshold = 2f;
    private float lastMoveTime = 0f;

    protected override void Start()
    {
        base.Start();
        SetMovementStrategy(new SnakeMove());

        navMeshAgent = GetComponent<NavMeshAgent>();

        lastPosition = transform.position;
        InvokeRepeating(nameof(CheckIfStuck), 0, 0.5f);
    }
    protected override void HandleCollision(Animals otherAnimal)
    {
        if (otherAnimal is Frog)
        {
            Eat(otherAnimal);
            ScoreUIManager.Instance.IncrementPreyCount();
        }
        else if (otherAnimal is Snake)
        {
            Eat(otherAnimal);
            ScoreUIManager.Instance.IncrementPredatorCount();
        }
    }

    private void Eat(Animals prey)
    {
        if (prey is Frog)
        {
            prey.Die();
            ShowTastyLabel();
            ScoreUIManager.Instance.DeadFrogsCounter();
        }
        else if (prey is Snake)
        {
            prey.Die();
            ShowTastyLabel();
            ScoreUIManager.Instance.DeadSnakesCounter();
        }

    }

    private void ShowTastyLabel()
    {
        GameObject label = Instantiate(tastyLabelPrefab, labelPosition.position, Quaternion.identity);
        label.transform.SetParent(FindObjectOfType<Canvas>().transform);
        label.transform.position = new Vector3(900f, 500f, 500f);
        Destroy(label, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Animals otherAnimal = collision.gameObject.GetComponent<Animals>();

        if (otherAnimal != null)
        {
            HandleCollision(otherAnimal);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            RotateAndFindNewTarget();
        }
    }

    private void RotateAndFindNewTarget()
    {
        if (isRotating) return;

        isRotating = true;
        float randomAngle = Random.Range(90f, 180f);
        transform.Rotate(0, randomAngle, 0);
        Vector3 forwardDirection = transform.forward;
        Vector3 newTargetPosition = transform.position + forwardDirection * 5f;

        navMeshAgent.SetDestination(newTargetPosition);

        Invoke(nameof(ResetRotationFlag), 1f);
    }

    private void ResetRotationFlag()
    {
        isRotating = false;
    }

    private void CheckIfStuck()
    {
        if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
        {
            if (Time.time - lastMoveTime > stuckTimeThreshold)
            {
                RotateAndFindNewTarget();
                lastMoveTime = Time.time;
            }
        }
        else
        {
            lastMoveTime = Time.time;
            lastPosition = transform.position;
        }
    }
}
