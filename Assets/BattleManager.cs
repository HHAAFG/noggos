using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public int positionX;
    public int positionY;
    public bool whitePlayer;
    Camera _main;
    public GameObject availableMarker;
    public List<Tuple<int, int>> availableMovements = new List<Tuple<int, int>>();
    public Piece selectedPiece;

    public BaseSkill selectedSkill;

    public Piece targetedPiece;
    bool isSelected;
    private List<Piece> pieceLocations;
    public List<CharacterStats> charStatQueue;

    private int charIndex;

    public List<BaseSkill> allSkills = new List<BaseSkill>();

    RaycastHit hit;
    Ray ray;

    public GameObject skillButtonObject;
    public Transform buttonTarget;
    // Start is called before the first frame update
    void Start()
    {

        isSelected = false;
        _main = Camera.main;
        instance = this;
        // whitePlayer = true;
        GetCharacterInitiative();
        NextInLine();
    }

    // Update is called once per frame
    void Update()
    {

        if (isSelected)
        {
            MovePiece();
        }
        if(selectedSkill != null){
            SetTarget();
        }
    }

    public void DestroySkillButtons()
    {
        var buttons = FindObjectsOfType<SkillButton>();

        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
    }
    public void LoadCharacter()
    {

        DestroySkillButtons();
        var thisChar = selectedPiece.GetComponent<CharacterStats>();
        for (int i = 0; i < thisChar.avaiableSkills.Count; i++)
        {
            string skillName = thisChar.avaiableSkills[i];
            var currentSkill = allSkills.Find(x => x.skillName == skillName);

            var go = Instantiate(skillButtonObject, buttonTarget).GetComponent<SkillButton>();
            go.thisText.text = currentSkill.skillName;
            go.thisImage.sprite = currentSkill.skillIcon;
            go.thisSkill = currentSkill;
        }
    }
    private void NextInLine()
    {
        SelectPiece(charStatQueue[charIndex].GetComponent<Piece>());
        LoadCharacter();
    }

    private void GetCharacterInitiative()
    {

        foreach (var charStat in Board.instance.pieces.OrderByDescending(x => x.gameObject.GetComponent<CharacterStats>().initiative))
        {

            charStatQueue.Add(charStat.GetComponent<CharacterStats>());
        }

    }

    public void ShowAvailableSpots()
    {
        DestroyMarkers();
        // TODO 
        // Ta bort distinctfulkoden
        if (!selectedPiece.hasMoved)
        {
            foreach (var availMov in availableMovements.Distinct())
            {
                int targetX = availMov.Item1;
                int targetY = availMov.Item2;

                Vector3 newPosition = new Vector3(targetX * 2.5f, 0, targetY * 2.5f);
                var marker = Instantiate(availableMarker, newPosition, Quaternion.identity).GetComponent<MarkerGrid>();
                marker.x = targetX;
                marker.y = targetY;

            }
        }

    }

    public void SelectPiece(Piece currentPiece)
    {

        selectedPiece = currentPiece;
        //Changes color to selected
        selectedPiece.thisRenderer.material = selectedPiece.selectedMaterial;

        positionX = selectedPiece.x;
        positionY = selectedPiece.y;

        Board.instance.GetLoc();
        pieceLocations = Board.instance.pieceLocations;

        selectedPiece.Movement();
        AvailableMovement(selectedPiece.gridX, selectedPiece.gridY, pieceLocations);
        isSelected = true;

    }

    public void MousePointer()
    {

        ray = _main.ScreenPointToRay(Input.mousePosition);

    }
    public void SetTarget()
    {

        MousePointer();
        if (Physics.Raycast(ray, out hit) && Input.GetKeyUp(KeyCode.Mouse0))
        {
            var target = hit.collider.gameObject.GetComponent<Piece>();
            if (target != null)
            {
                targetedPiece = target;
                Debug.Log(targetedPiece);
            }
        }

    }

    public void MovePiece()
    {
        MousePointer();
        if (Physics.Raycast(ray, out hit) && Input.GetKeyUp(KeyCode.Mouse0) && isSelected)
        {
            var target = hit.collider.gameObject.GetComponent<MarkerGrid>();
            if (target != null)
            {
                selectedPiece.transform.position = new Vector3(calcPos(target.x), 0, calcPos(target.y));
                selectedPiece.x = target.x;
                selectedPiece.y = target.y;
                selectedPiece.hasMoved = true;
                isSelected = false;
                whitePlayer = !whitePlayer;
                DestroyMarkers();
            }
        }
    }
    public void DestroyMarkers()
    {
        var markers = FindObjectsOfType<DestroyMarker>();

        foreach (var marker in markers)
        {
            marker.SelfDestruct();
        }
    }

    public void AvailableMovement(int[] pMoveX, int[] pMoveY, List<Piece> pieceLocations)
    {
        int xCombined = 0;
        int yCombined = 0;

        for (int i = 0; i < pMoveX.Length; i++)
        {
            bool isClear = true;
            //Innanför brädet
            if (InBounds(pMoveX[i], positionX) && InBounds(pMoveY[i], positionY))
            {
                foreach (var pieceLocation in pieceLocations)
                {
                    xCombined = pMoveX[i] + positionX;
                    yCombined = pMoveY[i] + positionY;
                    //Står en medspelarpjäs i vägen
                    if (xCombined == pieceLocation.x && yCombined == pieceLocation.y)
                    {
                        isClear = false;
                    }
                }
                if (isClear)
                {
                    availableMovements.Add(new Tuple<int, int>(xCombined, yCombined));
                }
            }
        }
    }

    public bool InBounds(int move, int position)
    {
        if (move + position >= 0 && move + position <= 17)
        {
            return true;
        }
        return false;
    }
    public void EndTurn()
    {
        //Changes color to normal
        selectedPiece.hasMoved = false;
        selectedPiece.thisRenderer.material = selectedPiece.normalMaterial;
        availableMovements.Clear();
        charIndex++;
        if (charIndex == charStatQueue.Count)
        {
            charIndex = 0;
        }
        DestroyMarkers();
        NextInLine();
    }
    public float calcPos(int position)
    {
        return position * 2.5f;
    }
}
