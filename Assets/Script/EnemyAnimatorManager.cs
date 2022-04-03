using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    public Animator anim;

    private void Awake()
    {
        this.anim = this.GetComponent<Animator>();
    }
}
