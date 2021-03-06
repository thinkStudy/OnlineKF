USE [OnlineKF]
GO
/****** Object:  Table [dbo].[WordsMessage]    Script Date: 05/19/2016 15:19:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WordsMessage](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[messagecontent] [nvarchar](4000) NOT NULL,
	[email] [varchar](50) NULL,
	[name] [nvarchar](50) NOT NULL,
	[phone] [varchar](50) NOT NULL,
	[sex] [int] NULL,
	[serviceid] [int] NULL,
	[createData] [datetime] NULL,
 CONSTRAINT [PK_WordsMessage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'留言内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage', @level2type=N'COLUMN',@level2name=N'messagecontent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage', @level2type=N'COLUMN',@level2name=N'email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage', @level2type=N'COLUMN',@level2name=N'phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别 0：男 1：女' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage', @level2type=N'COLUMN',@level2name=N'sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分配的客服id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage', @level2type=N'COLUMN',@level2name=N'serviceid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage', @level2type=N'COLUMN',@level2name=N'createData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'l留言信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WordsMessage'
GO
/****** Object:  Table [dbo].[ServicePerson]    Script Date: 05/19/2016 15:19:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ServicePerson](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[loginname] [nvarchar](50) NOT NULL,
	[loginpwd] [varchar](100) NOT NULL,
	[age] [int] NULL,
	[compayid] [int] NOT NULL,
	[personlevel] [int] NULL,
	[serviceNumber] [int] NULL,
	[maxcount] [int] NULL,
	[remark] [nvarchar](1000) NULL,
	[createTime] [datetime] NULL,
 CONSTRAINT [PK_ServicePerson] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_ServicePerson] UNIQUE NONCLUSTERED 
(
	[loginname] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服人员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'loginname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'loginpwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年龄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'age'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属公司 对应公司ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'compayid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务人员等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'personlevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前服务的人数，优先分配人数少的客服' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'serviceNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大服务人数 0 无限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'maxcount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备用字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服人员信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServicePerson'
GO
/****** Object:  Table [dbo].[QuestionPerson]    Script Date: 05/19/2016 15:19:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QuestionPerson](
	[id] [varchar](500) NOT NULL,
	[serviceid] [int] NOT NULL,
	[servicelevel] [int] NULL,
	[createTime] [datetime] NULL,
 CONSTRAINT [PK_QuestionPerson] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询人员信息 自动创建GUID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuestionPerson', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分配的客服id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuestionPerson', @level2type=N'COLUMN',@level2name=N'serviceid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'给客服人员评分 （1-5分）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuestionPerson', @level2type=N'COLUMN',@level2name=N'servicelevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuestionPerson', @level2type=N'COLUMN',@level2name=N'createTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询人员信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QuestionPerson'
GO
/****** Object:  Table [dbo].[MessageData]    Script Date: 05/19/2016 15:19:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MessageData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[questionid] [varchar](500) NOT NULL,
	[message] [text] NULL,
	[backmessage] [text] NULL,
	[onlycount] int NOT NULL,
	[createtime] [datetime] NULL,
 CONSTRAINT [PK_MessageData] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应咨询用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageData', @level2type=N'COLUMN',@level2name=N'questionid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询聊天信息,超过8万字自动备份到backMessage，以json格式存储{sm:{m:"客服发言",t:"[0 文字 1图片 2文件]"},qm:{m:"咨询人员发言",t:"[0 文字 1图片 2文件]"}} ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageData', @level2type=N'COLUMN',@level2name=N'message'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聊天信息备份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageData', @level2type=N'COLUMN',@level2name=N'backmessage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未读信息数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageData', @level2type=N'COLUMN',@level2name=N'onlycount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageData', @level2type=N'COLUMN',@level2name=N'createtime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'咨询信息存储' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MessageData'
GO
/****** Object:  Table [dbo].[CompayInfo]    Script Date: 05/19/2016 15:19:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompayInfo](
	[compayid] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[compayinfo] [nvarchar](4000) NULL,
	[type] [int] NULL,
	[remark] [nvarchar](1000) NULL,
 CONSTRAINT [PK_CompayInfo] PRIMARY KEY CLUSTERED 
(
	[compayid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompayInfo', @level2type=N'COLUMN',@level2name=N'compayid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompayInfo', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompayInfo', @level2type=N'COLUMN',@level2name=N'compayinfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业类型 0游戏 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompayInfo', @level2type=N'COLUMN',@level2name=N'type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备用字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompayInfo', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'企业信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompayInfo'
GO
/****** Object:  Default [DF_MessageData_isonly]    Script Date: 05/19/2016 15:19:45 ******/
ALTER TABLE [dbo].[MessageData] ADD  CONSTRAINT [DF_MessageData_isonly]  DEFAULT ((0)) FOR [isonly]
GO
/****** Object:  Default [DF_MessageData_createtime]    Script Date: 05/19/2016 15:19:45 ******/
ALTER TABLE [dbo].[MessageData] ADD  CONSTRAINT [DF_MessageData_createtime]  DEFAULT (getdate()) FOR [createtime]
GO
/****** Object:  Default [DF_WordsMessage_createData]    Script Date: 05/19/2016 15:19:45 ******/
ALTER TABLE [dbo].[WordsMessage] ADD  CONSTRAINT [DF_WordsMessage_createData]  DEFAULT (getdate()) FOR [createData]
GO
