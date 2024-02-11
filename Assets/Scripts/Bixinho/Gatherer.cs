using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : BixinhoBase
{
    [Header("Gatherer Signals")]
    public Signal OnFruitSpotted;
    public Signal OnFruitCollected;

    [SerializeField] private List<float> speedByLevel;
    [SerializeField] private Animator animator;
    private Fruit currentFruitTarget;


    protected override void Start()
    {
        base.Start();

        NavigationAgent.speed = speedByLevel[0];
    }

    protected override IEnumerator ManageActivation()
    {
        while (true)
        {
            // Espera até que hajam frutas no chão para ativar.
            yield return new WaitUntil(() => SetTargetedFruit());

            Activate();
            activated = true;

            yield return new WaitUntil(() => activated == false);
        }
    }

    protected override void Activate()
    {
        // Define como destino a posição da fruta-alvo.
        var position = currentFruitTarget.transform.position;
        NavigationAgent.SetDestination(position);
        animator.SetBool("Walking", true);
    }

    public override void LevelUp()
    {
        base.LevelUp();

        NavigationAgent.speed = speedByLevel[Level];
    }

    /// <summary>
    /// Verifica se existe alguma fruta no chão e, caso verdadeiro, escolhe uma como alvo.
    /// </summary>
    /// <returns>Se há frutas no chão.</returns>
    private bool SetTargetedFruit()
    {
        var fruits = Bonsai.FruitsOnTheGround;
        currentFruitTarget = null;

        if (fruits.Count == 0) return false;
        else
        {
            currentFruitTarget = fruits[Random.Range(0, fruits.Count)];
            Bonsai.FruitsOnTheGround.Remove(currentFruitTarget);
            currentFruitTarget.OnDespawn += RemoveCurrentTarget;
            OnFruitSpotted.Fire();
            return true;
        }
    }

    private void RemoveCurrentTarget(Fruit fruit)
    {
        Debug.Log(currentFruitTarget);
        currentFruitTarget.OnDespawn -= RemoveCurrentTarget;
        activated = false;
        NavigationAgent.SetDestination(transform.position);
        animator.SetBool("Walking", false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent<Fruit>(out var fruit) && activated)
        {
            // Caso a fruta encontrada não seja a fruta alvo, retorne.
            if (fruit != currentFruitTarget) return;

            // Caso seja a fruta alvo, remove ela do mapa e define o bixinho como inativo (necessário
            // para que ele se mantenha no loop de fazer a ação)
            OnFruitCollected.Fire();
            currentFruitTarget.RemoveFruit(true);
            RemoveCurrentTarget(currentFruitTarget);
        }
    }
}
