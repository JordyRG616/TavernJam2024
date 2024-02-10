using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : BixinhoBase
{
    private Fruit currentFruitTarget;


    protected override IEnumerator ManageActivation()
    {
        while (true)
        {
            // Espera at� que hajam frutas no ch�o para ativar.
            yield return new WaitUntil(() => SetTargetedFruit());

            Activate();
            activated = true;

            yield return new WaitUntil(() => activated == false);
        }
    }

    protected override void Activate()
    {
        // Define como destino a posi��o da fruta-alvo.
        var position = currentFruitTarget.transform.position;
        NavigationAgent.SetDestination(position);
    }

    /// <summary>
    /// Verifica se existe alguma fruta no ch�o e, caso verdadeiro, escolhe uma como alvo.
    /// </summary>
    /// <returns>Se h� frutas no ch�o.</returns>
    private bool SetTargetedFruit()
    {
        var fruits = Bonsai.FruitsOnTheGround;
        currentFruitTarget = null;

        if (fruits.Count == 0) return false;
        else
        {
            currentFruitTarget = fruits[Random.Range(0, fruits.Count)];
            Bonsai.FruitsOnTheGround.Remove(currentFruitTarget);
            return true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent<Fruit>(out var fruit))
        {
            // Caso a fruta encontrada n�o seja a fruta alvo, retorne.
            if (fruit != currentFruitTarget) return;

            // Caso seja a fruta alvo, remove ela do mapa e define o bixinho como inativo (necess�rio
            // para que ele se mantenha no loop de fazer a a��o)
            currentFruitTarget.RemoveFruit(true);
            currentFruitTarget = null;
            activated = false;
        }
    }
}
