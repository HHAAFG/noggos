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

    public Piece targetedPiece;
    bool isSelected;
    private List<Piece> pieceLocations;
    public List<CharacterStats> charStatQueue;

    private int charIndex;


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
    }

    public void CheckEnemyInRange()
    {
        DestroyMarkers();
        foreach(var piece in Board.instance.pieces){
            var p = piece.GetComponent<Piece>(); 
            var sp = selectedPiece.GetComponent<CharacterStats>();
            if(p != selectedPiece){
                    if(Vector3.Distance(selectedPiece.transform.position, p.transform.position) < sp.attackRange){
                        Debug.Log(piece.gameObject.GetComponent<CharacterStats>().attackRange);
                        var cp = p.GetComponent<CharacterStats>();
                        cp.TakeDamage(sp.attack);
                        
                    }
                    
            }
            
        }
    
    }

    private void NextInLine(){
        //

        SelectPiece(charStatQueue[charIndex].GetComponent<Piece>());

    }

    private void GetCharacterInitiative(){
        
        foreach(var charStat in Board.instance.pieces.OrderByDescending(x => x.gameObject.GetComponent<CharacterStats>().initiative))
        {

            charStatQueue.Add(charStat.GetComponent<CharacterStats>());
        }
        
    }

    public void ShowAvailableSpots()
    {
        DestroyMarkers();
        // TODO 
        // Ta bort distinctfulkoden
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

    public void MovePiece()
    {
        RaycastHit hit;
        Ray ray = _main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit) && Input.GetKeyDown(KeyCode.Mouse0) && isSelected)
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

    public bool CanPawnAttack()
    {
        return true;
    }

    public void DestroyEnemy()
    {
        foreach (var piece in pieceLocations)
        {
            var jude = FindObjectsOfType<Piece>().ToList();
            var cp = jude.Where(x => x.x == selectedPiece.x && x.y == selectedPiece.y && x.white != whitePlayer).FirstOrDefault();
            if(cp != null)
            {
             
                cp.gameObject.SetActive(false);
            }
            
        }
        // var enemy = pieceLocations.Where(piece => piece.x == positionX && piece.y == positionY && piece.white != whitePlayer).FirstOrDefault();

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
        selectedPiece.thisRenderer.material = selectedPiece.normalMaterial;
        availableMovements.Clear();
        charIndex++;
        if(charIndex == charStatQueue.Count){
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
