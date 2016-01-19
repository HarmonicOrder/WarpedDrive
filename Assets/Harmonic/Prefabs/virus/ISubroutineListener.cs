using UnityEngine;
using System.Collections;

public interface ISubroutineListener {
    void OnSubroutineActive(Subroutine sub);

    void OnSubroutineInactive(Subroutine sub);
}
