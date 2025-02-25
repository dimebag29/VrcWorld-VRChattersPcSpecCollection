import json
import boto3

def lambda_handler(event, context):
    
    # VRChat以外からの通信だったら弾く
    try:
        UserAgent = event["headers"]["user-agent"]
    except:
        return "Please stop attacking the server. (Could not get UserAgent)"
    if False == ("UnityPlayer" in UserAgent):
        return "Please stop attacking the server. (Access from outside VRChat)"
    
    
    dynamodb = boto3.resource('dynamodb')
    table = dynamodb.Table('DynamoDB_PcSpecTable')

    DataKeyList = ["CountNum_Cpu_Pcvr", "CountNum_Gpu_Pcvr", "CountNum_Ram_Pcvr", "CountNum_Os_Pcvr", "CountNum_Cpu_Desktop", "CountNum_Gpu_Desktop", "CountNum_Ram_Desktop", "CountNum_Os_Desktop"]
    DataList = []
    for i, DataKey in enumerate(DataKeyList):
        TempDict = table.get_item(Key={'PartitionKey' : DataKeyList[i]}).get('Item')
        del TempDict['PartitionKey']
        SortedKeyList = sorted(TempDict)
        TempDict = {k: TempDict[k] for k in SortedKeyList}
        DataList.append(TempDict)

    ReturnStr = ""
    for i, DataKey in enumerate(DataKeyList):
        for k, v in DataList[i].items():
            if 0 != v:
                n = int(k[1:4]) # D006の006だけ抜き取ってintに変換
                ReturnStr += f"{n}:{v},"
        ReturnStr += "|"
        

    return ReturnStr
