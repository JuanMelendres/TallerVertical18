using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Desapear : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler {
	private float rim=2.0f;
	private MeshRenderer mat;
	private Transform size;
	// Use this for initialization
	void Start () {
		mat = GetComponent<MeshRenderer> ();
		size = this.transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter(PointerEventData data)
	{
		mat.material.color = Color.red;
		mat.material.SetFloat ("_True",1.0f);
		StartCoroutine (desapear ());

	}

	private IEnumerator desapear ()
	{
		while (this.rim > 0.0f) 
		{
			mat.material.SetFloat ("_RimPower", this.rim);
			this.rim-=0.01f;
			yield return null;

		}

		yield return new WaitForSeconds (0.5f);

	}

	public void OnPointerExit(PointerEventData data)
	{
		mat.material.color = Color.green;
		mat.material.SetFloat ("_True",0.0f);
		mat.material.SetFloat ("_RimPower",2.0f);
		size.localScale = Vector3.one;
		this.rim = 2.0f;
		StopAllCoroutines ();
	}
}
