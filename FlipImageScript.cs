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
    //Animation Speed
        foreach (AnimationState state in Button.GetComponent<Animation>()) //Change speeds
        {
            state.speed = 2F;
        }
        foreach (AnimationState state in GetComponent<Animation>())
        {
            state.speed = 4F;
        }
    //
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool flipped = true;

    public void Flip()
    {
    //Button Press Animation
        Animation animButton = Button.GetComponent<Animation>();
        Animation anim = GetComponent<Animation>();
        if (!animButton.IsPlaying("ButtonPressed")) //If the button NOT is playing it's "pressed" animation
        {
            animButton.Play("ButtonPressed"); //Play the button animation
    //Pick Correct Spin Animation
            if (flipped == true) //If that was true, Find out if it has been flipped or not
            {
                anim.Play("SelectionButtonSpinCW"); //If it has been flipped, unflip it
            } else {
                anim.Play("SelectionButtonSpinCCW"); //If it hasn't been flipped, flip it
            }
    //  {
            flipped = !flipped; //Change the boolean for flipping to tell the script whether or not the buttons have been flipped.
        }
        
    }
}