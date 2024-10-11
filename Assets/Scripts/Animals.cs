using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animals : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected bool isDead = false;
    private AnimalMove movementStrategy;

    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(RandomMovement());
    }
    public bool IsDead()
    {
        return isDead;
    }
    protected virtual void Update()
    {
        if (isDead) return;
        movementStrategy?.Move(this);
    }

    protected IEnumerator RandomMovement()
    {
        while (!isDead)
        {
            if (this is Frog)
            {
                while (!isDead)
                {
                    Vector3 randomDirection = Random.insideUnitSphere.normalized * 2f;
                    randomDirection.y = 0;
                    Vector3 targetPosition = transform.position + randomDirection;
                    yield return StartCoroutine(RotateTowards(targetPosition));
                    yield return StartCoroutine(JumpToPosition(targetPosition));
                    yield return new WaitForSeconds(Random.Range(2f, 5f));
                }
            }
            if (this is Snake)
            {
                Vector3 startPosition = transform.position;
                Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                navMeshAgent.SetDestination(randomPosition);

                while (!isDead && Vector3.Distance(startPosition, randomPosition) > 0.1f)
                {
                    if (navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial || navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
                    {
                        randomPosition = RotateAndFindNewTarget();
                        navMeshAgent.SetDestination(randomPosition);
                    }

                    yield return null;
                }

                yield return new WaitForSeconds(Random.Range(2f, 5f));
            }
            else
            {
                Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                navMeshAgent.SetDestination(randomPosition);
                yield return new WaitForSeconds(Random.Range(2f, 5f));
            }
        }
    }
    private IEnumerator JumpToPosition(Vector3 targetPosition)
    {
        float jumpTime = 0.5f;
        float jumpHeight = 1f;
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < jumpTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / jumpTime);
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            currentPosition.y = Mathf.Sin(Mathf.PI * progress) * jumpHeight;
            transform.position = currentPosition;

            yield return null;
        }
        transform.position = targetPosition;
    }
    private IEnumerator RotateTowards(Vector3 targetPosition)
    {
        float rotationSpeed = 360f;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
    private Vector3 RotateAndFindNewTarget()
    {
        float randomAngle = Random.Range(90f, 180f);
        transform.Rotate(0, randomAngle, 0);
        Vector3 forwardDirection = transform.forward;
        Vector3 newTargetPosition = transform.position + forwardDirection * 10f;

        return newTargetPosition;
    }
    public void SetMovementStrategy(AnimalMove strategy)
    {
        movementStrategy = strategy;
    }

    public virtual void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Animals otherAnimal = collision.gameObject.GetComponent<Animals>();
        if (otherAnimal != null && !isDead)
        {
            HandleCollision(otherAnimal);
        }
    }

    protected abstract void HandleCollision(Animals otherAnimal);
}
