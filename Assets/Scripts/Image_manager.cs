using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Image_manager : MonoBehaviour {
	private bool is_fading=false;
	private int direccion=1;
	private MeshRenderer plane_h;
	private float tranparency;
	private float lerp;


	// Use this for initialization
	void Start () {
		
		this.lerp=0.0f;
		this.tranparency = 0.0f;
		this.plane_h = GameObject.FindGameObjectWithTag ("History").GetComponent<MeshRenderer>();

		//StartCoroutine (Check ());
	}
		
	/*private IEnumerator Check()
	{
		yield return new WaitForSeconds (5.0f);
		if (is_fading) {
			if (direccion == 1) {
				StartCoroutine (fadeIn ());
			} else {
				StartCoroutine (fadeOut ());
			}
		}

		Reboot ();
	}

	private void Reboot()
	{
		StopAllCoroutines ();
		StartCoroutine (Check ());
	}*/
	public IEnumerator fadeOut() {
		this.direccion= -1;
		is_fading = true;
		while (this.tranparency >= 0) {
			this.plane_h.material.SetFloat ("_fade", tranparency);
			tranparency -= 0.01f;
			yield return null;
		}
		if (tranparency <= 0) {
			is_fading = false;
		}

	}

	public IEnumerator fadeIn() {
		this.direccion= 1;
		is_fading = true;
		while (this.tranparency <=1) {
			this.plane_h.material.SetFloat ("_fade", tranparency);
			tranparency += 0.01f;
			yield return null;
		}
		if (tranparency >= 1) {
			is_fading = false;
		}

	}

	public void Fading(string fade)
	{
		if (fade == "in") {
			StartCoroutine (fadeIn ());
		} else 
		{
			StartCoroutine (fadeOut ());
		}
	}

	public void ChangeImages (Texture2D tex) {
		this.lerp = 0.0f;
		this.plane_h.material.SetFloat ("_lerp",lerp);
		this.plane_h.material.SetTexture ("_MainTex", this.plane_h.material.GetTexture ("_SecTex"));
		this.plane_h.material.SetTexture ("_SecTex", tex);
	}

	public void ChangeImagesSingle(Texture2D tex) {
		this.lerp = 0.0f;
		this.plane_h.material.SetFloat ("_lerp",lerp);
		this.plane_h.material.SetTexture ("_MainTex", tex);
	}
	public void ChangeImagesSingleSelect(Texture2D tex) {
		this.lerp = 0.0f;
		this.plane_h.material.SetFloat ("_lerp",lerp);
		this.plane_h.material.SetTexture ("_MainTex", tex);
		StartCoroutine (fades ());
	}

	private IEnumerator fades()
	{
		yield return StartCoroutine (fadeIn ());
		yield return new WaitForSeconds (0.5f);
		yield return StartCoroutine (fadeOut ());
	}

	public IEnumerator TransitionOne() {
		Debug.Log (this.lerp);
		while (lerp <= 1) {
			this.plane_h.material.SetFloat ("_lerp", lerp);
			lerp += 0.02f;
			yield return null;
		}
	} 
}
