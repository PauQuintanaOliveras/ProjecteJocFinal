using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour {
	void Start() {
		StartCoroutine(loader());
	}

	IEnumerator loader() {
		SceneManager.LoadScene("Lights On");
		AsyncOperation claseLoad = SceneManager.LoadSceneAsync("Clase", LoadSceneMode.Additive);
		LightProbes.Tetrahedralize();
		/*
		while(!claseLoad.isDone)
			yield return null;
		*/
		yield return null;
	}
}
