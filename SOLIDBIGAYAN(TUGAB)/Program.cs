using System;


public interface IOrderProcessor
{
	void ProcessOrder(int orderId);
}

public class StandardOrderProcessor : IOrderProcessor
{
	public void ProcessOrder(int orderId)
	{
		Console.WriteLine($"[Order {orderId}] Processing a standard order.");
	}
}


public class ExpressOrderProcessor : IOrderProcessor
{
	public void ProcessOrder(int orderId)
	{
		Console.WriteLine($"[Order {orderId}] Processing an express order.");
	}
}

public interface IPaymentProcessor
{
	void ProcessPayment(decimal amount, int orderId);
}


public class CreditCardPaymentProcessor : IPaymentProcessor
{
	public void ProcessPayment(decimal amount, int orderId)
	{
		Console.WriteLine($"[Order {orderId}] Processing credit card payment of ${amount}");
	}
}

public class PayPalPaymentProcessor : IPaymentProcessor
{
	public void ProcessPayment(decimal amount, int orderId)
	{
		Console.WriteLine($"[Order {orderId}] Processing PayPal payment of ${amount}");
	}
}

public class CashOnDeliveryPaymentProcessor : IPaymentProcessor
{
	public void ProcessPayment(decimal amount, int orderId)
	{
		Console.WriteLine($"[Order {orderId}] Cash on Delivery selected. Payment of ${amount} will be collected on delivery.");
	}
}


public class OrderService
{
	private readonly IOrderProcessor _orderProcessor;
	private readonly IPaymentProcessor _paymentProcessor;
	private static int _orderCounter = 1000; 
	public OrderService(IOrderProcessor orderProcessor, IPaymentProcessor paymentProcessor)
	{
		_orderProcessor = orderProcessor;
		_paymentProcessor = paymentProcessor;
	}

	public void CompleteOrder(decimal amount)
	{
		int orderId = ++_orderCounter; 
		Console.WriteLine($"\n--- Order Summary ---");
		Console.WriteLine($"Order ID: {orderId}");
		Console.WriteLine($"Amount: ${amount}");
		Console.Write("Confirm order? (yes/no): ");

		string confirmation = Console.ReadLine()?.Trim().ToLower();
		if (confirmation != "yes")
		{
			Console.WriteLine("Order canceled.");
			return;
		}

		_orderProcessor.ProcessOrder(orderId);
		_paymentProcessor.ProcessPayment(amount, orderId);
		Console.WriteLine($"[Order {orderId}] Order completed successfully!\n");
	}
}


class Program
{
	static void Main()
	{
		Console.WriteLine("Welcome to the Order Processing System!");


		Console.WriteLine("Choose order type:");
		Console.WriteLine("1 - Standard Order");
		Console.WriteLine("2 - Express Order");
		Console.Write("Enter your choice: ");

		int orderChoice;
		while (!int.TryParse(Console.ReadLine(), out orderChoice) || (orderChoice != 1 && orderChoice != 2))
		{
			Console.Write("Invalid choice. Please enter 1 or 2: ");
		}


		Console.WriteLine("\nChoose payment method:");
		Console.WriteLine("1 - Credit Card");
		Console.WriteLine("2 - PayPal");
		Console.WriteLine("3 - Cash on Delivery");
		Console.Write("Enter your choice: ");

		int paymentChoice;
		while (!int.TryParse(Console.ReadLine(), out paymentChoice) || (paymentChoice < 1 || paymentChoice > 3))
		{
			Console.Write("Invalid choice. Please enter 1, 2, or 3: ");
		}

	
		Console.Write("\nEnter payment amount: ");
		decimal paymentAmount;
		while (!decimal.TryParse(Console.ReadLine(), out paymentAmount) || paymentAmount <= 0)
		{
			Console.Write("Invalid amount. Please enter a valid positive number: ");
		}


		IOrderProcessor orderProcessor = orderChoice == 1 ? new StandardOrderProcessor() : new ExpressOrderProcessor();
		IPaymentProcessor paymentProcessor = paymentChoice switch
		{
			1 => new CreditCardPaymentProcessor(),
			2 => new PayPalPaymentProcessor(),
			3 => new CashOnDeliveryPaymentProcessor(),
			_ => throw new InvalidOperationException("Invalid payment option")
		};


		var orderService = new OrderService(orderProcessor, paymentProcessor);
		orderService.CompleteOrder(paymentAmount);
	}
}
