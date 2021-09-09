﻿CREATE TABLE CatalogueBook (
  Id INTEGER IDENTITY PRIMARY KEY,
  Isbn VARCHAR(100) NOT NULL,
  Title VARCHAR(100) NOT NULL,
  Author VARCHAR(100) NOT NULL);


CREATE TABLE CatalogueBookInstance (
  Id INTEGER IDENTITY PRIMARY KEY,
  Isbn VARCHAR(100) NOT NULL,
  Book_id UNIQUEIDENTIFIER  NOT NULL);

CREATE SEQUENCE CatalogueBookSeq;
CREATE SEQUENCE CatalogueBookInstanceSeq;

CREATE TABLE BookDatabaseEntity (
  Id INTEGER IDENTITY PRIMARY KEY,
  BookId UNIQUEIDENTIFIER UNIQUE,
  BookType VARCHAR(100) NOT NULL,
  BookState VARCHAR(100) NOT NULL,
  AvailableAtBranch UNIQUEIDENTIFIER,
  OnHoldAtBranch UNIQUEIDENTIFIER,
  OnHoldByPatron UNIQUEIDENTIFIER,
  CheckedOutAtBranch UNIQUEIDENTIFIER,
  CheckedOutByPatron UNIQUEIDENTIFIER,
  OnHoldTill DATETIME,
  Version INTEGER);
  
CREATE SEQUENCE BookDatabaseEntitySeq;


CREATE TABLE PatronDatabaseEntity (Id INTEGER IDENTITY PRIMARY KEY, PatronType VARCHAR(100) NOT NULL, PatronId UNIQUEIDENTIFIER UNIQUE);

CREATE TABLE HoldDatabaseEntity (
  Id INTEGER IDENTITY PRIMARY KEY,
  BookId UNIQUEIDENTIFIER NOT NULL,
  PatronId UNIQUEIDENTIFIER NOT NULL,
  LibraryBranchId UNIQUEIDENTIFIER NOT NULL,
  PatronDatabaseEntity INTEGER NOT NULL,
  Till DATETIME NOT NULL,
  CONSTRAINT [FK_HoldDatabaseEntity_PatronDatabaseEntity] FOREIGN KEY (PatronDatabaseEntity) REFERENCES PatronDatabaseEntity ([Id]));