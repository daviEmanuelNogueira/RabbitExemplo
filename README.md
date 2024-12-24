# RabbitExemplo

 

No CMD na pasta /RabbitExemplo rodar:

```
 docker-compose up -d
```

Depois no DB Rodas o Script:


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

#NOTA o WS não espera o RBQ (RabbitMQ) Iniciar, esperar um pouco s startar manualmente
