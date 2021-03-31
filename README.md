* To build database

Steps

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
