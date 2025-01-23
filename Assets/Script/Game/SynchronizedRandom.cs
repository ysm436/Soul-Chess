using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SynchronizedRandom : MonoBehaviour
{
    PhotonView photonView;
    static int[] randomSeedArray = new int[100];
    static int index = 0;

    public void Init(bool isHost)
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene") return;
        photonView = GetComponent<PhotonView>();
        if (isHost)
            Synchronize();
    }
    public void Synchronize()
    {
        index = 0;
        for (int i = 0; i < 100; i++)
            randomSeedArray[i] = Random.Range(0, int.MaxValue);
        photonView.RPC("SetSeed", RpcTarget.Others, index, randomSeedArray);
    }

    [PunRPC]
    private void SetSeed(int index, int[] seedArray)
    {
        SynchronizedRandom.index = index;
        randomSeedArray = seedArray;
    }

    static public int Range(int minInclusive, int maxExclusive)
    {
        int value = randomSeedArray[index] % (maxExclusive - minInclusive) + minInclusive;

        index++;
        if (index >= 100) index = 0;

        return value;
    }
}
