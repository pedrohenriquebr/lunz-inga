USE [LuzIngaDb];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contact]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Contact](
        [ContactId] [int] IDENTITY(1,1) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
    (
        [ContactId] ASC
    )
    )
END
