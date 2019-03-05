using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu_manager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {
	public bool is_play;
	public GameObject the_other;
	private Audio_manager audio;
	private bool end_game=false;
	private float lerp= 0.0f;
	private MeshRenderer meshR;
	public void Awake()
	{
		this.audio = GameObject.FindGameObjectWithTag ("Audio").GetComponent<Audio_manager> ();

	}
	public void Start()
	{
		this.meshR = this.GetComponent<MeshRenderer> ();
	}
	public void OnPointerEnter(PointerEventData data)
	{
		print ("hi");
		StartCoroutine (changeTex ());
	}

	public void OnPointerExit(PointerEventData data)
	{
		lerp = 0.0f;
		meshR.material.SetFloat("_Radial", lerp);
		StopAllCoroutines ();
	}

	private IEnumerator changeTex()
	{
		while(lerp<=1.2f)
		{
			meshR.material.SetFloat("_Radial", lerp);
			lerp += 0.02f;
			yield return null;
		}
		if (is_play) {
			
			this.audio.PlayAudio (0, default(AudioClip));
			the_other.SetActive (false);
			this.gameObject.SetActive(false);
		} else {
			
			Application.Quit();
		}
	}
}
