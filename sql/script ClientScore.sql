CREATE DATABASE ClienteScoreDB;

USE ClienteScoreDB;

-- DROP TABLE ClienteScoreDB.dbo.Cliente;

CREATE TABLE ClienteScoreDB.dbo.Cliente (
	Id bigint IDENTITY(1,1) NOT NULL,
	Nome nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DataNascimento date NOT NULL,
	CPF char(11) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	RendimentoAnual decimal(18,2) NOT NULL,
	Telefone nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DDD char(2) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT '00' NOT NULL,
	Score int DEFAULT 0 NOT NULL,
	CONSTRAINT PK__Cliente__3214EC0762A999C7 PRIMARY KEY (Id),
	CONSTRAINT UQ__Cliente__C1F897313BB08F3B UNIQUE (CPF)
);


-- DROP TABLE ClienteScoreDB.dbo.Endereco;

CREATE TABLE ClienteScoreDB.dbo.Endereco (
	Id bigint IDENTITY(1,1) NOT NULL,
	ClienteId bigint NOT NULL,
	Estado nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Cidade nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Rua nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Numero nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Complemento nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CEP char(8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK__Endereco__3214EC0708E26FF6 PRIMARY KEY (Id),
	CONSTRAINT UQ__Endereco__71ABD086C67944CF UNIQUE (ClienteId)
);


-- ClienteScoreDB.dbo.Endereco foreign keys

ALTER TABLE ClienteScoreDB.dbo.Endereco ADD CONSTRAINT Endereco_Cliente_FK FOREIGN KEY (ClienteId) REFERENCES ClienteScoreDB.dbo.Cliente(Id) ON DELETE CASCADE;

