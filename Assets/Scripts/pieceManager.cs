using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceManager : MonoBehaviour {

	public List<pieceSelectAlt> pieces_list = new List<pieceSelectAlt>();
	public int current_question = 1;

	public void deactivateOthers(pieceSelectAlt current_object){
		foreach (pieceSelectAlt piece_in_list in this.pieces_list) {
			if (current_object != piece_in_list) {
				piece_in_list.is_activated = false;
			}
		}
	}

	public void activateAll(){
		foreach (pieceSelectAlt piece_in_list in this.pieces_list) {
			if (!piece_in_list.is_on_goal) {
				piece_in_list.is_activated = true;
			}
		}
	}

	public void changeQuestion(){
		foreach (pieceSelectAlt piece_in_list in this.pieces_list) {
			if ((piece_in_list.appear_on_question == this.current_question) &&
				!piece_in_list.is_correct) {
				//Destroy (piece_in_list);
			}
		}
		this.current_question++;
	}
		
	// Use this for initialization
	void Start () {
		//this.current_question = 1;
	}

	/*
	// Update is called once per frame
	void Update () {		
	}
	*/
}
