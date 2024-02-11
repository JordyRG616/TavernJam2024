using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooper : BixinhoBase
{
    [Header("Pooper Signals")]
    public Signal OnPoop;

    [SerializeField] private List<float> intervalByLevel;
    [SerializeField] private GameObject poopModel;
    [SerializeField] private float poopDuration = 1f;
    [SerializeField] private float stepSize;
    [SerializeField] private Animator animator;
    private WaitForSeconds waitIncrement = new WaitForSeconds(0.01f);
    private WaitForSeconds waitForPoopDuration;

    protected override void Start()
    {
        base.Start();

        waitActivationInterval = new WaitForSeconds(intervalByLevel[0]);
        waitForPoopDuration = new WaitForSeconds(poopDuration);
        StartCoroutine(HandleMovement());
    }

    protected override void Activate()
    {
        StartCoroutine(HandleActivation());
    }

    public override void LevelUp()
    {
        base.LevelUp();

        waitActivationInterval = new WaitForSeconds(intervalByLevel[Level]);
    }

    private IEnumerator HandleActivation()
    {
        // Aumenta a fertilização do bonsai e desativa o agente de navegação, parando o boneco.
        NavigationAgent.isStopped = true;
        animator.SetBool("Pooping", true);

        yield return waitForPoopDuration;

        DropADeuce();

        // Depois da cagada, ativa o agente de navegação e define o bixinho como inativo (necessário
        // para que ele se mantenha no loop de fazer a ação).
        activated = false;
        NavigationAgent.isStopped = false;
        animator.SetBool("Pooping", false);
    }

    private void DropADeuce()
    {
        OnPoop.Fire();
        Instantiate(poopModel, transform.position, Quaternion.identity);
    }

    public void DoStep()
    {
        StopCoroutine(HandleMovement());
        StartCoroutine(HandleMovement());
    }

    private IEnumerator HandleMovement()
    {
        var step = 0f;
        Vector3 rdm = Random.insideUnitCircle.normalized;
        rdm = new Vector3(rdm.x, 0, rdm.y);
        rdm += transform.forward * stepSize;

        NavigationAgent.SetDestination(rdm);

        while(step < 1)
        {
            var speed = Mathf.Lerp(1.5f, 0, step);
            NavigationAgent.speed = speed;

            step += 0.02f;

            yield return waitIncrement;
        }
    }
}
