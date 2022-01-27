using System;

class Program
{
    static void Main(string[] args)
    {
        IPaymentSystem firstPaymentSystem = new Website("pay.system1.ru/order?amount=12000RUB&hash=", new HashID());
        //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
        IPaymentSystem secondPaymentSystem = new Website("order.system2.ru/pay?hash=", new HashID(new HashAmount()));
        //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
        IPaymentSystem thirdPaymentSystem = new Website("system3.com/pay?amount=12000&curency=RUB&hash=", new HashAmount(new HashID(new SystemKey())));
        //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}

        Order order = new Order(2048, 180);
        Console.WriteLine(firstPaymentSystem.GetPayingLink(order));
        Console.WriteLine(secondPaymentSystem.GetPayingLink(order));
        Console.WriteLine(thirdPaymentSystem.GetPayingLink(order));
    }
}

public class Order
{
    public readonly int Id;
    public readonly int Amount;

    public Order(int id, int amount) => (Id, Amount) = (id, amount);
}

public interface IPaymentSystem
{
    public string GetPayingLink(Order order);
}

public abstract class PaymentSystem : IPaymentSystem
{
    private IPaymentSystem _paymentSystem;

    private bool _systemIsNotLast => _paymentSystem != null;

    public PaymentSystem()
    { }

    public PaymentSystem(IPaymentSystem paymentSystem)
    {
        if (paymentSystem == null)
            throw new ArgumentNullException();

        _paymentSystem = paymentSystem;
    }

    public string GetPayingLink(Order order)
    {
        if (_systemIsNotLast)
            return GetSelfPayingLink(order) + _paymentSystem.GetPayingLink(order);
        else
            return GetSelfPayingLink(order);
    }

    protected abstract string GetSelfPayingLink(Order order);
}

public class Website : PaymentSystem
{
    private string _link;

    public Website(string link, IPaymentSystem paymentSystem) : base(paymentSystem)
    {
        _link = link;
    }

    protected override string GetSelfPayingLink(Order order)
    {
        return _link;
    }
}

public class HashID : PaymentSystem
{
    public HashID() : base()
    { }

    public HashID(IPaymentSystem paymentSystem) : base(paymentSystem)
    { }

    protected override string GetSelfPayingLink(Order order)
    {
        return order.Id.ToString();
    }
}

public class HashAmount : PaymentSystem
{
    public HashAmount() : base()
    { }

    public HashAmount(IPaymentSystem paymentSystem) : base(paymentSystem)
    { }

    protected override string GetSelfPayingLink(Order order)
    {
        return order.Amount.ToString();
    }
}

public class SystemKey : PaymentSystem
{
    public SystemKey() : base()
    { }

    public SystemKey(IPaymentSystem paymentSystem) : base(paymentSystem)
    { }

    protected override string GetSelfPayingLink(Order order)
    {
        return (order.Id + order.Amount).ToString();
    }
}