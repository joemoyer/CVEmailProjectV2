using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace EmailAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public EmailSender sender = new EmailSender();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            sender = new EmailSender(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/test", async context =>
                {
                    await context.Response.WriteAsync("TestReturn");
                });
                endpoints.MapPost("/SendEmail", async context =>
                {
                    context.Response.Headers.Add("Content-Type", "text/html");

                    var headersReq = context.Request.Headers;
                    var EmailAddress = context.Request.Headers["EmailAddress"];
                    var Subject = headersReq["Subject"];
                    var Body = headersReq["Body"];

                    try
                        {
                            sender.SendEmailAsync(EmailAddress, Subject, Body);
                        }

                        catch (SmtpException ex)
                        {
                        throw new ApplicationException
                            ("SmtpException has occured: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            EmailRetry(EmailAddress, Subject, Body, 2);
                            throw ex;
                        }
                            });
                        });
        }

        private void EmailRetry(string email, string subject, string body, int countdown){
            if (countdown <=0){
                return;
            }
            else{
                try
                {
                    sender.SendEmailAsync(email, subject, body);
                }

                catch (SmtpException ex)
                {
                throw new ApplicationException
                    ("SmtpException has occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    EmailRetry(email, subject, body, --countdown);
                    throw ex;
                }
            }

        }
    }
}
