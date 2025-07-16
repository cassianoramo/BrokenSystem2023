using UnityEngine;
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
	public float ForcaPulo = 1000f,  Velocidade, slideSpeed = 5f, comboTimer = 0f, comboResetTime = 5f;
	public bool jump = true,  SideCheck, isSlide = false, isAlive = true, atacando,atacando1 = false, atacando2 = false;
	public BoxCollider2D bc, slidecol,attackcol;
	public WaitForSeconds attacktime;
	public int Health, combo, combo1 = 2, combo2 = 6;
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
		attackcol = AttackCheck.GetComponent<BoxCollider2D> ();
		source = GetComponent<AudioSource> ();
		slidecol.enabled = false;
		attackcheck.SetActive (false);
		combo1 = 2;
		atacando1 = false;
		combo2 = 6;
		atacando2 = false;
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
			//bc.size = new Vector3 (0.5059768f, 0.8221698f, 0);
			//bc.offset = new Vector3 (0.1161193f, 0.01533556f, 0);
			Jump ();
			source.PlayOneShot (audiojump);
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			Doslide ();
		}
		if (Input.GetKeyDown (KeyCode.U) && !atacando2 ) {
				AttackSword ();
				
				
			source.PlayOneShot (audioattack);
			}
		if (Input.GetKeyDown (KeyCode.J)&& !atacando1) {
				AttackHand ();
				//comboTimer += Time.deltaTime;
			}
		if (Input.GetKeyDown (KeyCode.K) && !atacando) {
				AttackKick ();
				//comboTimer += Time.deltaTime;
			}
		 if (comboTimer >= comboResetTime)
        {
            FinishAnim();
			FinishAnim1();
			FinishAnim2();
			comboTimer = 0; 
        }
		if(atacando || atacando1 || atacando2){
			comboTimer += Time.deltaTime;
		}
				//timeAttack -= Time.deltaTime;
			
		Debug.Log (atacando2);
		Debug.Log (comboTimer);
		

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
		} else if(jump == false) {
			anim.SetTrigger ("Stand Hand");
			bc.size = new Vector3 (0.6692266f,1.129645f, 0);
			bc.offset = new Vector3 (-0.0277524f, -0.1362531f, 0);
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
			jump = false;
		}	
		if (tocaChao) {
			
			anim.SetBool ("Fall", false);
			anim.SetBool ("Wall Slide", false);
		}
	}
	void Jump(){
		if (tocaChao && rb2d.velocity.y >= 0 ) {
			rb2d.AddForce (new Vector2 (0f, ForcaPulo));
			jump = true;
			bc.size = new Vector3 (0.5059768f, 0.8221698f, 0);
			bc.offset = new Vector3 (0.1161193f, 0.01533556f, 0);
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
	void AttackHand(){
		if (tocaChao ) {
			//anim.SetTrigger ("Punch1");
			atacando1 =true;
			anim.SetTrigger (""+combo1);
			attackcheck.SetActive (true);
			attackcol.size = new Vector3 (0.5211939f, 0.2611685f, 0);
			attackcol.offset = new Vector3 (0.1179317f, -0.2626958f, 0);
			StartCoroutine ("stopAttack");
			//timeAttack = 0.4f;
		} 
	}
	void AttackKick(){
		if (tocaChao ) {
			atacando =true;
			anim.SetTrigger (""+combo);
			attackcheck.SetActive (true);
			attackcol.size = new Vector3 (0.4566334f,0.3068449f, 0);
			attackcol.offset = new Vector3 (0.08565144f, -0.1142401f, 0);
			StartCoroutine ("stopAttack");
			//timeAttack = 0.4f;
			
		}
	
		
		if (!tocaChao) {
			
			attackcheck.SetActive (true);
			
			attackcol.size = new Vector3 (0.2271488f, 0.2176008f, 0);
			attackcol.offset = new Vector3 (-0.1098354f, -0.473341f, 0);
			anim.SetTrigger ("AirKick1");
			StartCoroutine ("stopAttack");
			//timeAttack = 0.4f;
		} 
	}
	public void StartCombo(){
		atacando = false;
		if(combo<2){
			combo++;
			Debug.Log(combo);
		}
	}
	public void FinishAnim(){
		atacando = false;
		combo = 0;
	}
	public void StartCombo1(){
		atacando1 = false;
		if(combo1<6){
			combo1++;
			Debug.Log(combo1);
		}
	}
	public void FinishAnim1(){
		atacando1 = false;
		combo1 = 2;
	}
	public void StartCombo2(){
		atacando2 = false;
		if(combo2<8){
			combo2++;
			Debug.Log(combo2);
		}
	}
	public void FinishAnim2(){
		atacando2 = false;
		combo2 = 6;
		
	}
	void AttackSword (){
		if (tocaChao ) {
			attackcheck.SetActive (true);
			atacando2 =true;
			anim.SetTrigger (""+combo2);
			attackcol.size = new Vector3 (0.6114954f, 1.213438f, 0);
			attackcol.offset = new Vector3 (0.1630824f, 0.0328362f, 0);
			//anim.SetTrigger ("Sword1");
			StartCoroutine ("stopAttack");
			//timeAttack = 0.4f;
		}
		if (!tocaChao ) {
			attackcheck.SetActive (true);
			
			attackcol.size = new Vector3 (0.6114954f, 0.7895675f, 0);
			attackcol.offset = new Vector3 (0.1630824f, 0.02957559f, 0);
			anim.SetTrigger ("AirAttack1");
			StartCoroutine ("stopAttack");
			//timeAttack = 0.4f;
		}
	}
	IEnumerator stopAttack(){
	 yield return new WaitForSeconds(0.5f);
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
			//jump = false;
			//anim.SetTrigger ("Stand Hand");
			Update ();
		}
		if (other.gameObject.CompareTag ("Fase1")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			//UnityEngine.SceneManagement.SceneManager.LoadScene("Fase1");
		}
	}
}
