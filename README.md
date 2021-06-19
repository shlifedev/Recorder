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
 
## 개발 예정 인풋방식 
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
