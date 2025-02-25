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
    

    # URLのクエリストリングスから情報をもらう
    PartitionNum, CountUpDataKey = GetCountUpDataInfo(event["queryStringParameters"])

    # カウントアップするデータのPartitionを選ぶ (PcvrかDesktopか)
    PartitionList = ["CountNum_Cpu_Pcvr", "CountNum_Gpu_Pcvr", "CountNum_Ram_Pcvr", "CountNum_Os_Pcvr", "CountNum_Cpu_Desktop", "CountNum_Gpu_Desktop", "CountNum_Ram_Desktop", "CountNum_Os_Desktop"]
    PartitionKey = PartitionList[PartitionNum]

    # データ取得
    dynamodb = boto3.resource("dynamodb")
    table = dynamodb.Table("DynamoDB_PcSpecTable")

    # カウントアップ
    table.update_item(
        Key = {"PartitionKey" : PartitionKey},                                  # Partition選択
        UpdateExpression = f"SET {CountUpDataKey} = {CountUpDataKey} + :val",   # カウントアップコマンド
        ExpressionAttributeValues = {":val" : 1}                                # カウントアップ数
    )
    

    return "OK"

def GetCountUpDataInfo(QueryDict):
    PartitionNum = int(QueryDict['a'])
    CountUpDataKey = f"D{QueryDict['b'].zfill(3)}"  # カウントアップするデータのKey生成。例えばQueryDict['b']が'6'ならD006になる
    return PartitionNum, CountUpDataKey