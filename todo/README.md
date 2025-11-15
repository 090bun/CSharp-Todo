
# C# TODO 練習 API 文件
本專案為以 **ASP.NET Core Web API** 實作的後端練習，內容包含：

- Todo CRUD（含批次修改）
- User 管理與角色權限（Admin/User）
- JWT Token 驗證機制
- BCrypt 密碼雜湊
- MS SQL Server連結及操作

---

## 技術架構
- ASP.NET Core Web API
- Entity Framework Core
- Microsoft SQL Server
- JWT 驗證
- BCrypt 密碼雜湊
---

###  資料表設計簡述

#### **User**
| 欄位 | 型別 | 說明 |
|------|------|------|
| Id | int | 主鍵 |
| Name | nvarchar | 使用者姓名 |
| Account | nvarchar | 帳號 |
| Password | nvarchar | 加密後密碼（BCrypt） |
| Role | nvarchar | admin / user |

#### **UserInfo**
| 欄位 | 型別 | 說明 |
|------|------|------|
| Id | int | 主鍵 |
| Address | nvarchar | 地址 |
| Phone | nvarchar | 手機 |
| Birthday | nvarchar | 生日 |
| UserId | int | 外鍵（對應 User.Id） |

#### **Todo**
| 欄位 | 型別 | 說明 |
|------|------|------|
| Id | uniqueidentifier | Todo 的主鍵 |
| Title | nvarchar | 標題 |
| Description | nvarchar | 內容 |
| CreateAt | datetime | 建立時間 |
| UpdateAt | datetime | 最後更新時間 |
| FinishAt | datetime | 完成時間 |
| DeleteAt | datetime | 刪除時間（軟刪除使用） |
| UserId | int | 外鍵（對應 User.Id） |


---

## API 目錄

###  TODO 模組
- 查詢 Todo
- 新增 Todo
- 修改 Todo
- 刪除 Todo
- 完成 Todo

###  USER 模組
- 查詢 User
- 修改 User權限
- 更新 User
- 新增 User

###  身分驗證
- Login（登入取得 Token）

---

# TODO

## 查詢Todo
**GET** `/api/todo`


### Response 200
```json
{
  "success": true,
  "message": "查詢成功",
  "data": []
}
```

---

## 新增Todo
**POST** `/api/todo`

### Body
```json
{
  "Title": "示例標題",
  "Description": "示例內容"
}
```

---

## 修改Todo（單筆/多筆）
**PATCH** `/api/todo`

### Body
```json
[
  {
    "Id": "GUID",
    "Title": "新標題",
    "Description": "新內容"
  }
]
```

---

## 刪除Todo
**DELETE** `/api/todo`

### Body
```json
[
  { "Id": "GUID" }
]
```

---

## 完成Todo
**PATCH** `/api/todo/finish`

### Body
```json
[
  { "Id": "GUID" }
]
```

---

# USER

## 查詢User
**GET** `/api/user`

---

## 修改User權限
**PATCH** `/api/user/role`

### Body
```json
[
  { "id": 1, "role": "admin" }
]
```

---

## 更新 User
**PATCH** `/api/user`

### Body
```json
{
  "name": "ABC",
  "userInfo": {
    "address": "123 Main St",
    "phone": "0913111111"
  }
}
```

---

## 新增User
**POST** `/api/user`

### Body
```json
{
  "password": "000000",
  "name": "zzz",
  "account": "zzz",
  "role": "user",
  "userInfo": {
    "address": "123 Main St",
    "birthday": "1990/01/01",
    "phone": "0913549990"
  }
}
```

---

# Login登入

## Login
**POST** `/api/login`

### Body
```json
{
  "account": "zzz",
  "password": "000000"
}
```

---

#  注意事項
- 所有受保護 API 必須帶入 Authorization Header(Login除外)
- 密碼儲存使用 BCrypt Hash（不可逆）
- Token 內容包含 UserId 與 Role（admin/user）
---
