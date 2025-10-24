using ElectronyatShop.Data;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Controllers;

[Authorize("CustomerRole")]
public class CartController(UserManager<ApplicationUser> userManager, ElectronyatShopDbContext context)
    : Controller
{
    #region Controller Constructor and Attributes

    private string? UserId { get; set; }

    private Cart? Cart { get; set; }

    #endregion

    #region Controller Actions

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        SetCart();
        CartViewModel cartViewModel = new()
        {
            Id = Cart?.Id ?? throw new NullReferenceException("Cart is not initialized"),
            SubTotalPrice = 0,
            CartItems = Cart.CartItems?.ToList()
        };
        foreach (var item in Cart?.CartItems ?? [])
        {
            var product = await context.Products.FindAsync(item.ProductId);
            if (product is not null)
                cartViewModel.SubTotalPrice += product.ActualPrice * item.Quantity;
        }
        return View("Index", cartViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromForm] CartItemViewModel cartItem)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(actionName: "Index", controllerName: "Product");

        SetCart();
        
        // Check if product exists and has enough quantity
        var product = await context.Products.FindAsync(cartItem.ProductId);
        if (product == null)
        {
            TempData["Error"] = "Product not found!";
            return RedirectToAction(actionName: "Index", controllerName: "Product");
        }

        if (product.AvailableQuantity < cartItem.Quantity)
        {
            TempData["Error"] = "Not enough quantity available!";
            return RedirectToAction(actionName: "Details", controllerName: "Product", new { id = cartItem.ProductId });
        }

        // Check if item already exists in cart
        var existingItem = Cart?.CartItems?.FirstOrDefault(item => item.ProductId == cartItem.ProductId);
        
        if (existingItem != null)
        {
            // Update existing item
            existingItem.Quantity += cartItem.Quantity;
            context.CartItems.Update(existingItem);
        }
        else
        {
            // Add new item
            var newItem = new CartItem 
            { 
                CartId = Cart?.Id ?? 0, 
                ProductId = cartItem.ProductId, 
                Quantity = cartItem.Quantity
            };
            context.CartItems.Add(newItem);
        }

        await context.SaveChangesAsync();
        TempData["Success"] = "Product added to cart successfully!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart([FromForm]int id)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");

        SetCart();
        var item = await context.CartItems.FindAsync(id);
        if (item is null) 
        {
            TempData["Error"] = "Item not found in cart!";
            return RedirectToAction("Index");
        }

        Cart?.CartItems?.Remove(item);
        context.CartItems.Remove(item);
        await context.SaveChangesAsync();
        
        TempData["Success"] = "Item removed from cart!";
        return RedirectToAction("Index");
    }

    #endregion

    #region Controller Logic

    private void SetCart()
    {
        UserId = userManager.GetUserId(User);
        Cart = new Cart();
        if (UserId is not null)
        {
            Cart = context.Carts
                .Include(userCart => userCart.CartItems)
                .FirstOrDefault(c => c.UserId == UserId);
            
            // Create cart if not exists
            if (Cart == null)
            {
                Cart = new Cart { UserId = UserId, TotalPrice = 0 };
                context.Carts.Add(Cart);
                context.SaveChanges();
            }
        }
    }

    #endregion
}