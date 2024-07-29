using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execution : Effect
{
    public int basicAD;
    public int insteadAD;
    public int standardHP;

    public override void EffectAction()
    {
        foreach (var target in targets)
        {
            if ((target as ChessPiece).HP < standardHP)
            {
                (target as ChessPiece).HP -= insteadAD;
            }
            else
            {
                (target as ChessPiece).HP -= basicAD;
            }
            /*
                        if(!(target as ChessPiece).isAlive)
                        {
                            GameManager.instance.KillPiece(target as ChessPiece);
                        }
            */
        }
    }

}
