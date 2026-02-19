using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject game, enemyGenerator;
	public AudioClip jumpClip, dieClip, pointClip;
	public ParticleSystem dust;
	private float startY;
	private Animator animator;
	private AudioSource audioPlayer;

	void Start() {
		animator = GetComponent<Animator>();
		audioPlayer = GetComponent<AudioSource>();
		startY = transform.position.y;
	}

	void Update() {
		bool isGrounded = transform.position.y == startY;
		bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
		bool userAction = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0);
		if (isGrounded && gamePlaying && userAction) {
			UpdateState("PlayerJump");
			audioPlayer.clip = jumpClip;
			audioPlayer.Play();
		}
	}

	public void UpdateState(string state = null) {
		if (state != null) {
			animator.Play(state);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy") {
			UpdateState ("PlayerDie");
			game.GetComponent<GameController> ().gameState = GameState.Ending;
			enemyGenerator.SendMessage ("StopGenerator", true);
			game.SendMessage ("ResetTimeScale", 0.5f);
			game.GetComponent<AudioSource> ().Stop ();
			audioPlayer.clip = dieClip;
			audioPlayer.Play ();
			DustStop ();
		} else if (other.gameObject.tag == "Point") {
			game.SendMessage("IncreasePoints");
			audioPlayer.clip = pointClip;
			audioPlayer.Play();
		}
	}

	void GameReady() {
		game.GetComponent<GameController>().gameState = GameState.Ready;
	}

	void DustPlay() {
		dust.Play();
	}

	void DustStop() {
		dust.Stop();
	}
}