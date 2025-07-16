using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour {

	public void Update(){
		if (Input.GetKeyDown ("return")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		}
	if (Input.GetKeyDown ("escape")) {
			Debug.Log ("Quit");
			Application.Quit ();
	   }
   }
}