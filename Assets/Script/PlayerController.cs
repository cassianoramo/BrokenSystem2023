﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	private Animator anim;
	public GameObject attackcheck;
	private Rigidbody2D rb2d;
	public Transform posPe, WallCheck, AttackCheck;
	public LayerMask Ground, EnemyLayer;
	[HideInInspector] public bool touchWall = false, tocaChao = false, viradoDireita = true;
	public float ForcaPulo = 1000f,  Velocidade, slideSpeed = 5f, timeAttack;
	public bool jump,  SideCheck, isSlide = false, isAlive = true;
	public BoxCollider2D bc, slidecol,attackcol;
	public WaitForSeconds attacktime;
	public int AnimaCombo = 0, Health;
	public AudioClip audiojump;
	public AudioClip audiohurt;
	public AudioClip audioattack;
	public AudioClip audiodeath;
	private AudioSource source;
	public bool GameOver = false;
	public bool Restart = false;
	public GameObject Restartt;
	public GameObject GameOverr;

	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		bc = bc.GetComponent<BoxCollider2D> ();
		source = GetComponent<AudioSource> ();
		slidecol.enabled = false;
		attackcheck.SetActive (false);
	}
	void Update () {
		//The groundcheck
		if(Input.GetKeyDown(KeyCode.R) && Restart == true)
		{
			Debug.Log ("dfghjk");
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex -1);
		}
		if (Input.GetKeyDown ("escape")) {
			Debug.Log ("Quit");
			//Application.Quit;
		}
		if(isAlive == false){
			return;
		}
		tocaChao = Physics2D.Linecast (transform.position, posPe.position, 1 << LayerMask.NameToLayer ("Ground"));
		touchWall = Physics2D.Linecast (transform.position, WallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		Fall ();
		if (Input.GetKeyDown("space")) {
			bc.size = new Vector3 (0.4929347f, 0.8221698f, 0);
			bc.offset = new Vector3 (0.1226404f, 0.01533556f, 0);
			Jump ();
			source.PlayOneShot (audiojump);
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			Doslide ();
		}
		if (Input.GetKeyDown (KeyCode.U) && timeAttack <= 0) {
				AttackSword ();
			source.PlayOneShot (audioattack);
			}
		/*if (Input.GetKeyDown (KeyCode.J)) {
				AttackHand ();
			}*/
		timeAttack -= Time.deltaTime;
	}
	void FixedUpdate()
	{
		if(isAlive == false){
			return;
		}
		if( touchWall ) {
			anim.SetBool ("Wall Slide", true);
			anim.SetBool ("Fall", false);
		}else if(rb2d.velocity.y >= 0){
			anim.SetBool ("Wall Slide", false);
			anim.SetTrigger("Jump");
		}
		LayerControl ();
		//Player moviment
		float translationY = 0;
		float translationX = Input.GetAxis ("Horizontal") * Velocidade;
		transform.Translate (translationX, translationY, 0);
		transform.Rotate (0, 0, 0);
		//Animations
		if (translationX != 0 && tocaChao)  {
			anim.SetTrigger ("Run");
		   bc.size = new Vector3 (0.7746387f, 1.127496f, 0);
			bc.offset = new Vector3 (0.1723526f, -0.1373274f, 0);
		} else {
			anim.SetTrigger ("Stand Hand");
			bc.size = new Vector3 (0.7540007f,1.168771f, 0);
			bc.offset = new Vector3 (-0.07013941f, -0.1166899f, 0);
		}
		//Player direction
		if (translationX > 0 && !viradoDireita || translationX < 0 && viradoDireita) {
			if (touchWall && !tocaChao) {
				rb2d.velocity = (new Vector2 (rb2d.velocity.x, 0f));
				rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			}
			Flip (); 
		}
}
	void Fall()
	{
		if (!tocaChao && rb2d.velocity.y <= 0 && !touchWall) {   
			if (touchWall) {
				anim.SetBool ("Wall Slide", true);
				anim.SetBool ("Fall", false);
			}else
			anim.SetBool ("Fall", true);
			anim.ResetTrigger ("Jump");
			anim.SetBool ("Wall Slide", false);
			bc.size = new Vector3 (0.4929347f, 0.9783585f, 0);
			bc.offset = new Vector3 (0.1226404f, -0.0627588f, 0);
		}	
		if (tocaChao) {
			anim.SetBool ("Fall", false);
			anim.SetBool ("Wall Slide", false);
		}
	}
	void Jump(){
		if (tocaChao && rb2d.velocity.y >= 0 ) {
			rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			anim.SetTrigger ("Jump");
			anim.SetBool ("Wall Slide", false);
		}
}
	//Flip script
	void Flip()
	{
		viradoDireita = !viradoDireita;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
	void LayerControl(){
		if (!tocaChao) {
			anim.SetLayerWeight (1, 1);
		} else {
			anim.SetLayerWeight (1, 0);
		}
	}
	private void Doslide(){
		isSlide = true;
		anim.SetTrigger ("isSliding");
		slidecol.enabled = true;
		bc.enabled = false;
		StartCoroutine ("stopSlide");
	}
		IEnumerator stopSlide(){
		yield return new WaitForSeconds(1f);
		anim.SetTrigger ("Stand Hand");
		slidecol.enabled = false;
		bc.enabled = true;
		isSlide = false;
		}
	/*void AttackHand(){
		if (tocaChao && AnimaCombo == 0) {
			anim.SetTrigger ("Punch1");
			AnimaCombo = 1;
		} else if (tocaChao && AnimaCombo == 1) {
			anim.SetTrigger ("Punch2");
			AnimaCombo = 2;
		} else if (tocaChao && AnimaCombo == 2) {
			anim.SetTrigger ("Punch3");
		}
	}*/
	void AttackSword (){
		if (tocaChao && AnimaCombo == 0) {
			attackcheck.SetActive (true);
			anim.SetTrigger ("Sword1");
			StartCoroutine ("stopAttack");
			timeAttack = 0.7f;
		}
		if (!tocaChao && AnimaCombo == 0) {
			attackcheck.SetActive (true);
			anim.SetTrigger ("AirAttack1");
			StartCoroutine ("stopAttack");
			timeAttack = 0.7f;
		}
	}
	IEnumerator stopAttack(){
	 yield return new WaitForSeconds(1f);
		attackcheck.SetActive (false);
	}
	 void  OnTriggerEnter2D (Collider2D other){
		if (!isAlive) {
			return;
		}
		if (other.gameObject.CompareTag ("Obstacle")) {
			if (tocaChao) {
				anim.SetTrigger ("Hurt");
				Health--;
				source.PlayOneShot (audiohurt);
			} else {
				anim.SetTrigger ("Fall Hurt");
				Health--;
				source.PlayOneShot (audiohurt);
			}
			if (Health < 1) {
				anim.SetLayerWeight (1, 1);
				anim.SetTrigger ("Dead");
				isAlive = false;
				GameOverr.SetActive(true);
				Restartt.SetActive (true);
				Restart = true;
				Update ();
			}
			jump = false;
			anim.SetTrigger ("Stand Hand");
			Update ();
		}
		if (other.gameObject.CompareTag ("Fase1")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			//UnityEngine.SceneManagement.SceneManager.LoadScene("Fase1");
		}
	}
}

