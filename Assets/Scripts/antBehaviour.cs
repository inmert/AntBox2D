using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehaviour : MonoBehaviour
{
    public float maxSpeed;
    public float steerStrength;
    public float wanderStrength;

    // Attachments
    public TrailRenderer trailRenderer;
    public collisionFood foodHandler;

    Vector2 position;
    Vector2 velocity;
    Vector2 desiredDirection;

    void Start()
    {
        // Properties
        wanderStrength = 0.05F;
        maxSpeed = 3;
        steerStrength = 2F;

        // Functions
        position = gameObject.transform.position;
        renderTrail();
    }

    void Update()
    {
        antIdleBehaviour();
        //handleFood();
        float angle = -Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, angle));
    }

    void antIdleBehaviour()
    {
        // Get the viewport boundaries of the main camera
        Vector3 minViewportBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 maxViewportBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        // Define a buffer distance from the boundaries
        float bufferDistance = 2.0f;

        // Check if the ant is close to the boundaries
        if (position.x <= minViewportBounds.x + bufferDistance || position.x >= maxViewportBounds.x - bufferDistance ||
            position.y <= minViewportBounds.y + bufferDistance || position.y >= maxViewportBounds.y - bufferDistance)
        {
            // Set a new random direction away from the boundary
            desiredDirection = (new Vector2(Random.Range(minViewportBounds.x + bufferDistance, maxViewportBounds.x - bufferDistance),
                                             Random.Range(minViewportBounds.y + bufferDistance, maxViewportBounds.y - bufferDistance))
                                             - position).normalized;
        }
        else
        {
            // Continue with normal wandering behavior
            desiredDirection = (desiredDirection + Random.insideUnitCircle * wanderStrength).normalized;
        }

        Vector2 desiredVelocity = desiredDirection * maxSpeed;
        Vector2 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength;
        Vector2 acceleration = Vector2.ClampMagnitude(desiredSteeringForce, steerStrength) / 1;

        velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, maxSpeed);
        position += velocity * Time.deltaTime;
    }

    void handleFood()
    {
        GameObject foodItem = foodHandler.hasCollided(); // Get the collided food item

        if (foodItem != null)
        {
            // Calculate direction towards the food item
            Vector2 directionToFood = (foodItem.transform.position - transform.position).normalized;

            // Update desired direction to move towards the food item
            desiredDirection = directionToFood;

            // Recalculate desired velocity and steering force
            Vector2 desiredVelocity = desiredDirection * maxSpeed;
            Vector2 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength;
            Vector2 acceleration = Vector2.ClampMagnitude(desiredSteeringForce, steerStrength) / 1;

            // Update velocity and position based on acceleration
            velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, maxSpeed);
            position += velocity * Time.deltaTime;

            float distanceToFood = Vector2.Distance(transform.position, foodItem.transform.position);
            if (distanceToFood < 1f)
            {
                Destroy(foodItem);
            }


        }
    }


    void renderTrail()
    {
        //2D
        trailRenderer.time = 4f; // Trail duration
        trailRenderer.startWidth = 0.2f; // Trail start width
        trailRenderer.endWidth = 0.02f; // Trail end width
    }
}
