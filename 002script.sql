USE [gymDatabase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Admin](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[authLevel][int] NOT NULL,
 CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Member](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[Address] [varchar](50) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[CardId] [bigint] NOT NULL,
	[TypeId] [char](10) NOT NULL,
	[NumOfEntrances] [int] NOT NULL,
	[Gender] [char](10) NULL,
	[LastEntrance] [datetime] NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Measurement](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [bigint] NOT NULL,
	[Timestamp][datetime] NULL,
	[Height][int] NULL,
	[Weight][int] NULL,
	[BodyFat][numeric](4, 2) NULL,
	[Neck][numeric](5, 2) NULL,
	[Shoulders][numeric](5, 2) NULL,
	[LeftBicep][numeric](5, 2) NULL,
	[LeftForearm][numeric](5, 2) NULL,
	[RightBicep][numeric](5, 2) NULL,
	[RightForearm][numeric](5, 2) NULL,
	[Chest][numeric](5, 2) NULL,
	[Waist][numeric](5, 2) NULL,
	[Hips][numeric](5, 2) NULL,
	[LeftThighs][numeric](5, 2) NULL,
	[RightThighs][numeric](5, 2) NULL,
	[LeftCalves][numeric](5, 2) NULL,
	[RightCalves][numeric](5, 2) NULL,
 CONSTRAINT [PK_Measurement] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [varchar](max) NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[ItemPrice] [numeric](5, 2) NOT NULL,
	[ItemCount] [int] NOT NULL,
	[ItemDescription] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Exercise](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description][varchar](max) NOT NULL,
	[Link][varchar](max) NOT NULL,
	[Day] [varchar](50) NOT NULL,
	[Time] [varchar](50) NOT NULL,
	[Enrolled] [int] NOT NULL,
	[Price] [int] NOT NULL,

 CONSTRAINT [PK_Exercise] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Report](
	[MemberId] [bigint] NOT NULL,
	[EntranceDate] [datetime2](7) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ShopPayment](
	[MemberId] [bigint] NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemName] [varchar](50) NOT NULL,
	[ItemPrice][numeric](5, 2) NOT NULL,
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ShopPayment](
	[MemberId] [bigint] NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[ItemId] [int] NOT NULL,
	[ItemName] [varchar](50) NOT NULL,
	[ItemPrice][numeric](5, 2) NOT NULL,
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TrainingEnrollment](
	[MemberId] [bigint] NOT NULL,
	[EnrollmentDate] [datetime2](7) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[ExerciseId] [int] NOT NULL,
	[ExerciseName] [varchar](50) NOT NULL,
	[ExercisePrice][numeric](5, 2) NOT NULL,
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentDate] [date] NOT NULL,
	[ExpirationDate] [date] NOT NULL,
	[Price] [float] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Surname] [varchar](50) NOT NULL,
	[MemberId] [bigint] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] NOT NULL,
	[COMM] [int] NOT NULL,
	[PATH] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Report]  WITH NOCHECK ADD  CONSTRAINT [FK_Report_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([id])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_Member]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Member]
GO
ALTER TABLE [dbo].[Measurement]  WITH CHECK ADD  CONSTRAINT [FK_Measurement_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([id])
GO
ALTER TABLE [dbo].[Measurement] CHECK CONSTRAINT [FK_Measurement_Member]
GO
ALTER TABLE [dbo].[ShopPayment]  WITH NOCHECK ADD  CONSTRAINT [FK_ShopPayment_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([id])
GO
ALTER TABLE [dbo].[ShopPayment] CHECK CONSTRAINT [FK_ShopPayment_Member]
GO
ALTER TABLE [dbo].[ShopPayment]  WITH NOCHECK ADD  CONSTRAINT [FK_ShopPayment_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Items] ([id])
GO
ALTER TABLE [dbo].[ShopPayment] CHECK CONSTRAINT [FK_ShopPayment_Items]
GO

ALTER TABLE [dbo].[TrainingEnrollment]  WITH NOCHECK ADD  CONSTRAINT [FK_TrainingEnrollment_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([id])
GO
ALTER TABLE [dbo].[TrainingEnrollment] CHECK CONSTRAINT [FK_TrainingEnrollment_Member]
GO
ALTER TABLE [dbo].[TrainingEnrollment]  WITH NOCHECK ADD  CONSTRAINT [FK_TrainingEnrollment_Exercise] FOREIGN KEY([ExerciseId])
REFERENCES [dbo].[Exercise] ([id])
GO
ALTER TABLE [dbo].[TrainingEnrollment] CHECK CONSTRAINT [FK_TrainingEnrollment_Exercise]
GO