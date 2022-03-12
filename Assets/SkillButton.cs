using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public Text thisText;
    public Image thisImage;
    public BaseSkill thisSkill;
    BattleManager bm;

    private void Start() {
        bm = BattleManager.instance;
    }

    public void ActivateSkill(){
        bm.selectedSkill = thisSkill;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bm.selectedPiece.transform.position, thisSkill.skillRange);
        Gizmos.color = Color.blue;
    }
}
