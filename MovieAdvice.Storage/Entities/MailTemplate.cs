using MovieAdvice.Storage.Enums;

namespace MovieAdvice.Storage.Entities
{
    public class MailTemplate : BaseEntity
    {
        public TemplateType TemplateType { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
