using SingletonPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    StoryManager _storyManager;

    // ���丮 ������ ������� ������
    private void InitializeDialog()
    {
        if(PlayerPrefs.GetString("Language") == "Korean")
        {
            // 1�� ����
            _storyManager.AppendDialog("???", "", "........", 0.25f); // 0
            _storyManager.AppendDialog("???", "", "......��!", 0.5f);
            _storyManager.AppendDialog("???", "", "������!", 1f);
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "������! �� ���ϴ� �־�!", 1f);
            _storyManager.AppendDialog("�̵���", "���Ӱ��ߺ�", "���� ���Ӱ��ߺΰ� ����, ������ �簡 ũ�δ�Ŭ�� �÷����� ���� �ʹٰ� �׷����ݾƿ�?", 1f);
            _storyManager.AppendDialog("����", "���Ӱ��ߺ�", "��, ���� <������ ��Ұ� ��>���� 1���� �� ������ �÷����ϰ� ���� ��������������", 1f); // 5
            _storyManager.AppendDialog("�Ƹ���", "���Ӱ��ߺ�", "������ �簡 ũ�δ�Ŭ, ��¥ �����Դϴ�.", 1f);
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "����, � �ɾ�! ������ ���� ������!", 1f); // 7
            _storyManager.AppendDialog("", "", "", 1f); // 8 (������)
            _storyManager.AppendDialog("�̵���", "���Ӱ��ߺ�", "�츮 ������ ���� �״��, ��ȭ�� ��ä�� ǳ���� ���� ��Ÿ�� RPG �����̾�.", 1f); // 9
            _storyManager.AppendDialog("", "", "", 1f); // 10 (������)
            _storyManager.AppendDialog("�̵���", "���Ӱ��ߺ�", "��, �׷��ϱ�� �ټ� ǻ������ ��Ҵ� ���� �� �־�. Ʈ�����ݾ�? ����ġ�� ������ �����ϸ� �������� �� �����ϱ�.", 1f); // 11
            _storyManager.AppendDialog("", "", "", 1f); // 12 (������)
            _storyManager.AppendDialog("", "", "", 1f); // 13 (������)
            _storyManager.AppendDialog("", "", "(�۾����!!)", 1f); // 14
            _storyManager.AppendDialog("", "", "", 1f); // 15 (������)
            _storyManager.AppendDialog("", "", "", 1f); // 16 (������)
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "��������!", 1f); // 17
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "�����Ǵ� ������ŭ ��̾��� �͵� ����! ���� ���⼭�� A��ư�� ������� �߾�!", 1f); // 18
            _storyManager.AppendDialog("", "", "", 1f); // 19 (������)
            _storyManager.AppendDialog("�Ƹ���", "���Ӱ��ߺ�", "�г� �������� �����Ǿ����ϴ�. ���� ��ġ �� 30%.", 1f); // 20
            _storyManager.AppendDialog("", "", "", 1f); // 21 (������)
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "�쿡. �������� �ݹ� �����ϳ�? �̴�� Ʃ�丮���� �����ϸ� RPG�� ���Ρ���", 1f); // 22
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "��! A��ư�� ������ �Ѵٴ� �� ��� �˾Ƴ� ����?", 1f); // 23
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "���⼱ �и� B��ư�� ���� �����ؾ� ���ٵ�����", 1f); // 24
            _storyManager.AppendDialog("�̵���", "���Ӱ��ߺ�", "���±��� ������ֽôٴ�. ���� �������̾�.", 1f); // 25
            _storyManager.AppendDialog("�̵���", "���Ӱ��ߺ�", "�̴�ζ��, ���� �ִ� �ð� Ŭ��� ��������������", 1f); // 26
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "��, �׷��� �� ������ RPG�� ���Ρ���", 1f); // 27
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "A��ư�� ��Ÿ��! �̹��� �������� �ƴϾ�!", 1f); // 28
            _storyManager.AppendDialog("", "", "", 1f); // 29 (������)
            _storyManager.AppendDialog("", "", "", 1f); // 30 (������)
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "ĩ. ���� Ǫ�������� ���ʡ� �̶� ���ϴ� �� ����Ѱ�.", 1f); // 31
            _storyManager.AppendDialog("�̵���", "���Ӱ��ߺ�", "������Ŭ�� �ɾ�� �� �κ��� �װ� �ƴ� �� ����.", 1f); // 32
            _storyManager.AppendDialog("�Ƹ���", "���Ӱ��ߺ�", "�г� �������� �����Ǿ����ϴ�. ���� ��ġ �� 60%!", 1f); // 33
            _storyManager.AppendDialog("", "", "", 1f); // 34 (������)
            _storyManager.AppendDialog("", "", "", 1f); // 35 (������)
            _storyManager.AppendDialog("", "", "", 1f); // 36 (������)
            _storyManager.AppendDialog("", "", "(�۾����!!)", 1f); // 37
            _storyManager.AppendDialog("�����", "���Ӱ��ߺ�", "���ӱⰡ �����ϴٴ�!? �̷� ���� �� ���� �����µ�?", 1f); // 38
            _storyManager.AppendDialog("�̵���", "���Ӱ��ߺ�", "������! �������� ������!?", 1f); // 39
            _storyManager.AppendDialog("����", "���Ӱ��ߺ�", "���ơ�", 1f); // 40
            _storyManager.AppendDialog("�Ƹ���", "���Ӱ��ߺ�", "����.", 1f); // 41
            _storyManager.AppendDialog("", "", "", 1f); // 42 (������)
            _storyManager.AppendDialog("???", "", "������!!!!!", 1f); // 43
            _storyManager.AppendDialog("", "", "", 1f); // 44
                                                       // 1�� ����
                                                       // 2�� ����
            _storyManager.AppendDialog("", "", "", 1f); // 45
            _storyManager.AppendDialog("???", "", "��翩, ����� ��ٸ��� �־����ϴ�.", 1f); // 46
            _storyManager.AppendDialog("�̵�����", "����", "�� �̸���, ���� '�̵�����'.", 1f); // 47
            _storyManager.AppendDialog("", "", "", 1f); // 48
            _storyManager.AppendDialog("�̵�����", "����", "�츮�� ���� '�з��Ͼ� ����'��, ���� �������� ���⿡ ó�߽��ϴ�.", 1f); // 49
            _storyManager.AppendDialog("�̵�����", "����", "�� ���⸦ �غ��ϰ�...", 1f); // 50
            _storyManager.AppendDialog("�̵�����", "����", "�غ��ϰ�...", 1f); // 51
            _storyManager.AppendDialog("", "", "", 1f); // 52
            _storyManager.AppendDialog("�̵�����", "����", "�� ������ ����� ����ϸ� ������...", 1f); // 53
            _storyManager.AppendDialog("", "", "", 1f); // 54
            _storyManager.AppendDialog("???", "", "��! ������ ��̱���!", 1f); // 55
            _storyManager.AppendDialog("", "", "", 1f); // 56
            _storyManager.AppendDialog("???", "", "��, �̰� �ƴϾ���.", 1f); // 57
            _storyManager.AppendDialog("???", "", "ũ��!", 1f); // 58
            _storyManager.AppendDialog("???", "", "��, ��翩. ����� ��ٸ��� �־����ϴ�.", 1f); // 59
            _storyManager.AppendDialog("�̵�����", "����", "�� �̸���, ���� '�̵�����'.", 1f); // 60
            _storyManager.AppendDialog("�̵�����", "����", "�츮�� ���� '�з��Ͼ� ����'��, ���� �������� ���⿡ ó�߽��ϴ�.", 1f); // 61
            _storyManager.AppendDialog("", "", "", 1f); // 62
            _storyManager.AppendDialog("�̵�����", "����", "��, �´�. ���ƾơ� �˼��ؿ�. ���� ��� �տ��� �̾߱��ϴ� �� �� ���ؼ�����", 1f); // 63
            _storyManager.AppendDialog("", "", "", 1f); // 64
            _storyManager.AppendDialog("�̵�����", "����", "�������ּż� �����ؿ�. �ƹ�ư. ũ��!", 1f); // 65
            _storyManager.AppendDialog("�̵�����", "����", "���� ��𸮾Ʒκ��� ���ѱ� ���� ��ã��, �з��Ͼ� ���带 ������ �� �ִ� �ڴ�, ���� ��Ż��Դϴ�.", 1f); // 66
            _storyManager.AppendDialog("", "", "", 1f); // 67
            _storyManager.AppendDialog("�̵�����", "����", "�����ƿ�! ���� �з��Ͼ� ���忡����, �ο��� �ƴ϶� �������� ��� �� �����ϰŵ��.", 1f); // 68
            _storyManager.AppendDialog("�̵�����", "����", "��? ��°�� �������� ��� �� �����ϳİ��?", 1f); // 69
            _storyManager.AppendDialog("�̵�����", "����", "�װ� ������. ���� ������ ���� �����ϱ� �����̿���.", 1f); // 70
            _storyManager.AppendDialog("�̵�����", "����", "���� ���� ������ �ο�� �Ǵ� �ϵ� �������䡦��.", 1f); // 71
            _storyManager.AppendDialog("�̵�����", "����", "���Բ�����, ���� �����ֽ� �� �����Ǳ��?", 1f); // 72
            _storyManager.AppendDialog("�̵�����", "����", "����, ���ڱ� �̷� ������ �帮�� �� ���ǰ� �ƴ�����, �׷�������", 1f); // 73
            _storyManager.AppendDialog("", "", "", 1f); // 74
            _storyManager.AppendDialog("�̵�����", "����", "������? �����մϴ�!", 1f); // 75
            _storyManager.AppendDialog("�̵�����", "����", "�׷� � ù ��° �������� �̵��ϵ��� ����!", 1f); // 76
            _storyManager.AppendDialog("", "", "", 1f); // 77
            _storyManager.AppendDialog("�̵�����", "����", "���Ⱑ ���� ��𸮾��� ù ��° �����̿���!", 1f); // 78
            _storyManager.AppendDialog("", "", "", 1f); // 79
            _storyManager.AppendDialog("�̵�����", "����", "���� ���̴� �� ���ӱⰡ ���� ��ϡ���", 1f); // 80
            _storyManager.AppendDialog("�̵�����", "����", "��, �ƴ�, ���� ��𸮾����� ���ѱ� ���̿���!", 1f); // 81
            _storyManager.AppendDialog("", "", "", 1f); // 82
            _storyManager.AppendDialog("�̵�����", "����", "��, �̰� ���� �β����׿�.", 1f); // 83
            _storyManager.AppendDialog("�̵�����", "����", "����� 1���� �Ϸ��� ������ ������ ����������", 1f); // 84
            _storyManager.AppendDialog("�̵�����", "����", "�ƽ��Ե� ���ô� ��ο���.", 1f); // 85
            _storyManager.AppendDialog("", "", "", 1f); // 86
            _storyManager.AppendDialog("�̵�����", "����", "��, �¾ƿ�. ���� ó���� ���� ���ھ ����� ������ ������, �÷��̸� �ŵ��Ҽ��� ���� ��ŷ�� ������ �ø������� ���ϰ� �Ǿ��ŵ��.", 1f); // 87
            _storyManager.AppendDialog("�̵�����", "����", "���� ���߸� �� �ִ� �� ������, ���μ��� �� �� �������.", 1f); // 88
            _storyManager.AppendDialog("�̵�����", "����", "���Բ��� �� �� ���ֽðھ��?", 1f); // 89
            _storyManager.AppendDialog("", "", "", 1f); // 90
            _storyManager.AppendDialog("", "", "< �¸�! >", 1f); // 91
            _storyManager.AppendDialog("", "", "< ���� ����: 41,600�� >\n< ��ŷ ���� ����! >", 1f); // 92
            _storyManager.AppendDialog("�̵�����", "����", "���ó� �̹���������.", 1f); // 93
            _storyManager.AppendDialog("", "", "", 1f); // 94
            _storyManager.AppendDialog("�̵�����", "����", "���� �Ƿ��� �þ�� ���� �� �����ε� �����µ�, ������ �׷����� �ʴ�����.", 1f); // 95
            _storyManager.AppendDialog("", "", "", 1f); // 96
            _storyManager.AppendDialog("�̵�����", "����", "�׷���. �ڸ� ���ѵ帱�Կ�.", 1f); // 97
            _storyManager.AppendDialog("", "", "[����Ű �ȳ�]\n�̵�: ����Ű, ����: Z, ��ų: X", 1f); // 98
            _storyManager.AppendDialog("", "", "", 1f); // 99
            _storyManager.AppendDialog("", "", "", 1f); // 100
            _storyManager.AppendDialog("�̵�����", "����", "������ �׾�����ٴ�, �ƽ��׿�.", 1f); // 101
            _storyManager.AppendDialog("�̵�����", "����", "��, ������ �������� �и� �� �Ͻ� �ſ���!", 1f); // 102
            _storyManager.AppendDialog("", "", "", 1f); // 103
            _storyManager.AppendDialog("�̵�����", "����", "������ �����߸��̱���! ����ؿ�!", 1f); // 104
            _storyManager.AppendDialog("�̵�����", "����", "�׷����� �ƽ��Ե� �ְ� ������ ���������� ���߳׿�.", 1f); // 105
            _storyManager.AppendDialog("�̵�����", "����", "������ ���Ѻ��� ���� �� �͵� ����������", 1f); // 106
            _storyManager.AppendDialog("�̵�����", "����", "���� ������ �ְ� ������ �������!", 1f); // 107
            _storyManager.AppendDialog("", "", "", 1f); // 108
            _storyManager.AppendDialog("", "", "< ���� ���ھ� ����! >", 1f); // 109
            _storyManager.AppendDialog("�̵�����", "����", "���! ��ħ�� ���� ���ھ� �����̿���!", 1f); // 110
            _storyManager.AppendDialog("�̵�����", "����", "ó���� �÷������� �� ���� ���� ������ ���� ������ �־��׿�.", 1f); // 111
            _storyManager.AppendDialog("�̵�����", "����", "�Ƿ��� �ü��� Ŭ���� Ÿ���� �پ���, �� ������ ���� ���ھ ������ �� �����ٴϡ���", 1f); // 112
            _storyManager.AppendDialog("�̵�����", "����", "�����մϴ�. ������ �ƴϾ��ٸ� �ٰ� �𸣴� ä���� �ſ���.", 1f); // 113
            _storyManager.AppendDialog("", "", "", 1f); // 114
            _storyManager.AppendDialog("�̵�����", "����", "����", 1f); // 115
            _storyManager.AppendDialog("�̵�����", "����", "�׷��� ������ �ֽôٴ�, ������ ���� ����Ͻ� ���̳׿�.", 1f); // 116
            _storyManager.AppendDialog("�̵�����", "����", "��ϵ� �̷��� ����߾�����顦��", 1f); // 117
            _storyManager.AppendDialog("", "", "", 1f); // 118
            _storyManager.AppendDialog("�̵�����", "����", "��!", 1f); // 119
            _storyManager.AppendDialog("�̵�����", "����", "����", 1f); // 120
            _storyManager.AppendDialog("�̵�����", "����", "���, ���� ���ߴ� ���� ��𸮾ƴ� �� ��Ͽ���.", 1f); // 121
            _storyManager.AppendDialog("�̵�����", "����", "�̸� �����帮�� ���ؼ� �˼��ؿ�.", 1f); // 122
            _storyManager.AppendDialog("", "", "", 1f); // 123
            _storyManager.AppendDialog("�̵�����", "����", "��, ���� ���� �̻�����?", 1f); // 124
            _storyManager.AppendDialog("�̵�����", "����", "�װ� ������, ��������", 1f); // 125
            _storyManager.AppendDialog("", "", "", 1f); // 126
            _storyManager.AppendDialog("�̵�����", "����", "��? ������ �ƹ����� �׷���.", 1f); // 127
            _storyManager.AppendDialog("�̵�����", "����", "���� ��ϵ�, ���� �� ������ �Ⱦ��ؼ� ���� ������ �ϴ� ���� ������� ������ �ְ� �ؿ�.", 1f); // 128
            _storyManager.AppendDialog("�̵�����", "����", "�׸��� ��ϴ�, ���ӿ� �����ϴ� ���� �Ϸ� ���� ������ �ϰ� �ϰŵ��.", 1f); // 129
            _storyManager.AppendDialog("�̵�����", "����", "������ ���Ѻ��� ���� ���� �ϰ� �;�������, ��Ͽ��� ��⸦ �����Ⱑ ������� ��� ���Ѻ��ٰ� ������ ��Ʈ���� �ؿ�.", 1f); // 130
            _storyManager.AppendDialog("�̵�����", "����", "��, �˼��ؿ�. ���� �𸣰� �β����� ��⸦ �ع��ȳ׿�.", 1f); // 131
            _storyManager.AppendDialog("", "", "", 1f); // 132
            _storyManager.AppendDialog("", "", "", 1f); // 133
            _storyManager.AppendDialog("�̵�����", "����", "��, Ȯ���� �׷��� ����.", 1f); // 134
            _storyManager.AppendDialog("", "", "", 1f); // 135
            _storyManager.AppendDialog("�̵�����", "����", "������ ����� Ȯ����.", 1f); // 136
            _storyManager.AppendDialog("�̵�����", "����", "��� �� ���ӵ� ���� ��ϰ� �ϴ� ���� ���Ѻ��Ҵ����, ���ۿ� ������ ��ġë�� ���� �𸣰ڳ׿�.", 1f); // 137
            _storyManager.AppendDialog("", "", "< ��ŷ ��� ���� �ð��� 20�� ���ҽ��ϴ�. >", 1f); // 138
            _storyManager.AppendDialog("�̵�����", "����", "��! ��ó���� ���� ���ھ��ε�, ��ŷ ����� ��ġ�ھ��! � ����ϼ���, ����!", 1f); // 139
            _storyManager.AppendDialog("", "", "", 1f); // 140
            _storyManager.AppendDialog("", "", "< �ִ� 6���ڱ��� �Է��� �����մϴ�. >", 1f); // 141
            _storyManager.AppendDialog("", "", "", 1f); // 142
            _storyManager.AppendDialog("�̵�����", "����", "���, ��°�� �� �̸�������", 1f); // 143
            _storyManager.AppendDialog("", "", "", 1f); // 144
            _storyManager.AppendDialog("�̵�����", "����", "����", 1f); // 145
            _storyManager.AppendDialog("�̵�����", "����", "���ƾƾƾ�!!!", 1f); // 146
            _storyManager.AppendDialog("", "", "", 1f); // 147
            _storyManager.AppendDialog("�̵�����", "����", "�� �� ��ĥ�� �����ص� �̷� �� ���� ��ŷ 1���� �޼��ϴٴϡ���", 1f); // 148
            _storyManager.AppendDialog("�̵�����", "����", "���� �����ؿ�. ��� ���� �����̿���. �桦��", 1f); // 149
            _storyManager.AppendDialog("�̵�����", "����", "�����ε� ���԰� �Բ���� ���� �� �س� �� ���� �� ���� ����� ����.", 1f); // 150
            _storyManager.AppendDialog("�̵�����", "����", "��, � ���� �������� �̵�����!", 1f); // 151
            _storyManager.AppendDialog("", "", "(��� ���� ��Ű�� �Ⱦ����� ���Ѹ� �ռ����� �̵������� �ڵ��󰬴�.)", 1f); // 152
            _storyManager.AppendDialog("", "", "", 1f); // 153
                                                        // 2�� ����
        }
        else if (PlayerPrefs.GetString("Language") == "Japanese")
        {

        }
    }

    void Awake()
    {
        _storyManager = StoryManager.Instance;

        InitializeDialog();
    }

}
