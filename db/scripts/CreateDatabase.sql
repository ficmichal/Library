CREATE DATABASE [Library] CONTAINMENT = NONE 
ON PRIMARY 
( NAME = N'Library', FILENAME = N'/var/opt/mssql/data/Library.mdf' , SIZE = 8192KB , FILEGROWTH = 65536KB ) 
LOG ON  ( NAME = N'Library_log', FILENAME = N'/var/opt/mssql/data/Library_log.ldf' , SIZE = 8192KB , FILEGROWTH = 65536KB )
GO

USE [Library]
GO