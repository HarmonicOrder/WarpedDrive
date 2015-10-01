using UnityEngine;
using System.Collections;

public interface ILockTarget {

	void EnableLockedOnGui();
	void DisableLockedOnGui();

	Transform transform {get;}
}
