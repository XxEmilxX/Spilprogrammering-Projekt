using System.Collections;
using System.Linq;
using UnityEngine;

public class Monster : Singleton<Monster>
{
    [SerializeField] private Transform startPointsParent;
    [SerializeField] private Camera jumpscareCam;
    [SerializeField] private AudioListener jumpscareListener;
    [SerializeField] private Light jumpscareLight;
    [SerializeField] private float approximateTime = 15f;
    [SerializeField] private float approximateSpeed = 3f;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip jumpscareClip;

    private Vector3 currentStartPoint;
    private bool isAttacking;
    private bool firstTime = true;
    private Transform[] startPoints;
    private Animator animator;

    private void Start()
    {
        startPoints = startPointsParent.Cast<Transform>().ToArray();
        animator = GetComponent<Animator>();
        SetMonsterOnRandomPoint();
    }

    private void SetMonsterOnRandomPoint()
    {
        int randomPoint = Random.Range(0, startPoints.Length);
        currentStartPoint = startPoints[randomPoint].position;

        transform.SetPositionAndRotation(startPoints[randomPoint].position, startPoints[randomPoint].rotation);

        if(firstTime)
        {
            firstTime = false;
        }
        else
        {
            IncreaseDifficulty();
        }

        StartCoroutine(AttackPlayerAtRandomTime());
    }

    IEnumerator AttackPlayerAtRandomTime()
    {
        float randomTime = Random.Range(approximateTime / 1.5f, approximateTime * 1.5f);
        yield return new WaitForSeconds(randomTime);
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        float randomSpeed = Random.Range(approximateSpeed / 1.25f, approximateSpeed * 1.25f);

        while(isAttacking)
        {
            transform.position += transform.forward * randomSpeed * Time.deltaTime;
            yield return null;
        }

        while(Vector3.Distance(transform.position, currentStartPoint) > 1f)
        {
            transform.position -= transform.forward * randomSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        SetMonsterOnRandomPoint();
    }

    private void IncreaseDifficulty()
    {
        approximateSpeed += 0.75f;
        approximateTime /= 1.15f;
    }

    public void ScareMonster()
    {
        isAttacking = false;
    }

    public void DefeatMonster()
    {
        isAttacking = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Room"))
        {
            JumpscarePlayer();
        }
    }

    private void JumpscarePlayer()
    {
        StopAllCoroutines();
        Debug.Log("You got jumpscared!");

        TransitionCamera.Instance().StartTransitionCamera(PlayerController.Instance().playerCam, jumpscareCam.transform);
        TransitionCamera.Instance().onTransitionDone += Monster_onTransitionDone;
        jumpscareCam.enabled = true;
        jumpscareLight.enabled = true;
        jumpscareListener.enabled = true;
        PlayerController.Instance().gameObject.SetActive(false);
    }

    private void Monster_onTransitionDone()
    {
        animator.SetTrigger("Jumpscare");
        source.loop = false;
        source.clip = jumpscareClip;
        source.spatialBlend = 0f;
        source.Play();
        TransitionCamera.Instance().onTransitionDone -= Monster_onTransitionDone;
    }

    public void FinishJumpscare()
    {
        GameOverCanvas.Instance().GameOver();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
