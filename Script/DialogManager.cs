using CoreLib;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    private void Awake()
    {
        DialogTable.SynchronizeByCSV();
        DialogTable.Temp_AppendDialog();
    }
}
