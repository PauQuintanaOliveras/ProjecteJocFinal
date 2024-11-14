using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleLights : Clickable {
	public string lightsOn = "Lights On";
	public string lightsOff = "Lights Off";
	public bool status = true;
	private bool clickable = true;

	void Update() {
		if(Input.GetKeyDown(KeyCode.L))
			Click();
	}

	public override void Click() {
		if(!clickable)
			return;
		clickable = false;
		status = !status;
		if(status) {
			StartCoroutine(SwapScenes(lightsOn, lightsOff));
		} else {
			StartCoroutine(SwapScenes(lightsOff, lightsOn));
		}
	}

	IEnumerator SwapScenes(string load, string unload) {
		SimulationMode og = Physics.simulationMode;
		Physics.simulationMode = SimulationMode.Script;
		AsyncOperation loadOp = SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive);
		AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(unload);
		while(!loadOp.isDone || !unloadOp.isDone)
			yield return null;
		Physics.simulationMode = og;
		LightProbes.Tetrahedralize();
		clickable = true;
	}
}
