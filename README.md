# Projeto Clientes

Projeto composto por 3 microsserviços que se comunicam através de mensageria:

## Clientes
API que recebe o cadastro de um cliente e envia as informações desse cliente para outros serviços via mensageria (RabbitMQ).

## Cartão Crédito
Worker que recebe a mensagem com as informações do cliente e gera um cartão de crédito para ele.

## Proposta Crédito
Worker que recebe a mensagem com as informações do cliente e gera uma proposta de crédito para ele.

## Como rodar
Para rodar o projeto, é necessário ter uma instância do RabbitMQ em funcionamento. Pode ser local, via docker ou de outra forma (dependendo de como você utilizar o RabbitMQ, pode ser necessário ajustar as configurações dos 3 microsserviços, para corrigir hostname, username e password).

Em seguida, rodar o serviço Clientes, que criará a exchange do RabbitMQ onde serão publicadas as mensagens.

Na sequência, rodar o Cartão Crédito e o Proposta Crédito, que criarão suas próprias filas ligadas à exchange criada pelo Clientes.

Para testar, pode-se usar o swagger aberto pelo Clientes, ou realizar uma chamada pelo Postman.

Exemplo de requisição:

'''
curl --location 'http://localhost:5127/cliente/cadastro' \
--header 'Content-Type: application/json' \
--data '{
    "nome": "Ana",
    "cpf": "1234567890",
    "nascimento": "2000-10-28"
}'
'''
