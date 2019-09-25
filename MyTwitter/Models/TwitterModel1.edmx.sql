
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/20/2019 01:19:10
-- Generated from EDMX file: E:\FSD\Kavitha\MyTwitter\MyTwitter\Models\TwitterModel1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Twitter];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PersonTweet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tweets] DROP CONSTRAINT [FK_PersonTweet];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonFollowers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Followers] DROP CONSTRAINT [FK_PersonFollowers];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[People]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People];
GO
IF OBJECT_ID(N'[dbo].[Tweets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tweets];
GO
IF OBJECT_ID(N'[dbo].[Followers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Followers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [user_Id] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [fullName] nvarchar(max)  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [joined] datetime  NOT NULL,
    [active] bit  NOT NULL
);
GO

-- Creating table 'Tweets'
CREATE TABLE [dbo].[Tweets] (
    [tweet_id] int IDENTITY(1,1) NOT NULL,
    [user_id] nvarchar(max)  NOT NULL,
    [message] nvarchar(max)  NOT NULL,
    [created] datetime  NOT NULL,
    [Person_user_Id] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Followers'
CREATE TABLE [dbo].[Followers] (
    [FId] int IDENTITY(1,1) NOT NULL,
    [user_id] nvarchar(max)  NOT NULL,
    [following_id] nvarchar(max)  NOT NULL,
    [Person_user_Id] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [user_Id] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([user_Id] ASC);
GO

-- Creating primary key on [tweet_id] in table 'Tweets'
ALTER TABLE [dbo].[Tweets]
ADD CONSTRAINT [PK_Tweets]
    PRIMARY KEY CLUSTERED ([tweet_id] ASC);
GO

-- Creating primary key on [FId] in table 'Followers'
ALTER TABLE [dbo].[Followers]
ADD CONSTRAINT [PK_Followers]
    PRIMARY KEY CLUSTERED ([FId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Person_user_Id] in table 'Tweets'
ALTER TABLE [dbo].[Tweets]
ADD CONSTRAINT [FK_PersonTweet]
    FOREIGN KEY ([Person_user_Id])
    REFERENCES [dbo].[People]
        ([user_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonTweet'
CREATE INDEX [IX_FK_PersonTweet]
ON [dbo].[Tweets]
    ([Person_user_Id]);
GO

-- Creating foreign key on [Person_user_Id] in table 'Followers'
ALTER TABLE [dbo].[Followers]
ADD CONSTRAINT [FK_PersonFollowers]
    FOREIGN KEY ([Person_user_Id])
    REFERENCES [dbo].[People]
        ([user_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonFollowers'
CREATE INDEX [IX_FK_PersonFollowers]
ON [dbo].[Followers]
    ([Person_user_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------