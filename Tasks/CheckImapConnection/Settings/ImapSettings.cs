namespace Sisyphus.Settings
{
    public class ImapSettings : SettingsRepresent
    {

        public string Domain { get; set; }
        public string ImapServer { get; set; }
        public int ImapPort { get; set; } = 993;
        public bool UseImapssl { get; set; } = true;
        public string ToAddressSearchCondition { get; set; } = "";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AdminMailAddress { get; set; } = "admin@itworks24.ru";
        public string MailPattern { get; set; } = @"<br/>Добрый день, {UserName}. <br/>С вашей электронной почты пришёл запрос на смену пароля.<br/>Ваше имя для входа в систему:<H2>{AccountName}</H2><br/>Ваш новый пароль:<H2>{UserPassword}</H2><H3>Подробные инструкции можно найти <a href=""http://blog.itworks24.ru"">здесь</a>.</H3><br/>Если вы нашли ошибку или у вас возникли технические трудности пишите на электронку {AdminMailAddress}<br/>Спасибо за внимание!";

        public ImapSettings(string groupName) : base(groupName)
        {
        }
    }
}
