# CURSUS2
한국산업기술대학교 졸업 프로젝트 소스 코드

구현 목적
 - 한국산업기술대학교 졸업 프로젝트 제출용
 - VR 기기와 자전거 + PC간 데이터 연동을 이용한 자전거 시뮬레이션 게임 제작을 목적으로 함
 - 기본적인 서버 기능을 통해 로그인 및 회원 가입, 소셜 기능, 경쟁 모드 구현을 목적으로 함
 
주요 활용 기술
 - C++, STL, boostAsio, Node js, MySql
 - Sql Server DB 프로시저 사용
 
 구현 세부 사항 및 담당 분야
 1. 클라이언트
  - Occulus VR에 사용할 UI 시스템 제작
  - 자전거에 부착된 센서 장비를 Unity3D로 보내 속도 동기화
  - 경쟁 모드 제작, UI 디자인 수정, 기상 효과 적용
  
2. 서버
  - boost ASIO를 이용한 크로스 플랫폼 빌드 서버 구현(CMake를 통해 윈도우, OSX에서 빌드)
  - 로그인 / 회원가입, 월드 내에서 위치 정보 전송, 플레이어 랭킹 저장 및 전송 구현
  - 대전 모드를 통한 각 플레이어 및 AI의 속도 수치 동기화, 트랙 대결 로직 처리
  - node.js 서버를 활용해 기상청으로 부터 날씨 정보를 받아 서버와 연동, 클라이언트에 송신
