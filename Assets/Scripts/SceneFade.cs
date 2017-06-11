using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour {
	public Texture2D fadeTex;
	public float fadeTime = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI(){
		alpha += fadeDir * fadeTime * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTex);
	}

	public void BeginFade(int direction){
		fadeDir = direction;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		BeginFade (-1);
	}

	void OnCollisionEnter(Collision col){
		StartCoroutine (SwitchScene());
	}

	IEnumerator SwitchScene(){
		BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
