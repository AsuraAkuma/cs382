using UnityEngine;
using System.Collections.Generic;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject launchPoint;
    public GameObject projectilePrefeb;
    public float velocityMult;
    public int trajectoryPoints; // Number of points in the trajectory line
    public float timeStep; // Time step between points

    [Header("Dynamic")]
    private Vector3 launchPos;
    private GameObject projectile;
    private bool aimingMode;
    private LineRenderer lineRenderer;

    void Awake()
    {
        Application.targetFrameRate = 60; // Limit to 60 FPS
        launchPoint.SetActive(false);
        launchPos = launchPoint.transform.position;

        // Initialize the LineRenderer
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.08f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.enabled = false; // Hide the line initially
    }

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(projectilePrefeb);
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        // Draw rubberband
        lineRenderer.SetPosition(0, launchPos);
        lineRenderer.enabled = true; // Show the trajectory line
    }

    void Update()
    {
        if (!aimingMode || projectile == null) return;

        // Convert mouse position to world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        mouseWorldPos.z = 0;

        // Calculate drag vector
        Vector3 dragVector = mouseWorldPos - launchPos;

        // Limit drag distance to slingshot radius
        float maxDragDistance = GetComponent<CircleCollider2D>().radius;
        if (dragVector.magnitude > maxDragDistance)
        {
            dragVector = dragVector.normalized * maxDragDistance;
        }

        // Move projectile based on drag
        Vector3 projectilePos = launchPos + dragVector;
        projectile.transform.position = projectilePos;

        // Draw trajectory prediction
        Vector2 launchVelocity = -(projectile.transform.position - launchPos) * velocityMult;
        // DrawTrajectory(launchVelocity);

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            Rigidbody2D projRB = projectile.GetComponent<Rigidbody2D>();
            projRB.bodyType = RigidbodyType2D.Dynamic;

            // **Apply velocity from projectile's current position**
            projRB.linearVelocity = launchVelocity;

            FollowCam.POI = projectile; // Set the camera to follow the projectile
            // Hide trajectory line
            lineRenderer.enabled = false;

            projectile = null;
            GameController.shotsTaken++;
            // GameController.CheckWin();
            print(GameController.shotsTaken + " " + GameController.maxShots);
        }
        lineRenderer.SetPosition(1, projectilePos);


    }


    // void DrawTrajectory(Vector2 launchVelocity)
    // {
    //     Vector2 startPos = projectile.transform.position;
    //     Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

    //     // Gravity directly from the Rigidbody2D gravityScale
    //     float gravity = Physics2D.gravity.y * rb.gravityScale;

    //     // Create a temporary Rigidbody2D for simulating the trajectory
    //     Rigidbody2D tempRb = new GameObject("TempRigidbody").AddComponent<Rigidbody2D>();
    //     tempRb.gravityScale = rb.gravityScale;
    //     tempRb.linearDamping = rb.linearDamping;  // Simulate drag effects on trajectory
    //     tempRb.linearVelocity = launchVelocity;

    //     Vector2 currentPosition = startPos;
    //     lineRenderer.positionCount = trajectoryPoints;

    //     // Calculate the trajectory over multiple points
    //     for (int i = 0; i < trajectoryPoints; i++)
    //     {
    //         float time = i * timeStep;

    //         // Apply gravity and drag over time
    //         tempRb.linearVelocity += new Vector2(0, gravity) * time;  // Apply gravity force
    //         tempRb.linearVelocity *= Mathf.Exp(-tempRb.linearDamping * time);  // Apply drag over time

    //         // Get the projected position with applied velocity
    //         Vector3 projectedPos = startPos + tempRb.linearVelocity * time;

    //         lineRenderer.SetPosition(i, new Vector3(projectedPos.x, projectedPos.y, 0));
    //     }

    //     Destroy(tempRb.gameObject); // Clean up after calculation
    // }
}
