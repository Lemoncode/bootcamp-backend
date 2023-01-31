SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Info] [nvarchar](max) NOT NULL,
	[Year] [int] NOT NULL,
	[PosterUrl] [nvarchar](max) NOT NULL,
	[Genre] [int] NOT NULL,
	[DownloadUrl] [nvarchar](max) NOT NULL,
	[AgeGroup] [nvarchar](max) NOT NULL,
	[Playability] [int] NOT NULL,
	[Rating] [int] NOT NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
