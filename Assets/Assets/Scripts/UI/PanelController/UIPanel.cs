using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IWKE;

public class UIPanel : MonoBehaviour, IUserInterfaceElement
{

	private Animator anim;
	public event UIHandler OnHide = () => { };

	public Animator UIAnimator
	{
		get
		{
			if(anim == null)
				anim = GetComponent<Animator>();
			return anim;
		}
	}

	public void Show()
	{
		//UIAnimator.SetTrigger("Open");
		UIAnimator.SetBool("IsOpen", true);
	}

	public void Hide()
	{
		//UIAnimator.SetTrigger("Close");
		UIAnimator.SetBool("IsOpen", false);
		//OnHide();
	}
}
