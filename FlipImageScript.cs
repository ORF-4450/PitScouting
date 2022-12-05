using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (!Button.GetComponent<Animation>().IsPlaying("ButtonPressed"))
        {
            Button.GetComponent<Animation>().Play("ButtonPressed");
            if (flipped == true)
            {
                GetComponent<Animation>().Play("SelectionButtonSpinCW");
            } else {
                GetComponent<Animation>().Play("SelectionButtonSpinCCW");
            }
            Animation anim = GetComponent<Animation>();
            flipped = !(flipped);
        }
        
    }
}