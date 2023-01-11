using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script flips the position selection area using one of two animations.
//One spins clockwise (CW), the other spins it counterclockwise (CCW)

public class FlipImageScript : MonoBehaviour
{
    public GameObject Button;
    // Start is called before the first frame update
    void Start()
    {
        foreach (AnimationState state in Button.GetComponent<Animation>())
        {
            state.speed = 2F;
        }
        foreach (AnimationState state in GetComponent<Animation>())
        {
            state.speed = 4F;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool flipped = true;

    public void Flip()
    {
        if (!Button.GetComponent<Animation>().IsPlaying("ButtonPressed")) //If the button NOT is playing it's "pressed" animation
        {
            Button.GetComponent<Animation>().Play("ButtonPressed"); //Play the button animation
            if (flipped == true) //If that was true, Find out if it has been flipped or not
            {
                GetComponent<Animation>().Play("SelectionButtonSpinCW"); //If it has been flipped, unflip it
            } else {
                GetComponent<Animation>().Play("SelectionButtonSpinCCW"); //If it hasn't been flipped, flip it
            }

            Animation anim = GetComponent<Animation>(); //Not sure if this does anything anymore.
            
            flipped = !flipped; //Change the boolean for flipping to tell the script whether or not the buttons have been flipped.
        }
        
    }
}