using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IWKE {

	interface IUserInterfaceElement {
		Animator UIAnimator {
			get;
		}
		void Show();
		void Hide();
	}

	public delegate void UIHandler();

}
