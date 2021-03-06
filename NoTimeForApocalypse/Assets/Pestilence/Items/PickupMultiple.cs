﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PickupMultiple : MonoBehaviour {

    public string itemName;
    public string setTag;
    public float decayTime = 0;
    [Range(0, 1)]public float probability = 1;
    
    private NpcUi ui;
    private bool active = false;

    // Use this for initialization
    void Start() {
        ui = NpcUi.current;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButton("Submit") && active) {
            if(probability!=1)Random.InitState(System.Environment.TickCount);
            if (probability == 1 || Random.value < probability){
                ExampleVariableStorage.current.SetTag(setTag);
                DialogueRunner.current.SwitchNode.Invoke();
            }
            if(decayTime > 0)
                StartCoroutine(decay());
            else
                gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            ui.Show((ExampleVariableStorage.current.IsTag(setTag)?"destroy ":"pick up ") + itemName, "");
            ui.SetActive(transform, true, new Vector2(0, 2f));
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            ui.SetActive(transform, false);
            active = false;
        }
    }

    IEnumerator decay(){
        ui.SetActive(transform, false);
        active = false;
        float startTime = Time.time;
        while(startTime + decayTime > Time.time){
            transform.localScale = new Vector3(1, 1-(Time.time - startTime)/decayTime, 1);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
