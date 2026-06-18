
CREATE DATABASE Clinica_Medica;
USE Clinica_Medica;

-- tabela dos medicos
CREATE TABLE medico (
    id_medico INT AUTO_INCREMENT PRIMARY KEY,
    crm VARCHAR(20) NOT NULL UNIQUE,        -- o crm nao pode repetir
    nome VARCHAR(100) NOT NULL,
    especialidade VARCHAR(100),
    telefone VARCHAR(15)
);

-- tabela dos pacientes
CREATE TABLE paciente (
    id_paciente INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(11) NOT NULL,
    data_nascimento DATE,
    endereco VARCHAR(100),
    telefone VARCHAR(15)
);

-- tabela dos consultorios (a sala onde atende)
CREATE TABLE consultorio (
    id_consultorio INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100),
    endereco VARCHAR(100),
    numero VARCHAR(20),
    complemento VARCHAR(100)
);

-- tabela das consultas
-- ela vem por ultimo porque liga paciente + medico + consultorio

CREATE TABLE consulta (
    id_consulta INT AUTO_INCREMENT PRIMARY KEY,
    data_consulta DATE,
    horario TIME,
    -- os status tem que ser igual o enum do c# 
    status_consulta ENUM('Agendada', 'Concluida', 'Cancelada', 'Reagendada') DEFAULT 'Agendada',
    id_paciente INT,
    id_medico INT,
    id_consultorio INT,
    FOREIGN KEY (id_paciente) REFERENCES paciente(id_paciente),
    FOREIGN KEY (id_medico) REFERENCES medico(id_medico),
    FOREIGN KEY (id_consultorio) REFERENCES consultorio(id_consultorio)
);

-- uns dados de exemplo pra testar o sistema
INSERT INTO medico (crm, nome, especialidade, telefone) VALUES
('12345', 'Dr. Joao Souza', 'Cardiologia', '31988887777'),
('54321', 'Dra. Maria Lima', 'Dermatologia', '31999996666'),
('98765', 'Dr. Pedro Alves', 'Cardiologia', '31977775555');

INSERT INTO consultorio (nome, endereco, numero, complemento) VALUES
('Sala 1', 'Rua das Flores', '100', 'Terreo'),
('Sala 2', 'Rua das Flores', '100', '1 andar');

INSERT INTO paciente (nome, cpf, data_nascimento, endereco, telefone) VALUES
('Carlos Silva', '11122233344', '1990-05-10', 'Av Brasil 500', '31966665555');
