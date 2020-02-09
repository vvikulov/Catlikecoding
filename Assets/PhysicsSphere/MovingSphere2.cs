using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere2 : MonoBehaviour {
	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;
	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f, maxAirAcceleration = 1f;
	private Vector3 velocity, desiredVelocity;
    private Rigidbody body;
    bool desiredJump;
    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;
    int groundContactCount;
    bool OnGround => groundContactCount > 0;
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;
    int jumpPhase;
    [SerializeField, Range(0, 90)]
    float maxGroundAngle = 25f;
    float minGroundDotProduct;
    Vector3 contactNormal;

    void OnValidate() {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    private void Awake() {
        body = GetComponent<Rigidbody>();
        OnValidate();
    }

    void Update() {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity =
            new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        desiredJump |= Input.GetButtonDown("Jump");//|= That way it remains true once enabled until we explicitly set it back to false
    }

    //Each physics step begins with invoking all FixedUpdate methods, after which PhysX does its thing, 
    //and at the end the collision methods get invoked
    void FixedUpdate() {
        UpdateState();
        AdjustVelocity();

        if (desiredJump) {
            desiredJump = false;
            Jump();
        }

        body.velocity = velocity;

        ClearState();
    }

    void ClearState() {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    void UpdateState() {
        velocity = body.velocity;
        if (OnGround) {
            jumpPhase = 0;
            if (groundContactCount > 1) {
                contactNormal.Normalize();
            }
        }
        else {
            contactNormal = Vector3.up;
        }
    }

    void Jump() {
        if (OnGround || jumpPhase < maxAirJumps) {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);
            if (alignedSpeed > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            velocity += contactNormal * jumpSpeed;
        }
    }

    void OnCollisionEnter(Collision collision) {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision) {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision) {
        for (int i = 0; i < collision.contactCount; i++) {
            Vector3 normal = collision.GetContact(i).normal;//normal is the direction that the sphere should be pushed, which is 
            //directly away from the collision surface
            if (normal.y >= minGroundDotProduct) {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    Vector3 ProjectOnContactPlane(Vector3 vector) {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    void AdjustVelocity() {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }
}
