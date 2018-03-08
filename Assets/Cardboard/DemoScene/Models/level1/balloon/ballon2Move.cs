using UnityEngine;
using System.Collections;

public class ballon2Move : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        iTween.MoveBy(this.gameObject, iTween.Hash("y",-4,"easetype","linear","time",100));
    }
	void Update () {
	
	}
}
