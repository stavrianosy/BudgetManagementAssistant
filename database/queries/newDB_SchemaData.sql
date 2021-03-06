USE [BMA]
GO
/****** Object:  Table [dbo].[Budget]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Budget](
	[BudgetId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Amount] [float] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Comments] [nvarchar](max) NULL,
	[Repeat] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NOT NULL,
	[CreatedUser_UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Budget] PRIMARY KEY CLUSTERED 
(
	[BudgetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BudgetThreshold]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BudgetThreshold](
	[BudgetThresholdId] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [float] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.BudgetThreshold] PRIMARY KEY CLUSTERED 
(
	[BudgetThresholdId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FieldType]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldType](
	[FieldTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[DefaultValue] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.FieldType] PRIMARY KEY CLUSTERED 
(
	[FieldTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Notification]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Time] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RecurrenceRule]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecurrenceRule](
	[RecurrenceRuleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.RecurrenceRule] PRIMARY KEY CLUSTERED 
(
	[RecurrenceRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RecurrenceRulePart]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecurrenceRulePart](
	[RecurrenceRulePartId] [int] IDENTITY(1,1) NOT NULL,
	[RecurrenceRule_RecurrenceRuleId] [int] NULL,
 CONSTRAINT [PK_dbo.RecurrenceRulePart] PRIMARY KEY CLUSTERED 
(
	[RecurrenceRulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RecurrenceRuleRulePart]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecurrenceRuleRulePart](
	[RecurrenceRule_RecurrenceRuleId] [int] NOT NULL,
	[RulePart_RulePartId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.RecurrenceRuleRulePart] PRIMARY KEY CLUSTERED 
(
	[RecurrenceRule_RecurrenceRuleId] ASC,
	[RulePart_RulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RulePart]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RulePart](
	[RulePartId] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [nvarchar](max) NULL,
	[FieldType_FieldTypeId] [int] NULL,
 CONSTRAINT [PK_dbo.RulePart] PRIMARY KEY CLUSTERED 
(
	[RulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RulePartValue]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RulePartValue](
	[RulePartValueId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[RulePart_RulePartId] [int] NULL,
	[RecurrenceRulePart_RecurrenceRulePartId] [int] NULL,
 CONSTRAINT [PK_dbo.RulePartValue] PRIMARY KEY CLUSTERED 
(
	[RulePartValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Security]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Security](
	[SecurityId] [int] IDENTITY(1,1) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.Security] PRIMARY KEY CLUSTERED 
(
	[SecurityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [float] NOT NULL,
	[TipAmount] [float] NOT NULL,
	[NameOfPlace] [nvarchar](max) NULL,
	[Comments] [nvarchar](max) NULL,
	[TransactionDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Category_CategoryId] [int] NULL,
	[TransactionReasonType_TypeTransactionReasonId] [int] NULL,
	[TransactionType_TypeTransactionId] [int] NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransactionImage]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransactionImage](
	[TransactionImageId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Path] [nvarchar](max) NULL,
	[Image] [varbinary](max) NULL,
	[Thumbnail] [varbinary](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Transaction_TransactionId] [int] NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.TransactionImage] PRIMARY KEY CLUSTERED 
(
	[TransactionImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TypeFrequency]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeFrequency](
	[TypeFrequencyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Count] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.TypeFrequency] PRIMARY KEY CLUSTERED 
(
	[TypeFrequencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TypeInterval]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeInterval](
	[TypeIntervalId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Purpose] [nvarchar](max) NULL,
	[Amount] [float] NOT NULL,
	[Comments] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[TransactionType_TypeTransactionId] [int] NULL,
	[Category_CategoryId] [int] NULL,
	[TransactionReasonType_TypeTransactionReasonId] [int] NULL,
	[RecurrenceRuleValue_RecurrenceRulePartId] [int] NULL,
	[RecurrenceRangeRuleValue_RecurrenceRulePartId] [int] NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.TypeInterval] PRIMARY KEY CLUSTERED 
(
	[TypeIntervalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TypeIntervalConfiguration]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeIntervalConfiguration](
	[TypeIntervalConfigurationId] [int] IDENTITY(1,1) NOT NULL,
	[GeneratedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.TypeIntervalConfiguration] PRIMARY KEY CLUSTERED 
(
	[TypeIntervalConfigurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TypeSavingsDencity]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeSavingsDencity](
	[TypeSavingsDencityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.TypeSavingsDencity] PRIMARY KEY CLUSTERED 
(
	[TypeSavingsDencityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TypeTransaction]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeTransaction](
	[TypeTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[IsIncome] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.TypeTransaction] PRIMARY KEY CLUSTERED 
(
	[TypeTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TypeTransactionReason]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeTransactionReason](
	[TypeTransactionReasonId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NULL,
	[CreatedUser_UserId] [int] NULL,
 CONSTRAINT [PK_dbo.TypeTransactionReason] PRIMARY KEY CLUSTERED 
(
	[TypeTransactionReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TypeTransactionReasonCategory]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeTransactionReasonCategory](
	[TypeTransactionReason_TypeTransactionReasonId] [int] NOT NULL,
	[Category_CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TypeTransactionReasonCategory] PRIMARY KEY CLUSTERED 
(
	[TypeTransactionReason_TypeTransactionReasonId] ASC,
	[Category_CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 9/2/2013 10:14:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Birthdate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUser_UserId] [int] NOT NULL,
	[CreatedUser_UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Category] ON 

GO
INSERT [dbo].[Category] ([CategoryId], [Name], [FromDate], [ToDate], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (1, N'Other', CAST(0x00008EAC00000000 AS DateTime), CAST(0x00008EAC00000000 AS DateTime), CAST(0x0000A22D01690138 AS DateTime), 0, CAST(0x0000A22D01690138 AS DateTime), 2, 2)
GO
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[FieldType] ON 

GO
INSERT [dbo].[FieldType] ([FieldTypeId], [Name], [DefaultValue], [Type]) VALUES (1, N'label', N'', N'Label')
GO
INSERT [dbo].[FieldType] ([FieldTypeId], [Name], [DefaultValue], [Type]) VALUES (2, N'int', N'1', N'Int')
GO
INSERT [dbo].[FieldType] ([FieldTypeId], [Name], [DefaultValue], [Type]) VALUES (3, N'dayNumber', N'', N'DayNum')
GO
INSERT [dbo].[FieldType] ([FieldTypeId], [Name], [DefaultValue], [Type]) VALUES (4, N'date', N'20000101', N'DateInt')
GO
INSERT [dbo].[FieldType] ([FieldTypeId], [Name], [DefaultValue], [Type]) VALUES (5, N'truefalse', N'False', N'Bit')
GO
INSERT [dbo].[FieldType] ([FieldTypeId], [Name], [DefaultValue], [Type]) VALUES (6, N'text', N'', N'String')
GO
INSERT [dbo].[FieldType] ([FieldTypeId], [Name], [DefaultValue], [Type]) VALUES (7, N'position', N'1', N'Position')
GO
SET IDENTITY_INSERT [dbo].[FieldType] OFF
GO
SET IDENTITY_INSERT [dbo].[RecurrenceRule] ON 

GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (1, N'RuleRangeNoEndDate', CAST(0x0000A22D01690182 AS DateTime), 0, CAST(0x0000A22D01690182 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (2, N'RuleRangeTotalOcurrences', CAST(0x0000A22D01690184 AS DateTime), 0, CAST(0x0000A22D01690184 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (3, N'RuleRangeEndBy', CAST(0x0000A22D01690185 AS DateTime), 0, CAST(0x0000A22D01690185 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (4, N'RuleDailyEveryDays', CAST(0x0000A22D01690186 AS DateTime), 0, CAST(0x0000A22D01690186 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (5, N'RuleWeeklyEveryWeek', CAST(0x0000A22D01690187 AS DateTime), 0, CAST(0x0000A22D01690187 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (6, N'RuleMonthlyDayNum', CAST(0x0000A22D01690188 AS DateTime), 0, CAST(0x0000A22D01690188 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (7, N'RuleMonthlyPrecise', CAST(0x0000A22D01690188 AS DateTime), 0, CAST(0x0000A22D01690188 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (8, N'RuleYearlyOnMonth', CAST(0x0000A22D01690188 AS DateTime), 0, CAST(0x0000A22D01690188 AS DateTime), 2, 2)
GO
INSERT [dbo].[RecurrenceRule] ([RecurrenceRuleId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (9, N'RuleYearlyOnTheWeekDay', CAST(0x0000A22D0169018A AS DateTime), 0, CAST(0x0000A22D0169018A AS DateTime), 2, 2)
GO
SET IDENTITY_INSERT [dbo].[RecurrenceRule] OFF
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (1, 1)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (2, 1)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (3, 1)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (2, 3)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (3, 4)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (4, 5)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (4, 6)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (5, 7)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (5, 8)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (6, 9)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (6, 10)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (7, 11)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (7, 12)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (7, 13)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (8, 14)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (9, 14)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (8, 15)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (8, 16)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (9, 17)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (9, 18)
GO
INSERT [dbo].[RecurrenceRuleRulePart] ([RecurrenceRule_RecurrenceRuleId], [RulePart_RulePartId]) VALUES (9, 19)
GO
SET IDENTITY_INSERT [dbo].[RulePart] ON 

GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (1, N'RangeStartDate', 4)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (2, N'RangeNoEndDate', 1)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (3, N'RangeTotalOcurrences', 2)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (4, N'RangeEndBy', 4)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (5, N'DailyEveryDay', 2)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (6, N'DailyOnlyWeekdays', 5)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (7, N'WeeklyEveryWeek', 2)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (8, N'WeeklyDayName', 6)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (9, N'MonthlyDayNumber', 3)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (10, N'MonthlyEveryMonth', 2)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (11, N'MonthlyCountOfWeekDay', 7)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (12, N'MonthlyDayName', 3)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (13, N'MonthlyCountOfMonth', 2)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (14, N'YearlyEveryYear', 2)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (15, N'YearlyOnDayPos', 7)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (16, N'YearlyMonthName', 6)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (17, N'YearlyPositions', 7)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (18, N'YearlyDayName', 6)
GO
INSERT [dbo].[RulePart] ([RulePartId], [FieldName], [FieldType_FieldTypeId]) VALUES (19, N'YearlyMonthNameSec', 6)
GO
SET IDENTITY_INSERT [dbo].[RulePart] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeFrequency] ON 

GO
INSERT [dbo].[TypeFrequency] ([TypeFrequencyId], [Name], [Count], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (1, N'Hourly', 1, CAST(0x0000A22D01690149 AS DateTime), 0, CAST(0x0000A22D01690149 AS DateTime), 2, 2)
GO
INSERT [dbo].[TypeFrequency] ([TypeFrequencyId], [Name], [Count], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (2, N'Daily', 24, CAST(0x0000A22D0169014B AS DateTime), 0, CAST(0x0000A22D0169014B AS DateTime), 2, 2)
GO
INSERT [dbo].[TypeFrequency] ([TypeFrequencyId], [Name], [Count], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (3, N'Weekly', 168, CAST(0x0000A22D0169014C AS DateTime), 0, CAST(0x0000A22D0169014C AS DateTime), 2, 2)
GO
INSERT [dbo].[TypeFrequency] ([TypeFrequencyId], [Name], [Count], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (4, N'Monthly', 672, CAST(0x0000A22D0169014E AS DateTime), 0, CAST(0x0000A22D0169014E AS DateTime), 2, 2)
GO
INSERT [dbo].[TypeFrequency] ([TypeFrequencyId], [Name], [Count], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (5, N'Yearly', 8736, CAST(0x0000A22D0169014F AS DateTime), 0, CAST(0x0000A22D0169014F AS DateTime), 2, 2)
GO
SET IDENTITY_INSERT [dbo].[TypeFrequency] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeTransaction] ON 

GO
INSERT [dbo].[TypeTransaction] ([TypeTransactionId], [Name], [IsIncome], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (1, N'Income', 1, CAST(0x0000A22D01690143 AS DateTime), 0, CAST(0x0000A22D01690143 AS DateTime), 2, 2)
GO
INSERT [dbo].[TypeTransaction] ([TypeTransactionId], [Name], [IsIncome], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (2, N'Expense', 0, CAST(0x0000A22D01690147 AS DateTime), 0, CAST(0x0000A22D01690147 AS DateTime), 2, 2)
GO
SET IDENTITY_INSERT [dbo].[TypeTransaction] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeTransactionReason] ON 

GO
INSERT [dbo].[TypeTransactionReason] ([TypeTransactionReasonId], [Name], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (1, N'Other', CAST(0x0000A22D0169013A AS DateTime), 0, CAST(0x0000A22D0169013A AS DateTime), 2, 2)
GO
SET IDENTITY_INSERT [dbo].[TypeTransactionReason] OFF
GO
INSERT [dbo].[TypeTransactionReasonCategory] ([TypeTransactionReason_TypeTransactionReasonId], [Category_CategoryId]) VALUES (1, 1)
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserId], [UserName], [Password], [Email], [FirstName], [LastName], [Birthdate], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (1, N'admin', N'1234', N'admin@bma.com', N'AdminName', N'AdminSurname', CAST(0x0000A22D01690132 AS DateTime), CAST(0x0000A22D01690132 AS DateTime), 0, CAST(0x0000A22D01690132 AS DateTime), 1, 1)
GO
INSERT [dbo].[User] ([UserId], [UserName], [Password], [Email], [FirstName], [LastName], [Birthdate], [ModifiedDate], [IsDeleted], [CreatedDate], [ModifiedUser_UserId], [CreatedUser_UserId]) VALUES (2, N'System', N'system', N'system@system.com', N'SysName', N'SysSurname', CAST(0x0000A22D01690136 AS DateTime), CAST(0x0000A22D01690136 AS DateTime), 0, CAST(0x0000A22D01690136 AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[Budget]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[Budget]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[BudgetThreshold]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[BudgetThreshold]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[Category]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[Category]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[Notification]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[Notification]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[RecurrenceRule]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[RecurrenceRule]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecurrenceRule_RecurrenceRuleId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_RecurrenceRule_RecurrenceRuleId] ON [dbo].[RecurrenceRulePart]
(
	[RecurrenceRule_RecurrenceRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecurrenceRule_RecurrenceRuleId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_RecurrenceRule_RecurrenceRuleId] ON [dbo].[RecurrenceRuleRulePart]
(
	[RecurrenceRule_RecurrenceRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RulePart_RulePartId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_RulePart_RulePartId] ON [dbo].[RecurrenceRuleRulePart]
(
	[RulePart_RulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FieldType_FieldTypeId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_FieldType_FieldTypeId] ON [dbo].[RulePart]
(
	[FieldType_FieldTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecurrenceRulePart_RecurrenceRulePartId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_RecurrenceRulePart_RecurrenceRulePartId] ON [dbo].[RulePartValue]
(
	[RecurrenceRulePart_RecurrenceRulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RulePart_RulePartId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_RulePart_RulePartId] ON [dbo].[RulePartValue]
(
	[RulePart_RulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[Security]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[Security]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Category_CategoryId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_Category_CategoryId] ON [dbo].[Transaction]
(
	[Category_CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[Transaction]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[Transaction]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionReasonType_TypeTransactionReasonId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_TransactionReasonType_TypeTransactionReasonId] ON [dbo].[Transaction]
(
	[TransactionReasonType_TypeTransactionReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionType_TypeTransactionId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_TransactionType_TypeTransactionId] ON [dbo].[Transaction]
(
	[TransactionType_TypeTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[TransactionImage]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[TransactionImage]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_TransactionId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_TransactionId] ON [dbo].[TransactionImage]
(
	[Transaction_TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[TypeFrequency]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[TypeFrequency]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Category_CategoryId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_Category_CategoryId] ON [dbo].[TypeInterval]
(
	[Category_CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[TypeInterval]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[TypeInterval]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecurrenceRangeRuleValue_RecurrenceRulePartId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_RecurrenceRangeRuleValue_RecurrenceRulePartId] ON [dbo].[TypeInterval]
(
	[RecurrenceRangeRuleValue_RecurrenceRulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecurrenceRuleValue_RecurrenceRulePartId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_RecurrenceRuleValue_RecurrenceRulePartId] ON [dbo].[TypeInterval]
(
	[RecurrenceRuleValue_RecurrenceRulePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionReasonType_TypeTransactionReasonId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_TransactionReasonType_TypeTransactionReasonId] ON [dbo].[TypeInterval]
(
	[TransactionReasonType_TypeTransactionReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionType_TypeTransactionId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_TransactionType_TypeTransactionId] ON [dbo].[TypeInterval]
(
	[TransactionType_TypeTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[TypeIntervalConfiguration]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[TypeIntervalConfiguration]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[TypeSavingsDencity]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[TypeSavingsDencity]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[TypeTransaction]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[TypeTransaction]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[TypeTransactionReason]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[TypeTransactionReason]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Category_CategoryId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_Category_CategoryId] ON [dbo].[TypeTransactionReasonCategory]
(
	[Category_CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TypeTransactionReason_TypeTransactionReasonId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_TypeTransactionReason_TypeTransactionReasonId] ON [dbo].[TypeTransactionReasonCategory]
(
	[TypeTransactionReason_TypeTransactionReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreatedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedUser_UserId] ON [dbo].[User]
(
	[CreatedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ModifiedUser_UserId]    Script Date: 9/2/2013 10:14:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedUser_UserId] ON [dbo].[User]
(
	[ModifiedUser_UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Budget]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Budget_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Budget] CHECK CONSTRAINT [FK_dbo.Budget_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[Budget]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Budget_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Budget] CHECK CONSTRAINT [FK_dbo.Budget_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[BudgetThreshold]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BudgetThreshold_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[BudgetThreshold] CHECK CONSTRAINT [FK_dbo.BudgetThreshold_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[BudgetThreshold]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BudgetThreshold_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[BudgetThreshold] CHECK CONSTRAINT [FK_dbo.BudgetThreshold_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Category_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_dbo.Category_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Category_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_dbo.Category_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Notification_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_dbo.Notification_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Notification_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_dbo.Notification_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[RecurrenceRule]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RecurrenceRule_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[RecurrenceRule] CHECK CONSTRAINT [FK_dbo.RecurrenceRule_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[RecurrenceRule]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RecurrenceRule_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[RecurrenceRule] CHECK CONSTRAINT [FK_dbo.RecurrenceRule_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[RecurrenceRulePart]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RecurrenceRulePart_dbo.RecurrenceRule_RecurrenceRule_RecurrenceRuleId] FOREIGN KEY([RecurrenceRule_RecurrenceRuleId])
REFERENCES [dbo].[RecurrenceRule] ([RecurrenceRuleId])
GO
ALTER TABLE [dbo].[RecurrenceRulePart] CHECK CONSTRAINT [FK_dbo.RecurrenceRulePart_dbo.RecurrenceRule_RecurrenceRule_RecurrenceRuleId]
GO
ALTER TABLE [dbo].[RecurrenceRuleRulePart]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RecurrenceRuleRulePart_dbo.RecurrenceRule_RecurrenceRule_RecurrenceRuleId] FOREIGN KEY([RecurrenceRule_RecurrenceRuleId])
REFERENCES [dbo].[RecurrenceRule] ([RecurrenceRuleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecurrenceRuleRulePart] CHECK CONSTRAINT [FK_dbo.RecurrenceRuleRulePart_dbo.RecurrenceRule_RecurrenceRule_RecurrenceRuleId]
GO
ALTER TABLE [dbo].[RecurrenceRuleRulePart]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RecurrenceRuleRulePart_dbo.RulePart_RulePart_RulePartId] FOREIGN KEY([RulePart_RulePartId])
REFERENCES [dbo].[RulePart] ([RulePartId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecurrenceRuleRulePart] CHECK CONSTRAINT [FK_dbo.RecurrenceRuleRulePart_dbo.RulePart_RulePart_RulePartId]
GO
ALTER TABLE [dbo].[RulePart]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RulePart_dbo.FieldType_FieldType_FieldTypeId] FOREIGN KEY([FieldType_FieldTypeId])
REFERENCES [dbo].[FieldType] ([FieldTypeId])
GO
ALTER TABLE [dbo].[RulePart] CHECK CONSTRAINT [FK_dbo.RulePart_dbo.FieldType_FieldType_FieldTypeId]
GO
ALTER TABLE [dbo].[RulePartValue]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RulePartValue_dbo.RecurrenceRulePart_RecurrenceRulePart_RecurrenceRulePartId] FOREIGN KEY([RecurrenceRulePart_RecurrenceRulePartId])
REFERENCES [dbo].[RecurrenceRulePart] ([RecurrenceRulePartId])
GO
ALTER TABLE [dbo].[RulePartValue] CHECK CONSTRAINT [FK_dbo.RulePartValue_dbo.RecurrenceRulePart_RecurrenceRulePart_RecurrenceRulePartId]
GO
ALTER TABLE [dbo].[RulePartValue]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RulePartValue_dbo.RulePart_RulePart_RulePartId] FOREIGN KEY([RulePart_RulePartId])
REFERENCES [dbo].[RulePart] ([RulePartId])
GO
ALTER TABLE [dbo].[RulePartValue] CHECK CONSTRAINT [FK_dbo.RulePartValue_dbo.RulePart_RulePart_RulePartId]
GO
ALTER TABLE [dbo].[Security]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Security_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Security] CHECK CONSTRAINT [FK_dbo.Security_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[Security]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Security_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Security] CHECK CONSTRAINT [FK_dbo.Security_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transaction_dbo.Category_Category_CategoryId] FOREIGN KEY([Category_CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_dbo.Transaction_dbo.Category_Category_CategoryId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transaction_dbo.TypeTransaction_TransactionType_TypeTransactionId] FOREIGN KEY([TransactionType_TypeTransactionId])
REFERENCES [dbo].[TypeTransaction] ([TypeTransactionId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_dbo.Transaction_dbo.TypeTransaction_TransactionType_TypeTransactionId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transaction_dbo.TypeTransactionReason_TransactionReasonType_TypeTransactionReasonId] FOREIGN KEY([TransactionReasonType_TypeTransactionReasonId])
REFERENCES [dbo].[TypeTransactionReason] ([TypeTransactionReasonId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_dbo.Transaction_dbo.TypeTransactionReason_TransactionReasonType_TypeTransactionReasonId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transaction_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_dbo.Transaction_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Transaction_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_dbo.Transaction_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TransactionImage]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TransactionImage_dbo.Transaction_Transaction_TransactionId] FOREIGN KEY([Transaction_TransactionId])
REFERENCES [dbo].[Transaction] ([TransactionId])
GO
ALTER TABLE [dbo].[TransactionImage] CHECK CONSTRAINT [FK_dbo.TransactionImage_dbo.Transaction_Transaction_TransactionId]
GO
ALTER TABLE [dbo].[TransactionImage]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TransactionImage_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TransactionImage] CHECK CONSTRAINT [FK_dbo.TransactionImage_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[TransactionImage]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TransactionImage_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TransactionImage] CHECK CONSTRAINT [FK_dbo.TransactionImage_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TypeFrequency]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeFrequency_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeFrequency] CHECK CONSTRAINT [FK_dbo.TypeFrequency_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[TypeFrequency]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeFrequency_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeFrequency] CHECK CONSTRAINT [FK_dbo.TypeFrequency_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TypeInterval]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeInterval_dbo.Category_Category_CategoryId] FOREIGN KEY([Category_CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[TypeInterval] CHECK CONSTRAINT [FK_dbo.TypeInterval_dbo.Category_Category_CategoryId]
GO
ALTER TABLE [dbo].[TypeInterval]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeInterval_dbo.RecurrenceRulePart_RecurrenceRangeRuleValue_RecurrenceRulePartId] FOREIGN KEY([RecurrenceRangeRuleValue_RecurrenceRulePartId])
REFERENCES [dbo].[RecurrenceRulePart] ([RecurrenceRulePartId])
GO
ALTER TABLE [dbo].[TypeInterval] CHECK CONSTRAINT [FK_dbo.TypeInterval_dbo.RecurrenceRulePart_RecurrenceRangeRuleValue_RecurrenceRulePartId]
GO
ALTER TABLE [dbo].[TypeInterval]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeInterval_dbo.RecurrenceRulePart_RecurrenceRuleValue_RecurrenceRulePartId] FOREIGN KEY([RecurrenceRuleValue_RecurrenceRulePartId])
REFERENCES [dbo].[RecurrenceRulePart] ([RecurrenceRulePartId])
GO
ALTER TABLE [dbo].[TypeInterval] CHECK CONSTRAINT [FK_dbo.TypeInterval_dbo.RecurrenceRulePart_RecurrenceRuleValue_RecurrenceRulePartId]
GO
ALTER TABLE [dbo].[TypeInterval]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeInterval_dbo.TypeTransaction_TransactionType_TypeTransactionId] FOREIGN KEY([TransactionType_TypeTransactionId])
REFERENCES [dbo].[TypeTransaction] ([TypeTransactionId])
GO
ALTER TABLE [dbo].[TypeInterval] CHECK CONSTRAINT [FK_dbo.TypeInterval_dbo.TypeTransaction_TransactionType_TypeTransactionId]
GO
ALTER TABLE [dbo].[TypeInterval]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeInterval_dbo.TypeTransactionReason_TransactionReasonType_TypeTransactionReasonId] FOREIGN KEY([TransactionReasonType_TypeTransactionReasonId])
REFERENCES [dbo].[TypeTransactionReason] ([TypeTransactionReasonId])
GO
ALTER TABLE [dbo].[TypeInterval] CHECK CONSTRAINT [FK_dbo.TypeInterval_dbo.TypeTransactionReason_TransactionReasonType_TypeTransactionReasonId]
GO
ALTER TABLE [dbo].[TypeInterval]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeInterval_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeInterval] CHECK CONSTRAINT [FK_dbo.TypeInterval_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[TypeInterval]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeInterval_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeInterval] CHECK CONSTRAINT [FK_dbo.TypeInterval_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TypeIntervalConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeIntervalConfiguration_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeIntervalConfiguration] CHECK CONSTRAINT [FK_dbo.TypeIntervalConfiguration_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[TypeIntervalConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeIntervalConfiguration_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeIntervalConfiguration] CHECK CONSTRAINT [FK_dbo.TypeIntervalConfiguration_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TypeSavingsDencity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeSavingsDencity_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeSavingsDencity] CHECK CONSTRAINT [FK_dbo.TypeSavingsDencity_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[TypeSavingsDencity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeSavingsDencity_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeSavingsDencity] CHECK CONSTRAINT [FK_dbo.TypeSavingsDencity_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TypeTransaction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeTransaction_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeTransaction] CHECK CONSTRAINT [FK_dbo.TypeTransaction_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[TypeTransaction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeTransaction_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeTransaction] CHECK CONSTRAINT [FK_dbo.TypeTransaction_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TypeTransactionReason]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeTransactionReason_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeTransactionReason] CHECK CONSTRAINT [FK_dbo.TypeTransactionReason_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[TypeTransactionReason]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeTransactionReason_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TypeTransactionReason] CHECK CONSTRAINT [FK_dbo.TypeTransactionReason_dbo.User_ModifiedUser_UserId]
GO
ALTER TABLE [dbo].[TypeTransactionReasonCategory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeTransactionReasonCategory_dbo.Category_Category_CategoryId] FOREIGN KEY([Category_CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TypeTransactionReasonCategory] CHECK CONSTRAINT [FK_dbo.TypeTransactionReasonCategory_dbo.Category_Category_CategoryId]
GO
ALTER TABLE [dbo].[TypeTransactionReasonCategory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TypeTransactionReasonCategory_dbo.TypeTransactionReason_TypeTransactionReason_TypeTransactionReasonId] FOREIGN KEY([TypeTransactionReason_TypeTransactionReasonId])
REFERENCES [dbo].[TypeTransactionReason] ([TypeTransactionReasonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TypeTransactionReasonCategory] CHECK CONSTRAINT [FK_dbo.TypeTransactionReasonCategory_dbo.TypeTransactionReason_TypeTransactionReason_TypeTransactionReasonId]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_dbo.User_CreatedUser_UserId] FOREIGN KEY([CreatedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_dbo.User_dbo.User_CreatedUser_UserId]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_dbo.User_ModifiedUser_UserId] FOREIGN KEY([ModifiedUser_UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_dbo.User_dbo.User_ModifiedUser_UserId]
GO
USE [master]
GO
ALTER DATABASE [BMA] SET  READ_WRITE 
GO
