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
            for (int i = 0; i < gameData.BOARD_SIZE_HEIGHT; i++)
            {
                for (int j = 0; j < gameData.BOARD_SIZE_WIDTH; j++)
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
            for (int i = 0; i < gameData.BOARD_SIZE_HEIGHT; i++)
            {
                for (int j = 0; j < gameData.BOARD_SIZE_WIDTH; j++)
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

    // 기물 단순 이동
    public void MovePieceAnimation(ChessPiece chessPiece)
    {
        Vector2 destPosition = GetPositionUsingCoordinate(chessPiece.coordinate);

        chessPiece.GetComponent<Animator>().SetTrigger("moveTrigger");
        StartCoroutine(MovePieceAnimationC(chessPiece, chessPiece.transform.position, destPosition, chessPiece.GetComponent<ChessPiece>().moveDuration));
    }

    IEnumerator MovePieceAnimationC(ChessPiece chessPiece, Vector2 startPos, Vector2 destPos, float duration)
    {
        float elapsedTime = 0f;
        ChessPiece chessPieceComponent = chessPiece.GetComponent<ChessPiece>();

        while (elapsedTime < duration)
        {
            float t = chessPieceComponent.speedCurve.Evaluate(elapsedTime / duration);
            chessPiece.transform.position = Vector2.Lerp(startPos, destPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chessPiece.transform.position = destPos;
    }

    // 처치 성공 애니메이션
    public void KillAnimation(ChessPiece srcPiece, ChessPiece dstPiece)
    {
        StartCoroutine(KillAnimationC(srcPiece, dstPiece));
    }

    IEnumerator KillAnimationC(ChessPiece srcPiece, ChessPiece dstPiece)
    {
        Vector2 destPosition = GetPositionUsingCoordinate(srcPiece.coordinate);
        bool killTriggerFlag = false;
        float elapsedTime = 0f;

        ChessPiece srcPieceComponent = srcPiece.GetComponent<ChessPiece>();
        
        while (elapsedTime < srcPiece.moveDuration)
        {
            float t = srcPieceComponent.speedCurve.Evaluate(elapsedTime / srcPieceComponent.moveDuration);
            srcPiece.transform.position = Vector2.Lerp(srcPiece.transform.position, destPosition, t);

            elapsedTime += Time.deltaTime;

            if (!killTriggerFlag && Vector2.Distance(srcPiece.transform.position, dstPiece.transform.position) < 3)
            {
                dstPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                dstPiece.GetComponent<ChessPiece>().MakeAttackedEffect();
                killTriggerFlag = true;
            }

            yield return null;
        }

        srcPiece.transform.position = destPosition;
    }

    // 공격 후 처치 실패 애니메이션
    public void ForthBackPieceAnimation(ChessPiece srcPiece, ChessPiece dstPiece)
    {
        Vector2 destPosition = GetPositionUsingCoordinate(dstPiece.coordinate);

        srcPiece.GetComponent<Animator>().SetTrigger("returnTrigger");
        StartCoroutine(ForthBackPieceAnimationC(srcPiece, srcPiece.transform.position, destPosition, srcPiece.GetComponent<ChessPiece>().moveDuration, dstPiece));
    }

    IEnumerator ForthBackPieceAnimationC(ChessPiece srcPiece, Vector2 startPos, Vector2 destPos, float duration, ChessPiece dstPiece)
    {
        yield return StartCoroutine(MovePieceAnimationC(srcPiece, startPos, destPos, duration));
        StartCoroutine(AttackedAnimationC(dstPiece));
        yield return StartCoroutine(MovePieceAnimationC(srcPiece, destPos, startPos, duration));
    }

    // 공격당하는 애니메이션
    public void AttackedAnimation(ChessPiece objPiece)
    {
        StartCoroutine(GameBoard.instance.chessBoard.AttackedAnimationC(objPiece));
    }


    public IEnumerator AttackedAnimationC(ChessPiece dstPiece)
    {
        Animator dstPieceAnimator = dstPiece.GetComponent<Animator>();
        dstPieceAnimator.SetTrigger("attackedTrigger");
        dstPieceAnimator.SetBool("isVibrated", true);
        dstPiece.GetComponent<ChessPiece>().MakeAttackedEffect();
        yield return new WaitForSeconds(0.5f);
        dstPieceAnimator.SetBool("isVibrated", false);
        dstPiece.transform.position = GetPositionUsingCoordinate(dstPiece.coordinate);
    }
}