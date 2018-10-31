using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trilateration: MonoBehaviour {

	const bool return_middle = true;

	public static Vector3 Trilaterate (SignalSnapShot p1, SignalSnapShot p2, SignalSnapShot p3) {
	
		var ex = vector_divide (vector_subtract (p2.pos, p1.pos), norm (vector_subtract (p2.pos, p1.pos)));

		var i = dot (ex, vector_subtract (p3.pos, p1.pos));
		var a = vector_subtract (vector_subtract (p3.pos, p1.pos), vector_multiply (ex, i));
		var ey = vector_divide (a, norm (a));
		var ez = vector_cross (ex, ey);
		var d = norm (vector_subtract (p2.pos, p1.pos));
		var j = dot (ey, vector_subtract (p3.pos, p1.pos));

		var x = (sqr (p1.radius) - sqr (p2.radius) + sqr (d)) / (2 * d);
		var y = (sqr (p1.radius) - sqr (p3.radius) + sqr (i) + sqr (j)) / (2 * j) - (i / j) * x;

		var b = sqr (p1.radius) - sqr (x) - sqr (y);

		Debug.Log (b);

		var z = Mathf.Sqrt (b);

		// no solution found
		if (float.IsNaN(z)) {
			Debug.Log ("NO SOLUTION FOUND");
			return Vector3.one;
		}

		Vector3 middlePoint = vector_add (p1.pos, vector_add (vector_multiply (ex, x), vector_multiply (ey, y)));

		//unused for now...
		//Vector3 point1 = vector_add (a, vector_multiply (ez, z));
		//Vector3 point2 = vector_subtract (a, vector_multiply (ez, z));

		if (z.Equals(0)) {
			Debug.Log ("1 Solution");
		} else {
			Debug.Log ("2 Solutions");
		}

		return middlePoint;
	}

	static float sqr (float a) {
		return a * a;
	}

	static float norm (Vector3 a) {
		return Mathf.Sqrt (sqr (a.x) + sqr (a.y) + sqr (a.z));
	}

	static float dot (Vector3 a, Vector3 b) {
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	static Vector3 vector_subtract (Vector3 a, Vector3 b) {
		return new Vector3 (
			a.x - b.x,
			a.y - b.y,
			a.z - b.z
		);
	}

	static Vector3 vector_add (Vector3 a, Vector3 b) {
		return new Vector3 (
			a.x + b.x,
			a.y + b.y,
			a.z + b.z
		);
	}

	static Vector3 vector_divide (Vector3 a, float b) {
		return new Vector3 (
			a.x / b,
			a.y / b,
			a.z / b
		);
	}

	static Vector3 vector_multiply (Vector3 a, float b) {
		return new Vector3 (
			a.x * b,
			a.y * b,
			a.z * b
		);
	}

	static Vector3 vector_cross (Vector3 a, Vector3 b) {
		return new Vector3 (
			a.y * b.z - a.z * b.y,
			a.z * b.x - a.x * b.z,
			a.x * b.y - a.y * b.x
		);
	}
}
