using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAdvice.Repository.Migrations
{
    public partial class initialdatas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
if not exists(select * from MailTemplates where [Name]='MovieAdvice')
begin
	INSERT [dbo].[MailTemplates] ([TemplateType], [Name], [Title], [Body]) VALUES (1, 'MovieAdvice', 'Film Tavsiyesi', '<div style=""font-family: Inter, -apple-system, BlinkMacSystemFont, ''Segoe UI'', Roboto, ''Helvetica Neue'', Arial, sans-serif, ''Apple Color Emoji'', ''Segoe UI Emoji'', ''Segoe UI Symbol;''"">
		<div style=""width: 100%; display: flex; padding: 10px 0;"">
			Aşağıda bilgileri yer alan filmi {{FullName}} kullanıcısı size tavsiye etti. 
		</div>
		<div style=""width: 100%; display: flex; padding: 10px 0;"">
			<div style=""width: 150px; font-weight: bold; font-weight: bold;"">Başlığı</div>
			<div style=""width: 500px;"">{{Title}}</div>
		</div>
		<div style=""width: 100%; display: flex; padding: 10px 0;"">
			<div style=""width: 150px; font-weight: bold;"">Özet</div>
			<div style=""width: 500px;"">{{Overview}}</div>
		</div>
		<div style=""width: 100%; display: flex; padding: 10px 0;"">
			<div style=""width: 150px; font-weight: bold;"">Ortalama Puanı</div>
			<div style=""width: 500px;"">{{AverageRate}}</div>
		</div>
	</div>	')
end
GO
if not exists(select * from WebsiteParameters where Code='EmailSettings' and ParentId is null)
begin
	INSERT [dbo].[WebsiteParameters] ([ParentId], [Code], [Value], [Required], [Visible]) VALUES (NULL, 'EmailSettings', NULL, 0, 1)
end
GO
if not exists(select * from WebsiteParameters where Code='Password' and ParentId=(select Id from WebsiteParameters where Code='EmailSettings' and ParentId is null))
begin
	INSERT [dbo].[WebsiteParameters] ([ParentId], [Code], [Value], [Required], [Visible]) VALUES (1, 'Password', 'pnzchogsmqhldedr', 1, 1)
end
GO
if not exists(select * from WebsiteParameters where Code='EmailAddress' and ParentId=(select Id from WebsiteParameters where Code='EmailSettings' and ParentId is null))
begin	
	INSERT [dbo].[WebsiteParameters] ([ParentId], [Code], [Value], [Required], [Visible]) VALUES (1, 'EmailAddress', 'test.mehmet.mail@gmail.com', 1, 1)
end
GO
if not exists(select * from WebsiteParameters where Code='Port' and ParentId=(select Id from WebsiteParameters where Code='EmailSettings' and ParentId is null))
begin
	INSERT [dbo].[WebsiteParameters] ([ParentId], [Code], [Value], [Required], [Visible]) VALUES (1, 'Port', '587', 1, 1)
end
GO
if not exists(select * from WebsiteParameters where Code='Host' and ParentId=(select Id from WebsiteParameters where Code='EmailSettings' and ParentId is null))
begin
	INSERT [dbo].[WebsiteParameters] ([ParentId], [Code], [Value], [Required], [Visible]) VALUES (1, 'Host', 'smtp.gmail.com', 1, 1)
end
GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
