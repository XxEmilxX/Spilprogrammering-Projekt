using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private Animator animator;
    private Transform pCam;

    private void Start()
    {
        pCam = PlayerController.Instance().playerCam;
        animator = GetComponent<Animator>();
    }

    public void StartTransition()
    {
        TransitionCamera.Instance().onTransitionDone += TransitionAnimation_onTransitionDone;
        TransitionCamera.Instance().StartTransitionCamera(pCam, destination);
    }

    private void TransitionAnimation_onTransitionDone()
    {
        animator.SetTrigger("OutsideOpen");
        TransitionCamera.Instance().onTransitionDone -= TransitionAnimation_onTransitionDone;
    }

    public void OnAnimationFinished()
    {
        TransitionCamera.Instance().onTransitionDone += TransitionAnimation_onAnimationFinished;
        TransitionCamera.Instance().StartTransitionCamera(destination, pCam);
    }

    private void TransitionAnimation_onAnimationFinished()
    {
        TransitionCamera.Instance().onTransitionDone -= TransitionAnimation_onAnimationFinished;
    }
}
