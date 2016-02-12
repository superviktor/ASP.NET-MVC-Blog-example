using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VBlog.Models;

namespace VBlog.Controllers
{
    public class AccountsController : Controller
    {
        private VBlogModel model = new VBlogModel();
        public ActionResult Login(string name, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                Random random = new Random();
                byte[] randomData = new byte[sizeof (long)];
                random.NextBytes(randomData);
                string newNonce = BitConverter.ToInt64(randomData, 0).ToString("X16");
                Session["Nonce"] = newNonce;
                return View(model:newNonce);
            }
            Admin admin = model.Admins.FirstOrDefault(x => x.Name == name);
            var nonce = Session["Nonce"] as string;
            if (admin == null || string.IsNullOrWhiteSpace(nonce))
            {
                return RedirectToAction("Index", "Posts");
            }

            string computedHash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashInput = Encoding.ASCII.GetBytes(admin.Password + nonce);
                byte[] hashData = sha256.ComputeHash(hashInput);
                var sb = new StringBuilder();
                foreach (var value in hashData)
                {
                    sb.AppendFormat("{0:X2}", value);
                }
                computedHash = sb.ToString();
            }
            Session["IsAdmin"] = (String.Equals(computedHash, hash, StringComparison.CurrentCultureIgnoreCase));
            return RedirectToAction("Index", "Posts");
        }

        public ActionResult Logout()
        {
            Session["Nonce"] = null;
            Session["IsAdmin"] = null;
            return RedirectToAction("Index", "Posts");

        }
    }
}