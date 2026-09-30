using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BenchIntroSequence : MonoBehaviour
{
    [Header("Intro Dummy Characters")]
    public Animator pandaAnimator;
    public Animator bearAnimator;

    public GameObject pandaDummy;
    public GameObject bearDummy;


    [Header("Dialogue Boxes")]
    public GameObject pandaTextbox;
    public GameObject bearTextbox;


    [Header("Animation Controls")]
    public string bearNoAnimation = "No";
    public string pandaGetOffBool = "GetOff";

    public string pandaGetOffStateName = "GetOff";


    [Header("Intro Camera")]
    public Camera introCamera;


    [Header("Run Path")]
    public Transform panda;

    public List<Transform> runWaypoints;

    public float pandaRunSpeed = 4f;


    [Header("Timing")]
    public float startDelay = 2f;
    public float dialogueDuration = 2f;


    void Start()
    {
        StartCoroutine(IntroSequence());
    }


    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(startDelay);


        // Panda asks for kiss
        pandaTextbox.SetActive(true);

        yield return new WaitForSeconds(dialogueDuration);


        // Bear says NO
        pandaTextbox.SetActive(false);

        bearTextbox.SetActive(true);

        bearAnimator.CrossFade(bearNoAnimation, 0.1f);

        yield return new WaitForSeconds(dialogueDuration);


        bearTextbox.SetActive(false);


        // Panda gets off bench
        pandaAnimator.SetBool(pandaGetOffBool, true);


        // wait until GetOff animation starts
        yield return new WaitUntil(() =>
            pandaAnimator
            .GetCurrentAnimatorStateInfo(0)
            .IsName(pandaGetOffStateName)
        );


        // wait until GetOff animation finishes
        yield return new WaitUntil(() =>
            pandaAnimator
            .GetCurrentAnimatorStateInfo(0)
            .normalizedTime >= 1f
        );


        // now panda begins moving through maze path
        yield return StartCoroutine(RunPath());


        // disable intro-only dummy characters
        pandaDummy.SetActive(false);
        bearDummy.SetActive(false);


        // disable intro camera
        if(introCamera != null)
            introCamera.enabled = false;
    }


    IEnumerator RunPath()
    {
        foreach(Transform wp in runWaypoints)
        {
            while(Vector3.Distance(
                panda.position,
                wp.position
            ) > 0.15f)
            {
                panda.position =
                    Vector3.MoveTowards(
                        panda.position,
                        wp.position,
                        pandaRunSpeed * Time.deltaTime
                    );

                panda.forward =
                    (wp.position - panda.position).normalized;

                yield return null;
            }
        }
    }
}