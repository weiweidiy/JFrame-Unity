using System.Collections;
using System.Collections.Generic;
using Adic;
using JFrame.Game.Model;
using JFrame.Game.View;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [Inject]
    SceneController controller;

    [Inject]
    PlayerAccount account;
    // Start is called before the first frame update
    void Start()
    {
        this.Inject();

        var btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            controller.SwitchToBattle(account);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
