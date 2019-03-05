using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pieceSelectAlt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public float gaze_time = 2f;
	public float speed = 3.5f;
	public int appear_on_question;
	public float rotate_speed = 25f;
	public bool is_correct = true;
	public bool is_activated = false;
	public bool is_on_goal = false;
	public bool StartScene = false;
	public bool is_final = false;
	public Transform start_position;
	public Transform target;
	public AudioClip Audio;
	public Texture2D history;
	public Material final;

	private float rim = 2.0f;
	private float timer;
	private bool gazed_at = false;
	private bool is_moving = false;
	private bool is_bar_active = true;
	private bool call_activate_others_once = true;
	private bool is_on_start_position = false;
	private float my_distance = 0f;
	private float distance_from_start = 0f;
	private float x_angle;
	private pieceManager my_piece_manager;
	private GameObject load_bar;
	private Audio_manager AudioM;
	private MeshRenderer meshR;
	private Image_manager iMan;


	// Use this for initialization.
	void Start () {
		my_piece_manager =  GameObject.Find("PiecesManagerObj").GetComponent<pieceManager>();
		this.load_bar = this.transform.GetChild (0).gameObject;
		this.load_bar.SetActive (false);
		this.is_activated = false;
		this.meshR = this.GetComponentInChildren<MeshRenderer> ();
		this.AudioM = GameObject.FindGameObjectWithTag ("Audio").GetComponent<Audio_manager> ();
		this.iMan = GameObject.FindGameObjectWithTag ("History").GetComponent<Image_manager> ();
	}

	//Handle pointer enter.
	public void OnPointerEnter(PointerEventData data){
		if (this.is_activated) {
			this.gazed_at = true;
			meshR.material.SetFloat ("_True",1.0f);
			StartCoroutine (desapear ());
		}
	}

	//Handle pointer exit. 
	public void OnPointerExit(PointerEventData data){
		this.gazed_at = false;
		if (this.is_activated) {
			meshR.material.SetFloat ("_True",0.0f);
			meshR.material.SetFloat ("_RimPower",2.0f);
			this.rim = 2.0f;
		}
		StopAllCoroutines ();
	}

	// Update is called once per frame
	void Update () {

		if (this.appear_on_question == this.my_piece_manager.current_question && StartScene ) {

			my_distance = Vector3.Distance (this.target.position, this.transform.position);
			distance_from_start = Vector3.Distance (this.start_position.position, this.transform.position);
			Transform child = load_bar.transform;

			//move piece to start position:
			if (!this.is_on_start_position) {
				float step = (speed + 4f) * Time.deltaTime;
				this.transform.position = Vector3.MoveTowards (this.transform.position, this.start_position.position, step);
				if (this.distance_from_start <= 0.1f) {
					this.is_activated = true;
					this.is_on_start_position = true;
				} else {
					this.is_activated = false;
				}
			}

			/**/
			//Handle bar behaviour: 
			if (this.gazed_at && this.is_bar_active) {
				
				timer += Time.deltaTime;
				//load_bar.SetActive (true);

				Vector3 newScale = new Vector3 ((timer + 1) / gaze_time, child.localScale.y, child.localScale.z);
				Vector3 newPosition = new Vector3 ((timer / gaze_time) / 2, child.localPosition.y, child.localPosition.z);

				child.localScale = newScale;
				child.localPosition = newPosition;
				if (timer >= gaze_time) {
					//ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
					this.startMovement ();
					timer = 0f;
				}
			} else {
				timer = 0f;
				load_bar.SetActive (false);
			}
			/**/

			//Move piece to board: 
			if (this.is_moving) {
				float step = speed * Time.deltaTime;
				this.transform.position = Vector3.MoveTowards (this.transform.position, this.target.position, step);

				this.x_angle += Time.deltaTime * this.rotate_speed;
				this.transform.rotation = Quaternion.Euler (-this.x_angle, 0, 0);

				//Reached location at board:
				if (this.my_distance <= 0.1f) {
					this.is_on_goal = true;
					this.is_activated = false;
					this.transform.rotation = Quaternion.Euler (-90, 0, 0);
					this.meshR.material = final;
					//Debug.Log ("LLegué a mi destino");

					if (this.call_activate_others_once) {
						this.activateOthersOnce ();
					}
					if (this.is_correct) {
						//Trigger correct answer sequence.
						if(this.is_final){
							AudioM.is_Final ();
						}
						//play audio
						AudioM.PlayAudio(1,this.Audio);
						iMan.ChangeImagesSingleSelect (history);
						//display image.
						//when audio ends:
						this.my_piece_manager.activateAll ();
						this.my_piece_manager.changeQuestion ();

						//New question and set of pieces appears
					} else {
						//Trigger incorrect answer sequence.
						AudioM.PlayAudio(2,this.Audio);
						iMan.ChangeImagesSingleSelect (history);
						//play audio
						//display image
						//when audio ends:
						this.my_piece_manager.activateAll ();
						this.is_bar_active = true;

						Destroy (this.gameObject);
					}

					this.is_moving = false;
				}
			}
		} else if((this.my_piece_manager.current_question > this.appear_on_question)
			&& !this.is_correct){
			Destroy (this.gameObject);
		}

	}//END UPDATE.

	//Call active others from pieceManager once:
	private void activateOthersOnce(){
		this.call_activate_others_once = false;
		this.my_piece_manager.activateAll ();

	}

	private void startMovement(){
		this.is_moving = true;
		this.is_bar_active = false;
		this.my_piece_manager.deactivateOthers (this);
	}

	private IEnumerator desapear () {
		while (this.rim > 0.0f) {
			meshR.material.SetFloat ("_RimPower", this.rim);
			this.rim-=0.02f;
			yield return null;
		}
		yield return new WaitForSeconds (0.5f);		
	}

}
