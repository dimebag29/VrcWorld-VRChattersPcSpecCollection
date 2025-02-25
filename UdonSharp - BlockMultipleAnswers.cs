using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;



[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]

public class BlockMultipleAnswers : UdonSharpBehaviour
{
    [UdonSynced] private bool AlreadyAnswered = false;
    public GameObject SendButton;
    public Text TextBox_Error;

    // Persistenceの準備完了時に呼ばれる
    public override void OnPlayerRestored(VRCPlayerApi player)
    {
        if (!Networking.LocalPlayer.IsOwner(gameObject))
        {
            gameObject.SetActive(false);
            return;
        }
        if (!player.isLocal)
        {
            return;
        }

        if(true == AlreadyAnswered)
        {
            TextBox_Error.text = "既に回答済みです\nAlready answered.";
            SendButton.SetActive(false);
        }

    }


    // インスタンス化されたオブジェクトを返す (https://hatuxes.hatenablog.jp/entry/2024/11/06/171536#PlayerObject%E3%81%AE%E4%BD%9C%E6%88%90%E6%96%B9%E6%B3%95)
    public static BlockMultipleAnswers GetInstance(VRCPlayerApi player)
    {
        GameObject[] playerObjectList = Networking.GetPlayerObjects(player);
        if (!Utilities.IsValid(playerObjectList))
        {
            return null;
        }

        foreach (GameObject playerObject in playerObjectList)
        {
            if (!Utilities.IsValid(playerObject))
            {
                continue;
            }

            BlockMultipleAnswers foundScript = playerObject.GetComponentInChildren<BlockMultipleAnswers>();
            if (!Utilities.IsValid(foundScript))
            {
                continue;
            }

            return foundScript;
        }
        return null;
    }

    // 永続変数更新
    public void Answered()
    {
        AlreadyAnswered = true;
        RequestSerialization();
    }

}
