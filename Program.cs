using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using Web_Api_sam.Data;
using Web_Api_sam.Models;
using Web_Api_sam.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program>_logger) =>
{
    APIResponse response = new();
    _logger.Log(LogLevel.Information, "Getting all Coupons");
    response.Result = CouponStore.couponList;
    response.IsSuccess = true;
    response.StatusCode=HttpStatusCode.OK;
    return Results.Ok(response);
    //return Results.Ok(CouponStore.couponList);
    
}).WithName("GetCoupons").Produces<APIResponse>(200);
app.MapGet("/api/coupon/{id:int}", (ILogger<Program> _logger, int id) =>
{
    APIResponse response = new();
     response.Result = CouponStore.couponList.FirstOrDefault(u => u.id == id);
   
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
    //return Results.Ok(CouponStore.couponList.FirstOrDefault(u => u.id == id));

}).WithName("GetCoupon").Produces<coupon>(200);
app.MapPost("/api/coupon", ([FromBody] couponcreateDTO coupon_C_DTO) =>
{
   

    if (string.IsNullOrEmpty(coupon_C_DTO.name))
    {
        return Results.BadRequest("Invalid id or coupon Name");
    }
   if(CouponStore.couponList.FirstOrDefault(u => u.name.ToLower() == coupon_C_DTO.name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon Name already Exists");
    }
    coupon coupon = new()
    {
        IaActive = coupon_C_DTO.IaActive,
        name = coupon_C_DTO.name,
        percent = coupon_C_DTO.percent
    };
    coupon.id = CouponStore.couponList.OrderByDescending(u => u.id).FirstOrDefault().id + 1;
    CouponStore.couponList.Add(coupon);
    couponDTO couponDTO = new()
    {
        id = coupon.id,
        IaActive = coupon.IaActive,
        name = coupon.name,
        percent = coupon.percent,
        create =coupon.create
    };
    //return Results.Ok(coupon);
    //return Results.Created($"/api/coupon/{coupon.id}",coupon);
    return Results.CreatedAtRoute("GetCoupon", new {id =coupon.id }, coupon);
}).WithName("CreateCoupon").Accepts<couponcreateDTO>("application/json").Produces<couponDTO>(201).Produces(400);


app.MapPut("/api/coupon", ([FromBody] Updatecoupon coupon_U_DTO) =>
{
    
    if (string.IsNullOrEmpty(coupon_U_DTO.name))
    {
        return Results.BadRequest("Invalid id or coupon Name");
    }
    coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.id == coupon_U_DTO.id);

    couponFromStore.IaActive = coupon_U_DTO.IaActive;
    couponFromStore.name = coupon_U_DTO.name;
    couponFromStore.percent = coupon_U_DTO.percent;
    couponFromStore.Lastupdate = DateTime.Now;
    
    
   
    return Results.Ok(couponFromStore);
    
}).WithName("Updatecoupon").Accepts<Updatecoupon>("application/json").Produces<coupon>(201).Produces(400);


app.MapDelete("/api/coupon/{id:int}", (int id) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.id == id);
    if (couponFromStore != null)
    {
        CouponStore.couponList.Remove(couponFromStore);
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid Id");
        return Results.BadRequest(response);
    }
});
app.UseHttpsRedirection();



app.Run();


