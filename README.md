## Концепты
https://github.com/rolandhemmer/dotnet-rest-api-example/

https://github.com/brunobritodev/RESTFul.API-Example/


## Curl Запросы

* Добавление листа

Принимает Json { "Title": \<ListTitle\> } и в параметрах запроса boardId

curl -X POST http://localhost:5229/api/list?boardId=1 -H "Content-Type: application/json" -d "{\"Title\": \"New Project List\"}"

* Получение листа

Принимает Парметры пути /<ListId> и возвращает объект { Id: \<ListId\>, Title: \<ListTitle\> }

curl -X GET http://localhost:5229/api/list/<ListId> -H "Content-Type: application/json"

* Получение задачи

curl -X GET http://localhost:5229/api/task/<TaskId> -H "Content-Type: application/json"

* Создание задачи

curl -X POST http://localhost:5229/api/task?listId=1 -H "Content-Type: application/json" -d "{\"Title\": \"New Task\"}"