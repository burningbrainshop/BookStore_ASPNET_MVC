using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;
using System.Net.Mail;

namespace BookStore.Services
{
    public class MemberService
    {
        private Repository<Member> _repository;

        public MemberService()
        {
            _repository = new Repository<Member>();
        }

        public Member GetByAccount(string account)
        {
            return _repository.SearchFor(m => m.Account.Equals(account)).FirstOrDefault();
        }

        public void InsertMember(Member entity, string validateUrl)
        {
            entity.Password = HashPassword(entity.Password);
            entity.CreatedOn = DateTime.Now;
            _repository.Insert(entity);
            
            string mailBody = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Views/Member/RegisterEmailTemplate.html"));
            mailBody = mailBody.Replace("{{Name}}", entity.Account);
            mailBody = mailBody.Replace("{{Date}}", entity.CreatedOn.ToLongDateString());
            mailBody = mailBody.Replace("{{ValidationURL}}", validateUrl);
            SendEmail(entity, mailBody);
        }

        public void SendEmail(Member entity, string mailBody)
        {
            SmtpClient smtpService = new SmtpClient("smtp.gmail.com");
            smtpService.Port = 587;
            smtpService.Credentials = new System.Net.NetworkCredential("shanlin69", "j7ne29un");
            smtpService.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("shanlin69@gmail.com");
            mail.To.Add(entity.Email);
            mail.Subject = "[UFO BookStore.com]會員註冊確認信(V.2)";
            mail.Body = mailBody;
            mail.IsBodyHtml = true;

            smtpService.Send(mail);
        }

        public bool EmailValidation(string account, string authcode)
        {
            var data = _repository.SearchFor(m => m.Account.Equals(account) && m.AuthCode.Equals(authcode)).FirstOrDefault();
            if (data != null)
            {
                data.AuthCode = null;
                _repository.Edit(data);
            }
            return (data != null) ? true : false;
        }

        public void EditMemberPassword(Member entity)
        {
            entity.Password = HashPassword(entity.Password);
            _repository.Edit(entity);
        }

        public bool CheckAccount(string account)
        {
            var data = _repository.SearchFor(m => m.Account == account).FirstOrDefault();
            return (data != null) ? false : true;
        }

        public string HashPassword(string password)
        {
            string crypPwd = null;
            string salt = "8vnM03ikpw54VDud3279yLwq7c2Xz612tFL";
            string saltPwd = string.Concat(salt, password);

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] pwdByte = Encoding.Default.GetBytes(saltPwd);
            byte[] hashPwd = sha1.ComputeHash(pwdByte);
            for (int i = 0; i < hashPwd.Length; i++)
            {
                crypPwd += hashPwd[i].ToString("x2");
            }
            return crypPwd;
        }

        public bool CheckMember(string account, string pwd)
        {
            pwd = HashPassword(pwd);
            var data = _repository.SearchFor(m => m.Account.Equals(account) && m.Password.Equals(pwd)).FirstOrDefault();
            return (data != null) ? true : false;
        }
    }
}