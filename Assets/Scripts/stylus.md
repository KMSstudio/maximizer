# Stylus

문서 「Stylus.md」는 Maximizer 프로젝트의 스크립트 작성 스타일과 구조 규칙을 규정하기 위하여 작성되었습니다.

작성자 - 강명석  
최신개정일 - `2025.04.30`  
최초작성일 - `2025.04.30`

---

## 세줄 요약

- verbose 한 것이 싫다. 정말 싫다.
- 함수 안에다가 주석 쓰지 마라
- 주석 스타일은 통일해서 함수 선언문 바로 위에 쓴다.

---

## 1. Brace Style

- 모든 여는 중괄호 `{`는 해당 제어문이나 함수 선언과 **같은 줄**에 위치해야 합니다.

```csharp
void FunctionName() {
    // ...
}
```

---

## 2. Control Structures

- 제어문이 한 문장만을 포함할 경우, 아래와 같이 한 줄로 작성합니다. 
- `return`문의 경우, 한 문장으로 생각하지 않는 것이 바람직합니다. 다시 말해, `if (condition) { instruction; return; }` 처럼 작성할 수 있습니다.
- 한 문장이 **간단한** 한 자연어로 설명될 수 있을 경우, 한 줄에 작성하는 것을 권장합니다.

```csharp
if (${condition}) { ${instruction}; }
```

예시:

```csharp
if (x > 0) { DoSomething(); }
for (int i = 0; i < 5; i++) { Debug.Log(i); }
```

---

## 3. Function Logging

- 자연어로 설명할 수 있는 기능을 가지는 함수 중 자주 call 되지 않는 함수는 아래와 같은 로그를 출력할 것을 권장합니다.

```csharp
Debug.Log("${function_name} call");
```

---

## 4. Functional Annotation

- 자연어로 설명할 수 있는 기능을 가지는 함수 상단에는 영문으로 주석을 작성하는 것이 권장됩니다.
- 설명이 길거나 인자가 복잡할 경우 JSdoc 스타일 사용을 권장합니다.

예시:

```csharp
// Try to spawn an entity at a fixed position
void TrySpawnEntity() {
    Debug.Log("TrySpawnEntity call");
    // ...
}
```

---

## 5. Intra-function Comment Avoidance

- 함수 내부에서는 주석 사용을 지양합니다.
