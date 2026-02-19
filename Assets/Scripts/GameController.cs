using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {Idle, Playing, Ending, Ready};

public class GameController : MonoBehaviour {

	public float speed = 0.02f, scaleTime = 10f, scaleInc = 0.2f;
	public RawImage background, platform;
	public GameObject uiIdle, uiScore, uiInfo, player, enemyGenerator;
	public GameState gameState = GameState.Idle;
	public Text pointsText, recordText;
	private int points = 0;
	private AudioSource musicGame;

	void Start() {
		musicGame = GetComponent<AudioSource>();
		recordText.text = "BEST: " + GetMaxScore().ToString();
	}

	void Update() {
		bool startGame = Input.GetKeyDown(KeyCode.Space);
		bool exitGame = Input.GetKeyDown(KeyCode.Escape);
		if (gameState == GameState.Idle && startGame) {
			gameState = GameState.Playing;
			uiIdle.SetActive(false);
			uiScore.SetActive(true);
			player.SendMessage("UpdateState", "PlayerRun");
			player.SendMessage("DustPlay");
			enemyGenerator.SendMessage("StartGenerator");
			musicGame.Play();
			InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
		} else if (gameState == GameState.Playing) {
			Parallax();
		} else if (gameState == GameState.Ready) {
			uiInfo.SetActive(true);
			if (startGame) {
				RestartGame ();
			} else if (exitGame) {
				Application.Quit();
			}
		}
	}

	void Parallax() {
		float finalSpeed = speed * Time.deltaTime;
		background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
		platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
	}

	public void RestartGame() {
		ResetTimeScale();
		SceneManager.LoadScene("Scene");
	}

	void GameTimeScale() {
		Time.timeScale += scaleInc;
	}

	public void ResetTimeScale(float newTimeScale = 1f) {
		CancelInvoke("GameTimeScale");
		Time.timeScale = newTimeScale;
	}

	public void IncreasePoints() {
		pointsText.text = (++points).ToString();
		if (points > GetMaxScore()) {
			recordText.text = "BEST: " + points.ToString();
			SaveScore(points);
		}
	}

	public int GetMaxScore() {
		return PlayerPrefs.GetInt("Max Points", 0);
	}

	public void SaveScore(int currentPoints) {
		PlayerPrefs.SetInt("Max Points", currentPoints);
	}
}