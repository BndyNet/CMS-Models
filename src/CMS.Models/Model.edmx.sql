
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/01/2017 22:57:20
-- Generated from EDMX file: G:\Git\CMS-Models\src\CMS.Models\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CMS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserUserLoginHistory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserLoginHistories] DROP CONSTRAINT [FK_UserUserLoginHistory];
GO
IF OBJECT_ID(N'[dbo].[FK_ArticleCategoryArticle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Articles] DROP CONSTRAINT [FK_ArticleCategoryArticle];
GO
IF OBJECT_ID(N'[dbo].[FK_ArticleAttributeDefinitionArticleAttributeValue]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticleAttributeValues] DROP CONSTRAINT [FK_ArticleAttributeDefinitionArticleAttributeValue];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BlockInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlockInfoes];
GO
IF OBJECT_ID(N'[dbo].[Menus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menus];
GO
IF OBJECT_ID(N'[dbo].[Articles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Articles];
GO
IF OBJECT_ID(N'[dbo].[ArticleCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticleCategories];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[SinglePages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SinglePages];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Attachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attachments];
GO
IF OBJECT_ID(N'[dbo].[UserLoginHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserLoginHistories];
GO
IF OBJECT_ID(N'[dbo].[Links]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Links];
GO
IF OBJECT_ID(N'[dbo].[UserRights]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRights];
GO
IF OBJECT_ID(N'[dbo].[AppExceptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppExceptions];
GO
IF OBJECT_ID(N'[dbo].[ArticleAttributeDefinitions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticleAttributeDefinitions];
GO
IF OBJECT_ID(N'[dbo].[ArticleAttributeValues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticleAttributeValues];
GO
IF OBJECT_ID(N'[dbo].[Resources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Resources];
GO
IF OBJECT_ID(N'[dbo].[ResourceDirectories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ResourceDirectories];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BlockInfoes'
CREATE TABLE [dbo].[BlockInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Menus'
CREATE TABLE [dbo].[Menus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [ShortText] nvarchar(max)  NULL,
    [Icon] nvarchar(max)  NULL,
    [Uri] nvarchar(max)  NULL,
    [Category] smallint  NOT NULL,
    [ContentType] int  NOT NULL,
    [ParentId] int  NULL,
    [DisplayOrder] int  NOT NULL,
    [IsHidden] bit  NOT NULL
);
GO

-- Creating table 'Articles'
CREATE TABLE [dbo].[Articles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Pictures] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [Views] int  NOT NULL,
    [EnableComment] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [CategoryId] int  NOT NULL,
    [EnableAttachment] bit  NULL,
    [HasImageAttachments] bit  NOT NULL,
    [UserId] int  NULL,
    [UserGroupId] int  NULL
);
GO

-- Creating table 'ArticleCategories'
CREATE TABLE [dbo].[ArticleCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [EnableComment] bit  NOT NULL,
    [EnableAttachment] bit  NOT NULL,
    [DisplayOrder] int  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [RefCommentId] int  NULL,
    [ClientIP] nvarchar(max)  NULL,
    [BOId] int  NOT NULL,
    [BOType] int  NOT NULL
);
GO

-- Creating table 'SinglePages'
CREATE TABLE [dbo].[SinglePages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Views] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [EnableComment] bit  NOT NULL,
    [EnableAttachment] bit  NULL,
    [HasImageAttachments] bit  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LoginName] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [RealName] nvarchar(max)  NULL,
    [Role] int  NOT NULL,
    [GroupIds] nvarchar(max)  NULL,
    [Enabled] bit  NOT NULL
);
GO

-- Creating table 'Attachments'
CREATE TABLE [dbo].[Attachments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FileName] nvarchar(max)  NULL,
    [FileExtensionName] nvarchar(max)  NULL,
    [FilePath] nvarchar(max)  NULL,
    [UploadedDate] datetime  NOT NULL,
    [BOId] int  NULL,
    [BOType] int  NULL,
    [IsImage] bit  NOT NULL
);
GO

-- Creating table 'UserLoginHistories'
CREATE TABLE [dbo].[UserLoginHistories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [ClientIP] nvarchar(max)  NOT NULL,
    [Browser] nvarchar(max)  NULL,
    [LoggedOn] datetime  NOT NULL
);
GO

-- Creating table 'Links'
CREATE TABLE [dbo].[Links] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NULL,
    [LinkUrl] nvarchar(max)  NULL,
    [LinkImage] nvarchar(max)  NULL,
    [DisplayOrder] int  NOT NULL
);
GO

-- Creating table 'UserRights'
CREATE TABLE [dbo].[UserRights] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [BOId] int  NOT NULL,
    [BOType] int  NOT NULL
);
GO

-- Creating table 'AppExceptions'
CREATE TABLE [dbo].[AppExceptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] nvarchar(max)  NOT NULL,
    [Message] nvarchar(max)  NULL,
    [Detail] nvarchar(max)  NULL,
    [OccuredDate] datetime  NOT NULL
);
GO

-- Creating table 'ArticleAttributeDefinitions'
CREATE TABLE [dbo].[ArticleAttributeDefinitions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ArticleCategoryId] int  NOT NULL,
    [AttrName] nvarchar(max)  NOT NULL,
    [InitialValue] nvarchar(max)  NULL,
    [AttrDescription] nvarchar(max)  NULL,
    [DisplayOrder] int  NULL,
    [ValueType] int  NOT NULL
);
GO

-- Creating table 'ArticleAttributeValues'
CREATE TABLE [dbo].[ArticleAttributeValues] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ArticleId] int  NOT NULL,
    [AttrId] int  NOT NULL,
    [AttrValue] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Resources'
CREATE TABLE [dbo].[Resources] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FileName] nvarchar(max)  NULL,
    [FileExtensionName] nvarchar(50)  NULL,
    [FilePath] nvarchar(max)  NULL,
    [CreatedDate] datetime  NULL,
    [UploadBy] int  NULL,
    [UploadUser] nvarchar(50)  NULL,
    [GroupIds] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [DirectoryId] int  NULL
);
GO

-- Creating table 'ResourceDirectories'
CREATE TABLE [dbo].[ResourceDirectories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DirectoryName] nvarchar(max)  NOT NULL,
    [ParentDirectoryId] int  NULL,
    [UserGroupId] int  NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'BlockInfoes'
ALTER TABLE [dbo].[BlockInfoes]
ADD CONSTRAINT [PK_BlockInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Menus'
ALTER TABLE [dbo].[Menus]
ADD CONSTRAINT [PK_Menus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [PK_Articles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticleCategories'
ALTER TABLE [dbo].[ArticleCategories]
ADD CONSTRAINT [PK_ArticleCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SinglePages'
ALTER TABLE [dbo].[SinglePages]
ADD CONSTRAINT [PK_SinglePages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Attachments'
ALTER TABLE [dbo].[Attachments]
ADD CONSTRAINT [PK_Attachments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserLoginHistories'
ALTER TABLE [dbo].[UserLoginHistories]
ADD CONSTRAINT [PK_UserLoginHistories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Links'
ALTER TABLE [dbo].[Links]
ADD CONSTRAINT [PK_Links]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRights'
ALTER TABLE [dbo].[UserRights]
ADD CONSTRAINT [PK_UserRights]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AppExceptions'
ALTER TABLE [dbo].[AppExceptions]
ADD CONSTRAINT [PK_AppExceptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticleAttributeDefinitions'
ALTER TABLE [dbo].[ArticleAttributeDefinitions]
ADD CONSTRAINT [PK_ArticleAttributeDefinitions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticleAttributeValues'
ALTER TABLE [dbo].[ArticleAttributeValues]
ADD CONSTRAINT [PK_ArticleAttributeValues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [PK_Resources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ResourceDirectories'
ALTER TABLE [dbo].[ResourceDirectories]
ADD CONSTRAINT [PK_ResourceDirectories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'UserLoginHistories'
ALTER TABLE [dbo].[UserLoginHistories]
ADD CONSTRAINT [FK_UserUserLoginHistory]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserLoginHistory'
CREATE INDEX [IX_FK_UserUserLoginHistory]
ON [dbo].[UserLoginHistories]
    ([UserId]);
GO

-- Creating foreign key on [CategoryId] in table 'Articles'
ALTER TABLE [dbo].[Articles]
ADD CONSTRAINT [FK_ArticleCategoryArticle]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[ArticleCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArticleCategoryArticle'
CREATE INDEX [IX_FK_ArticleCategoryArticle]
ON [dbo].[Articles]
    ([CategoryId]);
GO

-- Creating foreign key on [AttrId] in table 'ArticleAttributeValues'
ALTER TABLE [dbo].[ArticleAttributeValues]
ADD CONSTRAINT [FK_ArticleAttributeDefinitionArticleAttributeValue]
    FOREIGN KEY ([AttrId])
    REFERENCES [dbo].[ArticleAttributeDefinitions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArticleAttributeDefinitionArticleAttributeValue'
CREATE INDEX [IX_FK_ArticleAttributeDefinitionArticleAttributeValue]
ON [dbo].[ArticleAttributeValues]
    ([AttrId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------