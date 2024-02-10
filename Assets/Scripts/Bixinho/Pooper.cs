using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooper : BixinhoBase
{
    [SerializeField] private int fertilizationAmount;
    [SerializeField] private float poopDuration = 1f;
    private Bonsai bonsai;
    private WaitForSeconds waitToChangeDestination = new WaitForSeconds(1.5f);
    private WaitForSeconds waitForPoopDuration;

    protected override void Start()
    {
        base.Start();

        bonsai = GameMaster.GetManager<Bonsai>();
        waitForPoopDuration = new WaitForSeconds(poopDuration);
        StartCoroutine(HandleMovement());
    }

    protected override void Activate()
    {
        StartCoroutine(HandleActivation());
    }

    private IEnumerator HandleActivation()
    {
        // Aumenta a fertilização do bonsai e desativa o agente de navegação, parando o boneco.
        bonsai.CurrentFertilization += fertilizationAmount;
        NavigationAgent.isStopped = true;

        yield return waitForPoopDuration;
        
        // Depois da cagada, ativa o agente de navegação e define o bixinho como inativo (necessário
        // para que ele se mantenha no loop de fazer a ação).
        activated = false;
        NavigationAgent.isStopped = false;
    }

    /// <summary>
    /// Define pontos aleatorios de movimento em intervalos definidos. É completamente placeholder.
    /// </summary>
    /// <returns></returns>
    private IEnumerator HandleMovement()
    {
        while(true)
        {
            Vector3 rdm = Random.insideUnitCircle.normalized * 5.5f;
            rdm = new Vector3(rdm.x, 0, rdm.y);

            NavigationAgent.SetDestination(rdm);

            yield return waitToChangeDestination;
        }
    }
}
