using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossController : EnemyController {

	public Slider healthbar;
	public float TimeAttack = 0;
	public int AttackNum, AttackOrder;
	public Transform impactposition;
	public Transform impactposition1;
	public GameObject impact;
	public GameObject impact1;
	public AudioClip AudioHit;
	public AudioClip AudioHurt;
	public AudioClip AudioDeath;
	private AudioSource source;
	public GameObject Victorii;
	public Rigidbody2D rb2d;

	void Start () {
		isAlive = true;
		rb2d = GetComponent<Rigidbody2D> ();
		bcGround = GetComponent<BoxCollider2D> ();
		source = GetComponent<AudioSource> ();
		health = 10;
		PlayerRay = 2.5f;
		EnemyAttack.SetActive (false);
		AttackNum = 0;
		AttackOrder = -1;
		bcEnemy.size = new Vector3 (4.573245f, 6.431195f, 0);
		bcEnemy.offset = new Vector3 (-0.832418f, -0.05688047f, 0);
	}

	void Update () {
		if (Input.GetKeyDown ("escape")) {
			Debug.Log ("Quit");
			Application.Quit();
		}
		TimeAttack += Time.deltaTime;
		healthbar.value = health;
		if (isAlive == false) {
			StartCoroutine ("GoMenu");
			return;
		}
		float distance = PlayerPosition ();
		isMoving = (distance <= AttackRay);
		FrontPlayer = (distance <= PlayerRay); 
		if (isMoving) {
			if ((player.position.x > transform.position.x && !right) || (player.position.x < transform.position.x && right)) {
				Flip1 ();
			}
		}
		if (canHurt) {
			hurt = false;
		}
		timeAttack -= Time.deltaTime;
		if (timeAttack >= 0) {
			hurt = true;
		}
		if (FrontPlayer && timeAttack <= 0) {
			if (hurt) {
				return;
			}
			AttackingBoss = true;
			anim.SetTrigger ("AttackBoss1");
			AttackOrder = 0;
			//canHurt = false;
		}
		if (TimeAttack >= 20f && AttackNum == 0) {
			anim.SetTrigger ("AttackBoss3");
			bcEnemy.size = new Vector3 (3.150693f, 9.90391f, 0);
			bcEnemy.offset = new Vector3 (0.7156613f, 4.231712f, 0);
			TimeAttack = 0;
			AttackNum = 1;
			AttackOrder = 1;
		}
		if (TimeAttack >= 20f && AttackNum == 1) {
			anim.SetTrigger ("AttackBoss2");
			TimeAttack = 0;
			AttackOrder = 2;
			AttackNum = 0;
		}
	}
	void FixedUpdate(){
		if (isMoving) {
			if (hurt) {
				return;
			}
			rb2d.velocity = new Vector2 (speed, rb2d.velocity.y);
			anim.SetTrigger ("WalkBoss");
		} else {
			anim.SetTrigger ("StandBoss");
			AttackRay = 20;
		}
		if (health <= 0) {
			anim.SetTrigger ("DeadBoss");
			Victorii.SetActive (true);
			isAlive = false;
			rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
			StartCoroutine ("Deadbc");
		} else
		bcEnemy.size = new Vector3 (4.573245f, 6.431195f, 0);
		bcEnemy.offset = new Vector3 (-0.832418f, -0.05688047f, 0);
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Attack")&& canHurt) {
			anim.SetTrigger ("HurtBoss");
			health -= 1;
			AttackRay = 0;
			hurt = true;
			canHurt = false;
			source.PlayOneShot (AudioHurt);
			Debug.Log ("Enemy Hurt");
			StartCoroutine ("stopHurt");
		}
	}
	IEnumerator stopHurt (){
		yield return new WaitForSeconds(1.2f);
		Debug.Log ("stopattack");
		hurt = false;
		canHurt = true;
	}
	IEnumerator Deadbc (){
		yield return new WaitForSeconds(0.5f);
		bcGround.enabled = false;
		bcEnemy.size = new Vector3 (5.077364f, 0.2784014f, 0);
		bcEnemy.offset = new Vector3 (-0.5803585f,-3.133277f, 0);
	}
	public void Flip1(){
		speed *= -1;
		right = !right;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
	public void AttackBoss(){
		EnemyAttack.SetActive (true);
		canHurt = false;
		source.PlayOneShot (AudioHit);
		if (AttackOrder == 1) {
			bcAttack.offset = new Vector3 (1.062162f, 1.295644f, 0);
			bcAttack.size = new Vector3 (5.148201f, 9.189079f, 0);
			Impact ();
		} else if (AttackOrder == 2) {
			bcAttack.offset = new Vector3 (0.5857995f, 0.5426848f, 0);
			bcAttack.size = new Vector3 (4.195472f, 1.966824f, 0);
			Impact1 ();
		} else if (AttackOrder == 0) {
			bcAttack.offset = new Vector3 (0.4229562f, -1.104609f, 0);
			bcAttack.size = new Vector3 (10.70534f, 2.510119f, 0);
		}
		StartCoroutine ("stopAttack");
		timeAttack = 1.5f;
	}
	IEnumerator stopAttack(){
			yield return new WaitForSeconds (0.1f);
		anim.SetTrigger ("StandBoss");
		AttackingBoss = false;
		Update ();
		EnemyAttack.SetActive (false);
		canHurt = true;
		hurt = false;
	}
	void Impact(){
	    if (impact != null) {
			var cloneimpact = Instantiate (impact, impactposition.position, Quaternion.identity) as GameObject;
				cloneimpact.transform.localScale = this.transform.localScale;
				Destroy (cloneimpact,10f);
			}
         }
	void Impact1(){
		if (impact1 != null) {
			var cloneimpact = Instantiate (impact1, impactposition1.position, Quaternion.identity) as GameObject;
			cloneimpact.transform.localScale = this.transform.localScale;
			Destroy (cloneimpact,10f);
		}
	}
	IEnumerator GoMenu (){
		yield return new WaitForSeconds (4f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 3);
	}
}



