using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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

    public GameObject blocker;
    public Canvas effectCanvas;
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;

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
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, blackBoardSquareSprites[UnityEngine.Random.Range(0, blackBoardSquareSprites.Count)]);
                    else
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, whiteBoardSquareSprites[UnityEngine.Random.Range(0, whiteBoardSquareSprites.Count)]);
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
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, blackBoardSquareSprites[UnityEngine.Random.Range(0, blackBoardSquareSprites.Count)]);
                    else
                        gameData.boardSquares[j, i].SetBoardSquare(j, i, whiteBoardSquareSprites[UnityEngine.Random.Range(0, whiteBoardSquareSprites.Count)]);
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
        blocker.SetActive(true);
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
                GameManager.instance.soundManager.PlaySFX("Destroy");
                dstPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                dstPiece.GetComponent<ChessPiece>().MakeAttackedEffect();
                killTriggerFlag = true;
            }

            yield return null;
        }

        srcPiece.transform.position = destPosition;
        blocker.SetActive(false);
    }

    // 공격 후 처치 실패 애니메이션
    public void ForthBackPieceAnimation(ChessPiece srcPiece, ChessPiece dstPiece)
    {
        blocker.SetActive(true);
        Vector2 destPosition = GetPositionUsingCoordinate(dstPiece.coordinate);

        srcPiece.GetComponent<Animator>().SetTrigger("returnTrigger");
        StartCoroutine(ForthBackPieceAnimationC(srcPiece, srcPiece.transform.position, destPosition, srcPiece.GetComponent<ChessPiece>().moveDuration, dstPiece));
    }

    IEnumerator ForthBackPieceAnimationC(ChessPiece srcPiece, Vector2 startPos, Vector2 destPos, float duration, ChessPiece dstPiece)
    {
        yield return StartCoroutine(MovePieceAnimationC(srcPiece, startPos, destPos, duration));
        GameManager.instance.soundManager.PlaySFX("Attack");
        StartCoroutine(AttackedAnimationC(dstPiece));
        yield return StartCoroutine(MovePieceAnimationC(srcPiece, destPos, startPos, duration));
        blocker.SetActive(false);
    }

    // 공격당하는 애니메이션
    public void AttackedAnimation(ChessPiece objPiece)
    {
        blocker.SetActive(true);
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
        blocker.SetActive(false);
    }

    public void KillByCardEffect(GameObject projectilePrefab, ChessPiece srcPiece, ChessPiece dstPiece)
    {
        blocker.SetActive(true);

        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);

        //FadeIn
        DOVirtual.Float(colorAdjustments.postExposure.value, -2f, 0.3f, (value) =>
        {
            colorAdjustments.postExposure.Override(value);
        }).SetEase(Ease.InOutSine).OnComplete(() => {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = srcPiece.transform.position;
            projectile.transform.DOMove(dstPiece.transform.position, 0.7f).SetEase(Ease.InOutQuint).OnComplete(() => {
                GameManager.instance.soundManager.PlaySFX("Destroy");
                dstPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                dstPiece.MakeAttackedEffect();
                dstPiece.Kill();
                Destroy(projectile);
                blocker.SetActive(false);

                //FadeOut
                DOVirtual.Float(colorAdjustments.postExposure.value, 0f, 0.3f, (value) =>
                {
                    colorAdjustments.postExposure.Override(value);
                }).SetEase(Ease.InOutSine);
            });
        });
    }

    public void TileEffect(GameObject tileEffectPrefab, ChessPiece objPiece)
    {
        GameObject tileEffectObj = Instantiate(tileEffectPrefab);
        tileEffectObj.transform.position = objPiece.transform.position;
        tileEffectObj.transform.parent = effectCanvas.transform;

        Image image = tileEffectObj.GetComponent<Image>();

        DOVirtual.Float(0f, 1f, 0.5f, (value) => {
            image.color = new Color(image.color.r, image.color.g, image.color.b, value);
        }).OnComplete(() => {
            DOVirtual.Float(1f, 0f, 0.5f, (value) => {
            image.color = new Color(image.color.r, image.color.g, image.color.b, value);
            }).OnComplete(() => {
                Destroy(tileEffectObj);
            });
        });
    }
}