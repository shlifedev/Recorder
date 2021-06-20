# ByulMacro
Very Simple Customizingable Macro

# 별 매크로
 게임이나 소프트웨어를 대상으로 사용 가능한 간단한 매크로 프로그램



# 기획용 메세지

## 기능 축약

 - 스크린 캡처
 - IMGUI 기반으로한 편리한 GUI
 - 특정 좌표지점 OCR 텍스트 인식
 - 특정 좌표지점 이미지 인식
 - 특정 이미지 서치를 위한 스크린 크랍 & 이미지 불러오기 기능
 - Condition 과 Action을 명확히한다.
 - 하드웨어 및 소프트웨어 기반 인풋제공
 - 앱플레이어를 위한 설정 제공
 - 매우 깔끔하고 편리한 GUI  
 - 
## 인풋 방식 
 인풋 방식은 아래의 메서드 중 `1,2,4` 항목 메서드는 반드시 채용하지만, 아두이노는 떨어지는 범용성, 커스텀 HID 드라이버는 개발 복잡도가 및 버그가 많아지므로 정말 필요한 경우 구현이 가능하도록 인터페이스화 하여 인풋을 구현한다.
 
1. `SendMessage` 방식 (백그라운드 인풋, detectable)
2. `SendInput` 방식 (글로벌 인풋, detectable)
3. `Arduino ATmega32u4` 칩셋 방식 (하드웨어 인풋 및 장치필요, undetectable)

4. 오토핫키 api 을 연동한 `Interception 드라이버` 방식 (Interception driver를 사용하며, 하드웨어 인풋, undetectable)  
- https://github.com/evilC/AutoHotInterception

5. 커스텀 `HID드라이버` 방식 (하드웨어 인풋, undetectable)  
- https://github.com/changeofpace/MouClassInputInjection  
- https://github.com/changeofpace/MouHidInputHook  

이 경우 좀 귀찮은건 드라이버와 통신 가능한 C#코드를 작성해야함


## 이미지 서칭 및 크랍방식

### 이미지 크립 구현 아이디어 
 - WPF의 `TopMost` 옵션을 이용하여 Crop Box를 그린다
 - 크랍된 `Left Top` 과 `Bottom Right`의 좌표값을 가져와서 https://stackoverflow.com/questions/3241220/how-to-capture-part-of-a-screen 를 이용하여 캡처
 - 캡처된 이미지는 WPF의 View로 보여주고 `Name Tag`를 지정하여 파일로 저장시킨다

### 커맨드 시스템 
 - 커맨드 시스템은 일반 유저가 쉽게 커스텀할 수 있게 만드는것이 포인트 이므로 복잡한 세부사항 설정이 없어야한다.
 - 커맨드 시스템은 Keyboard & Mouse 의 이벤트 레코딩이 가능해야한다
 - 커맨드 시스템은 Delay 조절이 가능해야한다

 
### 구현 방법1 - Condition Action Merge Method
 조건과 액션을 병합한 방식으로 개발한다. A->B->C->D 같은 꼬리물기 실행순서를 가진다
 
### 구현 방법2 - Condition Action Method
 조건과 액션을 병합하지 않고 분리한다. 예를 들면, 이미지를 찾은경우 -> 액션, 못찾은 경우 -> 액션의 분리형태, 좀 더 세밀한 조작이 가능하지만
 유저 입장에서는 굉장히 복잡한 작업이 필요하다. 스타크래프트 트리거 시스템을 기반으로 개선하여 작성하면 될 듯
 
### 구현 방법3 - FSM Method
 유니티 애니메이터 GUI 아이디어를 기반으로 FSM 방식의 구현을 제공하지만, 유저 입장에서는 굉장한 복잡한 작업이 필요하다. 
 
### 커맨드 종류
 - X,Y 위치로 마우스 이동 (순간이동)
 - X,Y 위치로 N 초 동안 마우스 이동 (절차적 이동)
 - 대상 프로세스의 상대좌표 X,Y에 클릭 이벤트 보내기
 - 딜레이 추가하기
 - *대화방 이름* 에 카카오톡 메세지 보내기
 - 시작 위치 sX, sY에서 끝 위치 lX,lY 까지 드래그 이벤트 보내기 
 
 ..등등 추가 예정
