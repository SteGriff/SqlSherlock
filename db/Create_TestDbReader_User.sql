USE [master]
GO
CREATE LOGIN [TestDbReader] WITH PASSWORD=N'TestDbReader', DEFAULT_DATABASE=[TestDb], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [TestDb]
GO
CREATE USER [TestDbReader] FOR LOGIN [TestDbReader]
GO
USE [TestDb]
GO
ALTER USER [TestDbReader] WITH DEFAULT_SCHEMA=[dbo]
GO
USE [TestDb]
GO
EXEC sp_addrolemember N'db_datareader', N'TestDbReader'
GO
