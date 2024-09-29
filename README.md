# Описание API

## Получение своих данных
GET: /api/user/me
-H Authorization: "Bearer <Token>"
Return: { "User" : { "Id": <UserId>, "Username": <Username> } } 

## Регистрация нового пользователя в системе
POST: /api/user/registration
Body: { "Username": <Username>, "PasswordHash": <PasswordHash> }
Return: { "Token": <Token> } 

## Вход в систему от пользователя
POST: /api/auth/login
Body: { "Username": <Username>, "PasswordHash": <PasswordHash>, "Id": <Id> }
Return: { "Token": <Token> } 


curl -X POST http://localhost:5229/api/auth/login -H "Content-Type: application/json" -d '{ "Username": "Rail1", "PasswordHash": "123456789" }'

curl -X POST http://localhost:5229/api/auth/login -H "Content-Type: application/json" -d '{ "Username": "Rail", "PasswordHash": "123456789" }'

curl -X GET http://localhost:5229/api/board/1 -H "Content-Type: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoiUmFpbDEiLCJqdGkiOiJkYWE2MTIzOS0xYzE0LTQ0YjEtYjU0MS03OGMwMWYzZTMyZGMiLCJleHAiOjE3Mjc1OTc0MTksImlzcyI6ImxvY2FsaG9zdDo1MjI5IiwiYXVkIjoibG9jYWxob3N0OjUyMjkifQ.QFw-tIhmEq4TBeDhkkLN63ywUt0qmOK2mXL49_uHcP0"

curl -X GET http://localhost:5229/api/board/1 -H "Content-Type: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoiUmFpbCIsImp0aSI6Ijg0YjQzYmQ5LTY2ZjUtNDNiYS1iY2U4LWJiYmJkZTE2YWJiZiIsImV4cCI6MTcyNzU5NzQyMywiaXNzIjoibG9jYWxob3N0OjUyMjkiLCJhdWQiOiJsb2NhbGhvc3Q6NTIyOSJ9.n04JAcwtPBSphQtGbaUGeiPMQuGi0g-8WC4o12NC6kQ"