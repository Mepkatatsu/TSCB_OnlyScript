using System;
using System.Collections.Generic;
using System.IO;
using CoreLib;

public static class DialogTable
{
    private static List<DialogData> _data = new List<DialogData>();

    // TODO: 코드에서 스토리 내용 빼기 위해 임시로 작성한 코드. textSpeed 반영이 필요(이건 스토리 별 행동 지정하는 부분에 넣어야 할 듯) StoryManager에 넣는 게 아니라 StoryManager에서 DialogTable을 참조하도록 수정이 필요함.
    public static void Temp_AppendDialog()
    {
        if (_data.IsEmpty())
            return;

        for (var i = 0; i < _data.Count; i++)
        {
            var data = _data[i];
            var dialog = LocalizeManager.language == LocalizeManager.Language.Korean ? data.koreanDialog : data.japaneseDialog;
            
            StoryManager.Instance.AppendDialog(data.name, data.department, dialog, 1f);
        }
    }
    
    public static void SynchronizeByCSV()
    {
        _data.Clear();
        
        var texts = File.ReadAllLines("Assets/Resources/Table/DialogTable.csv");
        
        for (var i = 1; i < texts.Length; i++)
        {
            var splitText = texts[i].Split(',');

            if (splitText.Length < 6)
                throw new Exception("splitText.Length < 5");
            
            var chapter = byte.Parse(splitText[0]);
            var dialogNum = short.Parse(splitText[1]);
            var name = splitText[2];
            var department = splitText[3];
            var koreanDialog = splitText[4];
            var japaneseDialog = splitText[5];

            var data = new DialogData()
            {
                chapter = chapter,
                dialogNum = dialogNum,
                name = name,
                department = department,
                koreanDialog = koreanDialog,
                japaneseDialog = japaneseDialog,
            };
            
            _data.Add(data);
        }
    }
    
    public static void SynchronizeByBinary()
    {
        _data.Clear();
        
        using var reader = new BinaryReader(File.OpenRead("Assets/Resources/Table/DialogTable.byte"));
        
        var count = reader.ReadInt32();

        for (var i = 0; i < count; i++)
        {
            var chapter = reader.ReadByte();
            var dialogNum = reader.ReadInt16();
            var name = reader.ReadString();
            var department = reader.ReadString();
            var koreanDialog = reader.ReadString();
            var japaneseDialog = reader.ReadString();
            
            var data = new DialogData()
            {
                chapter = chapter,
                dialogNum = dialogNum,
                name = name,
                department = department,
                koreanDialog = koreanDialog,
                japaneseDialog = japaneseDialog,
            };
            
            _data.Add(data);
        }
    }
    
    public static void SynchronizeToBinary()
    {
        using var writer = new BinaryWriter(File.OpenWrite("Assets/Resources/Table/DialogTable.byte"));
        
        writer.Write(_data.Count);

        for (var i = 0; i < _data.Count; i++)
        {
            var data = _data[i];
            writer.Write(data.chapter);
            writer.Write(data.dialogNum);
            writer.Write(data.name);
            writer.Write(data.department);
            writer.Write(data.koreanDialog);
            writer.Write(data.japaneseDialog);
        }
    }
    
    public static void ClearData()
    {
        _data.Clear();
    }
}

[Serializable]
public class DialogData : ISynchronizableObject
{
    public byte chapter;
    public short dialogNum;
    public string name;
    public string department;
    public string koreanDialog;
    public string japaneseDialog;
    
    public void Synchronize(ISynchronizer synchronizer)
    {
        chapter = synchronizer.Synchronize(chapter);
        dialogNum = synchronizer.Synchronize(dialogNum);
        name = synchronizer.Synchronize(name);
        department = synchronizer.Synchronize(department);
        koreanDialog = synchronizer.Synchronize(koreanDialog);
        japaneseDialog = synchronizer.Synchronize(japaneseDialog);
    }
}