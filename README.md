# Описание API

## Работа с пользователями
### 1. Получение своих данных
- **GET**: `/api/user/me`
- **-H Authorization**: `"Bearer <Token>"`
- **Return**: ```json
    {
        "Username": "<Username>"
    }```

### 2. Регистрация нового пользователя в системе
- **POST**: `/api/user/registration`
- **Body**: ```json 
    { 
        "Username": "<Username>", 
        "PasswordHash": "<PasswordHash>" 
    }```
- **Return**: ```json
    {
        "Token": "<Token>" 
    }```

### 3. Вход в систему от пользователя
- **POST**: `/api/auth/login`
- **Body**: ```json 
    { 
        "Username": "<Username>", 
        "PasswordHash": "<PasswordHash>"
    }```
- **Return**: ```json 
    { 
        "Token": <Token> 
    }```

## Работа с досками
### 1. Получение всех досок
- **GET**: `/api/board`
- **-H Authorization**: `Bearer <Token>`
- **Return**: ```json 
    [
        { 
            "Id": "<BoardId>", 
            "Title": "<BoardTitle>" 
        }, 
        ...
    ]```

### 2. Получение доски по id
- **GET**: `/api/board/<BoardId>`
- **-H Authorization**: `Bearer <Token>`
- **Return**: ```json
    { 
        "Id": "<BoardId>", 
        "Title": "<BoardTitle>", 
        "Lists": 
            [
                { 
                    "Id": "<ListId>", 
                    "Title": "<ListTitle>", 
                    "Tasks": 
                        [
                            { 
                                "Id": "<TaskId>", 
                                "Title": "<TaskTitle>" 
                            }, 
                            ...
                        ] 
                }, 
                    ...
            ]
    }```

### 3. Удаление доски по id
- **DELETE**: `/api/board/<BoardId>`
- **-H Authorization**: `Bearer <Token>`
- **Return**: `NoContent`

### 4. Обновление доски по id
- **PUT**: `/api/board/<BoardId>`
- **-H Authorization**: `Bearer <Token>`
- **Body**: ```json
    { 
        "Title": "<NewBoardTitle>"
    }```
- **Return**: ```json
    { 
        "Title": "<UpdatedBoardTitle>" 
    }```

### 5. Создание новой доски
- **POST**: `/api/board`
- **-H Authorization**: `Bearer <Token>`
- **Body**: ```json
    { 
        "Title": "<BoardTitle>"
    }```
- **Return**: ```json
    {
        "Id": "<BoardId>", 
        "Title": "<BoardTitle>" 
    }```

## Работа с листами
### 1. Создание нового листа
- **POST**: `/api/list`
- **-H Authorization**: `Bearer <Token>`
- **Body**: ```json
    {
        "Title": "<ListTitle>"
    }```
- **Return**: ```json
    {
        "Id": "<ListId>", 
        "Title": "<ListTitle>"
    }```

### 2. Обновление листа по id
- **PUT**: `/api/list/<ListId>`
- **-H Authorization**: `Bearer <Token>`
- **Body**: ```json 
    { 
        "Title": "<NewListTitle>"
    }```
- **Return**: ```json
    {
        "Id": "<ListId>", 
        "Title": "<UpdatedListId>" 
    }```

### 3. Получение листа по id
- **GET**: `/api/list/<ListId>`
- **-H Authorization**: `Bearer <Token>`
- **Return**: ```json
    { 
        "Id": "<ListId>", 
        "Title": "<ListTitle>", 
        "Tasks": 
            [
                { 
                    "Id": "<TaskId>", 
                    "Title": "<TaskTitle>" 
                }, 
                ...
            ] 
    }```

### 4. Удаление листа по id
- **DELETE**: `/api/list/<ListId>`
- **-H Authorization**: `Bearer <Token>`
- **Return**: `NoContent`

## Работа с тасками
### 1. Создание нового таска
- **POST**: `/api/task`
- **-H Authorization**: `Bearer <Token>`
- **Body**: ```json
    { 
        "Title": "<TaskTitle>"
    }```
- **Return**: ```json
    { 
        "Id": "<TaskId>", 
        "Title": "<TaskTitle>"
    }```

### 2. Получение таска по id
- **GET**: `/api/task/<TaskId>`
- **-H Authorization**: `Bearer <Token>`
- **Return**: ```json
    { 
        "Id": "<TaskId>", 
        "Title": "<TaskTitle>"
    }```

### 3. Обновление таска по id
- **PUT**: `/api/task/<TaskId>`
- **-H Authorization**: `Bearer <Token>`
- **Body**: ```json
    { 
        "Title": "<NewTaskTitle>", 
        "ListId": "<NewListId>" 
    }```
- **Return**: ```json
    { 
        "Id": "<TaskId>", 
        "Title": "<UpdatedTaskTitle>" 
    }```

### 4. Удаление таска по id
- **DELETE**: `/api/task/<TaskId>`
- **-H Authorization**: `Bearer <Token>`
- **Return**: `NoContent`