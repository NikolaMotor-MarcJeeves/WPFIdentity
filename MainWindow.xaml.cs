using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Windows;
using WPFIdentity.Models;

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace WPFIdentity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Credentials? Credentials { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();

            Credentials = new Credentials()
            {
                UserName= "Admin@Admin.com",
                Password= "Password"
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Credentials == null)
                return;

            if (Credentials.UserName == "Admin@Admin.com" && Credentials.Password == "Password")
            {
                // Creating the Security Context
                var Claims = new List<Claim>
                {
                    new Claim("Employee","Employee"),
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Email, "Admin@admin.com")
                };

                //Creating the Identity
                var identity = new ClaimsIdentity(Claims, "MyWFPCookieAuth");

                //Creating the Principal
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                //Create Auth Properties
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credentials.RememberMe
                };

                // Sign in
                // Sign in
                await HttpContext.SignInAsync("MyWFPCookieAuth", claimsPrincipal, authProperties);
                //Change Navigation
            }
        }
    }
}