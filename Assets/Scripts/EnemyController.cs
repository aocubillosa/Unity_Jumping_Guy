using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float velocity = 2f;
	private Rigidbody2D rb2D;

	void Start() {
		rb2D = GetComponent<Rigidbody2D>();
		rb2D.velocity = Vector2.left * velocity;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Destroyer") {
			Destroy(gameObject);
		}
	}
}