using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour {
	
	public Sprite[] HealthBar;
	public Image HealthBarUI;
	private PlayerController Player;

	void Start () {
		Player = GameObject.Find ("Player").GetComponent<PlayerController>();
	}
	void Update () {
		HealthBarUI.sprite = HealthBar [Player.Health];
	}
}
