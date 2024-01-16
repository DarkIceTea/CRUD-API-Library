# CRUD Books API

Проект CRUD Books API представляет собой ASP.NET Core веб-приложение для управления книгами.

# Документация API

API предоставляет следующие эндпоинты:

GET /books: Получение списка всех книг.

GET /book/{id}: Получение информации о книге по идентификатору.

GET /book/ISBN/{isbn}: Получение информации о книге по ISBN.

POST /book: Добавление новой книги.

PUT /book/{id}: Редактирование информации о книге по идентификатору.

DELETE /book/{id}: Удаление книги по идентификатору.

POST /login: аутентификация в системе

POST /register: регистрация в системе

Дополнительная информация о каждом эндпоинте и вариантах использования доступна в Swagger UI.

# Аутентификация

API поддерживает Bearer Token аутентификацию. Для использования аутентификации через Swagger UI, введите токен в соответствующее поле.
