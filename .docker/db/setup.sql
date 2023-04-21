USE [LuzIngaDb];
GO

IF NOT EXISTS (SELECT *
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo'
    AND TABLE_NAME = 'NewsLetterSubscription')
BEGIN
    CREATE TABLE [dbo].[NewsLetterSubscription](
        [NewsLetterSubscriptionId] [uniqueidentifier] NOT NULL DEFAULT newid(),
        [Email] [nvarchar](100) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        CONSTRAINT [PK_NewsLetterSubscription] PRIMARY KEY CLUSTERED 
    (
        [NewsLetterSubscriptionId] ASC
    )
    )
END
