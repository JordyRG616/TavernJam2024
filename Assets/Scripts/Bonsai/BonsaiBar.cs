using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonsaiBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image barFill;
    [SerializeField] Bonsai bonsaiTree;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        levelText.text = "Level " + bonsaiTree.Level.ToString();
        barFill.fillAmount = bonsaiTree.BonsaiBarFillAmount();
    }
}
