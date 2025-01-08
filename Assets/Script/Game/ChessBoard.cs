using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public Vector2 basePosition
    {
        get
        {
            return transform.position + new Vector3(0.5f, 0.5f, 0);
        }
    }
    public GameObject boardSquareSample;


    public List<Sprite> blackBoardSquareSprites = new List<Sprite>();
    public List<Sprite> whiteBoardSquareSprites = new List<Sprite>();


    public void SetBoardSquares(GameData gameData)
    {
        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
        {
            for (int i = 0; i < GameData.BOARD_SIZE; i++)
            {
                for (int j = 0; j < GameData.BOARD_SIZE; j++)
                {
                    gameData.boardSquares[j, i] = Instantiate<GameObject>(boardSquareSample, new Vector2(j, i) + basePosition, Quaternion.identity, transform).GetComponent<BoardSquare>();

                    if ((i + j) % 2 == 0)
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, blackBoardSquareSprites[Random.Range(0, blackBoardSquareSprites.Count)]);
                    else
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, whiteBoardSquareSprites[Random.Range(0, whiteBoardSquareSprites.Count)]);
                }
            }
        }
        else
        {
            for (int i = 0; i < GameData.BOARD_SIZE; i++)
            {
                for (int j = 0; j < GameData.BOARD_SIZE; j++)
                {
                    gameData.boardSquares[j, i] = Instantiate<GameObject>(boardSquareSample, new Vector2(7 - j, 7 - i) + basePosition, Quaternion.identity, transform).GetComponent<BoardSquare>();

                    if ((i + j) % 2 == 0)
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, blackBoardSquareSprites[Random.Range(0, blackBoardSquareSprites.Count)]);
                    else
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, whiteBoardSquareSprites[Random.Range(0, whiteBoardSquareSprites.Count)]);
                }
            }
        }
    }

    public Vector2 GetPositionUsingCoordinate(Vector2 coordinate)
    {
        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            return coordinate + basePosition;
        else
            return Vector2.one * 7 + basePosition - coordinate;
    }

    /* public void SetPiecePositionByCoordinate(ChessPiece chessPiece)
    {
        if (GameBoard.instance.playerColor == GameBoard.PlayerColor.White)
            chessPiece.transform.position = (Vector2)chessPiece.coordinate + basePosition;
        else
            chessPiece.transform.position = Vector2.one * 7 + basePosition - (Vector2)chessPiece.coordinate;
    } */

    public void MovePieceAnimation(ChessPiece chessPiece)
    {
        Vector2 destPosition = GetPositionUsingCoordinate(chessPiece.coordinate);

        chessPiece.GetComponent<Animator>().SetTrigger("moveTrigger");
        StartCoroutine(MovePieceAnimationC(chessPiece, chessPiece.transform.position, destPosition, chessPiece.GetComponent<ChessPiece>().moveDuration));
    }

    IEnumerator MovePieceAnimationC(ChessPiece chessPiece, Vector2 startPos, Vector2 destPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            chessPiece.transform.position = Vector2.Lerp(startPos, destPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chessPiece.transform.position = destPos;
    }

    public void AttackAnimation(ChessPiece srcPiece, ChessPiece dstPiece)
    {
        Vector2 destPosition = GetPositionUsingCoordinate(srcPiece.coordinate);

        srcPiece.GetComponent<Animator>().SetTrigger("moveTrigger");
        StartCoroutine(AttackAnimationC(srcPiece, srcPiece.transform.position, destPosition, srcPiece.GetComponent<ChessPiece>().moveDuration, dstPiece));
    }

    IEnumerator AttackAnimationC(ChessPiece srcPiece, Vector2 startPos, Vector2 destPos, float duration, ChessPiece dstPiece)
    {
        yield return StartCoroutine(MovePieceAnimationC(srcPiece, startPos, destPos, duration));
        dstPiece.GetComponent<Animator>().SetTrigger("attackedTrigger");
        dstPiece.GetComponent<ChessPiece>().MakeAttackedEffect();
    }

    public void BackForthPieceAnimation(ChessPiece srcPiece, ChessPiece dstPiece)
    {
        Vector2 destPosition = GetPositionUsingCoordinate(dstPiece.coordinate);

        srcPiece.GetComponent<Animator>().SetTrigger("returnTrigger");
        StartCoroutine(BackForthPieceAnimationC(srcPiece, srcPiece.transform.position, destPosition, srcPiece.GetComponent<ChessPiece>().moveDuration, dstPiece));
    }

    IEnumerator BackForthPieceAnimationC(ChessPiece srcPiece, Vector2 startPos, Vector2 destPos, float duration, ChessPiece dstPiece)
    {
        yield return StartCoroutine(AttackAnimationC(srcPiece, startPos, destPos, duration, dstPiece));
        yield return StartCoroutine(MovePieceAnimationC(srcPiece, destPos, startPos, duration));
    }
}