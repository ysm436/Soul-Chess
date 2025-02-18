using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Tween FadeInTween()
    {
        blocker.SetActive(true);
        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);
        Debug.Log("fadeIn");
        return DOVirtual.Float(colorAdjustments.postExposure.value, -1f, 0.3f, (value) =>
            {
                colorAdjustments.postExposure.Override(value);
            }).SetEase(Ease.InOutSine);
    }

    public Tween FadeOutTween()
    {
        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);

        Debug.Log("fadeOut");
        return DOVirtual.Float(-1, 0f, 0.3f, (value) =>
            {
                colorAdjustments.postExposure.Override(value);
            }).SetEase(Ease.InOutSine).OnComplete(() => {
                blocker.SetActive(false);
            });
    }

    public void KillByCardEffect(GameObject projectilePrefab, ChessPiece srcPiece, ChessPiece dstPiece)
    {
        blocker.SetActive(true);

        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);

        //FadeIn
        DOVirtual.Float(colorAdjustments.postExposure.value, -1f, 0.3f, (value) =>
        {
            colorAdjustments.postExposure.Override(value);
        }).SetEase(Ease.InOutSine).OnComplete(() => {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = srcPiece.transform.position;
            projectile.SetActive(true);
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

    public void DamageByCardEffect(GameObject projectilePrefab, ChessPiece srcPiece, ChessPiece dstPiece, int damage)
    {
        blocker.SetActive(true);

        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);

        //FadeIn
        DOVirtual.Float(colorAdjustments.postExposure.value, -1f, 0.3f, (value) =>
        {
            colorAdjustments.postExposure.Override(value);
        }).SetEase(Ease.InOutSine).OnComplete(() => {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = srcPiece.transform.position;
            projectile.transform.DOMove(dstPiece.transform.position, 0.7f).SetEase(Ease.InOutQuint).OnComplete(() => {
                dstPiece.MinusHP(damage);
                if (dstPiece.isAlive)
                {
                    GameManager.instance.soundManager.PlaySFX("Attack");
                    GameBoard.instance.chessBoard.AttackedAnimation(dstPiece);
                }
                else
                {
                    GameManager.instance.soundManager.PlaySFX("Destroy");
                    dstPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                    dstPiece.MakeAttackedEffect();
                }
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
        tileEffectObj.transform.SetParent(effectCanvas.transform, true);

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

    public void DamageByThunderEffect(GameObject thunderEffect, List<ChessPiece> targetList, int damage)
    {
        blocker.SetActive(true);
        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);

        Sequence thunderSequence = DOTween.Sequence();

        Tween tr1 = DOVirtual.Float(colorAdjustments.postExposure.value, -1f, 0.3f, (value) =>
        {
            colorAdjustments.postExposure.Override(value);
        }).SetEase(Ease.InOutSine);
        thunderSequence.Append(tr1);

        Tween tr2(ChessPiece target)
        {
            GameManager.instance.soundManager.PlaySFX("Electricity");
            GameObject thunderObj = Instantiate(thunderEffect);
            thunderObj.transform.position = target.transform.position;

            return DOVirtual.DelayedCall(0.3f, () => {
                target.MinusHP(damage);
                if (target.isAlive)
                {
                    GameManager.instance.soundManager.PlaySFX("Attack");
                    GameBoard.instance.chessBoard.AttackedAnimation(target);
                }
                else
                {
                    GameManager.instance.soundManager.PlaySFX("Destroy");
                    target.GetComponent<Animator>().SetTrigger("killedTrigger");
                    target.MakeAttackedEffect();
                }
                DOVirtual.DelayedCall(0.2f, ()=> {
                    Destroy(thunderObj);
                });
            });
        }
        targetList.Reverse();
        foreach (var target in targetList)
        {
            thunderSequence.Append(tr2(target));
            thunderSequence.Append(DOVirtual.DelayedCall(0.2f, () => {}));
        }
        
        Tween tr3 = DOVirtual.Float(colorAdjustments.postExposure.value, 0f, 0.3f, (value) =>
        {
            colorAdjustments.postExposure.Override(value);
        }).SetEase(Ease.InOutSine).OnComplete(() => {
            blocker.SetActive(false);
        });
        thunderSequence.Append(tr3);
    }

    public void DamageByPoseidonEffect(GameObject poseidonEffect, ChessPiece srcPiece, List<ChessPiece> targetList, int damage)
    {
        blocker.SetActive(true);
        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);

        GameObject effectParent = Instantiate(poseidonEffect.transform.GetChild(0).gameObject);
        GameObject poseidonEffect1 = effectParent.transform.GetChild(0).gameObject;
        GameObject poseidonEffect2 = effectParent.transform.GetChild(1).gameObject;
        GameObject poseidonEffect3 = effectParent.transform.GetChild(2).gameObject;
        GameObject projectile = poseidonEffect.transform.GetChild(1).gameObject;

        Material effectMaterial = poseidonEffect1.GetComponent<Renderer>().sharedMaterial;

        Sequence poseidonSequence = DOTween.Sequence();

        Tween poseidonFadeIn = DOVirtual.Float(colorAdjustments.postExposure.value, -1f, 0.3f, (value) =>
        {
            colorAdjustments.postExposure.Override(value);
            effectMaterial.SetFloat("_Alpha", 1 - Mathf.InverseLerp(-1f, 0f, value));
        }).SetEase(Ease.InOutSine);
        poseidonSequence.Append(poseidonFadeIn);

        Tween peMove1 = poseidonEffect1.transform.DOLocalMoveX(-15, 5); // velocity = 3
        Tween peMove2 = poseidonEffect2.transform.DOLocalMoveX(15, 5);
        Tween peMove3 = poseidonEffect3.transform.DOLocalMoveX(-15, 5);
        poseidonSequence.Join(peMove1);
        poseidonSequence.Join(peMove2);
        poseidonSequence.Join(peMove3);

        Tween Sound = DOVirtual.DelayedCall(1.7f, () => {
            GameManager.instance.soundManager.PlaySFX("Water");
        });
        poseidonSequence.Join(Sound);

        Tween projectileMove(ChessPiece target)
        {
            return DOVirtual.DelayedCall(1.7f, () => {
                GameObject projectileObj = Instantiate(projectile);
                projectileObj.SetActive(true);
                projectileObj.transform.position = srcPiece.transform.position;
                projectileObj.transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.InOutQuint).OnComplete(() => {
                    target.MinusHP(damage);
                    if (target.isAlive)
                    {
                        GameManager.instance.soundManager.PlaySFX("Attack");
                        GameBoard.instance.chessBoard.AttackedAnimation(target);
                    }
                    else
                    {
                        GameManager.instance.soundManager.PlaySFX("Destroy");
                        target.GetComponent<Animator>().SetTrigger("killedTrigger");
                        target.MakeAttackedEffect();
                    }
                    Destroy(projectileObj);
                });
            });
        }
        
        foreach (var target in targetList)
        {
            poseidonSequence.Join(projectileMove(target));
        }

        Tween poseidonFadeOut = DOVirtual.DelayedCall(2.6f, () => {
            DOVirtual.Float(colorAdjustments.postExposure.value, 0f, 0.3f, (value) =>
            {
                colorAdjustments.postExposure.Override(value);
                effectMaterial.SetFloat("_Alpha", 1 - Mathf.InverseLerp(-1f, 0f, value));
            }).SetEase(Ease.InOutSine).OnComplete(() => {
                Destroy(effectParent);
                blocker.SetActive(false);
                poseidonSequence.Kill();
            });
        });
        poseidonSequence.Join(poseidonFadeOut);
    }

    public Tween DamageByHephaestusEffect(GameObject hephaestusEffect, ChessPiece srcPiece, int damage)
    {
        Tween hepheastusTween = null;

        hepheastusTween = DOVirtual.DelayedCall(0f, () => {
            List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece =>
                piece.soul != null).ToList();

            if (enemyPieceList.Count == 0)
            {
                hepheastusTween.Kill();
                Debug.Log("Hephaestus: No Target");
                return;
            }
            Debug.Log(enemyPieceList.Count());

            GameObject effectParent = Instantiate(hephaestusEffect.transform.GetChild(0).gameObject);
            GameObject fireEffect = effectParent.transform.GetChild(0).gameObject;
            GameObject fireEffect2 = effectParent.transform.GetChild(1).gameObject;

            GameObject projectile = hephaestusEffect.transform.GetChild(1).gameObject;
            Material effectMaterial = fireEffect.GetComponent<Renderer>().sharedMaterial;

            DOVirtual.Float(0f, 1f, 0.3f, (value) =>
            {
                effectMaterial.SetFloat("_Alpha", value);
            }).SetEase(Ease.InOutSine).OnComplete(() => {
                GameManager.instance.soundManager.PlaySFX("Fire");
                foreach (var target in enemyPieceList)
                {
                    GameObject projectileObj = Instantiate(projectile);
                    projectileObj.SetActive(true);
                    projectileObj.transform.position = srcPiece.transform.position;
                    projectileObj.transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.InOutQuint).OnComplete(() => {
                        target.MinusHP(damage);
                        if (target.isAlive)
                        {
                            GameManager.instance.soundManager.PlaySFX("Attack");
                            GameBoard.instance.chessBoard.AttackedAnimation(target);
                        }
                        else
                        {
                            GameManager.instance.soundManager.PlaySFX("Destroy");
                            target.GetComponent<Animator>().SetTrigger("killedTrigger");
                            target.MakeAttackedEffect();
                        }
                        Destroy(projectileObj);
                    });
                }

                DOVirtual.DelayedCall(1f, () => {
                    Destroy(effectParent);
                    DOVirtual.Float(1f, 0f, 0.3f, (value) =>
                    {
                        effectMaterial.SetFloat("_Alpha", value);
                    });
                });
            });
        });

        return hepheastusTween;
    }

    public Tween DamageByThorEffect(GameObject projectileEffect, ChessPiece srcPiece, int damage)
    {
        Tween thorTween = null;

        thorTween = DOVirtual.DelayedCall(0f, () => {
            List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece =>
            piece.pieceColor != srcPiece.pieceColor && piece.soul != null).ToList(); //영혼 부여된 기물만 제거

            if (enemyPieceList.Count == 0)
            {
                thorTween.Kill();
                Debug.Log("Thor: No Target");
                return;
            }
            ChessPiece dstPiece = enemyPieceList[SynchronizedRandom.Range(0, enemyPieceList.Count)];
            // 기절한 경우 2배의 피해
            if (dstPiece.GetKeyword(Keyword.Type.Stun) == 1)
            {
                damage *= 2;
            }

            GameObject projectile = Instantiate(projectileEffect);
            projectile.transform.position = srcPiece.transform.position;
            projectile.transform.DOMove(dstPiece.transform.position, 0.7f).SetEase(Ease.InOutQuint).OnComplete(() => {
                dstPiece.MinusHP(damage);
                if (dstPiece.isAlive)
                {
                    GameManager.instance.soundManager.PlaySFX("Attack");
                    GameBoard.instance.chessBoard.AttackedAnimation(dstPiece);
                }
                else
                {
                    GameManager.instance.soundManager.PlaySFX("Destroy");
                    dstPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                    dstPiece.MakeAttackedEffect();
                }
                Destroy(projectile);
            });
        });

        return thorTween;
    }

    public void KillByTitanomachiaEffect(GameObject projectileEffect, ChessPiece srcPiece, int repeat)
    {
        Sequence titanomachiaSequence = DOTween.Sequence();

        titanomachiaSequence.Append(FadeInTween());

        Tween titanoKillEffect()
        {
            return DOVirtual.DelayedCall(0f, () => {
                List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects.Where(piece =>
                piece.pieceType != ChessPiece.PieceType.King).ToList();

                if (pieceList.Count != 0)
                {
                    ChessPiece dstPiece = pieceList[SynchronizedRandom.Range(0, pieceList.Count())];
                    GameObject projectile = Instantiate(projectileEffect);
                    projectile.transform.position = srcPiece.transform.position;
                    projectile.transform.DOMove(dstPiece.transform.position, 0.3f).SetEase(Ease.InOutQuint).OnComplete(() => {
                        GameManager.instance.soundManager.PlaySFX("Destroy");
                        dstPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                        dstPiece.MakeAttackedEffect();
                        dstPiece.Kill();
                        Destroy(projectile);
                    });
                }
            });
        };

        for (int i = 0; i < repeat; i++)
        {
            titanomachiaSequence.Append(titanoKillEffect());
            titanomachiaSequence.AppendInterval(0.5f);
        }
        
        titanomachiaSequence.Append(FadeOutTween());
    }

    public void DamageByKrakenEffect(GameObject projectileEffect, ChessPiece srcPiece, int repeat, int damage)
    {
        Sequence krakenSequence = DOTween.Sequence();
        Vector3 srcPosition = srcPiece.transform.position;

        krakenSequence.Append(FadeInTween());

        Tween krakenKillEffect()
        {
            return DOVirtual.DelayedCall(0f, () => {
                List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects.Where(piece =>
                piece.pieceColor != srcPiece.pieceColor).ToList();

                if (pieceList.Count != 0)
                {
                    Debug.Log(pieceList.Count());
                    ChessPiece dstPiece = pieceList[SynchronizedRandom.Range(0, pieceList.Count())];
                    GameManager.instance.soundManager.PlaySFX("Water");
                    GameObject projectileObj = Instantiate(projectileEffect);
                    projectileObj.transform.position = srcPosition;
                    projectileObj.transform.DOMove(dstPiece.transform.position, 0.3f).SetEase(Ease.InOutQuint).OnComplete(() => {
                        dstPiece.MinusHP(damage);
                        if (dstPiece.isAlive)
                        {
                            GameManager.instance.soundManager.PlaySFX("Attack");
                            GameBoard.instance.chessBoard.AttackedAnimation(dstPiece);
                        }
                        else
                        {
                            GameManager.instance.soundManager.PlaySFX("Destroy");
                            dstPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                            dstPiece.MakeAttackedEffect();
                        }
                        Destroy(projectileObj);
                    });
                }
            });
        };

        for (int i = 0; i < repeat; i++)
        {
            krakenSequence.Append(krakenKillEffect());
            krakenSequence.AppendInterval(0.6f);
        }
        
        krakenSequence.Append(FadeOutTween());
    }

    public void CheckEffect(GameObject checkUI)
    {
        checkUI.SetActive(true);
        if (colorAdjustments == null)
            volume.profile.TryGet(out colorAdjustments);
        
        Color startColor = colorAdjustments.colorFilter.value;
        Color targetColor = new Color(1f, 0.588f, 0.588f);
        Color tempColor;
        Color originStartColor = startColor;
        Color originTargetColor = targetColor;

        Sequence redFilterSeq = DOTween.Sequence();

        Tween redFilter1 = DOTween.To(() => startColor, x => 
        {
            tempColor = x;
            colorAdjustments.colorFilter.value = tempColor;
        }, originTargetColor, 0.5f);

        Tween redFilter2 = DOTween.To(() => targetColor, x => 
            {
                Debug.Log(x);
                tempColor = x;
                colorAdjustments.colorFilter.value = tempColor;
            }, originStartColor, 0.5f);

        Tween checkOff = DOVirtual.DelayedCall(0f, () => {
                checkUI.SetActive(false);
            });
        
        redFilterSeq.Append(redFilter1);
        redFilterSeq.Append(redFilter2);
        redFilterSeq.Append(checkOff);
    }

    public List<ChessPiece> GetAllPieces(GameBoard.PlayerColor color)
    {
        List<ChessPiece> chessPieces = new List<ChessPiece>();

        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            ChessPiece piece = transform.GetChild(i).GetComponent<ChessPiece>();
            if (piece == null || !piece.isAlive)
                continue;
            if (piece.pieceColor == color)
            {
                chessPieces.Add(piece);
            }
        }

        return chessPieces;
    }
}