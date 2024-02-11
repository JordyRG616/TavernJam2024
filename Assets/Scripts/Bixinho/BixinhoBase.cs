using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class BixinhoBase : MonoBehaviour
{
    [Header("Bixinho Signals")]
    public Signal OnLevelUp;
    public Signal OnSpawn;

    [SerializeField] protected float activationInterval;
    [field:SerializeField] public BixinhoType type { get; protected set; }

    public int Level { get; protected set; }
    protected int maxLevel = 4;
    public bool IsMaxLevel => Level == maxLevel;

    protected NavMeshAgent NavigationAgent { get; private set; }

    protected bool activated;
    protected WaitForSeconds waitActivationInterval;
    

    protected virtual void Start()
    {
        GameMaster.GetManager<RanchManager>().RegisterBixinho(this);

        NavigationAgent = GetComponent<NavMeshAgent>();
        waitActivationInterval = new WaitForSeconds(activationInterval);
        StartCoroutine(ManageActivation());
    }

    private void OnEnable()
    {
        OnSpawn.Fire();
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

    public virtual void LevelUp()
    {
        if (Level == maxLevel) return;

        OnLevelUp.Fire();
        Level++;
    }

    /// <summary>
    /// A ação que o bixinho vai realizar sempre que for ativado.
    /// </summary>
    protected abstract void Activate();
}

public enum BixinhoType { Gatherer, Headbanger, Pooper}