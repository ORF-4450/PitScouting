using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Attach to anything with a button
//Button must have scale of 1 by 1




public class ButtonMash : MonoBehaviour
{
    [SerializeField] private AnimationClip animClip;
    private GameObject animStore; //GameObject storing animations
    private Animation anim;
    private string animName = "ButtonMashAnimation"; //Doesn't matter what this is
    private Button buttonComp;

    // Start is called before the first frame update
    void Start()
    {
    //#Add animation and button component
        this.gameObject.AddComponent<Animation>();
        if (this.gameObject.GetComponent<Button>() == null)
        {
            this.gameObject.AddComponent<Button>();
        }
    //#Set Variables
        anim = GetComponent<Animation>();
        animStore = GameObject.Find("AnimationHolder");
        animClip = animStore.GetComponent<animStore>().buttonMashAnim;
        buttonComp = GetComponent<Button>();
    //#Setup Animation
        anim.AddClip(animClip,"ButtonMashAnimation");
        anim.playAutomatically = false;
        //#Change Speed
            foreach (AnimationState state in anim)
            {
                state.speed = 2F;
            }
    //#Setup Button
        buttonComp.onClick.AddListener(ButtonPressAnim);
    }

    void ButtonPressAnim()
    {
        anim.Play(animName); //Plays animation
    }
}