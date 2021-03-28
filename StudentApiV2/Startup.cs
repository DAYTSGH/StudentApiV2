using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Jose;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using StudentApiV2.Config;
using StudentApiV2.Data;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using StudentApiV2.Interfaces;
using StudentApiV2.Services;
using StudentApiV2.Utils;

namespace StudentApiV2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //�������
            services.AddCors(m => m.AddPolicy("any", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            
            //Controller����
            services.AddControllers()
                .AddNewtonsoftJson(setup=>
            {
                //����patch����
                setup.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            }).AddXmlDataContractSerializerFormatters();
            //Session����
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(15);
            //    options.Cookie.HttpOnly = true;
            //});
            //JWT����
            services.Configure<Token>(Configuration.GetSection("JwtSettings"));

            var token = Configuration.GetSection("JwtSettings").Get<Token>();

            //��ţ�ƴ洢����
            services.Configure<QnySetting>(this.Configuration.GetSection("Qny"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                //Token Validation Parameters
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };
            });

            //AutoMapper����
            IServiceCollection serviceCollections = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Controller����
            services.AddScoped<IAcademyRepository, AcademyRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<INoticeRepository, NoticeRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IProfessionRepository, ProfessionRepository>();
            services.AddScoped<ICourseRepository,CourseRepository>();
            services.AddScoped<ITeach_CourseRepository, Teach_CourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IScoreRepository, ScoreRepository>();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IStatusesRepository, StatusesRepository>();
            //DbContext����
            services.AddDbContext<ManageDbContext>(options=>
            {
                options.UseSqlServer
                (Configuration.GetConnectionString("StudentV2"));
            });
            //ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                //������Ϣ
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //����ĵ���Ϣ
                c.SwaggerDoc("V2", new OpenApiInfo
                {
                    Title = "StudentApi",
                    Version = "V2"
                });
            });

            //��֤����Ҫ��Session����
            //services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //����
                app.UseHttpsRedirection().UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                app.UseDeveloperExceptionPage();
            }

            //OPTIONS����
            //app.UseOptionsRequest();

            app.UseAuthentication();

            app.UseSession();
            //Session����
            //app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            //����Swagger����
            app.UseSwagger();
            //�������
            app.UseCors();

            //��֤����Ҫ��session����
            //app.UseSession();

            app.UseSwaggerUI(
                m=> 
                {
                    m.SwaggerEndpoint("/swagger/V2/swagger.json", "StudentApi");
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
