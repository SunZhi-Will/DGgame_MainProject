using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterCaller : MonoBehaviour {

    public instantiateEffectCaller EffectCaller;
    public string parameterCaller;
    public Animator thisAnimator;

	// Use this for initialization
	void Start ()
    {
        thisAnimator = gameObject.GetComponent<Animator>();
        EffectCaller = gameObject.GetComponent<instantiateEffectCaller>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            thisAnimator.SetTrigger(parameterCaller);
            EffectCaller.ResetTimers();
            EffectCaller.fired = true;
        }
    }
}
