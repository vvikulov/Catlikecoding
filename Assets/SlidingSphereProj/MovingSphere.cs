using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour {
	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;
	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f;
	[SerializeField]
	Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);
	[SerializeField, Range(0f, 1f)]
	float bounciness = 0.5f;
	private Vector3 velocity;

	void Update() {
		Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		////Always normalizing the input vector limits the position to always lie on the circle, unless the input is neutral in which case we end up at the origin
		//playerInput.Normalize();//because of on a keyboard diagonal movement will be fastest (but it should be the same as others).
		//						//Without it, for vertical and horizontal maximum will be 1, but for horizontal it will be sqrt(2) 
		//						//because of the Pythagoream theorem. The axis values define the lengths of two sides of a right 
		//						//triangle and the combined vector is the hypothenuse. Hence, the magnitude of the input vector is sqrt(x^2 + y^2)

		playerInput = Vector2.ClampMagnitude(playerInput, 1f);//only adjusting the input vector if its magnitude exceeds one
		//Acceleration: Vn+1 = Vn + at where V0 being the zero vector.
		//Deceleration: an acceleration opposite to the current velocity
		Vector3 desiredVelocity =
			new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
		float maxSpeedChange = maxAcceleration * Time.deltaTime;//how much we're able to change velocity this update
		//if (velocity.x < desiredVelocity.x) {
		//	velocity.x =
		//		Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);
		//}
		//else if (velocity.x > desiredVelocity.x) {
		//	velocity.x =
		//		Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);
		//}
		//Or instead
		velocity.x =
			Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
		velocity.z =
			Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
		Vector3 displacement = velocity * Time.deltaTime;
		Vector3 newPosition = transform.localPosition + displacement;
		if (newPosition.x < allowedArea.xMin) {
			newPosition.x = allowedArea.xMin;
			velocity.x = -velocity.x * bounciness;
		}
		else if (newPosition.x > allowedArea.xMax) {
			newPosition.x = allowedArea.xMax;
			velocity.x = -velocity.x * bounciness;
		}
		if (newPosition.z < allowedArea.yMin) {
			newPosition.z = allowedArea.yMin;
			velocity.z = -velocity.z * bounciness;
		}
		else if (newPosition.z > allowedArea.yMax) {
			newPosition.z = allowedArea.yMax;
			velocity.z = -velocity.z * bounciness;
		}
		transform.localPosition = newPosition;
	}
}
