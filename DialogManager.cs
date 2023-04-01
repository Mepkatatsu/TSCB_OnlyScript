using SingletonPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    StoryManager _storyManager;

    // 스토리 내용을 순서대로 저장함
    private void InitializeDialog()
    {
        if(PlayerPrefs.GetString("Language") == "Korean")
        {
            // 1장 시작
            _storyManager.AppendDialog("???", "", "........", 0.25f); // 0
            _storyManager.AppendDialog("???", "", "......님!", 0.5f);
            _storyManager.AppendDialog("???", "", "선생님!", 1f);
            _storyManager.AppendDialog("모모이", "게임개발부", "선생님! 왜 멍하니 있어!", 1f);
            _storyManager.AppendDialog("미도리", "게임개발부", "저희 게임개발부가 만든, 테일즈 사가 크로니클을 플레이해 보고 싶다고 그러셨잖아요?", 1f);
            _storyManager.AppendDialog("유즈", "게임개발부", "여, 역시 <올해의 쿠소게 상>에서 1위를 한 게임은 플레이하고 싶지 않으실지도……", 1f); // 5
            _storyManager.AppendDialog("아리스", "게임개발부", "테일즈 사가 크로니클, 진짜 갓겜입니다.", 1f);
            _storyManager.AppendDialog("모모이", "게임개발부", "자자, 어서 앉아! 세팅은 벌써 끝났어!", 1f); // 7
            _storyManager.AppendDialog("", "", "", 1f); // 8 (선택지)
            _storyManager.AppendDialog("미도리", "게임개발부", "우리 게임은 제목 그대로, 동화적 색채가 풍성한 정통 판타지 RPG 게임이야.", 1f); // 9
            _storyManager.AppendDialog("", "", "", 1f); // 10 (선택지)
            _storyManager.AppendDialog("미도리", "게임개발부", "어, 그러니까…… 다소 퓨전적인 요소는 있을 수 있어. 트렌드잖아? 지나치게 정석에 고집하면 진부해질 수 있으니까.", 1f); // 11
            _storyManager.AppendDialog("", "", "", 1f); // 12 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 13 (선택지)
            _storyManager.AppendDialog("", "", "(퍼어어어엉!!)", 1f); // 14
            _storyManager.AppendDialog("", "", "", 1f); // 15 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 16 (선택지)
            _storyManager.AppendDialog("모모이", "게임개발부", "아하하하!", 1f); // 17
            _storyManager.AppendDialog("모모이", "게임개발부", "예측되는 전개만큼 재미없는 것도 없지! 원래 여기서는 A버튼을 눌렀어야 했어!", 1f); // 18
            _storyManager.AppendDialog("", "", "", 1f); // 19 (선택지)
            _storyManager.AppendDialog("아리스", "게임개발부", "분노 게이지가 감지되었습니다. 현재 수치 약 30%.", 1f); // 20
            _storyManager.AppendDialog("", "", "", 1f); // 21 (선택지)
            _storyManager.AppendDialog("모모이", "게임개발부", "헤에. 생각보다 금방 적응하네? 이대로 튜토리얼을 진행하면 RPG의 꽃인……", 1f); // 22
            _storyManager.AppendDialog("모모이", "게임개발부", "헉! A버튼을 눌러야 한다는 걸 어떻게 알아낸 거지?", 1f); // 23
            _storyManager.AppendDialog("모모이", "게임개발부", "여기선 분명 B버튼을 눌러 폭발해야 할텐데……", 1f); // 24
            _storyManager.AppendDialog("미도리", "게임개발부", "여태까지 기억해주시다니. 역시 선생님이야.", 1f); // 25
            _storyManager.AppendDialog("미도리", "게임개발부", "이대로라면, 역대 최단 시간 클리어도 가능할지도……", 1f); // 26
            _storyManager.AppendDialog("모모이", "게임개발부", "그, 그래도 이 다음은 RPG의 꽃인……", 1f); // 27
            _storyManager.AppendDialog("모모이", "게임개발부", "A버튼을 연타해! 이번엔 거짓말이 아니야!", 1f); // 28
            _storyManager.AppendDialog("", "", "", 1f); // 29 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 30 (선택지)
            _storyManager.AppendDialog("모모이", "게임개발부", "칫. 역시 푸니젤리가 ‘훗’ 이라 말하는 건 어색한가.", 1f); // 31
            _storyManager.AppendDialog("미도리", "게임개발부", "……태클을 걸어야 할 부분은 그게 아닐 것 같아.", 1f); // 32
            _storyManager.AppendDialog("아리스", "게임개발부", "분노 게이지가 감지되었습니다. 현재 수치 약 60%!", 1f); // 33
            _storyManager.AppendDialog("", "", "", 1f); // 34 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 35 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 36 (선택지)
            _storyManager.AppendDialog("", "", "(퍼어어어엉!!)", 1f); // 37
            _storyManager.AppendDialog("모모이", "게임개발부", "게임기가 폭발하다니!? 이런 적은 한 번도 없었는데……", 1f); // 38
            _storyManager.AppendDialog("미도리", "게임개발부", "선생님! 선생님은 괜찮아!?", 1f); // 39
            _storyManager.AppendDialog("유즈", "게임개발부", "으아……", 1f); // 40
            _storyManager.AppendDialog("아리스", "게임개발부", "…….", 1f); // 41
            _storyManager.AppendDialog("", "", "", 1f); // 42 (선택지)
            _storyManager.AppendDialog("???", "", "선생님!!!!!", 1f); // 43
            _storyManager.AppendDialog("", "", "", 1f); // 44
                                                       // 1장 종료
                                                       // 2장 시작
            _storyManager.AppendDialog("", "", "", 1f); // 45
            _storyManager.AppendDialog("???", "", "용사여, 당신을 기다리고 있었습니다.", 1f); // 46
            _storyManager.AppendDialog("미도리엘", "여신", "제 이름은, 여신 '미도리엘'.", 1f); // 47
            _storyManager.AppendDialog("", "", "", 1f); // 48
            _storyManager.AppendDialog("미도리엘", "여신", "우리의 세계 '밀레니엄 랜드'는, 지금 미증유의 위기에 처했습니다.", 1f); // 49
            _storyManager.AppendDialog("미도리엘", "여신", "이 위기를 극복하고...", 1f); // 50
            _storyManager.AppendDialog("미도리엘", "여신", "극복하고...", 1f); // 51
            _storyManager.AppendDialog("", "", "", 1f); // 52
            _storyManager.AppendDialog("미도리엘", "여신", "이 다음에 뭐라고 얘기하면 좋을까...", 1f); // 53
            _storyManager.AppendDialog("", "", "", 1f); // 54
            _storyManager.AppendDialog("???", "", "앗! 정신이 드셨군요!", 1f); // 55
            _storyManager.AppendDialog("", "", "", 1f); // 56
            _storyManager.AppendDialog("???", "", "헉, 이게 아니었지.", 1f); // 57
            _storyManager.AppendDialog("???", "", "크흠!", 1f); // 58
            _storyManager.AppendDialog("???", "", "요, 용사여. 당신을 기다리고 있었습니다.", 1f); // 59
            _storyManager.AppendDialog("미도리엘", "여신", "제 이름은, 여신 '미도리엘'.", 1f); // 60
            _storyManager.AppendDialog("미도리엘", "여신", "우리의 세계 '밀레니엄 랜드'는, 지금 미증류의 위기에 처했습니다.", 1f); // 61
            _storyManager.AppendDialog("", "", "", 1f); // 62
            _storyManager.AppendDialog("미도리엘", "여신", "앗, 맞다. 으아아… 죄송해요. 제가 사람 앞에서 이야기하는 걸 잘 못해서……", 1f); // 63
            _storyManager.AppendDialog("", "", "", 1f); // 64
            _storyManager.AppendDialog("미도리엘", "여신", "이해해주셔서 감사해요. 아무튼. 크흠!", 1f); // 65
            _storyManager.AppendDialog("미도리엘", "여신", "마왕 모모리아로부터 빼앗긴 땅을 되찾고, 밀레니엄 랜드를 구원할 수 있는 자는, 오직 당신뿐입니다.", 1f); // 66
            _storyManager.AppendDialog("", "", "", 1f); // 67
            _storyManager.AppendDialog("미도리엘", "여신", "괜찮아요! 저희 밀레니엄 랜드에서는, 싸움이 아니라 게임으로 모든 걸 결정하거든요.", 1f); // 68
            _storyManager.AppendDialog("미도리엘", "여신", "네? 어째서 게임으로 모든 걸 결정하냐고요?", 1f); // 69
            _storyManager.AppendDialog("미도리엘", "여신", "그건 말이죠. 저희가 게임을 정말 좋아하기 때문이에요.", 1f); // 70
            _storyManager.AppendDialog("미도리엘", "여신", "물론 게임 때문에 싸우게 되는 일도 많지만요…….", 1f); // 71
            _storyManager.AppendDialog("미도리엘", "여신", "용사님께서는, 저를 도와주실 수 있으실까요?", 1f); // 72
            _storyManager.AppendDialog("미도리엘", "여신", "물론, 갑자기 이런 말씀을 드리는 건 예의가 아니지만, 그래도……", 1f); // 73
            _storyManager.AppendDialog("", "", "", 1f); // 74
            _storyManager.AppendDialog("미도리엘", "여신", "정말요? 감사합니다!", 1f); // 75
            _storyManager.AppendDialog("미도리엘", "여신", "그럼 어서 첫 번째 신전으로 이동하도록 하죠!", 1f); // 76
            _storyManager.AppendDialog("", "", "", 1f); // 77
            _storyManager.AppendDialog("미도리엘", "여신", "여기가 마왕 모모리아의 첫 번째 신전이에요!", 1f); // 78
            _storyManager.AppendDialog("", "", "", 1f); // 79
            _storyManager.AppendDialog("미도리엘", "여신", "저기 보이는 저 게임기가 제가 언니……", 1f); // 80
            _storyManager.AppendDialog("미도리엘", "여신", "아, 아니, 마왕 모모리아한테 빼앗긴 땅이에요!", 1f); // 81
            _storyManager.AppendDialog("", "", "", 1f); // 82
            _storyManager.AppendDialog("미도리엘", "여신", "앗, 이건 조금 부끄럽네요.", 1f); // 83
            _storyManager.AppendDialog("미도리엘", "여신", "사실은 1위를 하려고 수차례 도전을 했지만……", 1f); // 84
            _storyManager.AppendDialog("미도리엘", "여신", "아쉽게도 보시는 대로에요.", 1f); // 85
            _storyManager.AppendDialog("", "", "", 1f); // 86
            _storyManager.AppendDialog("미도리엘", "여신", "네, 맞아요. 실은 처음엔 하이 스코어에 가까운 점수를 냈지만, 플레이를 거듭할수록 점점 랭킹에 점수를 올리지조차 못하게 되었거든요.", 1f); // 87
            _storyManager.AppendDialog("미도리엘", "여신", "뭔가 빠뜨린 게 있는 것 같은데, 저로서는 알 수 없었어요.", 1f); // 88
            _storyManager.AppendDialog("미도리엘", "여신", "용사님께서 한 번 봐주시겠어요?", 1f); // 89
            _storyManager.AppendDialog("", "", "", 1f); // 90
            _storyManager.AppendDialog("", "", "< 승리! >", 1f); // 91
            _storyManager.AppendDialog("", "", "< 최종 점수: 41,600점 >\n< 랭킹 진입 실패! >", 1f); // 92
            _storyManager.AppendDialog("미도리엘", "여신", "역시나 이번에도…….", 1f); // 93
            _storyManager.AppendDialog("", "", "", 1f); // 94
            _storyManager.AppendDialog("미도리엘", "여신", "점점 실력이 늘어가는 것을 저 스스로도 느끼는데, 점수는 그렇지가 않더라고요.", 1f); // 95
            _storyManager.AppendDialog("", "", "", 1f); // 96
            _storyManager.AppendDialog("미도리엘", "여신", "그럼요. 자리 비켜드릴게요.", 1f); // 97
            _storyManager.AppendDialog("", "", "[조작키 안내]\n이동: 방향키, 공격: Z, 스킬: X", 1f); // 98
            _storyManager.AppendDialog("", "", "", 1f); // 99
            _storyManager.AppendDialog("", "", "", 1f); // 100
            _storyManager.AppendDialog("미도리엘", "여신", "적한테 죽어버리다니, 아쉽네요.", 1f); // 101
            _storyManager.AppendDialog("미도리엘", "여신", "하, 하지만 다음번엔 분명 잘 하실 거예요!", 1f); // 102
            _storyManager.AppendDialog("", "", "", 1f); // 103
            _storyManager.AppendDialog("미도리엘", "여신", "보스를 쓰러뜨리셨군요! 대단해요!", 1f); // 104
            _storyManager.AppendDialog("미도리엘", "여신", "그렇지만 아쉽게도 최고 점수에 도달하지는 못했네요.", 1f); // 105
            _storyManager.AppendDialog("미도리엘", "여신", "옆에서 지켜보니 뭔가 알 것도 같은데……", 1f); // 106
            _storyManager.AppendDialog("미도리엘", "여신", "다음 번에는 최고 점수를 노려봐요!", 1f); // 107
            _storyManager.AppendDialog("", "", "", 1f); // 108
            _storyManager.AppendDialog("", "", "< 하이 스코어 갱신! >", 1f); // 109
            _storyManager.AppendDialog("미도리엘", "여신", "우와! 마침내 하이 스코어 갱신이에요!", 1f); // 110
            _storyManager.AppendDialog("미도리엘", "여신", "처음에 플레이했을 때 가장 높은 점수를 받은 이유가 있었네요.", 1f); // 111
            _storyManager.AppendDialog("미도리엘", "여신", "실력이 늘수록 클리어 타임이 줄어들고, 그 때문에 하이 스코어를 갱신할 수 없었다니……", 1f); // 112
            _storyManager.AppendDialog("미도리엘", "여신", "감사합니다. 용사님이 아니었다면 줄곧 모르는 채였을 거예요.", 1f); // 113
            _storyManager.AppendDialog("", "", "", 1f); // 114
            _storyManager.AppendDialog("미도리엘", "여신", "……", 1f); // 115
            _storyManager.AppendDialog("미도리엘", "여신", "그렇게 말씀해 주시다니, 용사님은 정말 상냥하신 분이네요.", 1f); // 116
            _storyManager.AppendDialog("미도리엘", "여신", "언니도 이렇게 상냥했었더라면……", 1f); // 117
            _storyManager.AppendDialog("", "", "", 1f); // 118
            _storyManager.AppendDialog("미도리엘", "여신", "앗!", 1f); // 119
            _storyManager.AppendDialog("미도리엘", "여신", "……", 1f); // 120
            _storyManager.AppendDialog("미도리엘", "여신", "사실, 제가 말했던 마왕 모모리아는 제 언니에요.", 1f); // 121
            _storyManager.AppendDialog("미도리엘", "여신", "미리 말씀드리지 못해서 죄송해요.", 1f); // 122
            _storyManager.AppendDialog("", "", "", 1f); // 123
            _storyManager.AppendDialog("미도리엘", "여신", "여, 역시 조금 이상하죠?", 1f); // 124
            _storyManager.AppendDialog("미도리엘", "여신", "그게 말이죠, 실은……", 1f); // 125
            _storyManager.AppendDialog("", "", "", 1f); // 126
            _storyManager.AppendDialog("미도리엘", "여신", "네? 음…… 아무래도 그렇죠.", 1f); // 127
            _storyManager.AppendDialog("미도리엘", "여신", "저도 언니도, 지는 걸 굉장히 싫어해서 같이 게임을 하다 보면 어느샌가 다투고 있곤 해요.", 1f); // 128
            _storyManager.AppendDialog("미도리엘", "여신", "그리고 언니는, 게임에 몰두하다 보면 하루 종일 게임을 하곤 하거든요.", 1f); // 129
            _storyManager.AppendDialog("미도리엘", "여신", "옆에서 지켜보다 보면 저도 하고 싶어지지만, 언니에게 얘기를 꺼내기가 어려워서 계속 지켜보다가 울음을 터트리곤 해요.", 1f); // 130
            _storyManager.AppendDialog("미도리엘", "여신", "앗, 죄송해요. 저도 모르게 부끄러운 얘기를 해버렸네요.", 1f); // 131
            _storyManager.AppendDialog("", "", "", 1f); // 132
            _storyManager.AppendDialog("", "", "", 1f); // 133
            _storyManager.AppendDialog("미도리엘", "여신", "아, 확실히 그렇긴 하죠.", 1f); // 134
            _storyManager.AppendDialog("", "", "", 1f); // 135
            _storyManager.AppendDialog("미도리엘", "여신", "음…… 듣고보니 확실히.", 1f); // 136
            _storyManager.AppendDialog("미도리엘", "여신", "방금 그 게임도 만약 언니가 하는 것을 지켜보았더라면, 진작에 이유를 눈치챘을 지도 모르겠네요.", 1f); // 137
            _storyManager.AppendDialog("", "", "< 랭킹 등록 제한 시간이 20초 남았습니다. >", 1f); // 138
            _storyManager.AppendDialog("미도리엘", "여신", "헉! 모처럼의 하이 스코어인데, 랭킹 등록을 놓치겠어요! 어서 등록하세요, 용사님!", 1f); // 139
            _storyManager.AppendDialog("", "", "", 1f); // 140
            _storyManager.AppendDialog("", "", "< 최대 6글자까지 입력이 가능합니다. >", 1f); // 141
            _storyManager.AppendDialog("", "", "", 1f); // 142
            _storyManager.AppendDialog("미도리엘", "여신", "어라, 어째서 제 이름을……", 1f); // 143
            _storyManager.AppendDialog("", "", "", 1f); // 144
            _storyManager.AppendDialog("미도리엘", "여신", "……", 1f); // 145
            _storyManager.AppendDialog("미도리엘", "여신", "으아아아앙!!!", 1f); // 146
            _storyManager.AppendDialog("", "", "", 1f); // 147
            _storyManager.AppendDialog("미도리엘", "여신", "몇 날 며칠을 도전해도 이룰 수 없던 랭킹 1위를 달성하다니……", 1f); // 148
            _storyManager.AppendDialog("미도리엘", "여신", "정말 감사해요. 모두 용사님 덕분이에요. 흑……", 1f); // 149
            _storyManager.AppendDialog("미도리엘", "여신", "앞으로도 용사님과 함께라면 뭐든 잘 해낼 수 있을 것 같은 기분이 들어요.", 1f); // 150
            _storyManager.AppendDialog("미도리엘", "여신", "자, 어서 다음 신전으로 이동하죠!", 1f); // 151
            _storyManager.AppendDialog("", "", "(우는 얼굴을 들키기 싫었는지 서둘리 앞서가는 미도리엘을 뒤따라갔다.)", 1f); // 152
            _storyManager.AppendDialog("", "", "", 1f); // 153
                                                        // 2장 종료
        }
        else if (PlayerPrefs.GetString("Language") == "Japanese")
        {
            _storyManager.AppendDialog("???", "", "........", 0.25f); // 0
            _storyManager.AppendDialog("???", "", "......生！", 0.5f);
            _storyManager.AppendDialog("???", "", "先生！", 1f);
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "先生！なんでぼーっとしてるの？", 1f);
            _storyManager.AppendDialog("ミドリ", "ゲーム開発部", "私たちゲーム開発部が作った、テイルズ・サガ・クロニクルをやってみたいと言いましたよね？", 1f);
            _storyManager.AppendDialog("ユズ", "ゲーム開発部", "や、やっぱり「今年のクソゲーランキング１位」のゲームはやりたくないかも……", 1f); // 5
            _storyManager.AppendDialog("アリス", "ゲーム開発部", "テイルズ・サガ・クロニクル、神ゲーですよ！", 1f);
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "さあさあ、早く座って！セッティングはもう終わったよ！", 1f); // 7
            _storyManager.AppendDialog("", "", "", 1f); // 8 (선택지)
            _storyManager.AppendDialog("ミドリ", "ゲーム開発部", "タイトルから分かるかもしれないけど、このゲームは童話テイストで、色彩豊かな王道ファンタジーRPGなの。", 1f); // 9
            _storyManager.AppendDialog("", "", "", 1f); // 10 (선택지)
            _storyManager.AppendDialog("ミドリ", "ゲーム開発部", "えっと、王道とは言っても、色々な要素を混ぜてたりするんだけどね。トレンドそのままでもダメだけど、王道に拘りすぎても古くなるからってことで。", 1f); // 11
            _storyManager.AppendDialog("", "", "", 1f); // 12 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 13 (선택지)
            _storyManager.AppendDialog("", "", "（ドカーーーーン！！）", 1f); // 14
            _storyManager.AppendDialog("", "", "", 1f); // 15 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 16 (선택지)
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "あははははっ！", 1f); // 17
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "予想できる展開ほどつまらないものはないよね！本当はここで指示通りじゃなくて、Aボタンを押さなきゃいけないの！", 1f); // 18
            _storyManager.AppendDialog("", "", "", 1f); // 19 (선택지)
            _storyManager.AppendDialog("アリス", "ゲーム開発部", "怒りレベルが感知されました。現在レベル３。", 1f); // 20
            _storyManager.AppendDialog("", "", "", 1f); // 21 (선택지)
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "お、いい感じ。そのまま進めば、RPGの花である戦闘が……", 1f); // 22
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "えっ！Aボタンを押さなきゃいけないってことを何でわかったの？", 1f); // 23
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "ここではきっとBボタンを押して爆発するべきなのに……", 1f); // 24
            _storyManager.AppendDialog("ミドリ", "ゲーム開発部", "今まで覚えてくださるなんて、やっぱり先生ですね。", 1f); // 25
            _storyManager.AppendDialog("ミドリ", "ゲーム開発部", "このままだと、史上最速クリアもできるかも……", 1f); // 26
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "で、でもこの後はRPGの花である戦闘が……", 1f); // 27
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "Aボタンを押して！今度は嘘じゃないから！", 1f); // 28
            _storyManager.AppendDialog("", "", "", 1f); // 29 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 30 (선택지)
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "うーん、やっぱりプニプニが「ふっ」って言うのは不自然かな。", 1f); // 31
            _storyManager.AppendDialog("ミドリ", "ゲーム開発部", "……ツッコミどころはそこじゃないと思う。", 1f); // 32
            _storyManager.AppendDialog("アリス", "ゲーム開発部", "怒りレベルが感知されました。現在レベル６！", 1f); // 33
            _storyManager.AppendDialog("", "", "", 1f); // 34 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 35 (선택지)
            _storyManager.AppendDialog("", "", "", 1f); // 36 (선택지)
            _storyManager.AppendDialog("", "", "（ドカーーーーン！！）", 1f); // 37
            _storyManager.AppendDialog("モモイ", "ゲーム開発部", "ゲーム機が爆発するなんて！？こんなことは一度もなかったのに……", 1f); // 38
            _storyManager.AppendDialog("ミドリ", "ゲーム開発部", "先生！先生は大丈夫？", 1f); // 39
            _storyManager.AppendDialog("ユズ", "ゲーム開発部", "あぅ……", 1f); // 40
            _storyManager.AppendDialog("アリス", "ゲーム開発部", "……。", 1f); // 41
            _storyManager.AppendDialog("", "", "", 1f); // 42 (선택지)
            _storyManager.AppendDialog("???", "", "先生！！！！！", 1f); // 43
            _storyManager.AppendDialog("", "", "", 1f); // 44
                                                        // 1장 종료
                                                        // 2장 시작
            _storyManager.AppendDialog("", "", "", 1f); // 45
            _storyManager.AppendDialog("???", "", "용사여, 당신을 기다리고 있었습니다.", 1f); // 46
            _storyManager.AppendDialog("미도리엘", "여신", "제 이름은, 여신 '미도리엘'.", 1f); // 47
            _storyManager.AppendDialog("", "", "", 1f); // 48
            _storyManager.AppendDialog("미도리엘", "여신", "우리의 세계 '밀레니엄 랜드'는, 지금 미증유의 위기에 처했습니다.", 1f); // 49
            _storyManager.AppendDialog("미도리엘", "여신", "이 위기를 극복하고...", 1f); // 50
            _storyManager.AppendDialog("미도리엘", "여신", "극복하고...", 1f); // 51
            _storyManager.AppendDialog("", "", "", 1f); // 52
            _storyManager.AppendDialog("미도리엘", "여신", "이 다음에 뭐라고 얘기하면 좋을까...", 1f); // 53
            _storyManager.AppendDialog("", "", "", 1f); // 54
            _storyManager.AppendDialog("???", "", "앗! 정신이 드셨군요!", 1f); // 55
            _storyManager.AppendDialog("", "", "", 1f); // 56
            _storyManager.AppendDialog("???", "", "헉, 이게 아니었지.", 1f); // 57
            _storyManager.AppendDialog("???", "", "크흠!", 1f); // 58
            _storyManager.AppendDialog("???", "", "요, 용사여. 당신을 기다리고 있었습니다.", 1f); // 59
            _storyManager.AppendDialog("미도리엘", "여신", "제 이름은, 여신 '미도리엘'.", 1f); // 60
            _storyManager.AppendDialog("미도리엘", "여신", "우리의 세계 '밀레니엄 랜드'는, 지금 미증류의 위기에 처했습니다.", 1f); // 61
            _storyManager.AppendDialog("", "", "", 1f); // 62
            _storyManager.AppendDialog("미도리엘", "여신", "앗, 맞다. 으아아… 죄송해요. 제가 사람 앞에서 이야기하는 걸 잘 못해서……", 1f); // 63
            _storyManager.AppendDialog("", "", "", 1f); // 64
            _storyManager.AppendDialog("미도리엘", "여신", "이해해주셔서 감사해요. 아무튼. 크흠!", 1f); // 65
            _storyManager.AppendDialog("미도리엘", "여신", "마왕 모모리아로부터 빼앗긴 땅을 되찾고, 밀레니엄 랜드를 구원할 수 있는 자는, 오직 당신뿐입니다.", 1f); // 66
            _storyManager.AppendDialog("", "", "", 1f); // 67
            _storyManager.AppendDialog("미도리엘", "여신", "괜찮아요! 저희 밀레니엄 랜드에서는, 싸움이 아니라 게임으로 모든 걸 결정하거든요.", 1f); // 68
            _storyManager.AppendDialog("미도리엘", "여신", "네? 어째서 게임으로 모든 걸 결정하냐고요?", 1f); // 69
            _storyManager.AppendDialog("미도리엘", "여신", "그건 말이죠. 저희가 게임을 정말 좋아하기 때문이에요.", 1f); // 70
            _storyManager.AppendDialog("미도리엘", "여신", "물론 게임 때문에 싸우게 되는 일도 많지만요…….", 1f); // 71
            _storyManager.AppendDialog("미도리엘", "여신", "용사님께서는, 저를 도와주실 수 있으실까요?", 1f); // 72
            _storyManager.AppendDialog("미도리엘", "여신", "물론, 갑자기 이런 말씀을 드리는 건 예의가 아니지만, 그래도……", 1f); // 73
            _storyManager.AppendDialog("", "", "", 1f); // 74
            _storyManager.AppendDialog("미도리엘", "여신", "정말요? 감사합니다!", 1f); // 75
            _storyManager.AppendDialog("미도리엘", "여신", "그럼 어서 첫 번째 신전으로 이동하도록 하죠!", 1f); // 76
            _storyManager.AppendDialog("", "", "", 1f); // 77
            _storyManager.AppendDialog("미도리엘", "여신", "여기가 마왕 모모리아의 첫 번째 신전이에요!", 1f); // 78
            _storyManager.AppendDialog("", "", "", 1f); // 79
            _storyManager.AppendDialog("미도리엘", "여신", "저기 보이는 저 게임기가 제가 언니……", 1f); // 80
            _storyManager.AppendDialog("미도리엘", "여신", "아, 아니, 마왕 모모리아한테 빼앗긴 땅이에요!", 1f); // 81
            _storyManager.AppendDialog("", "", "", 1f); // 82
            _storyManager.AppendDialog("미도리엘", "여신", "앗, 이건 조금 부끄럽네요.", 1f); // 83
            _storyManager.AppendDialog("미도리엘", "여신", "사실은 1위를 하려고 수차례 도전을 했지만……", 1f); // 84
            _storyManager.AppendDialog("미도리엘", "여신", "아쉽게도 보시는 대로에요.", 1f); // 85
            _storyManager.AppendDialog("", "", "", 1f); // 86
            _storyManager.AppendDialog("미도리엘", "여신", "네, 맞아요. 실은 처음엔 하이 스코어에 가까운 점수를 냈지만, 플레이를 거듭할수록 점점 랭킹에 점수를 올리지조차 못하게 되었거든요.", 1f); // 87
            _storyManager.AppendDialog("미도리엘", "여신", "뭔가 빠뜨린 게 있는 것 같은데, 저로서는 알 수 없었어요.", 1f); // 88
            _storyManager.AppendDialog("미도리엘", "여신", "용사님께서 한 번 봐주시겠어요?", 1f); // 89
            _storyManager.AppendDialog("", "", "", 1f); // 90
            _storyManager.AppendDialog("", "", "< 승리! >", 1f); // 91
            _storyManager.AppendDialog("", "", "< 최종 점수: 41,600점 >\n< 랭킹 진입 실패! >", 1f); // 92
            _storyManager.AppendDialog("미도리엘", "여신", "역시나 이번에도…….", 1f); // 93
            _storyManager.AppendDialog("", "", "", 1f); // 94
            _storyManager.AppendDialog("미도리엘", "여신", "점점 실력이 늘어가는 것을 저 스스로도 느끼는데, 점수는 그렇지가 않더라고요.", 1f); // 95
            _storyManager.AppendDialog("", "", "", 1f); // 96
            _storyManager.AppendDialog("미도리엘", "여신", "그럼요. 자리 비켜드릴게요.", 1f); // 97
            _storyManager.AppendDialog("", "", "[조작키 안내]\n이동: 방향키, 공격: Z, 스킬: X", 1f); // 98
            _storyManager.AppendDialog("", "", "", 1f); // 99
            _storyManager.AppendDialog("", "", "", 1f); // 100
            _storyManager.AppendDialog("미도리엘", "여신", "적한테 죽어버리다니, 아쉽네요.", 1f); // 101
            _storyManager.AppendDialog("미도리엘", "여신", "하, 하지만 다음번엔 분명 잘 하실 거예요!", 1f); // 102
            _storyManager.AppendDialog("", "", "", 1f); // 103
            _storyManager.AppendDialog("미도리엘", "여신", "보스를 쓰러뜨리셨군요! 대단해요!", 1f); // 104
            _storyManager.AppendDialog("미도리엘", "여신", "그렇지만 아쉽게도 최고 점수에 도달하지는 못했네요.", 1f); // 105
            _storyManager.AppendDialog("미도리엘", "여신", "옆에서 지켜보니 뭔가 알 것도 같은데……", 1f); // 106
            _storyManager.AppendDialog("미도리엘", "여신", "다음 번에는 최고 점수를 노려봐요!", 1f); // 107
            _storyManager.AppendDialog("", "", "", 1f); // 108
            _storyManager.AppendDialog("", "", "< 하이 스코어 갱신! >", 1f); // 109
            _storyManager.AppendDialog("미도리엘", "여신", "우와! 마침내 하이 스코어 갱신이에요!", 1f); // 110
            _storyManager.AppendDialog("미도리엘", "여신", "처음에 플레이했을 때 가장 높은 점수를 받은 이유가 있었네요.", 1f); // 111
            _storyManager.AppendDialog("미도리엘", "여신", "실력이 늘수록 클리어 타임이 줄어들고, 그 때문에 하이 스코어를 갱신할 수 없었다니……", 1f); // 112
            _storyManager.AppendDialog("미도리엘", "여신", "감사합니다. 용사님이 아니었다면 줄곧 모르는 채였을 거예요.", 1f); // 113
            _storyManager.AppendDialog("", "", "", 1f); // 114
            _storyManager.AppendDialog("미도리엘", "여신", "……", 1f); // 115
            _storyManager.AppendDialog("미도리엘", "여신", "그렇게 말씀해 주시다니, 용사님은 정말 상냥하신 분이네요.", 1f); // 116
            _storyManager.AppendDialog("미도리엘", "여신", "언니도 이렇게 상냥했었더라면……", 1f); // 117
            _storyManager.AppendDialog("", "", "", 1f); // 118
            _storyManager.AppendDialog("미도리엘", "여신", "앗!", 1f); // 119
            _storyManager.AppendDialog("미도리엘", "여신", "……", 1f); // 120
            _storyManager.AppendDialog("미도리엘", "여신", "사실, 제가 말했던 마왕 모모리아는 제 언니에요.", 1f); // 121
            _storyManager.AppendDialog("미도리엘", "여신", "미리 말씀드리지 못해서 죄송해요.", 1f); // 122
            _storyManager.AppendDialog("", "", "", 1f); // 123
            _storyManager.AppendDialog("미도리엘", "여신", "여, 역시 조금 이상하죠?", 1f); // 124
            _storyManager.AppendDialog("미도리엘", "여신", "그게 말이죠, 실은……", 1f); // 125
            _storyManager.AppendDialog("", "", "", 1f); // 126
            _storyManager.AppendDialog("미도리엘", "여신", "네? 음…… 아무래도 그렇죠.", 1f); // 127
            _storyManager.AppendDialog("미도리엘", "여신", "저도 언니도, 지는 걸 굉장히 싫어해서 같이 게임을 하다 보면 어느샌가 다투고 있곤 해요.", 1f); // 128
            _storyManager.AppendDialog("미도리엘", "여신", "그리고 언니는, 게임에 몰두하다 보면 하루 종일 게임을 하곤 하거든요.", 1f); // 129
            _storyManager.AppendDialog("미도리엘", "여신", "옆에서 지켜보다 보면 저도 하고 싶어지지만, 언니에게 얘기를 꺼내기가 어려워서 계속 지켜보다가 울음을 터트리곤 해요.", 1f); // 130
            _storyManager.AppendDialog("미도리엘", "여신", "앗, 죄송해요. 저도 모르게 부끄러운 얘기를 해버렸네요.", 1f); // 131
            _storyManager.AppendDialog("", "", "", 1f); // 132
            _storyManager.AppendDialog("", "", "", 1f); // 133
            _storyManager.AppendDialog("미도리엘", "여신", "아, 확실히 그렇긴 하죠.", 1f); // 134
            _storyManager.AppendDialog("", "", "", 1f); // 135
            _storyManager.AppendDialog("미도리엘", "여신", "음…… 듣고보니 확실히.", 1f); // 136
            _storyManager.AppendDialog("미도리엘", "여신", "방금 그 게임도 만약 언니가 하는 것을 지켜보았더라면, 진작에 이유를 눈치챘을 지도 모르겠네요.", 1f); // 137
            _storyManager.AppendDialog("", "", "< 랭킹 등록 제한 시간이 20초 남았습니다. >", 1f); // 138
            _storyManager.AppendDialog("미도리엘", "여신", "헉! 모처럼의 하이 스코어인데, 랭킹 등록을 놓치겠어요! 어서 등록하세요, 용사님!", 1f); // 139
            _storyManager.AppendDialog("", "", "", 1f); // 140
            _storyManager.AppendDialog("", "", "< 최대 6글자까지 입력이 가능합니다. >", 1f); // 141
            _storyManager.AppendDialog("", "", "", 1f); // 142
            _storyManager.AppendDialog("미도리엘", "여신", "어라, 어째서 제 이름을……", 1f); // 143
            _storyManager.AppendDialog("", "", "", 1f); // 144
            _storyManager.AppendDialog("미도리엘", "여신", "……", 1f); // 145
            _storyManager.AppendDialog("미도리엘", "여신", "으아아아앙!!!", 1f); // 146
            _storyManager.AppendDialog("", "", "", 1f); // 147
            _storyManager.AppendDialog("미도리엘", "여신", "몇 날 며칠을 도전해도 이룰 수 없던 랭킹 1위를 달성하다니……", 1f); // 148
            _storyManager.AppendDialog("미도리엘", "여신", "정말 감사해요. 모두 용사님 덕분이에요. 흑……", 1f); // 149
            _storyManager.AppendDialog("미도리엘", "여신", "앞으로도 용사님과 함께라면 뭐든 잘 해낼 수 있을 것 같은 기분이 들어요.", 1f); // 150
            _storyManager.AppendDialog("미도리엘", "여신", "자, 어서 다음 신전으로 이동하죠!", 1f); // 151
            _storyManager.AppendDialog("", "", "(우는 얼굴을 들키기 싫었는지 서둘리 앞서가는 미도리엘을 뒤따라갔다.)", 1f); // 152
            _storyManager.AppendDialog("", "", "", 1f); // 153
                                                        // 2장 종료
        }
    }

    void Awake()
    {
        _storyManager = StoryManager.Instance;

        InitializeDialog();
    }

}
