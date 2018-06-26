using Microsoft.EntityFrameworkCore.Migrations;

namespace BookFast.Facility.Data.Migrations
{
    public partial class FacilityService_004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "eventseq",
                schema: "fm");

            migrationBuilder.Sql(switchOnIdentity);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "eventseq",
                schema: "fm",
                incrementBy: 10);

            migrationBuilder.Sql(switchOffIdentity);
        }

        private const string switchOnIdentity = @"
CREATE TABLE [fm].[Events_01](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [EventName] [nvarchar](100) NOT NULL,
    [OccurredAt] [datetimeoffset](7) NOT NULL,
    [Payload] [nvarchar](max) NOT NULL,
    [Tenant] [nvarchar](50) NOT NULL,
    [User] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Events_01] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [fm].[Events_01] ADD  DEFAULT (N'') FOR [Tenant]
GO

ALTER TABLE [fm].[Events_01] ADD  DEFAULT (N'') FOR [User]
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;
  ALTER TABLE [fm].[Events] SWITCH TO [fm].[Events_01];
  DROP TABLE [fm].[Events];
  EXEC sys.sp_rename N'fm.Events_01', N'Events', 'OBJECT';
COMMIT TRANSACTION;
GO
";
        private const string switchOffIdentity = @"
CREATE TABLE [fm].[Events_01](
    [Id] [int] NOT NULL,
    [EventName] [nvarchar](100) NOT NULL,
    [OccurredAt] [datetimeoffset](7) NOT NULL,
    [Payload] [nvarchar](max) NOT NULL,
    [Tenant] [nvarchar](50) NOT NULL,
    [User] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Events_02] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [fm].[Events_01] ADD  DEFAULT (N'') FOR [Tenant]
GO

ALTER TABLE [fm].[Events_01] ADD  DEFAULT (N'') FOR [User]
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;
  ALTER TABLE [fm].[Events] SWITCH TO [fm].[Events_01];
  DROP TABLE [fm].[Events];
  EXEC sys.sp_rename N'fm.Events_01', N'Events', 'OBJECT';
COMMIT TRANSACTION;
GO
";
    }
}
