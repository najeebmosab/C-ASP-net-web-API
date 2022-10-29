using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Middlewares
{
    public class AllowedCorsMiddlewares
    {
        /*https://localhost:44353*/
        private readonly RequestDelegate _next;

        public AllowedCorsMiddlewares(RequestDelegate next)
        {
            _next = next;
        }
        // אני אקבל את הבקשה מהלקוח עם כל המידע של הבקשה

        public async Task Invoke(HttpContext context)
        {
            //פתרון לבעיית ה cors 
            //אחראי לשמות השרתים שמוותר להם לעבור דרך השרת

            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            // אחראי לסוג' ה headers שהשרת יכול לקבל
            // אם רוצים לחסום header מסים, לא נשים אותו במערך
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "*" });


            //אילו סוגי
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "*" });

            //before server לכיוון השרת
            await _next(context);
            //after server חזרה מהשרת
        }
    }

}
