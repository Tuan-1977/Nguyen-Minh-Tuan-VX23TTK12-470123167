using ElectronyatShop.Enums;
using ElectronyatShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Data;

public static class SeedData
{
    public static async Task SeedProductsAsync(ElectronyatShopDbContext context, bool forceReseed = false)
    {
        // Kiểm tra xem đã có sản phẩm chưa
        if (await context.Products.AnyAsync())
        {
            if (forceReseed)
            {
                Console.WriteLine("Clearing existing products...");
                context.Products.RemoveRange(context.Products);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Database already contains products. Skipping seed.");
                return;
            }
        }

        Console.WriteLine("Seeding products...");

        var products = new List<Product>
        {
            // Laptops
            new Product
            {
                Name = "Dell XPS 13",
                Description = "Powerful laptop with Intel i7, 16GB RAM, 512GB SSD. Perfect for professionals and content creators.",
                Price = 1499.99m,
                Image = "dell-xps-13.jpg",
                Type = ProductType.Laptop,
                AvailableQuantity = 15,
                Status = true,
                DiscountPercentage = 10
            },
            new Product
            {
                Name = "MacBook Air M2",
                Description = "Apple M2 chip, 16GB RAM, 512GB SSD. Ultra-portable and powerful.",
                Price = 1199.99m,
                Image = "macbook-air-m2.jpg",
                Type = ProductType.Laptop,
                AvailableQuantity = 10,
                Status = true,
                DiscountPercentage = 5
            },
            new Product
            {
                Name = "HP Pavilion 15",
                Description = "All-purpose laptop with AMD Ryzen 5, 8GB RAM, 512GB SSD.",
                Price = 699.99m,
                Image = "hp-pavilion-15.jpg",
                Type = ProductType.Laptop,
                AvailableQuantity = 20,
                Status = true,
                DiscountPercentage = 15
            },
            new Product
            {
                Name = "Lenovo ThinkPad X1",
                Description = "Business laptop with Intel i5, 16GB RAM, 256GB SSD. Lightweight and durable.",
                Price = 999.99m,
                Image = "lenovo-thinkpad-x1.jpg",
                Type = ProductType.Laptop,
                AvailableQuantity = 25,
                Status = true,
                DiscountPercentage = 0
            },
            new Product
            {
                Name = "ASUS ROG Strix G15",
                Description = "Gaming laptop with RTX 3070, AMD Ryzen 9, 16GB RAM, 1TB SSD.",
                Price = 1799.99m,
                Image = "asus-rog-strix-g15.jpg",
                Type = ProductType.Laptop,
                AvailableQuantity = 12,
                Status = true,
                DiscountPercentage = 8
            },

            // Mice
            new Product
            {
                Name = "Wireless Mouse Pro",
                Description = "Wireless ergonomic mouse with precision tracking and customizable buttons.",
                Price = 49.99m,
                Image = "wireless-mouse-pro.jpg",
                Type = ProductType.Mouse,
                AvailableQuantity = 50,
                Status = true,
                DiscountPercentage = 20
            },
            new Product
            {
                Name = "Gaming Mouse RGB",
                Description = "Gaming mouse with 16K DPI optical sensor and RGB lighting.",
                Price = 39.99m,
                Image = "wireless-mouse-pro.jpg",
                Type = ProductType.Mouse,
                AvailableQuantity = 45,
                Status = true,
                DiscountPercentage = 10
            },

            // Keyboards
            new Product
            {
                Name = "Gaming Keyboard RGB",
                Description = "Mechanical gaming keyboard with RGB lighting and programmable keys.",
                Price = 79.99m,
                Image = "gaming-keyboard-rgb.jpg",
                Type = ProductType.Keyboard,
                AvailableQuantity = 30,
                Status = true,
                DiscountPercentage = 25
            },
            new Product
            {
                Name = "HP OMEN 1100 Gaming Keyboard",
                Description = "Mechanical gaming keyboard with customizable RGB and anti-ghosting.",
                Price = 89.99m,
                Image = "Thursday-Jun-13-2024-05-14-44-HP-Omen-1100-Mechanical-Gaming-Keyboard.jpg",
                Type = ProductType.Keyboard,
                AvailableQuantity = 35,
                Status = true,
                DiscountPercentage = 15
            },

            // Headsets
            new Product
            {
                Name = "Sony WH-1000XM4",
                Description = "Premium wireless headphones with industry-leading noise cancellation.",
                Price = 349.99m,
                Image = "sony-wh-1000xm4.jpg",
                Type = ProductType.Headset,
                AvailableQuantity = 22,
                Status = true,
                DiscountPercentage = 10
            },
            new Product
            {
                Name = "Bose QuietComfort 45",
                Description = "Premium noise-cancelling headphones with balanced sound.",
                Price = 329.99m,
                Image = "bose-qc45.jpg",
                Type = ProductType.Headset,
                AvailableQuantity = 18,
                Status = true,
                DiscountPercentage = 15
            },
            new Product
            {
                Name = "AirPods Pro 2",
                Description = "Apple wireless earbuds with active noise cancellation and spatial audio.",
                Price = 249.99m,
                Image = "airpods-pro-2.jpg",
                Type = ProductType.Headset,
                AvailableQuantity = 38,
                Status = true,
                DiscountPercentage = 5
            },

            // Monitors
            new Product
            {
                Name = "4K Webcam Ultra",
                Description = "Professional 4K webcam with auto-focus and noise reduction.",
                Price = 199.99m,
                Image = "4k-webcam-ultra.jpg",
                Type = ProductType.Monitor,
                AvailableQuantity = 12,
                Status = true,
                DiscountPercentage = 18
            },
            new Product
            {
                Name = "USB-C Hub 7-in-1",
                Description = "Multi-port USB-C hub with HDMI, USB 3.0, SD card reader.",
                Price = 49.99m,
                Image = "usb-c-hub-7in1.jpg",
                Type = ProductType.Monitor,
                AvailableQuantity = 50,
                Status = true,
                DiscountPercentage = 20
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        Console.WriteLine($"Successfully seeded {products.Count} products!");
    }

    public static async Task SeedCustomersAsync(ElectronyatShopDbContext context)
    {
        // Kiểm tra xem đã có customer chưa (ngoài admin)
        if (await context.Users.CountAsync() > 1)
        {
            Console.WriteLine("Database already contains customers. Skipping seed.");
            return;
        }

        Console.WriteLine("Customer seed can be added here if needed.");
    }

    public static async Task SeedOrdersAsync(ElectronyatShopDbContext context)
    {
        // Kiểm tra xem đã có orders chưa
        if (await context.Orders.AnyAsync())
        {
            Console.WriteLine("Database already contains orders. Skipping seed.");
            return;
        }

        Console.WriteLine("Sample orders can be added here if needed.");
    }
}
