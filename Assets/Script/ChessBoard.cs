using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    ChessData chessData;

    public Vector2 basePosition;


    //디버깅용
    public ChessPiece DebugPiece;
    private List<Vector2Int> DebugCoordinates;
    int debugindex = 0;
    float timer = 0;

    private void Awake()
    {
        foreach (ChessPiece piece in GetComponentsInChildren<ChessPiece>())
        {
            chessData.TryAddPiece(piece);

            piece.chessData = chessData;
            SetPositionByCoordinate(piece);
        }
    }

    private void Start()
    {
        //디버깅용
        DebugCoordinates = DebugPiece.GetMovableCoordinates();
    }

    private void Update()
    {

        //디버깅용
        Debug.Log(timer);
        if (timer > 1 && debugindex < DebugCoordinates.Count)
        {
            DebugPiece.Move(DebugCoordinates[debugindex]);
            SetPositionByCoordinate(DebugPiece);
            debugindex++;
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    private void SetPositionByCoordinate(ChessPiece chessPiece)
    {
        chessPiece.transform.position = (Vector2)chessPiece.coordinate + basePosition;
    }


}
