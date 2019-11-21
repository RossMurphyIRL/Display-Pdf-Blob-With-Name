using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DisplayPdfBlob
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
      services.AddControllersWithViews();
      services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      //app.UseCors();
      
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/ Home / Error");
      }

      app.UseHttpsRedirection();

      var policyCollection = new HeaderPolicyCollection()
          .AddContentSecurityPolicy(builder =>
          {
            builder.AddObjectSrc().Self().Blob();
            builder.AddFormAction().Self();
            builder.AddFrameAncestors().Self().Blob();
          });

      app.UseSecurityHeaders(policyCollection);
      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "src";
        if (env.IsDevelopment())
        {
          spa.UseAngularCliServer(npmScript: "start");
        }
      });
      app.UseStaticFiles();
      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute("default", "{controller=Home}/{ action = Index}/{ id ?}");
      });

    }
  }
}
