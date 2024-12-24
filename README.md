# RabbitExemplo
 
No DB Rodas o Script:


```
CREATE DATABASE TESTE_DB
GO

USE TESTE_DB
GO

CREATE TABLE Notifications (
 Id INT IDENTITY,
 Message VARCHAR(1000)
 Date DATETIME
)
```


Depois no CMD na pasta /RabbitExemplo rodar:

```
 docker-compose up -d
```
