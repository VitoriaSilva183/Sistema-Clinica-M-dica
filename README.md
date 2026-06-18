# Sistema de Clínica Médica

Sistema desktop (Windows Forms / C# .NET 8) para uma clínica médica. Permite cadastrar pacientes, médicos e consultórios, e agendar consultas escolhendo especialidade, médico, dia e horário. Os dados ficam num banco MySQL.

## O que precisa pra rodar

- .NET 8 SDK
- MySQL instalado e rodando (localhost)

## Passo a passo

1. Abra o MySQL e rode o arquivo `banco.sql`. Ele cria o banco `Clinica_Medica`, as tabelas e já coloca uns dados de exemplo.
2. Se a senha do seu usuário `root` não for vazia, abra `Database/ConexaoBanco.cs` e ajuste a linha da string de conexão (campo `Password=`).
3. Na pasta do projeto, rode:

   ```
   dotnet run
   ```

4. A tela do menu abre. A partir dela dá pra abrir cada parte do sistema.

## Como usar

- **Pacientes / Médicos / Consultórios:** telas de cadastro. Clique numa linha da tabela pra editar, use "Limpar" pra começar um cadastro novo.
- **Agendar Consulta:** escolha a especialidade (os médicos aparecem filtrados por ela), depois médico, paciente, consultório, dia e horário. O sistema não deixa marcar dois horários iguais pro mesmo médico.
- **Ver Consultas:** lista tudo que está agendado, com opção de cancelar ou excluir.

## Organização das pastas

- `Models/` — as classes (Paciente, Medico, Consultorio, Consulta)
- `Database/` — a conexão com o banco
- `Repositories/` — as classes que fazem o INSERT / SELECT / UPDATE / DELETE
- `Forms/` — as telas
- `docs/` — requisitos (RF/RNF) e os diagramas
- `banco.sql` — script do banco
