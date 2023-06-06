using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadEnemy : EnemyController {

	public AudioClip AudioHit;
	private AudioSource source; 

	void Start () {
		source = GetComponent<AudioSource> ();
		health = 3;
		EnemyAttack.SetActive (false);
		PlayerRay = 1;
	}
	void Update () {
		if (isAlive == false) {
			return;
		}
		float distance = PlayerPosition ();
		isMoving = (distance <= AttackRay);
		FrontPlayer = (distance <= PlayerRay); 
		if (isMoving) {
			if ((player.position.x > transform.position.x && right) || (player.position.x < transform.position.x && !right)) {
				Flip ();
			}
		}
		/*if (canHurt) {
			hurt = false;
		}*/
		timeAttack -= Time.deltaTime;
		if (timeAttack >= 0) {
			hurt = true;
		}
		if (FrontPlayer && timeAttack <= 0) {
			if (hurt) {
				return;
			}
			anim.SetTrigger ("EnemyAttack");
			//canHurt = false;
		}
	}
	public void AttackEnemy(){
		EnemyAttack.SetActive (true);
		source.PlayOneShot (AudioHit);
		StartCoroutine ("stopAttack");
		timeAttack = 1.5f;
	}
	IEnumerator stopAttack(){
		yield return new WaitForSeconds(0.2f);
		EnemyAttack.SetActive (false);
		//canHurt = true;
		hurt = false;
	}
	void FixedUpdate(){
		if (isMoving) {
			if (hurt) {
				return;
			}
			rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
			anim.SetTrigger ("EnemyWalk");
		} else {
			anim.SetTrigger ("EnemyIdle");
			AttackRay = 7;
		}
		if (health <= 0) {
			anim.SetTrigger ("EnemyDead");
			isAlive = false;
			bcEnemy.size = new Vector3 (1.183126f, 0.0465185f, 0);
			bcEnemy.offset = new Vector3 (-0.02128273f,-0.8141115f, 0);
		}
	}
	void OnTriggerEnter2D(Collider2D AttackCheck) {
		if (AttackCheck.gameObject.CompareTag ("Attack")&& canHurt) {
			anim.SetTrigger ("EnemyHurt");
			health -= 1;
			AttackRay = 0;
			hurt = true;
			//canHurt = false;
			Debug.Log ("Enemy Hurt");
			StartCoroutine ("stopHurt");
		}
	}
		IEnumerator stopHurt (){
		yield return new WaitForSeconds(1.2f);
		hurt = false;
		canHurt = true;
		}
	}

