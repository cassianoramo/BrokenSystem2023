using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoss : MonoBehaviour {
	//Declara as variaveis de velocidade e de fisica do tiro
	public  Vector2 speed = new Vector2 (20,0);
	private Rigidbody2D rigidbodytiro;
	public Transform player;
	public BoxCollider2D impactbc;
	//Determina a velocidade do tiro e seu ponto de partida
	void Start () {
		impactbc = GetComponent<BoxCollider2D> ();
		impactbc.offset = new Vector3 (0.08383061f, 0.2009127f, 0);
		impactbc.size = new Vector3 (1.02046f, 3.324506f, 0);
		rigidbodytiro = GetComponent<Rigidbody2D>();
		rigidbodytiro.velocity = speed * this.transform.localScale.x;
	}
	//Destroi o tiro ao encostar em objetos com tag "Chao"
	void OnTriggerEnter2D(Collider2D objeto)
	{
		if (objeto.gameObject.tag == "Wall" || objeto.gameObject.tag == "Player") {
			Destroy (gameObject);
		}
	}
}



