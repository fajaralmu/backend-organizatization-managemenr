
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/08/2020 05:38:55
-- Generated from EDMX file: D:\Development\Visual Studio 2015\Projects\WEB-APP\BackendOrganizationManagement\BackendOrganizationManagement\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [mpi_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_division_institution_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[division] DROP CONSTRAINT [FK_division_institution_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_event_program_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[event] DROP CONSTRAINT [FK_event_program_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_member_position_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[member] DROP CONSTRAINT [FK_member_position_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_member_section_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[member] DROP CONSTRAINT [FK_member_section_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_post_user_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[post] DROP CONSTRAINT [FK_post_user_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_program_section_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[program] DROP CONSTRAINT [FK_program_section_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_section_division_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[section] DROP CONSTRAINT [FK_section_division_FK];
GO
IF OBJECT_ID(N'[dbo].[FK_user_institution_FK]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[user] DROP CONSTRAINT [FK_user_institution_FK];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[division]', 'U') IS NOT NULL
    DROP TABLE [dbo].[division];
GO
IF OBJECT_ID(N'[dbo].[event]', 'U') IS NOT NULL
    DROP TABLE [dbo].[event];
GO
IF OBJECT_ID(N'[dbo].[institution]', 'U') IS NOT NULL
    DROP TABLE [dbo].[institution];
GO
IF OBJECT_ID(N'[dbo].[member]', 'U') IS NOT NULL
    DROP TABLE [dbo].[member];
GO
IF OBJECT_ID(N'[dbo].[position]', 'U') IS NOT NULL
    DROP TABLE [dbo].[position];
GO
IF OBJECT_ID(N'[dbo].[post]', 'U') IS NOT NULL
    DROP TABLE [dbo].[post];
GO
IF OBJECT_ID(N'[dbo].[program]', 'U') IS NOT NULL
    DROP TABLE [dbo].[program];
GO
IF OBJECT_ID(N'[dbo].[section]', 'U') IS NOT NULL
    DROP TABLE [dbo].[section];
GO
IF OBJECT_ID(N'[dbo].[user]', 'U') IS NOT NULL
    DROP TABLE [dbo].[user];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'divisions'
CREATE TABLE [dbo].[divisions] (
    [name] varchar(100)  NOT NULL,
    [description] varchar(1000)  NULL,
    [institution_id] int  NOT NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'events'
CREATE TABLE [dbo].[events] (
    [name] varchar(100)  NOT NULL,
    [location] varchar(100)  NOT NULL,
    [info] varchar(100)  NOT NULL,
    [done] int  NOT NULL,
    [participant] int  NOT NULL,
    [program_id] int  NOT NULL,
    [user_id] int  NOT NULL,
    [created_date] datetime  NULL,
    [date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'institutions'
CREATE TABLE [dbo].[institutions] (
    [name] varchar(100)  NOT NULL,
    [description] varchar(1000)  NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'members'
CREATE TABLE [dbo].[members] (
    [name] varchar(100)  NULL,
    [description] varchar(1000)  NULL,
    [position_id] int  NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [section_id] int  NULL
);
GO

-- Creating table 'positions'
CREATE TABLE [dbo].[positions] (
    [name] varchar(100)  NOT NULL,
    [parent_position_id] int  NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [description] varchar(500)  NULL
);
GO

-- Creating table 'posts'
CREATE TABLE [dbo].[posts] (
    [name] varchar(100)  NULL,
    [body] varchar(1000)  NULL,
    [post_id] int  NULL,
    [user_id] int  NOT NULL,
    [type] int  NULL,
    [date] datetime  NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [title] varchar(100)  NULL
);
GO

-- Creating table 'programs'
CREATE TABLE [dbo].[programs] (
    [name] varchar(100)  NOT NULL,
    [description] varchar(1000)  NULL,
    [sect_id] int  NOT NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'sections'
CREATE TABLE [dbo].[sections] (
    [name] varchar(100)  NOT NULL,
    [division_id] int  NOT NULL,
    [parent_section_id] int  NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'users'
CREATE TABLE [dbo].[users] (
    [username] varchar(100)  NULL,
    [name] varchar(100)  NULL,
    [password] varchar(100)  NULL,
    [email] varchar(100)  NULL,
    [admin] int  NULL,
    [institution_id] int  NULL,
    [created_date] datetime  NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'divisions'
ALTER TABLE [dbo].[divisions]
ADD CONSTRAINT [PK_divisions]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'events'
ALTER TABLE [dbo].[events]
ADD CONSTRAINT [PK_events]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'institutions'
ALTER TABLE [dbo].[institutions]
ADD CONSTRAINT [PK_institutions]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'members'
ALTER TABLE [dbo].[members]
ADD CONSTRAINT [PK_members]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'positions'
ALTER TABLE [dbo].[positions]
ADD CONSTRAINT [PK_positions]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'posts'
ALTER TABLE [dbo].[posts]
ADD CONSTRAINT [PK_posts]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'programs'
ALTER TABLE [dbo].[programs]
ADD CONSTRAINT [PK_programs]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'sections'
ALTER TABLE [dbo].[sections]
ADD CONSTRAINT [PK_sections]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'users'
ALTER TABLE [dbo].[users]
ADD CONSTRAINT [PK_users]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [institution_id] in table 'divisions'
ALTER TABLE [dbo].[divisions]
ADD CONSTRAINT [FK_division_institution_FK]
    FOREIGN KEY ([institution_id])
    REFERENCES [dbo].[institutions]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_division_institution_FK'
CREATE INDEX [IX_FK_division_institution_FK]
ON [dbo].[divisions]
    ([institution_id]);
GO

-- Creating foreign key on [division_id] in table 'sections'
ALTER TABLE [dbo].[sections]
ADD CONSTRAINT [FK_section_division_FK]
    FOREIGN KEY ([division_id])
    REFERENCES [dbo].[divisions]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_section_division_FK'
CREATE INDEX [IX_FK_section_division_FK]
ON [dbo].[sections]
    ([division_id]);
GO

-- Creating foreign key on [program_id] in table 'events'
ALTER TABLE [dbo].[events]
ADD CONSTRAINT [FK_event_program_FK]
    FOREIGN KEY ([program_id])
    REFERENCES [dbo].[programs]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_event_program_FK'
CREATE INDEX [IX_FK_event_program_FK]
ON [dbo].[events]
    ([program_id]);
GO

-- Creating foreign key on [institution_id] in table 'users'
ALTER TABLE [dbo].[users]
ADD CONSTRAINT [FK_user_institution_FK]
    FOREIGN KEY ([institution_id])
    REFERENCES [dbo].[institutions]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_user_institution_FK'
CREATE INDEX [IX_FK_user_institution_FK]
ON [dbo].[users]
    ([institution_id]);
GO

-- Creating foreign key on [position_id] in table 'members'
ALTER TABLE [dbo].[members]
ADD CONSTRAINT [FK_member_position_FK]
    FOREIGN KEY ([position_id])
    REFERENCES [dbo].[positions]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_member_position_FK'
CREATE INDEX [IX_FK_member_position_FK]
ON [dbo].[members]
    ([position_id]);
GO

-- Creating foreign key on [user_id] in table 'posts'
ALTER TABLE [dbo].[posts]
ADD CONSTRAINT [FK_post_user_FK]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[users]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_post_user_FK'
CREATE INDEX [IX_FK_post_user_FK]
ON [dbo].[posts]
    ([user_id]);
GO

-- Creating foreign key on [sect_id] in table 'programs'
ALTER TABLE [dbo].[programs]
ADD CONSTRAINT [FK_program_section_FK]
    FOREIGN KEY ([sect_id])
    REFERENCES [dbo].[sections]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_program_section_FK'
CREATE INDEX [IX_FK_program_section_FK]
ON [dbo].[programs]
    ([sect_id]);
GO

-- Creating foreign key on [section_id] in table 'members'
ALTER TABLE [dbo].[members]
ADD CONSTRAINT [FK_member_section_FK]
    FOREIGN KEY ([section_id])
    REFERENCES [dbo].[sections]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_member_section_FK'
CREATE INDEX [IX_FK_member_section_FK]
ON [dbo].[members]
    ([section_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------