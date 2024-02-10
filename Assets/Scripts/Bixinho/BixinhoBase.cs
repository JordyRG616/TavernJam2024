using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class BixinhoBase : MonoBehaviour
{
    [SerializeField] protected float activationInterval;

    protected NavMeshAgent NavigationAgent { get; private set; }

    protected bool activated;
    protected WaitForSeconds waitActivationInterval;


    protected virtual void Start()
    {
        NavigationAgent = GetComponent<NavMeshAgent>();

        waitActivationInterval = new WaitForSeconds(activationInterval);
        StartCoroutine(ManageActivation());
    }

    /// <summary>
    /// Gerencia a ativação do bixinho. Por padrão, cada um tem um intervalo de ativação, depois do qual
    /// ele vai realizar sua ação e então retornar para o estado de idle.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator ManageActivation()
    {
        while (true)
        {
            yield return waitActivationInterval;

            Activate();

            // Define o bixinho como ativo. Este têm que ser definido como inativo (activated = false) 
            // em sua própria implementação, dependendo de como for sua ação.
            activated = true;
            yield return new WaitUntil(() => activated == false);
        }
    }

    /// <summary>
    /// A ação que o bixinho vai realizar sempre que for ativado.
    /// </summary>
    protected abstract void Activate();
}
