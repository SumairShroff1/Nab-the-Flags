using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int score;
    public TMP_Text scoreText;
    public Transform points;
    public Health health;
    public GameObject[] flags;
    public GameObject sceleton;


    private void Start() {
        foreach (GameObject Flag in flags)
        {
            ChangeFlagLocation(Flag);
        }
    }
    private void SpawnSceletons(){
        int Rd = Random.Range(0, points.childCount);

        Instantiate(sceleton, points.GetChild(Rd).position, Quaternion.identity).SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Flag"){
            AddScore();
            ChangeFlagLocation(other.transform.parent.gameObject);
        }
        if(other.tag == "Bone"){
            health.GetDamage();
        }
    }
    private void AddScore(){
        score++;
        scoreText.text = "Score: " + score;

        if(score > 70){
            return;
        }

        if(score % 2 == 0){
            SpawnSceletons();
        }
    }
    private void ChangeFlagLocation(GameObject _Flag){
        int Rd = Random.Range(0, points.childCount);

        GameObject FlagPoint = points.GetChild(Rd).gameObject;

        if(FlagPoint.name == "Used"){
            ChangeFlagLocation(_Flag);
        }else{
            FlagPoint.name = "Used";

            int LastParsedIndex; 
            if (int.TryParse(_Flag.name, out LastParsedIndex))
            {
                if(LastParsedIndex >= 0){
                    points.GetChild(LastParsedIndex).gameObject.name = "Point"; 
                }
            }
            _Flag.name = Rd + "";
            _Flag.transform.position = FlagPoint.transform.position;
        }
    }
}
