using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : Singleton<TestCamera>
{
    public GameObject Target;
    public GameObject T;
    public float Speed = 1.5f;
    public int Index;
    [SerializeField] private GameObject FireWorkParticle;

    private void Start()
    {
    }
    private void FixedUpdate()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        T = GameObject.FindGameObjectWithTag("Target");
        this.transform.LookAt(Target.transform);
        float car_Move = Mathf.Abs(Vector3.Distance(this.transform.position, T.transform.position) * Speed);
        this.transform.position = Vector3.MoveTowards(this.transform.position, T.transform.position, car_Move * Time.deltaTime);
    }
    IEnumerator LoadPlayer()
    {
        yield return new WaitForSeconds(1f);
        Target = GameObject.FindGameObjectWithTag("Player");
        T = GameObject.FindGameObjectWithTag("Target");
    }
    public void ActiveFireWorkParticle()
    {
        FireWorkParticle.SetActive(true);
    }
    public void InActiveFireWorkParticle()
    {
        FireWorkParticle.SetActive(false);
    }
}
