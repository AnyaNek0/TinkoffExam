using System;
using System.Collections.Generic;
using System.Linq;

public class Store
{
    public List<Product> Products { get; set; }
    public List<Order> Orders { get; set; }

    public string GetProductStatistics(int year)
    {
        int key = 1;
        string result = "";
        foreach (var item in Orders)
        {
            var collection = item.Items.OrderByDescending(e => e.Quantity);
            if (year == item.OrderDate.Year)
            {
                
                foreach(var itemInCollection in collection)
                {
                    foreach (var product in Products)
                    {
                        if (product.Id == itemInCollection.ProductId)
                            result += $"{key}) {product.Name} - {itemInCollection.Quantity} item(s)\n";
                    }
                    key++;
                }
            }
        }
        return result;
    }
    public string GetYearsStatistics()
    {
        var orderedData = Orders.OrderByDescending(e=>e.OrderDate);

        string result = "";

        foreach (var ordersByYear in orderedData)
        {
            
            result += $"{ordersByYear.OrderDate.Year} год - ";
            double countSum = 0;
            foreach (var item in Products)
            {
                foreach (var itemInOrder in ordersByYear.Items)
                {
                    if(item.Id == itemInOrder.ProductId)
                    {
                        countSum += itemInOrder.Quantity * item.Price;
                        
                    }
                }
            }
            result += $"{countSum} руб. \nMost selling: {GetPopularProduct(ordersByYear.OrderDate.Year)}\n";
        }
        return result;
    }
    string GetPopularProduct(int year)
    {
        string result = string.Empty;
        var orderedData = Orders.GroupBy(e => e.OrderDate.Year);
        foreach (var item in orderedData)
        {
            
            foreach(var itemOrdered in item)
            {
                if(itemOrdered.OrderDate.Year == year)
                {
                    var max = itemOrdered.Items.Where(product => product.Quantity
                                                == itemOrdered.Items.Max(product => product.Quantity)).First();
                    foreach (var product in Products)
                    {
                        if (max.ProductId == product.Id)
                        {
                            result = $"Most selling: {product.Name}({max.Quantity} item(s))";
                        }
                    }
                }
            }
        }
        return result;
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}

public class Order
{
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; }
    public DateTime OrderDate { get; set; }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var store = new Store
        {
            Products = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Price = 1000 },
                new() { Id = 2, Name = "Product 2", Price = 3000 },
                new() { Id = 3, Name = "Product 3", Price = 10000 }
            },
            Orders = new List<Order>
            {
                new()
                {
                    UserId = 1,
                    OrderDate = DateTime.UtcNow,
                    Items = new List<Order.OrderItem>
                    {
                        new() { ProductId = 1, Quantity = 2 }
                    }
                },
                new()
                {
                    UserId = 1,
                    OrderDate = DateTime.UtcNow,
                    Items = new List<Order.OrderItem>
                    {
                        new() { ProductId = 1, Quantity = 1 },
                        new() { ProductId = 2, Quantity = 0 },
                        new() { ProductId = 3, Quantity = 0 }
                    }
                },
                new()
                {
                    UserId = 1,
                    OrderDate = DateTime.MaxValue,
                    Items = new List<Order.OrderItem>
                    {
                        new() { ProductId = 1, Quantity = 3 },
                        new() { ProductId = 2, Quantity = 1 },
                        new() { ProductId = 3, Quantity = 1 }
                    }
                }
            }
        };

        Console.WriteLine(store.GetProductStatistics(9999));
        Console.WriteLine(store.GetYearsStatistics());
    }
}
