USE master;
CREATE DATABASE test;
GO

USE test;
CREATE TABLE [outpyt](
	id INT IDENTITY (1,1),
	myText NVARCHAR(20)
);
GO

USE test;
INSERT INTO [outpyt](myText) VALUES (N'Привет!');

use test;
        insert into [outpyt](myText) values (N'TEST');


USE master;
DROP DATABASE test;
GO