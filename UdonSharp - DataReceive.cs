using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.StringLoading;
using VRC.Udon.Common.Interfaces;

public class DataReceive : UdonSharpBehaviour
{
    public Text TextBox;
    public Text Text_Cpu_Pcvr0, Text_Cpu_Pcvr1, Text_Cpu_Pcvr2, Text_Cpu_Pcvr3, Text_Cpu_Pcvr4, Text_Cpu_Pcvr5, Text_Gpu_Pcvr0, Text_Gpu_Pcvr1, Text_Ram_Pcvr0, Text_Os_Pcvr0, Text_CpuGpu_Pcvr0;
    public Text Text_Cpu_Desktop0, Text_Cpu_Desktop1, Text_Cpu_Desktop2, Text_Cpu_Desktop3, Text_Cpu_Desktop4, Text_Cpu_Desktop5, Text_Gpu_Desktop0, Text_Gpu_Desktop1, Text_Ram_Desktop0, Text_Os_Desktop0, Text_CpuGpu_Desktop0;
    public Text Text_Cpu_All0, Text_Cpu_All1, Text_Cpu_All2, Text_Cpu_All3, Text_Cpu_All4, Text_Cpu_All5, Text_Gpu_All0, Text_Gpu_All1, Text_Ram_All0, Text_Os_All0, Text_CpuGpu_All0;


    private VRCUrl Url = new VRCUrl("https://~/");
    string[] DatasArray = new string[8];
    string[] DataArray = new string[500];
    int Key = 0;
    int Value = 0;

    int[] Cpu_Pcvr = new int[363];
    int[] Gpu_Pcvr = new int[164];
    int[] Ram_Pcvr = new int[67];
    int[] Os_Pcvr  = new int[5];

    int[] Cpu_Desktop = new int[363];
    int[] Gpu_Desktop = new int[164];
    int[] Ram_Desktop = new int[67];
    int[] Os_Desktop  = new int[5];

    int[] Cpu_All = new int[363];
    int[] Gpu_All = new int[164];
    int[] Ram_All = new int[67];
    int[] Os_All  = new int[5];

    int Cpu0Num = 52;
    int Cpu1Num = 65;
    int Cpu2Num = 82;
    int Cpu3Num = 53;
    int Cpu4Num = 55;
    int Cpu5Num = 56;
    int Gpu0Num = 83;
    int Gpu1Num = 81;
    int Ram0Num = 67;
    int Os0Num = 5;

    string Cpu_Pcvr0 = "";
    string Cpu_Pcvr1 = "";
    string Cpu_Pcvr2 = "";
    string Cpu_Pcvr3 = "";
    string Cpu_Pcvr4 = "";
    string Cpu_Pcvr5 = "";
    string Gpu_Pcvr0 = "";
    string Gpu_Pcvr1 = "";
    string Ram_Pcvr0 = "";
    string Os_Pcvr0 = "";

    string Cpu_Desktop0 = "";
    string Cpu_Desktop1 = "";
    string Cpu_Desktop2 = "";
    string Cpu_Desktop3 = "";
    string Cpu_Desktop4 = "";
    string Cpu_Desktop5 = "";
    string Gpu_Desktop0 = "";
    string Gpu_Desktop1 = "";
    string Ram_Desktop0 = "";
    string Os_Desktop0 = "";

    string Cpu_All0 = "";
    string Cpu_All1 = "";
    string Cpu_All2 = "";
    string Cpu_All3 = "";
    string Cpu_All4 = "";
    string Cpu_All5 = "";
    string Gpu_All0 = "";
    string Gpu_All1 = "";
    string Ram_All0 = "";
    string Os_All0 = "";

    int IntelUser_Pcvr = 0;
    int AmdUser_Pcvr = 0;
    int NvidiaUser_Pcvr = 0;
    int RadeonUser_Pcvr = 0;

    int IntelUser_Desktop = 0;
    int AmdUser_Desktop = 0;
    int NvidiaUser_Desktop = 0;
    int RadeonUser_Desktop = 0;

    int IntelUser_All = 0;
    int AmdUser_All = 0;
    int NvidiaUser_All = 0;
    int RadeonUser_All = 0;

    int UserCount_Pcvr = 0;
    int UserCount_Desktop = 0;
    int UserCount_All = 0;

    int IntelPercent = 0;
    int AmdPercent = 0;
    int NvidiaPercent = 0;
    int RadeonPercent = 0;

    int PcvrPercent = 0;
    int DesktopPercent = 0;


    void Start()
    {
    }

    public override void OnStringLoadSuccess(IVRCStringDownload result)
    {
        TextBox.text += $"ReceivedData : {result.Result}\n";
        Summary(result.Result);
    }
    public override void OnStringLoadError(IVRCStringDownload result)
    {
        TextBox.text += "Error!!!\n";
    }

    public void ReceiveData()
    {
        TextBox.text += "RequestResults...\n";
        VRCStringDownloader.LoadUrl(Url, (IUdonEventReceiver)this);
    }

    void Summary(string Result)
    {
        for(int i=0; i<DatasArray.Length; i++){DatasArray[i] = "";}

        for(int i=0; i<Cpu_Pcvr.Length; i++){Cpu_Pcvr[i] = 0;}
        for(int i=0; i<Gpu_Pcvr.Length; i++){Gpu_Pcvr[i] = 0;}
        for(int i=0; i<Ram_Pcvr.Length; i++){Ram_Pcvr[i] = 0;}
        for(int i=0; i<Os_Pcvr.Length; i++){Os_Pcvr[i] = 0;}
        for(int i=0; i<Cpu_Desktop.Length; i++){Cpu_Desktop[i] = 0;}
        for(int i=0; i<Gpu_Desktop.Length; i++){Gpu_Desktop[i] = 0;}
        for(int i=0; i<Ram_Desktop.Length; i++){Ram_Desktop[i] = 0;}
        for(int i=0; i<Os_Desktop.Length; i++){Os_Desktop[i] = 0;}

        //_text.text = Result;  // "2:1,|2:12,3:2,|2:2,|1:2,2:1,4:3,||4:5,|5:3,||"
        DatasArray = Result.Split('|');
        
        // "CountNum_Cpu_Pcvr", "CountNum_Gpu_Pcvr", "CountNum_Ram_Pcvr", "CountNum_Os_Pcvr", "CountNum_Cpu_Desktop", "CountNum_Gpu_Desktop", "CountNum_Ram_Desktop", "CountNum_Os_Desktop"
        for (int i = 0; i < DatasArray.Length; i++)
        {
            for(int x = 0; x < DataArray.Length; x++){DataArray[x] = "";}

            DataArray = DatasArray[i].Split(',');
            for (int j = 0; j < 500; j++)
            {
                if ("" == DataArray[j]){break;}
                
                Key   = int.Parse(DataArray[j].Split(':')[0]);
                Value = int.Parse(DataArray[j].Split(':')[1]);
                //Debug.Log($"i={i}, Key={Key}, Value={Value}");

                switch(i)
                {
                    case 0:
                    Cpu_Pcvr[Key] = Value; break;
                    case 1:
                    Gpu_Pcvr[Key] = Value; break;
                    case 2:
                    Ram_Pcvr[Key] = Value; break;
                    case 3:
                    Os_Pcvr[Key] = Value; break;
                    case 4:
                    Cpu_Desktop[Key] = Value; break;
                    case 5:
                    Gpu_Desktop[Key] = Value; break;
                    case 6:
                    Ram_Desktop[Key] = Value; break;
                    case 7:
                    Os_Desktop[Key] = Value; break;
                }
            }
        }

        // -----------------------------------------------------------------------------
        Cpu_Pcvr0 = "";
        Cpu_Pcvr1 = "";
        Cpu_Pcvr2 = "";
        Cpu_Pcvr3 = "";
        Cpu_Pcvr4 = "";
        Cpu_Pcvr5 = "";
        Gpu_Pcvr0 = "";
        Gpu_Pcvr1 = "";
        Ram_Pcvr0 = "";
        Os_Pcvr0 = "";

        IntelUser_Pcvr = 0;
        AmdUser_Pcvr = 0;
        NvidiaUser_Pcvr = 0;
        RadeonUser_Pcvr = 0;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Cpu0Num; i++)
        {
            if (0 != Cpu_Pcvr[i]) {Cpu_Pcvr0 += $"{Cpu_Pcvr[i]}\n";}
            else {Cpu_Pcvr0 += $"\n";}
            IntelUser_Pcvr += Cpu_Pcvr[i];
        }
        Text_Cpu_Pcvr0.text = Cpu_Pcvr0;

        for(int i=Cpu0Num; i<Cpu0Num+Cpu1Num; i++)
        {
            if (0 != Cpu_Pcvr[i]) {Cpu_Pcvr1 += $"{Cpu_Pcvr[i]}\n";}
            else {Cpu_Pcvr1 += $"\n";}
            IntelUser_Pcvr += Cpu_Pcvr[i];
        }
        Text_Cpu_Pcvr1.text = Cpu_Pcvr1;

        for(int i=Cpu0Num+Cpu1Num; i<Cpu0Num+Cpu1Num+Cpu2Num; i++)
        {
            if (0 != Cpu_Pcvr[i]) {Cpu_Pcvr2 += $"{Cpu_Pcvr[i]}\n";}
            else {Cpu_Pcvr2 += $"\n";}
            IntelUser_Pcvr += Cpu_Pcvr[i];
        }
        Text_Cpu_Pcvr2.text = Cpu_Pcvr2;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num; i++)
        {
            if (0 != Cpu_Pcvr[i]) {Cpu_Pcvr3 += $"{Cpu_Pcvr[i]}\n";}
            else {Cpu_Pcvr3 += $"\n";}
            IntelUser_Pcvr += Cpu_Pcvr[i];
        }
        Text_Cpu_Pcvr3.text = Cpu_Pcvr3;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num; i++)
        {
            if (0 != Cpu_Pcvr[i]) {Cpu_Pcvr4 += $"{Cpu_Pcvr[i]}\n";}
            else {Cpu_Pcvr4 += $"\n";}
            AmdUser_Pcvr += Cpu_Pcvr[i];
        }
        Text_Cpu_Pcvr4.text = Cpu_Pcvr4;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num+Cpu5Num; i++)
        {
            if (0 != Cpu_Pcvr[i]) {Cpu_Pcvr5 += $"{Cpu_Pcvr[i]}\n";}
            else {Cpu_Pcvr5 += $"\n";}
            AmdUser_Pcvr += Cpu_Pcvr[i];
        }
        Text_Cpu_Pcvr5.text = Cpu_Pcvr5;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Gpu0Num; i++)
        {
            if (0 != Gpu_Pcvr[i]) {Gpu_Pcvr0 += $"{Gpu_Pcvr[i]}\n";}
            else {Gpu_Pcvr0 += $"\n";}
            NvidiaUser_Pcvr += Gpu_Pcvr[i];
        }
        Text_Gpu_Pcvr0.text = Gpu_Pcvr0;

        for(int i=Gpu0Num; i<Gpu0Num+Gpu1Num; i++)
        {
            if (0 != Gpu_Pcvr[i]) {Gpu_Pcvr1 += $"{Gpu_Pcvr[i]}\n";}
            else {Gpu_Pcvr1 += $"\n";}
            RadeonUser_Pcvr += Gpu_Pcvr[i];
        }
        Text_Gpu_Pcvr1.text = Gpu_Pcvr1;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Ram0Num; i++)
        {
            if (0 != Ram_Pcvr[i]) {Ram_Pcvr0 += $"{Ram_Pcvr[i]}\n";}
            else {Ram_Pcvr0 += $"\n";}
        }
        Text_Ram_Pcvr0.text = Ram_Pcvr0;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Os0Num; i++)
        {
            if (0 != Os_Pcvr[i]) {Os_Pcvr0 += $"{Os_Pcvr[i]}\n";}
            else {Os_Pcvr0 += $"\n";}
        }
        Text_Os_Pcvr0.text = Os_Pcvr0;
        // -----------------------------------------------------------------------------
        UserCount_Pcvr = IntelUser_Pcvr + AmdUser_Pcvr;
        IntelPercent = 0; AmdPercent = 0; NvidiaPercent = 0; RadeonPercent = 0;
        if (0 != UserCount_Pcvr)
        {
            IntelPercent  = (int)(((float)IntelUser_Pcvr / (float)UserCount_Pcvr) * 100.0);
            AmdPercent    = 100 - IntelPercent;
            NvidiaPercent = (int)(((float)NvidiaUser_Pcvr / (float)UserCount_Pcvr) * 100.0);
            RadeonPercent = 100 - NvidiaPercent;
        }

        Text_CpuGpu_Pcvr0.text = $"PCVR Users: {UserCount_Pcvr}\n\nCPU:\n- intel: {IntelUser_Pcvr} ({IntelPercent}%)\n- AMD: {AmdUser_Pcvr} ({AmdPercent}%)\n\nGPU:\n- NVIDIA: {NvidiaUser_Pcvr} ({NvidiaPercent}%)\n- AMD: {RadeonUser_Pcvr} ({RadeonPercent}%)";





        // -----------------------------------------------------------------------------
        Cpu_Desktop0 = "";
        Cpu_Desktop1 = "";
        Cpu_Desktop2 = "";
        Cpu_Desktop3 = "";
        Cpu_Desktop4 = "";
        Cpu_Desktop5 = "";
        Gpu_Desktop0 = "";
        Gpu_Desktop1 = "";
        Ram_Desktop0 = "";
        Os_Desktop0 = "";

        IntelUser_Desktop = 0;
        AmdUser_Desktop = 0;
        NvidiaUser_Desktop = 0;
        RadeonUser_Desktop = 0;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Cpu0Num; i++)
        {
            if (0 != Cpu_Desktop[i]) {Cpu_Desktop0 += $"{Cpu_Desktop[i]}\n";}
            else {Cpu_Desktop0 += $"\n";}
            IntelUser_Desktop += Cpu_Desktop[i];
        }
        Text_Cpu_Desktop0.text = Cpu_Desktop0;

        for(int i=Cpu0Num; i<Cpu0Num+Cpu1Num; i++)
        {
            if (0 != Cpu_Desktop[i]) {Cpu_Desktop1 += $"{Cpu_Desktop[i]}\n";}
            else {Cpu_Desktop1 += $"\n";}
            IntelUser_Desktop += Cpu_Desktop[i];
        }
        Text_Cpu_Desktop1.text = Cpu_Desktop1;

        for(int i=Cpu0Num+Cpu1Num; i<Cpu0Num+Cpu1Num+Cpu2Num; i++)
        {
            if (0 != Cpu_Desktop[i]) {Cpu_Desktop2 += $"{Cpu_Desktop[i]}\n";}
            else {Cpu_Desktop2 += $"\n";}
            IntelUser_Desktop += Cpu_Desktop[i];
        }
        Text_Cpu_Desktop2.text = Cpu_Desktop2;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num; i++)
        {
            if (0 != Cpu_Desktop[i]) {Cpu_Desktop3 += $"{Cpu_Desktop[i]}\n";}
            else {Cpu_Desktop3 += $"\n";}
            IntelUser_Desktop += Cpu_Desktop[i];
        }
        Text_Cpu_Desktop3.text = Cpu_Desktop3;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num; i++)
        {
            if (0 != Cpu_Desktop[i]) {Cpu_Desktop4 += $"{Cpu_Desktop[i]}\n";}
            else {Cpu_Desktop4 += $"\n";}
            AmdUser_Desktop += Cpu_Desktop[i];
        }
        Text_Cpu_Desktop4.text = Cpu_Desktop4;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num+Cpu5Num; i++)
        {
            if (0 != Cpu_Desktop[i]) {Cpu_Desktop5 += $"{Cpu_Desktop[i]}\n";}
            else {Cpu_Desktop5 += $"\n";}
            AmdUser_Desktop += Cpu_Desktop[i];
        }
        Text_Cpu_Desktop5.text = Cpu_Desktop5;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Gpu0Num; i++)
        {
            if (0 != Gpu_Desktop[i]) {Gpu_Desktop0 += $"{Gpu_Desktop[i]}\n";}
            else {Gpu_Desktop0 += $"\n";}
            NvidiaUser_Desktop += Gpu_Desktop[i];
        }
        Text_Gpu_Desktop0.text = Gpu_Desktop0;

        for(int i=Gpu0Num; i<Gpu0Num+Gpu1Num; i++)
        {
            if (0 != Gpu_Desktop[i]) {Gpu_Desktop1 += $"{Gpu_Desktop[i]}\n";}
            else {Gpu_Desktop1 += $"\n";}
            RadeonUser_Desktop += Gpu_Desktop[i];
        }
        Text_Gpu_Desktop1.text = Gpu_Desktop1;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Ram0Num; i++)
        {
            if (0 != Ram_Desktop[i]) {Ram_Desktop0 += $"{Ram_Desktop[i]}\n";}
            else {Ram_Desktop0 += $"\n";}
        }
        Text_Ram_Desktop0.text = Ram_Desktop0;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Os0Num; i++)
        {
            if (0 != Os_Desktop[i]) {Os_Desktop0 += $"{Os_Desktop[i]}\n";}
            else {Os_Desktop0 += $"\n";}
        }
        Text_Os_Desktop0.text = Os_Desktop0;
        // -----------------------------------------------------------------------------
        UserCount_Desktop = IntelUser_Desktop + AmdUser_Desktop;
        IntelPercent = 0; AmdPercent = 0; NvidiaPercent = 0; RadeonPercent = 0;
        if (0 != UserCount_Desktop)
        {
            IntelPercent  = (int)(((float)IntelUser_Desktop / (float)UserCount_Desktop) * 100.0);
            AmdPercent    = 100 - IntelPercent;
            NvidiaPercent = (int)(((float)NvidiaUser_Desktop / (float)UserCount_Desktop) * 100.0);
            RadeonPercent = 100 - NvidiaPercent;
        }

        Text_CpuGpu_Desktop0.text = $"Desktop Users: {UserCount_Desktop}\n\nCPU:\n- intel: {IntelUser_Desktop} ({IntelPercent}%)\n- AMD: {AmdUser_Desktop} ({AmdPercent}%)\n\nGPU:\n- NVIDIA: {NvidiaUser_Desktop} ({NvidiaPercent}%)\n- AMD: {RadeonUser_Desktop} ({RadeonPercent}%)";





        // -----------------------------------------------------------------------------
        for (int a = 0; a < Cpu_All.Length; a++){Cpu_All[a] = Cpu_Pcvr[a] + Cpu_Desktop[a];}
        for (int a = 0; a < Gpu_All.Length; a++){Gpu_All[a] = Gpu_Pcvr[a] + Gpu_Desktop[a];}
        for (int a = 0; a < Ram_All.Length; a++){Ram_All[a] = Ram_Pcvr[a] + Ram_Desktop[a];}
        for (int a = 0; a < Os_All.Length; a++) {Os_All[a]  = Os_Pcvr[a]  + Os_Desktop[a]; }

        Cpu_All0 = "";
        Cpu_All1 = "";
        Cpu_All2 = "";
        Cpu_All3 = "";
        Cpu_All4 = "";
        Cpu_All5 = "";
        Gpu_All0 = "";
        Gpu_All1 = "";
        Ram_All0 = "";
        Os_All0 = "";

        IntelUser_All = 0;
        AmdUser_All = 0;
        NvidiaUser_All = 0;
        RadeonUser_All = 0;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Cpu0Num; i++)
        {
            if (0 != Cpu_All[i]) {Cpu_All0 += $"{Cpu_All[i]}\n";}
            else {Cpu_All0 += $"\n";}
            IntelUser_All += Cpu_All[i];
        }
        Text_Cpu_All0.text = Cpu_All0;

        for(int i=Cpu0Num; i<Cpu0Num+Cpu1Num; i++)
        {
            if (0 != Cpu_All[i]) {Cpu_All1 += $"{Cpu_All[i]}\n";}
            else {Cpu_All1 += $"\n";}
            IntelUser_All += Cpu_All[i];
        }
        Text_Cpu_All1.text = Cpu_All1;

        for(int i=Cpu0Num+Cpu1Num; i<Cpu0Num+Cpu1Num+Cpu2Num; i++)
        {
            if (0 != Cpu_All[i]) {Cpu_All2 += $"{Cpu_All[i]}\n";}
            else {Cpu_All2 += $"\n";}
            IntelUser_All += Cpu_All[i];
        }
        Text_Cpu_All2.text = Cpu_All2;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num; i++)
        {
            if (0 != Cpu_All[i]) {Cpu_All3 += $"{Cpu_All[i]}\n";}
            else {Cpu_All3 += $"\n";}
            IntelUser_All += Cpu_All[i];
        }
        Text_Cpu_All3.text = Cpu_All3;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num; i++)
        {
            if (0 != Cpu_All[i]) {Cpu_All4 += $"{Cpu_All[i]}\n";}
            else {Cpu_All4 += $"\n";}
            AmdUser_All += Cpu_All[i];
        }
        Text_Cpu_All4.text = Cpu_All4;

        for(int i=Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num; i<Cpu0Num+Cpu1Num+Cpu2Num+Cpu3Num+Cpu4Num+Cpu5Num; i++)
        {
            if (0 != Cpu_All[i]) {Cpu_All5 += $"{Cpu_All[i]}\n";}
            else {Cpu_All5 += $"\n";}
            AmdUser_All += Cpu_All[i];
        }
        Text_Cpu_All5.text = Cpu_All5;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Gpu0Num; i++)
        {
            if (0 != Gpu_All[i]) {Gpu_All0 += $"{Gpu_All[i]}\n";}
            else {Gpu_All0 += $"\n";}
            NvidiaUser_All += Gpu_All[i];
        }
        Text_Gpu_All0.text = Gpu_All0;

        for(int i=Gpu0Num; i<Gpu0Num+Gpu1Num; i++)
        {
            if (0 != Gpu_All[i]) {Gpu_All1 += $"{Gpu_All[i]}\n";}
            else {Gpu_All1 += $"\n";}
            RadeonUser_All += Gpu_All[i];
        }
        Text_Gpu_All1.text = Gpu_All1;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Ram0Num; i++)
        {
            if (0 != Ram_All[i]) {Ram_All0 += $"{Ram_All[i]}\n";}
            else {Ram_All0 += $"\n";}
        }
        Text_Ram_All0.text = Ram_All0;
        // -----------------------------------------------------------------------------
        for(int i=0; i<Os0Num; i++)
        {
            if (0 != Os_All[i]) {Os_All0 += $"{Os_All[i]}\n";}
            else {Os_All0 += $"\n";}
        }
        Text_Os_All0.text = Os_All0;
        // -----------------------------------------------------------------------------
        UserCount_All = UserCount_Pcvr + UserCount_Desktop;
        IntelPercent = 0; AmdPercent = 0; NvidiaPercent = 0; RadeonPercent = 0; PcvrPercent = 0; DesktopPercent = 0;
        if (0 != UserCount_All)
        {
            IntelPercent  = (int)(((float)IntelUser_All / (float)UserCount_All) * 100.0);
            AmdPercent    = 100 - IntelPercent;
            NvidiaPercent = (int)(((float)NvidiaUser_All / (float)UserCount_All) * 100.0);
            RadeonPercent = 100 - NvidiaPercent;

            PcvrPercent = (int)(((float)UserCount_Pcvr / (float)UserCount_All) * 100.0);
            DesktopPercent = 100 - PcvrPercent;
        }

        Text_CpuGpu_All0.text = $"All Users: {UserCount_All}\n- PCVR: {UserCount_Pcvr} ({PcvrPercent}%)\n- Desktop: {UserCount_Desktop} ({DesktopPercent}%)\n\nCPU:\n- intel: {IntelUser_All} ({IntelPercent}%)\n- AMD: {AmdUser_All} ({AmdPercent}%)\n\nGPU:\n- NVIDIA: {NvidiaUser_All} ({NvidiaPercent}%)\n- AMD: {RadeonUser_All} ({RadeonPercent}%)";






        TextBox.text += "ResultsUpdated!\n\n";
    }
}
