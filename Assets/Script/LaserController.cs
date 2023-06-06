using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

	public BoxCollider2D bcLaser;
	public GameObject Laser;
	protected Animator anim;
	public float TimeLaser = 0;
	public AudioClip TurnOn;
	public AudioClip TurnOff;
	public AudioClip Active;
	private AudioSource source;

	void Start () {
		bcLaser = GetComponent<BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();
	}
	void Update () {
		TimeLaser += Time.deltaTime;
		if (TimeLaser >= 4f) {
			source.PlayOneShot (TurnOn);
			anim.SetBool ("LaserOff", false);
			anim.SetTrigger ("LaserTurnOn");
			LaserActive ();
		}
		if (TimeLaser >= 6f) {
			source.PlayOneShot (TurnOff);
			anim.SetBool ("LaserActive", false);
			anim.SetTrigger ("LaserTurnOff");
			TimeLaser = 0f;
			LaserOff ();
		}
	}
	void 	LaserActive (){
		anim.SetBool ("LaserActive", true);
	}
	void LaserOff (){
		anim.SetBool ("LaserOff", true);
	}
	void ActivationLaser(){
		Laser.SetActive (true);
	}
	void DesactivationLaser(){
		Laser.SetActive (false);
	}
}
