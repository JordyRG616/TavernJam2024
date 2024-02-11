using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanchManager : ManagerBehaviour
{
    [SerializeField] private List<BixinhoBase> prefabs;

    public static Dictionary<BixinhoType, List<BixinhoBase>> BixinhosByType { get; private set; } = new Dictionary<BixinhoType, List<BixinhoBase>>();
    

    public void RegisterBixinho(BixinhoBase bixinho)
    {
        List<BixinhoBase> list = null;
        if (BixinhosByType.ContainsKey(bixinho.type))
        {
            list = BixinhosByType[bixinho.type];
        } else
        {
            list = new List<BixinhoBase>();
            BixinhosByType.Add(bixinho.type, list);
        }

        list.Add(bixinho);
    }

    public int GetBixinhoAmount(BixinhoType type)
    {
        return BixinhosByType[type].Count;
    }

    public void UpgradeRandomBixinho(BixinhoType type)
    {
        var model = prefabs.Find(x => x.type == type);
        model.LevelUp();

        if (!BixinhosByType.ContainsKey(type)) return;

        var list = BixinhosByType[type].FindAll(x => x.IsMaxLevel == false);

        if (list.Count == 0) return;

        var rdm = Random.Range(0, list.Count);
        var bixinho = list[rdm];
        bixinho.LevelUp();
    }
}
