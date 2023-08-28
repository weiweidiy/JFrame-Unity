using System.Collections;
using System.Collections.Generic;
using Adic;
using JFrame.Game.Models;
using JFrame.Game.View;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [Inject]
    SceneController controller;

    [Inject]
    SceneTransitionController tController;

    [Inject]
    PlayerAccount account;
    // Start is called before the first frame update
    void Start()
    {
        this.Inject();

        var btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            tController.TransitionToScene("Battle");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
