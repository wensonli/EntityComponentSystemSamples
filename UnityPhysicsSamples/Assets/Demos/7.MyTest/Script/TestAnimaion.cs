using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimaion : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    void Start()
    {
        animator.Play("Bounce");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
