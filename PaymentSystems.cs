using System;
using System.Collections.Generic;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderForm = new OrderForm();
            var paymentHandler = new PaymentHandler();

            var paymentSystem = orderForm.ShowForm();
            
            paymentSystem.Call();

            paymentHandler.ShowPaymentResult(paymentSystem);
        }
    }

    public class OrderForm
    {
        private PaymentSystemFactory _paymentSystemFactory;

        public OrderForm() => _paymentSystemFactory = new PaymentSystemFactory();

        public PaymentSystem ShowForm()
        {
            Console.WriteLine("Мы принимаем: " + _paymentSystemFactory.ShowSystemIds());

            //симуляция веб интерфейса
            Console.WriteLine("Какое системой вы хотите совершить оплату?");

            return _paymentSystemFactory.CreatePaymentSystem(Console.ReadLine());
        }
    }

    public class PaymentHandler
    {
        public void ShowPaymentResult(PaymentSystem paymentSystem)
        {
            Console.WriteLine($"Вы оплатили с помощью {paymentSystem.SystemId}");

            paymentSystem.CheckPayment();
        }
    }

    public abstract class PaymentSystem
    {
        public readonly string SystemId;

        public PaymentSystem(string systemId)
        {
            SystemId = systemId;
        }

        public abstract void Call();

        public abstract void CheckPayment();
        /*
            Console.WriteLine("Проверка платежа через " + SystemId);
            Console.WriteLine("Оплата прошла успешно!");
        */
        public abstract PaymentSystem Clone();
    }

    public class PaymentSystemFactory
    {
        private Dictionary<string, PaymentSystem> _paymentSystems;

        public PaymentSystemFactory()
        {
            _paymentSystems = new Dictionary<string, PaymentSystem>();

            _paymentSystems["QIWI"] = new QiwiPaymentSystem("QIWI");
            _paymentSystems["WebMoney"] = new WebMoneyPaymentSystem("WebMoney");
            _paymentSystems["Card"] = new CardPaymentSystem("Card");
        }

        public string ShowSystemIds()
        {
            string message = "";

            foreach(var Id in _paymentSystems.Keys)
            {
                if (message != "")
                    message += ", ";

                message += Id;
            }

            return message;
        }

        public PaymentSystem CreatePaymentSystem(string systemId)
        {
            if (_paymentSystems.ContainsKey(systemId) == false)
                throw new ArgumentException();

            return _paymentSystems[systemId].Clone();
        }
    }

    public class QiwiPaymentSystem : PaymentSystem
    {
        public QiwiPaymentSystem(string systemId) : base(systemId)
        { }

        public override void Call()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }

        public override void CheckPayment()
        {
            Console.WriteLine("Проверка платежа через QIWI...");
            Console.WriteLine("Оплата прошла успешно!");
        }

        public override PaymentSystem Clone()
        {
            return new QiwiPaymentSystem(SystemId);
        }
    }

    public class WebMoneyPaymentSystem : PaymentSystem
    {
        public WebMoneyPaymentSystem(string systemId) : base(systemId)
        { }

        public override void Call()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }

        public override void CheckPayment()
        {
            Console.WriteLine("Проверка платежа через WebMoney...");
            Console.WriteLine("Оплата прошла успешно!");
        }

        public override PaymentSystem Clone()
        {
            return new WebMoneyPaymentSystem(SystemId);
        }
    }

    public class CardPaymentSystem : PaymentSystem
    {
        public CardPaymentSystem(string systemId) : base(systemId)
        { }

        public override void Call()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }

        public override void CheckPayment()
        {
            Console.WriteLine("Проверка платежа через Card...");
            Console.WriteLine("Оплата прошла успешно!");
        }

        public override PaymentSystem Clone()
        {
            return new CardPaymentSystem(SystemId);
        }
    }
}