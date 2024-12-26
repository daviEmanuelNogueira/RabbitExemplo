# RabbitExemplo

Este é um exemplo simples de integração do RabbitMQ com uma aplicação .NET para demonstrar como usar filas para comunicação assíncrona entre uma API e um worker que processa dados.

A aplicação utiliza **RabbitMQ** para gerenciar mensagens, um **worker** para consumir as mensagens da fila e salvá-las no banco de dados, e o **SQL Server 2022** como banco de dados relacional.

---

## Funcionalidade

O sistema possui uma API que expõe um endpoint para receber mensagens e armazená-las em uma fila no RabbitMQ. Um worker processa essas mensagens consumindo a fila e salvando os dados no banco de dados.

### Fluxo do Sistema:

1. **Envio de Mensagem**:
    - O cliente envia um objeto JSON para o endpoint:
      ```
      POST http://localhost:8080/notification
      ```
      Corpo da requisição:
      ```json
      {
         "message": "Hello, RabbitMQ!",
         "date": "2024-12-25T01:00:00.000Z"
      }
      ```

2. **Armazenamento na Fila**:
    - A API armazena a mensagem na fila gerenciada pelo RabbitMQ.

3. **Processamento pelo Worker**:
    - Um worker consome as mensagens da fila RabbitMQ, processa os dados e os salva no banco de dados SQL Server 2022.

---

## Como rodar o projeto

### Pré-requisitos

- Docker.
- Ambiente configurado para suportar containers Docker.

### Passos para execução

1. Clone o repositório:
    ```bash
    git clone https://github.com/daviEmanuelNogueira/RabbitExemplo.git
    ```

2. Rodar com Docker:
     - Certifique-se de que o Docker está instalado e em funcionamento.
     - Navegue até a pasta do projeto e execute o container Docker:
          ```bash
          docker-compose up -d
          ```
     - Após a conclusão do build e pull das imagens, certifique-se de que os serviços foram executados no Docker com o comando:
         ```bash
         docker container ls
         ```

3. Testar:
     - Abra o Postman ou qualquer ferramenta de sua preferência e realize a chamada ao endpoint da API, conforme o Item 1 do **Fluxo do Sistema**.

## Tecnologias usadas

- **.NET 9**: Para desenvolvimento da Minimal API e Worker.
- **RabbitMQ 4.0-management**: Serviço de Mensageria.
- **Docker**: Para rodar a aplicação em um ambiente isolado, sem a necessidade de instalar o .NET localmente.
- **SQL Server 2022**: Para armazenamento das mensagens processadas.

## Conclusão

Este exemplo demonstra a simplicidade e a eficiência de usar RabbitMQ em conjunto com uma aplicação .NET para comunicação assíncrona e processamento em segundo plano. A arquitetura permite escalar o sistema horizontalmente, adicionando mais workers para lidar com o aumento da demanda.
