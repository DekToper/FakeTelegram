
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/09/2020 02:02:31
-- Generated from EDMX file: C:\Users\Діма\source\repos\WpfApp11\Server\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TelegramDataBase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GroupUsersGroups]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersGroups] DROP CONSTRAINT [FK_GroupUsersGroups];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUsersGroups]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersGroups] DROP CONSTRAINT [FK_UserUsersGroups];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[UsersGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersGroups];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsersGroups'
CREATE TABLE [dbo].[UsersGroups] (
    [GroupId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [GroupId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ImagePath] nvarchar(max)  NOT NULL,
	[DataPath] nvarchar(max) NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [GroupId], [UserId] in table 'UsersGroups'
ALTER TABLE [dbo].[UsersGroups]
ADD CONSTRAINT [PK_UsersGroups]
    PRIMARY KEY CLUSTERED ([GroupId], [UserId] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [GroupId] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([GroupId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [GroupId] in table 'UsersGroups'
ALTER TABLE [dbo].[UsersGroups]
ADD CONSTRAINT [FK_GroupUsersGroups]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([GroupId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'UsersGroups'
ALTER TABLE [dbo].[UsersGroups]
ADD CONSTRAINT [FK_UserUsersGroups]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUsersGroups'
CREATE INDEX [IX_FK_UserUsersGroups]
ON [dbo].[UsersGroups]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------