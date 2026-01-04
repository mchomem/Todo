# Presentation

This project is a software for simplified registration of tasks that a person needs to perform.

# Architecture

The project is divided into two parts: the backend, written in C#.net and accessing a SQL Server database, and the frontend, which uses HTML, CSS, and vanilla JavaScript.

# To build database

## Steps

1. Select Todo.Core project;
2. Open the Packag Manager Console panel and type the follow code:

	** Entity Framework line commands to create database with specific path to migrations folder.

	1. Init database:
	add-migration Init -outputdir "Models/DataBase/Migrations"

	update-database -verbose

	2. Creates a UserPicture entity with relationship to User.
	add-migration AddPictureInUser -outputdir "Models/DataBase/Migrations"

	update-database -verbose

	PS: to remove the last migration file, run this command in Packag Manager Console:

	remove-migration

## Update 2025

Since this solution involves initialization in more than one project, the following command line should be used to build the database.

* The line command to create a database is:

    update-database -Context TodoContext -Project Todo.Infra.Database -StartupProject Todo.API

