using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public int positionX;
    public int positionY;
    Camera _main;
    public GameObject availableMarker;
    public List<Tuple<int, int>> availableMovements = new List<Tuple<int, int>>();
    public Piece selectedPiece;


    public int currentTurn;

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

        GetCharacterInitiative();
        LoadCharacter();
        NextInLine();
    }

    // Update is called once per frame
    void Update()
    {

        if (isSelected)
        {
            MovePiece();
        }
        if (!string.IsNullOrEmpty(selectedPiece.GetComponent<CharacterStats>().selectedSkill?.skillName))
        {
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


        foreach (var charStat in charStatQueue)
        {

            var thisChar = charStat.GetComponent<CharacterStats>();
            foreach (var skill in thisChar.avaiableSkills)
            {

                var currentSkill = allSkills.Find(x => x.skillName == skill);
                thisChar.availableSkills.Add(new BaseSkill(currentSkill));
                Debug.Log(currentSkill);
            }
        }
    }
    public void GenerateButtons()
    {

        DestroySkillButtons();
        var thisChar = selectedPiece.GetComponent<CharacterStats>();
        for (int i = 0; i < thisChar.availableSkills.Count; i++)
        {
            var currentSkill = thisChar.availableSkills[i];

            var turnsLeft = currentSkill.turnLastUsed == null ? 0 : ((currentSkill.cooldown + currentSkill.turnLastUsed) - thisChar.currentTurn);
            var buttonText = currentSkill.skillName;
            if (turnsLeft > 0)
            {
                buttonText += $"cd:{turnsLeft}";
            }
            Debug.Log($"{currentSkill.skillName} Turns left: " + turnsLeft);
            var go = Instantiate(skillButtonObject, buttonTarget).GetComponent<SkillButton>();
            go.GetComponent<Button>().interactable = turnsLeft > 0 ? false : true;
            go.thisText.text = buttonText;
            go.thisImage.sprite = currentSkill.skillIcon;
            go.thisSkill = currentSkill;

        }
    }
    private void NextInLine()
    {
        SelectPiece(charStatQueue[charIndex].GetComponent<Piece>());
        GenerateButtons();
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

            var currentCharacter = selectedPiece.GetComponent<CharacterStats>();
            if (hit.collider.gameObject.GetComponent<Piece>() == null)
            {
                return;
            }
            else
            {
                var target = hit.collider.gameObject.GetComponent<Piece>();
                float distance = Vector3.Distance(currentCharacter.transform.position, target.transform.position);
                if (target != null && currentCharacter.selectedSkill.skillRange >= distance)
                {
                    //Dodamages
                    targetedPiece = target;
                    targetedPiece.GetComponent<CharacterStats>().TakeDamage(currentCharacter.selectedSkill.power);
                    if (currentCharacter.selectedSkill.cooldown > 0)
                    {
                        currentCharacter.selectedSkill.turnLastUsed = currentCharacter.currentTurn;
                    }

                    ClearSelections();
                    GenerateButtons();
                }
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
                DestroyMarkers();
            }
        }
    }
    public void ClearSelections()
    {
        selectedPiece.GetComponent<CharacterStats>().selectedSkill = null;
        targetedPiece = null;
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
        selectedPiece.GetComponent<CharacterStats>().currentTurn++;
        selectedPiece.hasMoved = false;
        selectedPiece.thisRenderer.material = selectedPiece.normalMaterial;
        availableMovements.Clear();
        charIndex++;
        if (charIndex == charStatQueue.Count)
        {
            charIndex = 0;
            currentTurn++;
        }
        DestroyMarkers();
        NextInLine();
    }
    public float calcPos(int position)
    {
        return position * 2.5f;
    }
}
