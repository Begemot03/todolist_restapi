# Описание API

## Работа с пользователями
### 1. **Получение своих данных**
- **GET**: `/api/user/me`
- **-H Authorization**: `"Bearer <Token>"`
- **Return**: ```json
    {
        "Id": "<UserId>",
        "Username": "<Username>"
    } ```

### Регистрация нового пользователя в системе
POST: /api/user/registration
Body: { "Username": <Username>, "PasswordHash": <PasswordHash> }
Return: { "Token": <Token> } 

### Вход в систему от пользователя
POST: /api/auth/login
Body: { "Username": <Username>, "PasswordHash": <PasswordHash>, "Id": <Id> }
Return: { "Token": <Token> } 

### Получение всех досок
GET: /api/board
-H Authorization: "Bearer <Token>"
Return: [{ "Id" <BoardId>, "Title": <BoardTitle> }, ...]

### Получение доски по id
GET: /api/board/<BoardId>
-H Authorization: "Bearer <Token>"
Return: { "Id": <BoardId>, "Title": <BoardTitle>, "Lists": [{ "Id": <ListId>, "Title": <ListTitle>, "Tasks": [{ "Id": <TaskId>, "Title": <TaskTitle> }, ...] }, ...] }

### Удаление доски по id
DELETE /api/board/<BoardId>
-H Authorization: "Bearer <Token>"
Return: NoContent

### Обновление доски по id
PUT /api/board/<BoardId>
-H Authorization: "Bearer <Token>"
Body: { "Title": <NewBoardTitle> }
Return: { "Title": <UpdatedBoardTitle> }

### Создание новой доски
POST /api/board
-H Authorization: "Bearer <Token>"
Body: { "Title": <BoardTitle> }
Return: { "Id": <BoardId>, "Title": <BoardTitle> }

### Создание нового листа
POST /api/list
-H Authorization: "Bearer <Token>"
Body: { "Title": <ListTitle> }
Return: { "Id": <ListId>, "Title": <ListTitle> }

### Обновление листа по id
PUT /api/list/<ListId>
-H Authorization: "Bearer <Token>"
Body: { "Title": <NewListTitle> }
Return: { "Id": <ListId>, "Title": <UpdatedListId> }

### Получение листа по id
GET /api/list/<ListId>
-H Authorization: "Bearer <Token>"
Return: { "Id": <ListId>, "Title": <ListTitle>, "Tasks": [{ "Id": <TaskId>, "Title": <TaskTitle> }, ...] }

### Удаление листа по id
DELETE /api/list/<ListId>
-H Authorization: "Bearer <Token>"
Return: NoContent

### Создание нового таска
POST /api/task
-H Authorization: "Bearer <Token>"
Body: { "Title": <TaskTitle> }
Return: { "Id": <TaskId>, "Title": <TaskTitle> }

### Получение таска по id
GET /api/task/<TaskId>
-H Authorization: "Bearer <Token>"
Return: { "Id": <TaskId>, "Title": <TaskTitle> }

### Обновление таска по id
PUT /api/task/<TaskId>
-H Authorization: "Bearer <Token>"
Body: { "Title": <NewTaskTitle>, "ListId": <NewListId> }
Return: { "Id": <TaskId>, "Title": <UpdatedTaskTitle> }

### Удаление таска по id
DELETE /api/task/<TaskId>
-H Authorization: "Bearer <Token>"
Return: NoContent