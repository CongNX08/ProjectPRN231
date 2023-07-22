namespace Cinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddRazorPages();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //    endpoints.MapDefaultControllerRoute();
            //    endpoints.MapFallbackToPage("/Movie/List"); // Thiết lập trang mặc định là /Movie/List
            //});

            app.Run();
        }
    }
}