using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Animator anim, shildAnim;
    // public BossMechanics bossMechanics;
    public Image[] hearts;
    public PlayableDirector[] timelines;
    public Sprite fullHeart, emptyHeart;
    public int health = 6;
    public float immunity = 10f;

    private bool iCanTakeDamage = true;

    // private void Start() 
    // {
    //     ChekHert();
    // }
    // public void GetDamage(){
    //     if(!iCanTakeDamage){
    //         return;
    //     }
    //     health--;
    //     anim.SetTrigger("GetDamage");

    //     ChekHert();

    //     if(health <= 0){
    //         timelines[2].Play();
    //     }else{
    //         StartCoroutine(Immunity());
    //     }

    //     if(health == 5){
    //         bossMechanics.UseAllAngry(10);
    //     }
    //     if(health == 4){
    //         bossMechanics.coldounSpawn = 0.7f;
    //         bossMechanics.StartCoroutine(bossMechanics.InfinitySDZones(999));
    //         bossMechanics.UseAllAngry(15);
    //         immunity = 30f;
    //     }
    //     if(health == 3){
    //         timelines[0].Play();
    //     }
    //     if(health == 2){
    //         bossMechanics.coldounSpawnZones = 1f;
    //         bossMechanics.coldounSpawn = 0.6f;
    //         bossMechanics.StartCoroutine(bossMechanics.InfinityAdd());
    //         bossMechanics.UseAllAngry(25);
    //         immunity = 55f;
    //     }
    //     if(health == 1){
    //         bossMechanics.pause = true;
    //         timelines[1].Play();
    //     }
    // }
    // IEnumerator Immunity(){
    //     shildAnim.SetBool("Spinning", true);
    //     iCanTakeDamage = false;

    //     yield return new WaitForSeconds(immunity);

    //     shildAnim.SetBool("Spinning", false);
    //     iCanTakeDamage = true;
    // }
    // public void ChekHert(){
    //     for (int i = 0; i < 6; i++){
    //         if(health > i){
    //             hearts[i].sprite = fullHeart;
    //         }else{
    //             hearts[i].sprite = emptyHeart;
    //         }
    //     }
    // }

    public void AttackPlayer(){
        anim.SetTrigger("Damage");
    }
}
