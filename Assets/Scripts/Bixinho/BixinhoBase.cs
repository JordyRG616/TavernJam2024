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
    /// Gerencia a ativa��o do bixinho. Por padr�o, cada um tem um intervalo de ativa��o, depois do qual
    /// ele vai realizar sua a��o e ent�o retornar para o estado de idle.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator ManageActivation()
    {
        while (true)
        {
            yield return waitActivationInterval;

            Activate();

            // Define o bixinho como ativo. Este t�m que ser definido como inativo (activated = false) 
            // em sua pr�pria implementa��o, dependendo de como for sua a��o.
            activated = true;
            yield return new WaitUntil(() => activated == false);
        }
    }

    /// <summary>
    /// A a��o que o bixinho vai realizar sempre que for ativado.
    /// </summary>
    protected abstract void Activate();
}
