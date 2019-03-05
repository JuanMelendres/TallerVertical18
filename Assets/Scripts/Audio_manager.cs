using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Audio_manager : MonoBehaviour {
	
	public AudioClip intro;
	public AudioClip preguntas;

	private int Question;
	private int count = 0;
	private GameObject[] pieces;
	private Animator anim;
	private AudioSource aSrc;

	void Start () {
		this.anim = this.GetComponentInChildren<Animator> ();
		this.pieces = GameObject.FindGameObjectsWithTag("Pieces");
		this.aSrc = this.GetComponent<AudioSource> ();
	}

	public IEnumerator start() {
		aSrc.clip = intro;
		aSrc.Play();
		yield return new WaitForSeconds(intro.length);

		foreach(GameObject t in pieces) {
			t.GetComponent<pieceSelectAlt>().StartScene = true;
		}

		aSrc.clip = preguntas;
		aSrc.Play ();
		yield return new WaitForSeconds(preguntas.length);
	}

	public void PlayAudio(int State, AudioClip clip) {
		StopAllCoroutines ();
		if (State == 0) {
			anim.SetInteger ("Index", count + 1);
			anim.SetTrigger ("Inicio");
			StartCoroutine (start ());
			return;
		} else if (State == 1) {
			count++;
			anim.SetInteger ("Index", count + 1);
			anim.SetTrigger ("Inicio");
		} 

		StartCoroutine (Audio (clip));
	}

	private IEnumerator Audio(AudioClip clip){
		aSrc.clip = clip;
		aSrc.Play();
		yield return new WaitForSeconds(clip.length);
	}

	public void is_Final(){
		anim.SetBool ("Last_pize", true);
	}
}
