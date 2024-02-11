using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PoopController : MonoBehaviour, IPointerClickHandler
{
    [Header("Signals")]
    public Signal OnPoopPooped;
    public Signal OnPoopCollected;
    public Signal OnPoopDespawn;

    [SerializeField] private int fertilizationAmount;
    [SerializeField] private int lifetime;

    private static WaitForSeconds waitToRemove;
    private Animator animator;

    private IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        if (waitToRemove == null)
        {
            waitToRemove = new WaitForSeconds(lifetime);
        }

        OnPoopPooped.Fire();
        yield return waitToRemove;

        animator.SetTrigger("Die");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameMaster.GetManager<Bonsai>().CurrentFertilization += fertilizationAmount;

        OnPoopCollected.Fire();
        Destroy(gameObject);
    }

    public void Destroy()
    {
        OnPoopDespawn.Fire();
        Destroy(gameObject);
    }
}
