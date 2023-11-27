using System.Collections;
using UnityEngine;

public class TransitionCamera : Singleton<TransitionCamera>
{
    private Vector3 destPos;
    private Quaternion destRot;

    private Camera cam;
    private Transform origin;
    private Transform trans;
    private Transform playerCam;
    private AudioListener aL;

    private float transitionProcess = 0f;
    private Vector3 transitionPos;
    private Quaternion transitionRot;

    [SerializeField] [Tooltip("Transition takes 1 second. This will be multiplied on that.")] private float transitionSpeed = 1f;

    public delegate void TransitionDoneAction();
    public event TransitionDoneAction onTransitionDone;

    private void Start()
    {
        trans = transform;

        aL = GetComponent<AudioListener>();
        cam = GetComponent<Camera>();
        playerCam = PlayerController.Instance().playerCam;
    }

    public void StartTransitionCamera(Transform origin, Transform destination)
    {
        this.origin = origin;
        destPos = destination.position;
        destRot = destination.rotation;

        aL.enabled = true;
        cam.enabled = true;
        GetComponent<AudioListener>().enabled = true;

        StartCoroutine(TransitionToDestination());
    }

    public void StopTransitionCamera()
    {
        StartCoroutine(TransitionToPlayerCam());
    }

    IEnumerator TransitionToDestination()
    {
        transitionProcess = 0;
        while(transitionProcess < 1)
        {
            transitionPos = Vector3.Lerp(origin.position, destPos, transitionProcess);
            transitionRot = Quaternion.Lerp(origin.rotation, destRot, transitionProcess);

            trans.position = transitionPos;
            trans.rotation = transitionRot;

            transitionProcess += transitionSpeed * Time.deltaTime;
            yield return null;
        }

        trans.position = destPos;
        trans.rotation = destRot;
        aL.enabled = false;
        cam.enabled = false;
        GetComponent<AudioListener>().enabled = false;
        onTransitionDone?.Invoke();
    }

    IEnumerator TransitionToPlayerCam()
    {
        transitionProcess = 0;
        while (transitionProcess < 1)
        {
            transitionPos = Vector3.Lerp(destPos, playerCam.position, transitionProcess);
            transitionRot = Quaternion.Lerp(destRot, playerCam.rotation, transitionProcess);

            trans.position = transitionPos;
            trans.rotation = transitionRot;

            transitionProcess += transitionSpeed * Time.deltaTime;
            yield return null;
        }

        cam.enabled = false;

        trans.position = destPos;
        trans.rotation = destRot;
    }
}
